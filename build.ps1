dotnet build FzLib --configuration Release --output ./Publish/libs
dotnet build FzLib.Avalonia/FzLib.Avalonia.csproj --configuration Release --output ./Publish/libs
dotnet pack .\FzLib\FzLib.csproj --output ./Publish/nugets
dotnet pack .\FzLib.Avalonia\FzLib.Avalonia.csproj --output ./Publish/nugets