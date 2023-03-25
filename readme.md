# PackageReference Cleaner

Cleans the following crazy NuGet package reference mess:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="4.3.1" Pack="false" />
  <PackageReference Include="Microsoft.SourceLink.GitHub" Version="1.1.1">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="NuGetizer" Version="1.0.1">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="PolySharp" Version="1.12.1">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="ThisAssembly.AssemblyInfo" Version="1.2.12">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="ThisAssembly.Git" Version="1.2.12">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="GitInfo" Version="3.0.5">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
</ItemGroup>
```

Into this perfectly clean and 100% equivalent beauty:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="4.3.1" Pack="false" />
  <PackageReference Include="Microsoft.SourceLink.GitHub" Version="1.1.1" PrivateAssets="all" />
  <PackageReference Include="NuGetizer" Version="1.0.1" PrivateAssets="all" />
  <PackageReference Include="PackageReferenceCleaner" Version="1.0.0" PrivateAssets="all" />
  <PackageReference Include="PolySharp" Version="1.12.1" PrivateAssets="all" />
  <PackageReference Include="ThisAssembly.AssemblyInfo" Version="1.2.12" PrivateAssets="all" />
  <PackageReference Include="ThisAssembly.Git" Version="1.2.12" PrivateAssets="all" />
  <PackageReference Include="GitInfo" Version="3.0.5" PrivateAssets="all" />
</ItemGroup>
```

## How it works

A diagnostic analyzer inspects the MSBuild project file looking for 
`<PackageReference>` with `PrivateAssets=all` (via inner element or 
attribute), removes all its child nodes and moves (if necessary) the 
`PrivateAssets` child element to an attribute.

This is done automatically whenever the analyzer runs, and nothing 
is saved unless a node was cleaned.