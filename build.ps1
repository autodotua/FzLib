dotnet build FzLib.Avalonia/FzLib.Avalonia.csproj --configuration Release
dotnet build FzLib.Windows --configuration Release
dotnet pack .\FzLib\FzLib.csproj
dotnet pack .\FzLib.Windows\FzLib.Windows.csproj
dotnet pack .\FzLib.Avalonia\FzLib.Avalonia.csproj