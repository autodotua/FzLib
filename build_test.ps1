try {
    try {
        dotnet
    }
    catch {
        throw "未安装.NET SDK"
    }
    
    Clear-Host

    Write-Output "正在发布win-x64"
    dotnet publish FzLib.Avalonia/Test/Desktop -r win-x64 -c Release -o Publish/Test/Windows --self-contained true /p:PublishSingleFile=true 
   
    Write-Output "正在发布linux-x64"
    dotnet publish FzLib.Avalonia/Test/Desktop -r linux-x64 -c Release -o Publish/Test/Linux --self-contained true /p:PublishSingleFile=true 
    
    Write-Output "操作完成"

    Invoke-Item Publish\Test
    pause
}
catch {
    Write-Error $_
}