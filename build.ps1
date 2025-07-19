dotnet build FzLib --configuration Release
dotnet build FzLib.Avalonia/FzLib.Avalonia.csproj --configuration Release
dotnet pack .\FzLib\FzLib.csproj
dotnet pack .\FzLib.Avalonia\FzLib.Avalonia.csproj