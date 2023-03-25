# PackageReference Cleaner

Cleans the following crazy NuGet mess:

![crazy references](https://raw.githubusercontent.com/devlooped/PackageReferenceCleaner/main/assets/crazy.png)

into this perfectly clean and 100% equivalent beauty:

![clean references](https://raw.githubusercontent.com/devlooped/PackageReferenceCleaner/main/assets/clean.png)

## How it works

A diagnostic analyzer inspects the MSBuild project file looking for 
`<PackageReference>` with `PrivateAssets=all` (via inner element or 
attribute), removes all its child nodes and moves (if necessary) the 
`PrivateAssets` child element to an attribute.

This is done automatically whenever the analyzer runs, and nothing 
is saved unless a node was cleaned.