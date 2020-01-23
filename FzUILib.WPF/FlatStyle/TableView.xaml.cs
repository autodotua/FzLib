using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FzLib.UI.FlatStyle
{
    /// <summary>
    /// TableView.xaml 的交互逻辑
    /// </summary>
    public partial class TableView : DataGrid
    {
        public TableView()
        {
            InitializeComponent();


        }
        ObservableCollection<ExpandoObject> items;

        Dictionary<object, IDictionary<string, object>> tagsBinding;

        public void Load(IEnumerable<string> columns)
        {
            Load();
            foreach (var column in columns)
            {
                AddTextColumn(column);
            }
        }
        public void Load()
        {
            items = new ObservableCollection<ExpandoObject>();
            tagsBinding = new Dictionary<object, IDictionary<string, object>>();
            ItemsSource = items;

        }

        public void AddTextColumn(string columnName, DataGridLength? width = null)
        {
            if (Columns.Any(p => p.Header.Equals(columnName)))
            {
                throw new Exception("重复的列名");
            }

            Columns.Add(new DataGridTextColumn() { Header = columnName, Width = width.HasValue?width.Value:DataGridLength.Auto, Binding = new Binding(columnName )});
        }

        public void AddCheckBoxColumn(string columnName, DataGridLength? width = null)
        {
            if (Columns.Any(p => p.Header.Equals(columnName)))
            {
                throw new Exception("重复的列名");
            }

            Columns.Add(new DataGridCheckBoxColumn() { Header = columnName, Width = width.HasValue ? width.Value : DataGridLength.Auto, Binding = new Binding(columnName) });
        }
     
        public void AddHyperlinkColumn(string columnName, DataGridLength? width = null)
        {
            if (Columns.Any(p => p.Header.Equals(columnName)))
            {
                throw new Exception("重复的列名");
            }

            Columns.Add(new DataGridHyperlinkColumn() { Header = columnName, Width = width.HasValue ? width.Value : DataGridLength.Auto, Binding = new Binding(columnName) });
        }


        public void AddRow<T>(IDictionary<string, T> columnValuePairs,object tag=null)
        {
            var item = new ExpandoObject() as IDictionary<string, object>;
            foreach (var column in columnValuePairs)
            {
                item.Add(column.Key, column.Value);
            }
            items.Add(item as ExpandoObject);
           if(tag!=null)
            {
                tagsBinding.Add(tag, item);
            }
        }

        public void SetCellValue<T>(object rowTag,string columnHeader,T value)
        {
            tagsBinding[rowTag][columnHeader] = value;
        }

        public string[] GetRows(string columnHeader)
        {
            List<string> row = new List<string>();
            foreach (var item in items)
            {
                row.Add((item as IDictionary<string, object>)[columnHeader] as string);
            }
            return row.ToArray();
        }






        public event ExecutedRoutedEventHandler ExecutePasteEvent;
        public event CanExecuteRoutedEventHandler CanExecutePasteEvent;

        // ******************************************************************
        static TableView()
        {
            CommandManager.RegisterClassCommandBinding(
                typeof(TableView),
                new CommandBinding(ApplicationCommands.Paste,
                    new ExecutedRoutedEventHandler(OnExecutedPasteInternal),
                    new CanExecuteRoutedEventHandler(OnCanExecutePasteInternal)));
        }

        // ******************************************************************
        #region Clipboard Paste

        // ******************************************************************
        private static void OnCanExecutePasteInternal(object target, CanExecuteRoutedEventArgs args)
        {
            ((TableView)target).OnCanExecutePaste(target, args);
        }

        // ******************************************************************
        /// <summary>
        /// This virtual method is called when ApplicationCommands.Paste command query its state.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCanExecutePaste(object target, CanExecuteRoutedEventArgs args)
        {
            if (CanExecutePasteEvent != null)
            {
                CanExecutePasteEvent(target, args);
                if (args.Handled)
                {
                    return;
                }
            }

            args.CanExecute = CurrentCell != null;
            args.Handled = true;
        }

        // ******************************************************************
        private static void OnExecutedPasteInternal(object target, ExecutedRoutedEventArgs args)
        {
            ((TableView)target).OnExecutedPaste(target, args);
        }

        // ******************************************************************
        /// <summary>
        /// This virtual method is called when ApplicationCommands.Paste command is executed.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        protected virtual void OnExecutedPaste(object target, ExecutedRoutedEventArgs args)
        {
            if (ExecutePasteEvent != null)
            {
                ExecutePasteEvent(target, args);
                if (args.Handled)
                {
                    return;
                }
            }

            // parse the clipboard data            [row][column]
            List<string[]> clipboardData = ClipboardHelper.ParseClipboardData();

            bool hasAddedNewRow = false;
            
#if DEBUG
            StringBuilder sb = new StringBuilder();
#endif 
            int minRowIndex = Items.IndexOf(CurrentItem);
            int maxRowIndex = Items.Count - 1;
            int startIndexOfDisplayCol = (SelectionUnit != DataGridSelectionUnit.FullRow) ? CurrentColumn.DisplayIndex : 0;
            int clipboardRowIndex = 0;
            for (int i = minRowIndex; i <= maxRowIndex && clipboardRowIndex < clipboardData.Count; i++, clipboardRowIndex++)
            {
                if (i < this.Items.Count)
                {
                    CurrentItem = Items[i];

                    BeginEditCommand.Execute(null, this);

                    int clipboardColumnIndex = 0;
                    for (int j = startIndexOfDisplayCol; clipboardColumnIndex < clipboardData[clipboardRowIndex].Length; j++, clipboardColumnIndex++)
                    {
                        // DataGridColumn column = ColumnFromDisplayIndex(j);
                        DataGridColumn column = null;
                        foreach (DataGridColumn columnIter in this.Columns)
                        {
                            if (columnIter.DisplayIndex == j)
                            {
                                column = columnIter;
                                break;
                            }
                        }

                        column?.OnPastingCellClipboardContent(Items[i], clipboardData[clipboardRowIndex][clipboardColumnIndex]);

#if DEBUG
                        sb.AppendFormat("{0,-10}", clipboardData[clipboardRowIndex][clipboardColumnIndex]);
                        sb.Append(" - ");
#endif
                    }

                    CommitEditCommand.Execute(this, this);
                    if (i == maxRowIndex)
                    {
                        maxRowIndex++;
                        hasAddedNewRow = true;
                    }
                }
                
#if DEBUG
                sb.Clear();
#endif
            }

            // update selection
            if (hasAddedNewRow)
            {
                UnselectAll();
                UnselectAllCells();

                CurrentItem = Items[minRowIndex];

                if (SelectionUnit == DataGridSelectionUnit.FullRow)
                {
                    SelectedItem = Items[minRowIndex];
                }
                else if (SelectionUnit == DataGridSelectionUnit.CellOrRowHeader ||
                         SelectionUnit == DataGridSelectionUnit.Cell)
                {
                    SelectedCells.Add(new DataGridCellInfo(Items[minRowIndex], Columns[startIndexOfDisplayCol]));

                }
            }
        }

        // ******************************************************************
        /// <summary>
        ///     Whether the end-user can add new rows to the ItemsSource.
        /// </summary>
        public bool CanUserPasteToNewRows
        {
            get { return (bool)GetValue(CanUserPasteToNewRowsProperty); }
            set { SetValue(CanUserPasteToNewRowsProperty, value); }
        }

        // ******************************************************************
        /// <summary>
        ///     DependencyProperty for CanUserAddRows.
        /// </summary>
        public static readonly DependencyProperty CanUserPasteToNewRowsProperty =
            DependencyProperty.Register("CanUserPasteToNewRows",
                                        typeof(bool), typeof(TableView),
                                        new FrameworkPropertyMetadata(true, null, null));

        // ******************************************************************
        #endregion Clipboard Paste

        private void SetGridToSupportManyEditEitherWhenValidationErrorExists()
        {
            this.Items.CurrentChanged += Items_CurrentChanged;


            //Type DatagridType = this.GetType().BaseType;
            //PropertyInfo HasCellValidationProperty = DatagridType.GetProperty("HasCellValidationError", BindingFlags.NonPublic | BindingFlags.Instance);
            //HasCellValidationProperty.
        }

        void Items_CurrentChanged(object sender, EventArgs e)
        {
            //this.Items[0].
            //throw new NotImplementedException();
        }

        // ******************************************************************
        private void SetGridWritable()
        {
            Type DatagridType = this.GetType().BaseType;
            PropertyInfo HasCellValidationProperty = DatagridType.GetProperty("HasCellValidationError", BindingFlags.NonPublic | BindingFlags.Instance);
            if (HasCellValidationProperty != null)
            {
                HasCellValidationProperty.SetValue(this, false, null);
            }
        }

        // ******************************************************************
        public void SetGridWritableEx()
        {
            BindingFlags bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo cellErrorInfo = this.GetType().BaseType.GetProperty("HasCellValidationError", bindingFlags);
            PropertyInfo rowErrorInfo = this.GetType().BaseType.GetProperty("HasRowValidationError", bindingFlags);
            cellErrorInfo.SetValue(this, false, null);
            rowErrorInfo.SetValue(this, false, null);
        }

    }


    public static class ClipboardHelper
    {
        public delegate string[] ParseFormat(string value);

        public static List<string[]> ParseClipboardData()
        {
            List<string[]> clipboardData = null;
            object clipboardRawData = null;
            ParseFormat parseFormat = null;

            // get the data and set the parsing method based on the format
            // currently works with CSV and Text DataFormats            
            IDataObject dataObj = System.Windows.Clipboard.GetDataObject();
            if ((clipboardRawData = dataObj.GetData(DataFormats.CommaSeparatedValue)) != null)
            {
                parseFormat = ParseCsvFormat;
            }
            else if ((clipboardRawData = dataObj.GetData(DataFormats.Text)) != null)
            {
                parseFormat = ParseTextFormat;
            }

            if (parseFormat != null)
            {
                string rawDataStr = clipboardRawData as string;

                if (rawDataStr == null && clipboardRawData is MemoryStream)
                {
                    // cannot convert to a string so try a MemoryStream
                    MemoryStream ms = clipboardRawData as MemoryStream;
                    StreamReader sr = new StreamReader(ms);
                    rawDataStr = sr.ReadToEnd();
                }
                Debug.Assert(rawDataStr != null, string.Format("clipboardRawData: {0}, could not be converted to a string or memorystream.", clipboardRawData));

                string[] rows = rawDataStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (rows != null && rows.Length > 0)
                {
                    clipboardData = new List<string[]>();
                    foreach (string row in rows)
                    {
                        clipboardData.Add(parseFormat(row));
                    }
                }
                else
                {
                    Debug.WriteLine("unable to parse row data.  possibly null or contains zero rows.");
                }
            }

            return clipboardData;
        }

        public static string[] ParseCsvFormat(string value)
        {
            return ParseCsvOrTextFormat(value, true);
        }

        public static string[] ParseTextFormat(string value)
        {
            return ParseCsvOrTextFormat(value, false);
        }

        private static string[] ParseCsvOrTextFormat(string value, bool isCSV)
        {
            List<string> outputList = new List<string>();

            char separator = isCSV ? ',' : '\t';
            int startIndex = 0;
            int endIndex = 0;

            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (ch == separator)
                {
                    outputList.Add(value.Substring(startIndex, endIndex - startIndex));

                    startIndex = endIndex + 1;
                    endIndex = startIndex;
                }
                else if (ch == '\"' && isCSV)
                {
                    // skip until the ending quotes
                    i++;
                    if (i >= value.Length)
                    {
                        throw new FormatException(string.Format("value: {0} had a format exception", value));
                    }
                    char tempCh = value[i];
                    while (tempCh != '\"' && i < value.Length)
                        i++;

                    endIndex = i;
                }
                else if (i + 1 == value.Length)
                {
                    // add the last value
                    outputList.Add(value.Substring(startIndex));
                    break;
                }
                else
                {
                    endIndex++;
                }
            }

            return outputList.ToArray();
        }
    }

}
