using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace PackageReferenceCleaner;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic, LanguageNames.FSharp)]
class CleanPackageReferences : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(new DiagnosticDescriptor(
        "DPRC001",
        "Clean the mess that NuGet does with PackageReferences",
        "Cleaned up {0} package references.",
        "Design", DiagnosticSeverity.Info, true));

#pragma warning disable RS1026 // Enable concurrent execution: cannot run concurrently, since only one can touch the project file
    public override void Initialize(AnalysisContext context)
#pragma warning restore RS1026 // Enable concurrent execution
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.RegisterCompilationAction(ctx =>
        {
            var globalOptions = ctx.Options.AnalyzerConfigOptionsProvider.GlobalOptions;
            var projectFile = globalOptions.TryGetValue("build_property.MSBuildProjectFullPath", out var fullPath) ?
                        fullPath : null;

            if (!string.IsNullOrEmpty(projectFile) && File.Exists(projectFile))
            {
                var doc = XDocument.Load(projectFile, LoadOptions.PreserveWhitespace);
                var references = doc.Descendants()
                    .Where(e => e.Name.LocalName == "PackageReference" && e.Elements().Any())
                    .Where(e => e.Attribute("PrivateAssets")?.Value == "all" || e.Element("PrivateAssets")?.Value == "all");

                var cleaned = 0;

                foreach (var item in references)
                {
                    var attributes = item.Attributes().ToArray();
                    item.RemoveAll();
                    item.Add(attributes);
                    if (item.Attribute("PrivateAssets") == null)
                        item.Add(new XAttribute("PrivateAssets", "all"));

                    cleaned++;
                }

                if (cleaned > 0)
                {
                    ctx.ReportDiagnostic(Diagnostic.Create(SupportedDiagnostics[0], null, cleaned));
                    doc.Save(projectFile);
                }
            }
        });
    }
}
