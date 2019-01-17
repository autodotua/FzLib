using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace FzLib.Control.Extension
{
    public class ListViewHelper<T>
    {
        public ListView ListView { get; private set; }
        public ListViewHelper(ListView listView)
        {
            ListView = listView;

        }
        public void EnableDragAndDropItem()
        {
            ListView.AllowDrop = true;
            ListView.MouseMove += SingleMouseMove;
            ListView.Drop += SingleDrop;
        }
        public void EnableDragAndDropItems()
        {
            ListView.AllowDrop = true;
            ListView.MouseMove += MultiMouseMove;
            ListView.Drop += MultiDrop;
        }
        public void DisableDragAndDropItems()
        {
            ListView.AllowDrop = true;
            ListView.MouseMove -= MultiMouseMove;
            ListView.Drop -= MultiDrop;
        }

        private void SingleMouseMove(object sender, MouseEventArgs e)
        {
            ListView listview = sender as ListView;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                T list = (T)listview.SelectedItem;
                DataObject data = new DataObject(typeof(T), list);

                DragDrop.DoDragDrop(listview, data, DragDropEffects.Move);
            }
        }

        private void SingleDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(T)))
            {
                T item = (T)e.Data.GetData(typeof(T));
                //index为放置时鼠标下元素项的索引  
                int index = GetCurrentIndex(new GetPositionDelegate(e.GetPosition));
                if (index > -1  )
                {
                    //拖动元素集合的第一个元素索引  
                    int oldIndex = (ListView.ItemsSource as ObservableCollection<T>).IndexOf(item);
                    if(oldIndex==index)
                    {
                        return;
                    }
                    //下边那个循环要求数据源必须为ObservableCollection<T>类型，T为对象  

                    (ListView.ItemsSource as ObservableCollection<T>).Move(oldIndex, index);
                    SingleItemDragDroped?.Invoke(this, new SingleItemDragDropedEventArgs(oldIndex, index));
                    // lvw.SelectedItems.Clear();
                    //ListView.SelectedIndex = index;
                }
            }
        }
        private void MultiMouseMove(object sender, MouseEventArgs e)
        {
            ListView listview = sender as ListView;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IList list = listview.SelectedItems as IList;
                DataObject data = new DataObject(typeof(IList), list);
                if (list.Count > 0)
                {
                    DragDrop.DoDragDrop(listview, data, DragDropEffects.Move);
                }
            }
        }

        private void MultiDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(IList)))
            {
                IList peopleList = e.Data.GetData(typeof(IList)) as IList;
                //index为放置时鼠标下元素项的索引  
                int index = GetCurrentIndex(new GetPositionDelegate(e.GetPosition));
                if (index > -1)
                {
                    T Logmess = (T)peopleList[0];
                    //拖动元素集合的第一个元素索引  
                    int OldFirstIndex = (ListView.ItemsSource as ObservableCollection<T>).IndexOf(Logmess);
                    //下边那个循环要求数据源必须为ObservableCollection<T>类型，T为对象  
                    for (int i = 0; i < peopleList.Count; i++)
                    {
                        (ListView.ItemsSource as ObservableCollection<T>).Move(OldFirstIndex, index);
                    }
                    ListView.SelectedItems.Clear();
                }
            }
        }

        private int GetCurrentIndex(GetPositionDelegate getPosition)
        {
            int index = -1;
            for (int i = 0; i < ListView.Items.Count; ++i)
            {
                ListViewItem item = GetListViewItem(i);
                if (item != null && this.IsMouseOverTarget(item, getPosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private bool IsMouseOverTarget(Visual target, GetPositionDelegate getPosition)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            System.Windows.Point mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }

        delegate Point GetPositionDelegate(IInputElement element);

        ListViewItem GetListViewItem(int index)
        {
            if (ListView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;
            return ListView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        public delegate void SingleItemDragDropedEventHandler(object sender, SingleItemDragDropedEventArgs e);
        public event SingleItemDragDropedEventHandler SingleItemDragDroped;

        public class SingleItemDragDropedEventArgs : EventArgs
        {
            public SingleItemDragDropedEventArgs(int oldIndex, int newIndex)
            {
                OldIndex = oldIndex;
                NewIndex = newIndex;
            }

            public int OldIndex { get; private set; }
            public int NewIndex { get; private set; }
        }
    }
}
