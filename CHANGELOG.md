# CHANGELOG

## v3

### 2026-06-24
- 修复了一些编译时的报错和警告
- 升级依赖并调整自定义窗口边框逻辑

### 2026-05-18
- 优化 GroupBoxWithBorder
- 分离 Themes 和 Styles 文件；优化 NumericUpDown 的 Styles

### 2026-05-11
- 发布 3.4.0-rc.2

### 2026-05-06~10
- 优化窗体样式
- 优化窗口
- 更新版本号
- 细节优化

### 2026-05-02~05
- **正在迁移至 Avalonia 12**
- 将 WindowButtons 分离代码和样式
- 优化 Samples；新增 SliderStyles

### 2026-04-26~27
- 优化 Converters，使用 DataSizeConverter 替代 FileLengthConverter 和 TransferSpeedConverter
- 新增 BitmapAssetValueConverter
- 新增 NumericUpDownVerticalButtonSpinnerTheme
- 修复三个 GB2312 编码文件到 UTF-8
- 优化 StringListConverter
- 修复了 Converters 中的一个错误

### 2026-04-26
- FormItem 新增 BoldHeader 样式
- 优化 FileFilterHelper

### 2026-04-08
- 优化 ApplicationInfo 字段
- 修复 StringListEditor 数据源为空导致报错的 BUG
- 移动 Net 到 Web 命名空间；新增 ServiceResult
- FileFilter 支持了目录的筛选

### 2025-10-30
- 更新 README
- StringlistEditor 新增支持调整新增按钮的位置

### 2025-10-28
- 补充了一些 Style
- 更新 README

### 2025-10-22~26
- 新增大量文档（FzLib 文档、FzLib.Avalonia 文档、对话框文档、README）
- 新增 Brushes 的 Samples
- 新增样式相关的 Samples
- 修复了因修改 StorageProvider 相关服务的命名空间导致 Samples 出错的 BUG

### 2025-10-19~22
- 新增异步集合扩展方法
- 优化对话框样式和文本显示
- 修复了 SmoothScrollBehavior 导致内部无法滚动的 BUG
- 优化未捕获异常处理；修复 SocketException 误报问题
- 为 IProgressOverlayService 新增反注册

### 2025-10-17
- 新增了更多的 BoolLogicConverter

### 2025-10-10~13
- 新增 RadioButtonGroup 控件
- FormItem 的 Label 等支持了模板
- 新增 RadioButtonGroup 样式的 ListBox
- 优化 FormItem 样式和布局；调整 Brushes 资源
- 优化 RadioButtonGroup 细节
- 新增 StringListEditor
- 修复了 StringListEditor 在 AOT 下存在的问题
- 修复了 StringListEditor 使用 dll 时按钮不显示、ScrollBarThickness 污染的问题
- 修复了对话框显示位置不正确的 BUG
- 为 RadioButtonGroup 和 StringListEditor 添加布局和间距控制功能

### 2025-10-03~07
- 修复了 ExtendedWindow 在添加背景时圆角失效的 BUG
- 新增用户和设备唯一识别码

### 2025-09-26
- 优化了 ExtendedWindow
- 使用 ShadowBox 代替 PopupDialog 中的阴影，效果更好

### 2025-09-16~18
- 优化了 PopupDialogContainer
- 优化了对话框动画
- PopupDialogContainer 新增关闭动画
- 优化了对话框，将 Popup 通过 Theme 描述，增加了呼出动画
- 新增 JsonSerializable 相关类
- 修复了一些 JsonSerializableExtensions 的 BUG

### 2025-09-13
- 修复了一些警告
- 优化了对话框的一些 UI，新增了三个按钮的 Command 绑定

### 2025-09-02~09
- 新增 ProgressOverlayService 及 ProgressRingBoxOverlay
- ProgressOverlayService 支持手动取消
- 修复了 Overlay 挡住 GridSplitter 的问题
- 修复了 ProgressOverlayService 在 Attach 后通过非主线程调用会报错的 BUG
- 新增 HttpRequester
- 新增 BoolToIntegerConverter
- EqualToBoolConverter 支持了与固定值比较以及比较类型的功能
- 调整了部分样式
- 新增 Avalonia 的依赖注入相关辅助类

### 2025-08-24
- 新增 TransferSpeedConverter

### 2025-08-14~16
- 加快了 TcpSingleInstanceHelper
- FileFilterRule 新增 IsEnabled 属性

### 2025-08-03~11
- 新增 TcpSingleInstanceHelper，支持通过 TCP 协议判断应用程序是否重复运行并确保唯一性
- 将 Converter 中的 2（Two）规范化为 To
- 新增文件选取器的 Fluent Builder
- 支持了在复制和加解密文件时，同时得到文件的 Hash
- 支持了仅计算解密后文件 Hash，在解密的同时计算文件 Hash

