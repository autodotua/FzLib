# FzLib.Avalonia

## `Behaviors`

### `SmoothScrollBehavior`

用于为ScrollViewer和包含ScrollViewer的TextBox、ListView等控件的滚动区域提供在鼠标滚轮滚动时平滑移动的行为。

```xaml
<ScrollViewer b:SmoothScrollBehavior.IsEnabled="True">
```

```xaml
<TextBox b:SmoothScrollBehavior.IsEnabled="True">
```

### `VisualDragBehavior`

为Visual（基本包含各类可见的控件）控件提供拖动功能，并限制在其父容器范围内。

```xaml
  <Border>
            <Grid RowDefinitions="*,8,Auto,8,*">
                <TextBlock
                    Grid.Row="2"
                    HorizontalAlignment="Center"
                    VerticalAlignment="Center"
                    IsHitTestVisible="False"
                    Text="点击黄块拖动控件" />
                <Border
                    Grid.Row="4"
                    Width="24"
                    Height="24"
                    HorizontalAlignment="Right"
                    VerticalAlignment="Bottom"
                    Background="Yellow">
                    <Interaction.Behaviors>
                        <b:VisualDragBehavior Target="{Binding #bd}" />
                    </Interaction.Behaviors>
                </Border>
            </Grid>
```

```csharp
var behavior = new VisualDragBehavior
{
    Target = bdDialog
};
Interaction.GetBehaviors(thumb).Add(behavior);
```

### `WindowDragBehavior`

为窗口提供拖动功能

```xaml
 <Border>
     <Interaction.Behaviors>
         <b:WindowDragBehavior />
     </Interaction.Behaviors>
</Border>
```

## `Controls`

### `CancelableTaskButton`

提供一个可以取消的按钮。点击按钮后，执行`Command`，同时按钮显示取消字样，保持可点击；再次点击，执行`CancelCommand`，并向`Command`通知Cancel，按钮置灰，直至完成`CancelCommand`。

```xaml
<c:CancelableTaskButton
    CancelCommand="{Binding DoSthCancelCommand}"
    TaskCommand="{Binding DoSthCommand}" />
```

```csharp
[RelayCommand(IncludeCancelCommand = true)]
private async Task DoSthAsync(CancellationToken cancellationToken)
{
    try
    {
        await Task.Delay(10 * 1000, cancellationToken);
    }
    catch (OperationCanceledException)
    {
        await dialogService.ShowWarningDialogAsync("任务被取消", "任务被取消");
    }
}
```

### `FilePickerTextBox`

提供一组由标签、文本框和按钮组成的文件选取条，点击按钮弹出文件选取对话框，可将文件拖入文本框。支持配置打开文件、保存文件或选择文件夹。

![FzLib.Avalonia.Controls.FilePickerTextBox](FzLib.Avalonia.Controls.FilePickerTextBox.png)

```xaml
<ct:FilePickerTextBox
    Label="打开文件："
    Type="OpenFile" />
<ct:FilePickerTextBox
    Label="打开文件（仅JPG和PNG）："
    StringFileTypeFilter="JPEG图片;*.jpg,*.jpeg;image/jpeg;|PNG图片;*.png;image/png;"
    Type="OpenFile" />
<ct:FilePickerTextBox
    Label="保存文件："
    SaveFileSuggestedFileName="建议的文件名"
    Type="SaveFile" />
<ct:FilePickerTextBox
    Label="打开目录："
    Type="OpenFolder" />
<ct:FilePickerTextBox
    ButtonContent="打开目录（无标签）"
    Type="OpenFolder" />
```

## `Controls.Forms`

### `FormItem`

提供一个表单容器，支持在容器的左侧和上方添加标签，下方添加描述

![FzLib.Avalonia.Controls.FormItem](FzLib.Avalonia.Controls.FormItem.png)

```xaml
<c:StackFormItemGroup>
    <c:FormItem Header="这是另一种标签形式，它可以显示在控件上方">
        <TextBox />
    </c:FormItem>
    <c:FormItem
        Header="{Binding .}"
        Label="当前选择状态：">
        <c:FormItem.HeaderTemplate>
            <DataTemplate x:DataType="vm:FormViewModel">
                <CheckBox
                    Content="Label、Header和Description均可设置模板"
                    IsChecked="{Binding FormHeaderIsChecked}" />
            </DataTemplate>
        </c:FormItem.HeaderTemplate>
        <TextBox
            IsReadOnly="True"
            Text="{Binding FormHeaderIsChecked}" />
    </c:FormItem>
    <c:FormItem
        Description="描述也可以共存"
        Header="Header可以和左侧标签共存"
        Label="左侧标签：">
        <TextBox />
    </c:FormItem>
</c:StackFormItemGroup>
```

