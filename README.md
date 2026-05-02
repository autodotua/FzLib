# FzLib v3

FzLib是一套本人开发过程中逐渐形成的工具库，包含数据、IO、加密解密、应用、界面等各方面的重复性代码。

分为FzLib、FzLib.Avalonia和FzLib.Samples三个项目：
- **FzLib**为一个 .NET 通用工具库，涵盖数据处理、文件操作、加密解密、安全、配置与应用逻辑等常用开发辅助功能
- **FzLib.Avalonia**为基于Avalonia框架的扩展库，为跨平台桌面应用提供 MVVM 支持、控件扩展与界面交互辅助
- **FzLib.Samples**包含了各模块的示例代码。


[![FzLib NuGet](https://img.shields.io/nuget/v/FzLib.svg?label=FzLib%20Nuget)](https://www.nuget.org/packages/FzLib/)
[![FzLib.Avalonia NuGet](https://img.shields.io/nuget/v/FzLib.Avalonia.svg?label=FzLib.Avalonia%20Nuget)](https://www.nuget.org/packages/FzLib.Avalonia/)
![Size](https://img.shields.io/github/repo-size/f-shake/FzLib.svg?label=Size)
![License](https://img.shields.io/github/license/f-shake/FzLib.svg)

## 分支

| 命名空间    | 包含的内容                                                   |
| ----------- | ------------------------------------------------------------ |
| `master`    | 2018~2022年形成的库，主要涵盖.NET通用开发、地理信息开发、WPF开发等，支持.NET Framework和.NET Core |
| `master_v2` | 2022~2025年形成的库，同时服务于.NET通用开发、Windows（WPF）开发和Avalonia开发，仅支持.NET |
| `master_v3` | 2025年以后形成的库，同时服务于.NET通用开发和Avalonia开发，仅支持.NET |
| `dev`       | 尚处于开发过程中的最新代码                                   |

## 项目结构

### FzLib

一个.NET通用工具库，涵盖数据处理、文件操作、加密、安全、配置与应用逻辑等常用开发辅助功能

| 命名空间                  | 包含的内容     |
|-----------------------|-----------|
| `Application`         | 应用程序信息    |
| `Application.Startup` | 应用程序开机启动  |
| `Collections`         | 集合辅助      |
| `Cryptography`        | 加密解密辅助    |     
| `IO`                | 文件目录及输入输出 |
|`IO.Pipes`| 管道通信相关    |
|`Net`| 网络请求辅助    |
|`Numeric`| 数值计算与转换   |
|`Programming`| 编程辅助      |
|`Text`| 文本处理      |

### FzLib.Avalonia

基于Avalonia框架的扩展库，为跨平台桌面应用提供MVVM支持、控件扩展与界面交互辅助

| 命名空间                  | 包含的内容     |
|-----------------------|-----------|
| `Behaviors`           | 控件行为      |
| `Controls`            | 自定义控件     |
| `Controls.Forms`      | 表单控件      |
| `Controls.Progresses` | 进度相关控件    |
| `Controls.Windows`    | 自定义窗体     |
| `Converters`          | 值转换器      |
| `Extensions`          | 扩展方法      |
| `DependencyInjection` | 依赖注入辅助    |
| `Dialogs`             | 对话框       |
| `Dialogs.Containers` | 对话框容器     |
| `Dialogs.Picker` | 文件目录对话框扩展 |
| `Dialogs.Presets` | 预设自定义对话框  |
| `Dialogs.Services` | 对话框服务     |
|`MarkupExtensions`| XAML标记扩展  |
|`Services`| 辅助MVVM的服务 |
|`Styles`| 样式和主题     |

## 构建

- 执行`build.ps1`构建类库
- 执行`build-samples.ps1`构建示例程序。指定`-w`仅构建Windows示例，指定`-m`仅构建MacOS示例，指定`-l`仅构建Linux示例。默认会自动检测当前系统（需要PowerShell7+），对本系统的程序启用AOT。

## 文档

- [FzLib文档](Assets/doc_FzLib.md)
- [FzLib.Avalonia文档](Assets/doc_FzLib_Avalonia.md)
