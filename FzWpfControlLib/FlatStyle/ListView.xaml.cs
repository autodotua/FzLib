using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FzLib.Control.FlatStyle
{
    /// <summary>
    /// ListView.xaml 的交互逻辑
    /// </summary>
    public partial class ListView : System.Windows.Controls.ListView
    {
        public ObservableCollection<ExpandoObject> itemsBinding;
        public List<IDictionary<string, object>> ItemsDictionary => itemsBinding.Cast<IDictionary<string, object>>().ToList();
        public GridView grid;
        public ListView()
        {
            InitializeComponent();
            if (!updatedColor )
            {
                UpdateColor(new SolidColorBrush(Colors.White));
            }
        }
        private List<string> columnHeaders;
        public void Load(params string[] columnHeaders)
        {
            Load((IList<string>)columnHeaders);
        }
        /// <summary>
        /// 加载列。当调用此方法使，将切换数据绑定为自动。
        /// </summary>
        /// <param name="columnHeaders">列名的集合</param>
        public void Load(IList<string> columnHeaders)
        {
            grid = new GridView();
            foreach (var i in columnHeaders)
            {
                AddColumn(i);
            }
            View = grid;
            itemsBinding = new ObservableCollection<ExpandoObject>();
            ItemsSource = itemsBinding;
            this.columnHeaders = new List<string>(columnHeaders);

        }
        /// <summary>
        /// 增加列
        /// </summary>
        /// <param name="name"></param>
        private void AddColumn(string name)
        {
            grid.Columns.Add(
                new GridViewColumn()
                {
                    DisplayMemberBinding = new Binding(name),
                    Header = name,
                }
                );
        }
        public int AddRow(params string[] texts)
        {
            return AddRow(texts, null);
        }
        /// <summary>
        /// 增加行
        /// </summary>
        /// <param name="texts">每一列的显示的文字</param>
        /// <param name="tag">标签，推荐为</param>
        /// <returns>索引</returns>
        public int AddRow(IList<string> texts, object tag = null)
        {
            if (texts.Count != columnHeaders.Count)
            {
                throw new Exception("新增项的列数不等于总列数");
            }
            var item = new ExpandoObject() as IDictionary<string, object>;

            for (int i = 0; i < columnHeaders.Count; i++)
            {
                item.Add(columnHeaders[i], texts[i]);
            }
            item.Add("tag", tag);
            itemsBinding.Add(item as ExpandoObject);
            return itemsBinding.Count;
        }
        /// <summary>
        /// 通过Tag标签来获取匹配的行
        /// </summary>
        /// <param name="tag">标签</param>
        /// <returns></returns>
        public List<IDictionary<string, string>> GetRowsDicByTag(object tag)
        {
            List<IDictionary<string, string>> items = new List<IDictionary<string, string>>();
            foreach (var i in itemsBinding)
            {
                try
                {
                    var dic = i as IDictionary<string, object>;
                    if (dic["tag"].Equals(tag))
                    {
                        items.Add(dic.ToDictionary(x => x.Key, x => x.Value as string));
                    }
                }
                catch { }
            }
            return items;
        }
        public object GetRowTagByIndex(int index)
        {
            return (itemsBinding[index] as IDictionary<string, object>)["tag"];
        }
        public IDictionary<string, object> GetRowDicByIndex(int index)
        {
            return (itemsBinding[index] as IDictionary<string, object>);

        }
        public List<int> GetRowsIndexByTag(object tag)
        {
            List<int> indexs = new List<int>();
            int index = -1;
            foreach (var i in itemsBinding)
            {
                index++;
                try
                {
                    var dic = i as IDictionary<string, object>;
                    if (dic["tag"].Equals(tag))
                    {
                        indexs.Add(index);
                    }
                }
                catch { }
            }
            return indexs;
        }
        /// <summary>
        /// 通过条件搜索来获取匹配的行
        /// </summary>
        /// <param name="conditions">条件字典</param>
        /// <returns>搜索出来的行的列名、显示的文字字典的列表</returns>
        public List<IDictionary<string, string>> GetRowsDicByConditions(IDictionary<string, string> conditions)
        {
            List<IDictionary<string, string>> items = new List<IDictionary<string, string>>();
            foreach (var i in itemsBinding)
            {
                var dic = i as IDictionary<string, object>;
                foreach (var condition in conditions)
                {
                    try
                    {
                        if (!dic[condition.Key].Equals(condition.Value))
                        {
                            continue;
                        }

                        items.Add(dic.ToDictionary(x => x.Key, x => x.Value as string));
                    }
                    catch { }
                }
            }
            return items;
        }
        /// <summary>
        /// 通过索引删除行
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveRowsByIndex(int index)
        {
            if (itemsBinding.Count <= index)
            {
                return false;
            }
            try
            {
                itemsBinding.RemoveAt(index);
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 通过tag标签删除匹配的行
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>删除的行数</returns>
        public int RemoveRowsByTag(object tag)
        {
            List<ExpandoObject> items = new List<ExpandoObject>();
            foreach (var i in itemsBinding)
            {
                try
                {
                    var dic = i as IDictionary<string, object>;
                    if (dic["tag"].Equals(tag))
                    {
                        items.Add(i);
                    }
                }
                catch { }
            }

            foreach (var i in items)
            {
                itemsBinding.Remove(i);
            }
            return items.Count;
        }
        /// <summary>
        /// 通过搜索条件删除匹配的行
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns>删除的行数</returns>
        public int RemoveRowsByConditions(IDictionary<string, string> conditions)
        {
            List<ExpandoObject> items = new List<ExpandoObject>();
            foreach (var i in itemsBinding)
            {
                foreach (var condition in conditions)
                {
                    try
                    {
                        var dic = i as IDictionary<string, object>;
                        if (!dic[condition.Key].Equals(condition.Value))
                        {
                            continue;
                        }

                        items.Add(i);
                    }
                    catch { }
                }
            }

            foreach (var i in items)
            {
                itemsBinding.Remove(i);
            }
            return items.Count;
        }
        public void ClearRows()
        {
            itemsBinding.Clear();
        }

        public new SolidColorBrush Background
        {
            get => Resources["back"] as SolidColorBrush;
            set => UpdateColor(value);
        }
        private bool updatedColor = false;
        private void UpdateColor(SolidColorBrush value)
        {
            Resources["back"] = value;
            DarkerBrushConverter.GetDarkerColor(value, out SolidColorBrush darker1, out SolidColorBrush darker2, out SolidColorBrush darker3, out SolidColorBrush darker4);
            Resources["darker1"] = darker1;
            Resources["darker2"] = darker2;
            Resources["header1"] = darker1;
            Resources["header2"] = darker2;

            Resources["darker3"] = darker3;
            Resources["darker4"] = darker4;

            updatedColor = true;
        }
        private static readonly DependencyProperty EnableTriggersProperty =
  DependencyProperty.Register("EnableTriggers",
      typeof(bool),
      typeof(ListView),
      new PropertyMetadata(true));
        
        public void CloseTriggers()
        {
                    DarkerBrushConverter.GetDarkerColor(Background, out SolidColorBrush darker1, out SolidColorBrush darker2, out SolidColorBrush darker3, out SolidColorBrush darker4);
                    Resources["header1"] = darker1;
                    Resources["header2"] = darker2;

                    Resources["back"] = Background;
                    Resources["darker1"] = Background;
                    Resources["darker2"] = Background;
                    Resources["darker3"] = Background;
                    Resources["darker4"] = Background;

                
            
        }




        public event KeyEventHandler ItemPreviewKeyDown;
        public event KeyEventHandler ItemPreviewDeleteKeyDown;

        public event MouseButtonEventHandler ItemPreviewMouseLeftButtonDoubleClick;
        public event MouseButtonEventHandler ItemPreviewMouseRightButtonDoubleClick;
        public event MouseButtonEventHandler ItemPreviewMouseDoubleClick;

        public event MouseButtonEventHandler ItemPreviewMouseUp;
        public event MouseButtonEventHandler ItemPreviewMouseLeftButtonUp;
        public event MouseButtonEventHandler ItemPreviewMouseRightButtonUp;
        public event MouseButtonEventHandler ItemPreviewMouseDown;
        public event MouseButtonEventHandler ItemPreviewMouseLeftButtonDown;
        public event MouseButtonEventHandler ItemPreviewMouseRightButtonDown;

        private void LvwItemPreviewMouseDoubleClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            ItemPreviewMouseDoubleClick?.Invoke(sender, e);
            if (e.ChangedButton == MouseButton.Left)
            {
                ItemPreviewMouseLeftButtonDoubleClick?.Invoke(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ItemPreviewMouseRightButtonDoubleClick?.Invoke(sender, e);
            }
        }

        private void ListViewItem_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ItemPreviewMouseUp?.Invoke(sender, e);
            if (e.ChangedButton == MouseButton.Left)
            {
                ItemPreviewMouseLeftButtonUp?.Invoke(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ItemPreviewMouseRightButtonUp?.Invoke(sender, e);
            }
        }

        private void ListViewItemMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            ItemPreviewMouseDown?.Invoke(sender, e);
            if (e.ChangedButton == MouseButton.Left)
            {
                ItemPreviewMouseLeftButtonDown?.Invoke(sender, e);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ItemPreviewMouseRightButtonDown?.Invoke(sender, e);
            }
        }

        private void LvwItemPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            ItemPreviewKeyDown?.Invoke(sender, e);
            if(e.Key==Key.Delete)
            {
                ItemPreviewDeleteKeyDown?.Invoke(sender, e);
            }
        }




        //private bool allowDragDrop;


        //public void InitializeDragDrop<T>(ObservableCollection<T> collection)
        //{
        //    this.collection = collection;
        //    allowDragDrop = true;
        //    AllowDrop = true;
        //    MouseMove += l_MouseMove;
        //    Drop += l_Drop;
        //}
        //ObservableCollection<T> collection;

        //private void l_MouseMove(object sender, MouseEventArgs e)
        //{
        //    ListView listview = sender as ListView;
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        System.Collections.IList list = listview.SelectedItems as System.Collections.IList;
        //        DataObject data = new DataObject(typeof(System.Collections.IList), list);
        //        if (list.Count > 0)
        //        {
        //            DragDrop.DoDragDrop(listview, data, DragDropEffects.Move);
        //        }
        //    }
        //}

        //private void l_Drop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(typeof(System.Collections.IList)))
        //    {
        //        System.Collections.IList peopleList = e.Data.GetData(typeof(System.Collections.IList)) as System.Collections.IList;
        //        //index为放置时鼠标下元素项的索引  
        //        int index = GetCurrentIndex(new GetPositionDelegate(e.GetPosition));
        //        if (index > -1)
        //        {
        //            object Logmess = peopleList[0] as object;
        //            //拖动元素集合的第一个元素索引  
        //            int OldFirstIndex = collection.IndexOf(collection.First(p => p.Equals(Logmess)));
        //            //下边那个循环要求数据源必须为ObservableCollection<T>类型，T为对象  
        //            for (int i = 0; i < peopleList.Count; i++)
        //            {
        //                collection.Move(OldFirstIndex, index);
        //            }
        //            SelectedItems.Clear();
        //        }
        //    }
        //}

        //private int GetCurrentIndex(GetPositionDelegate getPosition)
        //{
        //    int index = -1;
        //    for (int i = 0; i < Items.Count; ++i)
        //    {
        //        ListViewItem item = GetListViewItem(i);
        //        if (item != null && this.IsMouseOverTarget(item, getPosition))
        //        {
        //            index = i;
        //            break;
        //        }
        //    }
        //    return index;
        //}

        //private bool IsMouseOverTarget(Visual target, GetPositionDelegate getPosition)
        //{
        //    Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
        //    Point mousePos = getPosition((IInputElement)target);
        //    return bounds.Contains(mousePos);
        //}

        //delegate Point GetPositionDelegate(IInputElement element);

        //ListViewItem GetListViewItem(int index)
        //{
        //    if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        //        return null;
        //    return ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        //}
    }
}
