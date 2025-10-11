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

namespace FzLib.Avalonia.Controls;

public partial class StringListEditor : TemplatedControl
{
    public static readonly StyledProperty<ObservableStringList> ItemsSourceProperty =
        AvaloniaProperty.Register<StringListEditor, ObservableStringList>(
            nameof(ItemsSource));

    private Button addButton;
    
    private ItemsControl items;
    
    private ScrollViewer scr;
    public ObservableStringList ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
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
        var list = sender as ObservableStringList;
        if (list.Count == 0)
        {
            addButton.Margin = new Thickness();
        }
        else
        {
            addButton.Margin = new Thickness(8, 0, 0, 6);
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