### 2025-07-17~30
- 对原本重复使用的文件结构进行了整理
- 优化了通知的实现方式
- 新增了文件/目录复制、Hash、删除等文件操作有关的扩展
- 修改 UnhandledExceptionCatcher 为 Fluent Builder
- 新增 DialogService，方便在 ViewModel 中调用对话框
- 新增了基于 TopLevel 实现的扩展类，包括 WeakReferenceMessage
- 值转换器图片资源修改为静态的
- 为 ProgressRingOverlay（原 LoadingOverlay）实现了 MVVM 的支持
- 修改了一些命名空间
- 修改了预置对话框的调用方式，支持了 AOT
- 完成了 InputDialog 的改造
- 支持了 SelectDialog 和 CheckBoxDialog 的 AOT
- 与 FzLib.Test 合并
- 修改为 TabControl 布局
- 修改对话框和演示为 MVVM

---

## v2

### 2025-07-03~16
- 为 FormItem 新增了控件上方的标签（Header）
- 修复了 FormItem 即使无 Label，左侧也会有 Margin 的 BUG
- 优化了值转换器
- 新增 Bool2TextWrappingConverter
- 继续为 Converters 写测试案例

### 2025-04-06
- 尝试支持了 AOT
- 优化了一些底层不兼容 AOT 的代码

### 2024-11-25
- 支持了非模态窗口的对话框

### 2024-10-27
- 重新支持开机自启功能，支持跨平台（Windows、Linux）

### 2024-10-06
- 新增支持了自定义标题栏颜色

### 2024-10-05
- 修复 Win11 下 ExtendedWindow 的一些样式问题

### 2024-09-10~21
- 基本完成自定义窗口（ExtendedWindow）
- 支持了标题栏控件
- Linux 和 macOS 将不启用自定义窗口
- 优化 Windows 下 ExtendedWindow 的效果
- 修复了窗口边距的一些问题
- 修复了对话框的拖动相关问题
- 为 Popup 类型的对话框实现了拖动功能
- 支持了 Window 对话框拖动
- 修复了 StackFormItemGroup 初始化时会闪一下的 BUG

### 2024-08-29~30
- 新增 FormItemGroup
- 为 FormItem 增加了 Description

### 2024-06-25~28
- 增加了主题相关的资源
- 新增 FilePickerTextBox、FormItem
- 新增 Messages
- 新增了大量的 Converters 和资源
- 修复了对话框需要手动设置 Theme 的 BUG

### 2024-03-19
- 修复了 Loading 在延迟前取消无效的 BUG

### 2024-02-28
- 新增了覆盖层

### 2024-02-05
- 新增了自定义窗口左上角按钮的 Avalonia 实现
- 修改 DialogHost 的 Title 为 StyledProperty 提供

### 2024-01-19 ~ 2024-02-05
- 新增 Avalonia 对话框
- 优化了对话框调用方式，使用 DialogContainerType 指定弹窗类型
- 修复了多屏幕对话框位置和 Linux 窗口样式问题

### 2024-01-19
- 删除原有 WPF 项目，正式迁移至 Avalonia
- 重命名项目：FzStandardLib → FzLib，FzCoreLib.Windows → FzLib.Windows
- 新增 FzLib.Avalonia
- 重命名解决方案

---

## v1

### 2022-03-18
- 优化了异常捕捉
- 更新 NuGet 依赖
- 修复了 EqualConverter 参数必须为 string 的 BUG
- EqualConverter 支持了 Bool 和反转
- 优化了 Converters

### 2021-09-06
- 修复了一个 BUG

### 2021-07-08 ~ 2021-07-26
- 卸载 .NET Framework 项目，分离 FzAlgorithmLib、FzGeographyLib
- 新增 FzCoreLib.Windows，依赖 WindowsAPICodePack
- 新增 ModernWpf.FzExtension
- 大量重构：FileSystemDialog、TaskDialog、AES、RSA、Hash 改为扩展方法
- 新增 WpfDemo 项目
- 大量 Demo：Converter、扩展、系统主题、Json 序列化、文件 IO 等
- 重写 Json 序列化类为接口+扩展方法

### 2021-05-03
- 发布 NuGet 包

### 2021-03-05 ~ 2021-04-03
- 新增大量 WPF Converters
- JsonSerializationBase 新增 JsonSerializerSettings 支持
- 新增 EnumerateAccessibleDirectories

### 2020-07-16 ~ 2020-08-10
- FzCoreLib.Windows 新增 .NET Core 支持
- 新增 FileSystemTree、FileSystemWatcher
- 删除 FileProperty 命名空间及相关内容

### 2020-03-25
- 重写 FileSystemDialog

### 2020-01-16 ~ 2020-01-31
- 拆分桌面库：FzDesktopLib → FzDesktopLib.Windows + FzUILib.WPF
- 修改 ExtendedINotifyPropertyChanged 为扩展方法
- 修复 INotifyPropertyChanged 接口实现类无法绑定的 BUG

### 2019-12-05 ~ 2019-12-18
- Math：新增数学表达式计算功能，支持度分秒、角度弧度转换、阶乘等
- SQLite：大量修改，支持使用 SQL 语句查询

### 2019-10-27
- 重新启用 Geography 库

