using System;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using FzLib.Text;

namespace FzLib.Avalonia.Controls;

public class StringListEditor : TemplatedControl, ILayoutChangeable
{
    public static readonly StyledProperty<Dock> AddButtonPositionProperty =
        AvaloniaProperty.Register<StringListEditor, Dock>(
            nameof(AddButtonPosition), Dock.Right);

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<RadioButtonGroup, int>(
        nameof(Columns));

    public static readonly StyledProperty<ObservableStringList> ItemsSourceProperty =
        AvaloniaProperty.Register<StringListEditor, ObservableStringList>(
            nameof(ItemsSource));

    public static readonly StyledProperty<ItemsControlLayout> LayoutProperty =
        AvaloniaProperty.Register<StringListEditor, ItemsControlLayout>(
            nameof(Layout), ItemsControlLayout.HorizontalStack);

    public static readonly StyledProperty<int> RowsProperty = AvaloniaProperty.Register<RadioButtonGroup, int>(
        nameof(Rows));

    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<RadioButtonGroup, double>(
        nameof(Spacing), 8d);

    private Button addButton;

    private ItemsControl items;

    private ScrollViewer scr;

    public Dock AddButtonPosition
    {
        get => GetValue(AddButtonPositionProperty);
        set => SetValue(AddButtonPositionProperty, value);
    }

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public ObservableStringList ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public ItemsControlLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    protected override Type StyleKeyOverride { get; } = typeof(StringListEditor);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        scr = e.NameScope.Find<ScrollViewer>("PART_ScrollViewer");
        items = e.NameScope.Find<ItemsControl>("PART_Items");
        addButton = e.NameScope.Find<Button>("PART_AddButton");
        if (scr == null || items == null || addButton == null)
        {
            return;
        }

        ItemsControlExtensions.SetLayoutSubscribe(this, items, LayoutProperty, SpacingProperty, ColumnsProperty,
            RowsProperty);

        addButton.Click += AddButton_Click;

        items.ContainerPrepared += (s, e) =>
        {
            e.Container.Loaded += (s2, e2) =>
            {
                var textBox = e.Container.GetVisualDescendants().OfType<TextBox>().First();
                var removeButton = e.Container.GetVisualDescendants().OfType<Button>().First();
                if (textBox == null || removeButton == null)
                {
                    throw new InvalidOperationException("找不到TextBox或Button");
                }

                textBox.KeyDown += TextBox_KeyDown;
                removeButton.Click += RemoveButton_Click;
            };
        };

        this.GetObservable(ItemsSourceProperty).Subscribe(ItemsSourceChanged);
        this.GetObservable(AddButtonPositionProperty).Subscribe(p => UpdateAddButtonMargin());
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var list = GetItemsSourceList();
        list.Add("新项目");
        scr.Offset = new Vector(int.MaxValue, 0); //滚动到最右侧
        FocusTextBox(list.Count - 1);
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        var list = GetItemsSourceList();
        list.Clear();
    }

    private void FocusTextBox(int index)
    {
        //将光标移动到新插入的项上
        if (items.ContainerFromIndex(index) is not ContentPresenter container)
        {
            throw new ArgumentNullException(nameof(container));
        }

        container.Loaded += (s, _) =>
        {
            var txt = (s as ContentPresenter ?? throw new InvalidOperationException())
                .GetVisualDescendants()
                .OfType<TextBox>()
                .First();
            txt.Focus();
            txt.SelectAll();
        };
    }

    private ObservableStringList GetItemsSourceList()
    {
        if (ItemsSource is null)
        {
            throw new InvalidOperationException("ItemsSource为空");
        }

        return ItemsSource;
    }

    private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateAddButtonMargin();
    }

    private void UpdateAddButtonMargin()
    {
        if (ItemsSource == null || ItemsSource.Count == 0)
        {
            addButton.Margin = new Thickness();
        }
        else
        {
            addButton.Margin = AddButtonPosition switch
            {
                Dock.Left => new Thickness(0, 0, 8, 6),
                Dock.Bottom => new Thickness(0, 4, 0, 0),
                Dock.Right => new Thickness(8, 0, 0, 6),
                Dock.Top => new Thickness(0, 0, 0, 6),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private void ItemsSourceChanged(ObservableStringList obj)
    {
        if (obj != null)
        {
            obj.CollectionChanged += ItemsSource_CollectionChanged;
            ItemsSource_CollectionChanged(obj,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        addButton.IsEnabled = obj != null;
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var editableString = button.DataContext as EditableString;
        if (editableString == null)
        {
            throw new InvalidOperationException("DataContext为空");
        }

        var list = GetItemsSourceList();
        list.Remove(editableString);
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        //获取到当前TextBox的DataContext
        var textBox = sender as TextBox;
        if (textBox?.DataContext is not EditableString editableString)
        {
            throw new InvalidOperationException("DataContext为空");
        }

        int index = GetItemsSourceList().IndexOf(editableString);
        if (index == -1)
        {
            throw new InvalidOperationException("DataContext不在ItemsSource中");
        }

        //右侧插入一个空项
        var newText = new EditableString("");
        GetItemsSourceList().Insert(index + 1, newText);

        //如果光标在中间，则将光标前的字符串和光标后的字符串拆开
        var text = textBox.Text ?? "";
        var caretIndex = textBox.CaretIndex;
        if (caretIndex >= 0 && caretIndex < text.Length)
        {
            var text1 = text[..caretIndex];
            var text2 = text[caretIndex..];
            editableString.Value = text1;
            newText.Value = text2;
        }

        FocusTextBox(index + 1);
    }
}