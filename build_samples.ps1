param(
    [switch]$w,  # Windows
    [switch]$l,  # Linux
    [switch]$m   # macOS
)

try {
    $currentRuntime = ""

    # 检测当前系统
    if ($IsWindows) {
        $currentRuntime = "win-x64"
    } elseif ($IsLinux) {
        $currentRuntime = "linux-x64"
    } elseif ($IsMacOS) {
        $currentRuntime = "osx-x64"
    } else {
        Write-Host "无法确定当前操作系统，将不发布AOT版本"
        $currentRuntime = "unknown"
    }

    Write-Host "当前系统："
    Write-Host $currentRuntime

    # 编译指定平台或全部平台
    $platforms = @()
    if ($w) { $platforms += "win-x64" }
    if ($l) { $platforms += "linux-x64" }
    if ($m) { $platforms += "osx-x64" }

    # 如果没有指定平台参数，则编译所有平台
    if ($platforms.Count -eq 0) {
        $platforms = @("win-x64", "linux-x64", "osx-x64")
    }

    # 发布win-x64
    if ($platforms -contains "win-x64") {
        $aot = $currentRuntime -eq "win-x64"
        $s = -not $aot
        Write-Output "正在发布win-x64 (AOT: $aot)"
        dotnet publish FzLib.Samples -r win-x64 -c Release -o Publish/samples/win-x64 --self-contained true /p:PublishSingleFile=$s /p:PublishAot=$aot
        rm Publish/samples/win-x64/*.pdb
    }

    # 发布linux-x64
    if ($platforms -contains "linux-x64") {
        $aot = $currentRuntime -eq "linux-x64"
        $s = -not $aot
        Write-Output "正在发布linux-x64 (AOT: $aot)"
        dotnet publish FzLib.Samples -r linux-x64 -c Release -o Publish/samples/linux-x64 --self-contained true /p:PublishSingleFile=$s /p:PublishAot=$aot
        rm Publish/samples/linux-x64/*.pdb
    }

    # 发布macos-x64
    if ($platforms -contains "osx-x64") {
        $aot = $currentRuntime -eq "osx-x64"
        $s = -not $aot 
        Write-Output "正在发布macos-x64 (AOT: $aot)"
        dotnet publish FzLib.Samples -r osx-x64 -c Release -o Publish/samples/osx-x64 --self-contained true /p:PublishSingleFile=$s /p:PublishAot=$aot
        rm Publish/samples/osx-x64/*.pdb
    }

    rm -Recurse -Force FzLib.Samples/obj/Release
    rm -Recurse -Force FzLib.Avalonia/obj/Release
    rm -Recurse -Force FzLib/obj/Release
    Write-Output "操作完成"
    Invoke-Item Publish\Samples
    pause
}
catch {
    Write-Error $_
    pause
}