### 2019-06-01 ~ 2019-07-02
- 新增 BitmapSourceToBitmap、PanelExport.GetBitmap
- FileSystemDialog 移至 FzDesktopLib，新增 otherSetting 参数
- 重构 ItemControlHelper，支持 ListView、ListBox、DataGrid
- 移除 Win10 平台专属代码
- 新增统一样式的 DataGrid

### 2019-01-07 ~ 2019-01-30
- Number：适合文件大小的单位方法
- TaskDialog：DefaultOwner、cancelable 参数
- 修复 Csv 序列化写为 Cvs 的 BUG
- FlatStyle：CheckBox、UneditableComboBox 控件
- 移动 Control.ControlExtended → Control.Extension
- 序列化类添加 INotifyPropertyChanged 接口
- 新增 GetOpenFiles 方法
- Form 系列控件新增输入验证

### 2018-12-01 ~ 2018-12-29
- 新增 FzAlgorithmLib（算法库）
- TaskDialog 移植到 WPF
- SnakeBar 通知条
- 新增 Verification 验证类
- BinarySortTree 二叉排序树
- ExtendedDictionary 扩展字典
- 新增集合相关功能移至 Basic.Collection

### 2018-11-02 ~ 2018-11-30
- 大量 IO、FileProperty、Exif 功能
- ExtendedWindow、ExtendedUserControl
- Csv 序列化
- ExtendedObservableCollection（Range 操作）
- 托盘图标 TrayIcon
- 新增 FzGeographyLib（地理信息库）
- 坐标系转换（WGS84、GCJ02）、Shapefile、GPX 解析
- 距离/速度分析

### 2018-10-21
- 项目正式成立，整合标准库
- StandardCodes → FzLib (FzStandardLib)
- WpfCodes → FzLib (FzWpfLib)
- WpfControls → FzLib.Controls (FzWpfControlLib)
- 开始日志记录
- 清理 Release 文件，新建 Git

---

## WpfControls / WpfCodes

### 2018-10-14~15
- 【Notify】新增 TaskDialog 类
- 【Runtime】新增命名空间：SinglePipe、SingleInstance、Thread、UnhandledException
- 【FileFormatAssociation】文件关联

### 2018-10-06~12
- 【Startup】SourceFileName 改为公共属性
- 【Cryptography】新增命名空间：Aes、Hash、CommonSettings
- 修复 Aes 加密文件 BUG
- 新增 TrayIcon 的 InsertContextMenuItem
- Basic.String：Encoding 方法重构
- 新增 Information 类、文件对比方法

### 2018-09-17 ~ 2018-10-03
- 【IO】新增 CreateShortcut
- 命名空间大重构：System → Windows，FileSystem → IO，Exception → UnhandledException
- 【Config】SettingsBase → JsonSettingsBase，默认名 config.json
- 新增 FileSystemTree、Shortcut 类

### 2018-08-07 ~ 2018-09-01
- 新增 Device 命名空间：Mouse、KeyboardHelper、KeyboardHook
- SettingsBase 新增 Json 方式
- 升级 .NET Framework 4.7.2

### 2018-07-14 ~ 2018-07-31
- 【Pipe】程序间通信封装
- 【Startup】新增多实例检测
- 新增 Exception 类
- 新增 PerformanceInformation 类
- 新增 DeleteToRecycleBin 方法

### 2018-08
- 【Dialog】DialogHelper、ShowYesNo
- 【FlatStyle】TreeView、Button（圆角）
- 【Win10Style】命名空间（替代 MahApps）
- 【Progress】LoadingBar

### 2018-07
- 【TimePicker】TimeSpan、DateTime 支持
- 【FileSystem】FileDropListBox
- 【FlatStyle】TreeView

### 2018-06
- 【FlatStyle】TableView（支持 Excel 粘贴）
- 【Dialog】DialogBox 详细内容块
- 【TrayIcon】右键菜单改进

### 2018-05
- 【FlatStyle】Button、HorizontalSlider、ListBox、ListView
- 【Picker】ColorPicker、FontPicker、DatePicker、DateRangePicker
- 【Progress】LoadingOverlay

### 2018-04~06
- 创建项目
- 【Program】Config、SettingsBase、Startup、Thread
- 【WindowsApi】WindowMode、Clipboard、HotKey
- 【Basic】Enumerable（Csv 导入导出）、Number、IO
- 【Media】CanvasExport

### 2018-04
- 【Dialog】DialogBox、InputBox、Toast
- 【Text】GradualChangedTextBlock、HintTextBox、NumberTextBox、TimePicker、FileDropTextBox
- 【FlatStyle】Button、ListView

### 2018-03
- 【Dialog】InputBox、DialogBox
- 【Text】GradualChangedTextBlock、HintTextBox、TimePicker、StrokeableLabel、SearchComboBox
- 【Progress】ProgressBarWithPercentageText、LoadingOverlay
- 【FlatStyle】Button、ListView、CheckBox、UneditableComboBox

### 2018-02
- 【Dialog】DialogBox、InputBox
