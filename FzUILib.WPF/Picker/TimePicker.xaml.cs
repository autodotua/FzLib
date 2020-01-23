using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

namespace FzLib.UI.Picker
{
    /// <summary>
    /// TimePicker.xaml 的交互逻辑
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
            p.PlacementTarget = this;
            p.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            LoadTimeList(Enumerable.Range(0, 24), Enumerable.Range(0, 60), Enumerable.Range(0, 60));

        }

        private const int MaxHours = 10000;

        private void AddTimeToPopup()
        {
            lvwHour.Items.Clear();
            lvwMin.Items.Clear();
            lvwSec.Items.Clear();
            if (HourList != null)
            {
                foreach (var i in HourList)
                {
                    lvwHour.Items.Add(i.ToString("00"));
                }
            }
            if (MinList != null)
            {
                foreach (var i in MinList)
                {
                    lvwMin.Items.Add(i.ToString("00"));
                }
            }
            if (SecList != null)
            {
                foreach (var i in SecList)
                {
                    lvwSec.Items.Add(i.ToString("00"));
                }
            }
        }


        public void LoadTimeList(IEnumerable<int> hours, IEnumerable<int> mins, IEnumerable<int> secs)
        {
            if (hours == null && mins == null && secs == null)
            {
                throw new ArgumentNullException();
            }
            int max = (limitMode == LimitModes.TwentyFourHour ? 23 : (limitMode == LimitModes.TwelveHour ? 11 : MaxHours));

            if (hours != null)
            {
                if (hours.Any(p => (p < 0 && p > max)))
                {
                    throw new Exception("超出范围");
                }
                HourList = hours.Distinct().OrderBy(p => p);
            }

            max = 59;

            if (mins != null)
            {
                if (mins.Any(p => (p < 0 && p > max)))
                {
                    throw new Exception("超出范围");
                }
                MinList = mins.Distinct().OrderBy(p => p);
            }
            if (secs != null)
            {
                if (secs.Any(p => (p < 0 && p > max)))
                {
                    throw new Exception("超出范围");
                }
                SecList = secs.Distinct().OrderBy(p => p);
            }

            AddTimeToPopup();

        }

        public bool ShowMenuButton
        {
            get => btn.Visibility == Visibility.Visible;
            set => btn.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        private LimitModes limitMode = LimitModes.TwentyFourHour;

        public bool ReadOnly
        {
            set => Resources["readOnly"] = value;
        }

        public LimitModes LimitMode
        {
            get => limitMode;
            set
            {
                txtHour.MaxLength = value == LimitModes.TimeSpan ? 6 : 2;

                if (value == LimitModes.TimeSpan)
                {

                }

                limitMode = value;
            }
        }

        public IEnumerable<int> HourList { get; set; }
        public IEnumerable<int> MinList { get; set; }
        public IEnumerable<int> SecList { get; set; }

        private int MaxHour => LimitMode == LimitModes.TwentyFourHour ? 23 : 11;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            int max = (txt.Name == "txtHour") ? MaxHour : 59;
            if (Check(txt.Text))
            {
                txt.Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                txt.Background = new SolidColorBrush(Color.FromArgb(64, 255, 0, 0));
            }

            bool Check(string text)
            {
                if (!int.TryParse(text, out int num))
                {
                    return false;
                }
                if (num < 0)
                {
                    return false;
                }
                if (num > max)
                {
                    return false;
                }
                return true;
            }
        }
        public enum LimitModes
        {
            TwentyFourHour,
            TwelveHour,
            TimeSpan,
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            if (key <= 43 || key >= 74 && key <= 83)
            {
                return;
            }
            e.Handled = true;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (p.IsOpen)
            {
                p.IsOpen = false;
                return;
            }
            lvwHour.SelectedItem = txtHour.Text;
            lvwMin.SelectedItem = txtMin.Text;
            lvwSec.SelectedItem = txtSec.Text;
            p.IsOpen = true;
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (!p.IsOpen)
            //{
            //    return;
            //}
            ListView lvw = sender as ListView;
            string text = lvw.SelectedItem as string;
            switch (lvw.Name)
            {
                case "lvwHour":
                    txtHour.Text = text;
                    break;
                case "lvwMin":
                    txtMin.Text = text;
                    break;
                case "lvwSec":
                    txtSec.Text = text;
                    break;

            }
        }

        public DateTime? Time
        {
            get
            {
                try
                {
                    int? hour = Hour;
                    int? minute = Minute;
                    int? second = Second;
                    if(!(hour.HasValue && minute.HasValue && second.HasValue))
                    {
                        return null;
                    }
                    DateTime time = new DateTime(1, 1, 1, hour.Value,minute.Value,second.Value);
                    return time;
                }
                catch
                {
                    return null;
                }

            }
            set
            {
                if (!value.HasValue)
                {
                    throw new NullReferenceException();
                }

                Hour = value.Value.Hour;
                Minute = value.Value.Minute;
                Second = value.Value.Second;

            }
        }

        public TimeSpan? TimeSpan
        {
            get
            {
                try
                {
                    int? hour = Hour;
                    int? minute = Minute;
                    int? second = Second;
                    if (!(hour.HasValue && minute.HasValue && second.HasValue))
                    {
                        return null;
                    }
                    TimeSpan time = new TimeSpan(hour.Value, minute.Value, second.Value);
                    return time;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if(!value.HasValue)
                {
                    throw new NullReferenceException();
                }

                Hour = value.Value.Hours;
                Minute = value.Value.Minutes;
                Second = value.Value.Seconds;

            }
        }

        public int? Hour
        {
            get
            {
                   if(! int.TryParse(txtHour.Text,out int hour))
                    {
                        return null;
                    }
                    if(hour<0 || hour>MaxHour)
                    {
                        return null;
                    }
                    return hour;
            }
            set
            {
                if (!value.HasValue)
                {
                    throw new NullReferenceException();
                }
                if (value<0 || value>MaxHour)
                {
                    throw new IndexOutOfRangeException();
                }
                txtHour.Text = value.Value.ToString("00");
            }
        }
        public int? Minute
        {
            get
            {
                if (!int.TryParse(txtMin.Text, out int minute))
                {
                    return null;
                }
                if (minute < 0 || minute > 59)
                {
                    return null;
                }
                return minute;
            }
            set
            {
                if (!value.HasValue)
                {
                    throw new NullReferenceException();
                }
                if (value < 0 || value > 59)
                {
                    throw new IndexOutOfRangeException();
                }
                txtMin.Text = value.Value.ToString("00");
            }
        }
        public int? Second
        {
            get
            {
                if (!int.TryParse(txtSec.Text, out int second))
                {
                    return null;
                }
                if (second < 0 || second > MaxHour)
                {
                    return null;
                }
                return second;
            }
            set
            {
                if(!value.HasValue)
                {
                    throw new NullReferenceException();
                }
                if (value < 0 || value > 59)
                {
                    throw new IndexOutOfRangeException();
                }
                txtSec.Text = value.Value.ToString("00");
            }
        }

        public bool ShowSecond
        {
            get => txtSec.Visibility == Visibility.Visible;
            set=> txtSec.Visibility = tbkColon2.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            
        }

    }
    public class ToHalfMarginConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(-(double)value , 0,- (double)value , 0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
