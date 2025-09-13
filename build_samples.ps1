try {
    $s = $false
    $currentRuntime = ""
    
    # 检测当前系统
    if ($IsWindows) {
        $currentRuntime = "win-x64"
    } elseif ($IsLinux) {
        $currentRuntime = "linux-x64"
    } elseif ($IsMacOS) {
        $currentRuntime = "osx-x64"
    } else {
        Write "无法确定当前操作系统，将不发布AOT版本"
        $currentRuntime = "unknown"
        # throw "无法确定当前操作系统"
    }

    Write-Host "当前系统："
    Write-Host $currentRuntime

    # 发布win-x64
    $aot = $currentRuntime -eq "win-x64"
    Write-Output "正在发布win-x64 (AOT: $aot)"
    dotnet publish FzLib.Samples -r win-x64 -c Release -o Publish/samples/win-x64 --self-contained true /p:PublishSingleFile=$s /p:PublishAot=$aot
    rm Publish/samples/win-x64/*.pdb
   
    # 发布linux-x64
    $aot = $currentRuntime -eq "linux-x64"
    Write-Output "正在发布linux-x64 (AOT: $aot)"
    dotnet publish FzLib.Samples -r linux-x64 -c Release -o Publish/samples/linux-x64 --self-contained true /p:PublishSingleFile=$s /p:PublishAot=$aot
    rm Publish/samples/linux-x64/*.pdb
   
    # 发布macos-x64
    $aot = $currentRuntime -eq "osx-x64"
    Write-Output "正在发布macos-x64 (AOT: $aot)"
    dotnet publish FzLib.Samples -r osx-x64 -c Release -o Publish/samples/osx-x64 --self-contained true /p:PublishSingleFile=$s /p:PublishAot=$aot
    rm Publish/samples/osx-x64/*.pdb
    
    Write-Output "操作完成"

    Invoke-Item Publish\Samples
    pause
}
catch {
    Write-Error $_
    pause
}