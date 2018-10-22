using System;
using System.Collections.Generic;
using System.Linq;
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

namespace FzLib.Control.Picker
{
    /// <summary>
    /// DateRangePicker.xaml 的交互逻辑
    /// </summary>
    public partial class DateRangePicker : UserControl
    {
        public DateRangePicker()
        {
            InitializeComponent();
        }

        public DateTime? DateFrom
        {
            get => dateFrom.SelectedDate;
            set => dateFrom.SelectedDate = value;

        }

        public DateTime? DateTo
        {
            get => dateTo.SelectedDate;
            set => dateTo.SelectedDate = value;
        }

        private void dateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateSelectionChanged?.Invoke(sender, e);
            if (!IsAvailable)
            {
                return;
            }
            if (DateFrom.Value > DateTo.Value)
            {
                if ((sender as DatePicker).Name == "dateFrom")
                {
                    DateFrom = DateTo;
                }
                else
                {
                    DateTo = DateFrom;
                }
            }
            DateSelectionAvailableAndChanged?.Invoke(sender, e);
        }

        public bool IsAvailable => DateFrom.HasValue && DateTo.HasValue;
        public event SelectionChangedEventHandler DateSelectionChanged;

        public event SelectionChangedEventHandler DateSelectionAvailableAndChanged;
    }
}