### `StackFormItemGroup`

一个为`FormItem`特化的纵向`StackPanel`，能够自动调整内部所有`FormItem`的`Label`的宽度到一致。

![FzLib.Avalonia.Controls.StackFormItemGroup](FzLib.Avalonia.Controls.StackFormItemGroup.png)

```xaml
<c:StackFormItemGroup>
    <c:FormItem
        Description="这是一条表单项的描述"
        Label="姓名：">
        <TextBox Text="王某某" />
    </c:FormItem>
    <c:FormItem Label="打开文件：">
        <c:FilePickerTextBox Type="OpenFile" />
    </c:FormItem>
    <c:FormItem Label="左对齐：">
        <NumericUpDown
            HorizontalAlignment="Left"
            Value="20" />
    </c:FormItem>
</c:StackFormItemGroup>
```

### `WrapFormItemGroup`

一个为`FormItem`特化的横向`WrapPanel`，能够自动调整内部所有`FormItem`的`Label`的宽度到一致。

![FzLib.Avalonia.Controls.WrapFormItemGroup](FzLib.Avalonia.Controls.WrapFormItemGroup.png)

```xaml
<c:WrapFormItemGroup>
    <c:FormItem Label="文本框：">
        <TextBox
            Width="120"
            Text="" />
    </c:FormItem>
    <c:FormItem Label="选择框：">
        <ComboBox
            Width="120"
            SelectedIndex="0">
            <ComboBoxItem>选项1</ComboBoxItem>
            <ComboBoxItem>选项2</ComboBoxItem>
            <ComboBoxItem>选项3</ComboBoxItem>
            <ComboBoxItem>选项4</ComboBoxItem>
        </ComboBox>
    </c:FormItem>
    <c:FormItem Label="数字选择：">
        <NumericUpDown
            Width="120"
            FormatString="0" />
    </c:FormItem>
    <CheckBox
        VerticalAlignment="Center"
        Content="非表单元素" />
    <Button Content="非表单元素" />
    <Button
        Classes="Link"
        Content="非表单元素" />
</c:WrapFormItemGroup>
```

## `Controls.ItemsControls`

### `ILayoutChangeable`, `ItemsControlLayout`

`ILayoutChangeable`为`ItemsControl`的`ItemsPanel`提供抽象属性的接口。

`ItemsControlLayout`提供了不同`ItemsPanel`的枚举，其中`VerticalStack`和`HorizontalStack`为`StackPanel`，`VerticalWrap`和`HorizontalWrap`为`WrapPanel`，`UniformGrid`为`UniformGrid`。

### `RadioButtonGroup`

提供一组按钮样式的`RadioButton`，由`ListBox`实现，用户可以在选项之间切换，实现单选功能。可以使用`SelectedIndex`绑定选择的序号，或者使用`SelectedItem`绑定选择的项。

![FzLib.Avalonia.Controls.RadioButtonGroup](FzLib.Avalonia.Controls.RadioButtonGroup.png)

```xaml
<c:RadioButtonGroup
    ItemsSource="{me:EnumValues HorizontalAlignment}"
    Layout="{Binding #listLayout.SelectedItem}"
    SelectedIndex="{Binding SelectedIndex1}"
    SelectedItem="{Binding SelectedItem1}"
    Spacing="{Binding #sldSpacing.Value}" />
<c:RadioButtonGroup
    Columns="2"
    ItemsSource="{Binding Items2}"
    Layout="{Binding #listLayout.SelectedItem}"
    Rows="1"
    SelectedIndex="{Binding Selected2, Converter={x:Static cvt:Converters.BoolZeroOne}}"
    SelectedItem="{Binding SelectedItem2}"
    Spacing="{Binding #sldSpacing.Value}" />
```

### `StringListEditor`

用于编辑一个`string`列表，使用`ItemsSource`绑定一个`FzLib.Text.ObservableStringList`。支持修改字符串，支持在列表中追加、插入或删除字符串

![FzLib.Avalonia.Controls.StringListEditor](FzLib.Avalonia.Controls.StringListEditor.png)

```xaml
<c:StringListEditor ItemsSource="{Binding StringList}"/>
```

### `Progresses`

未完待续