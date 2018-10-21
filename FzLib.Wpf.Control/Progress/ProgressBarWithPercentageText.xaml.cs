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

namespace FzLib.Wpf.Control.Progress
{
    /// <summary>
    /// ProgressBarWithPercentageText.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBarWithPercentageText : ProgressBar
    {

        public ProgressBarWithPercentageText()
        {
            InitializeComponent();
            defautBarBrush = Foreground as SolidColorBrush;
            defautTextBrush = GetResource<SolidColorBrush>("TextColor");
            Style = Resources["style"] as Style;
            Loaded+=(p1,p2)=> ProgressBarValueChangedEventHandler(null, null); ;
        }

        public HorizontalAlignment TextHorizontalAlignment
        {
            get => GetResource<HorizontalAlignment>("textHorizon");
            set => SetResource("textHorizon", value);
        }

        private SolidColorBrush defautBarBrush;
        private SolidColorBrush defautTextBrush;

        private Dictionary<double, SolidColorBrush> barBrush;
        private Dictionary<double, SolidColorBrush> textBrush;

        private T GetResource<T>(string name)
        {
            object obj = Resources[name];
            if (!(obj is T))
            {
                throw new Exception($"资源{name}不是该类型");
            }
            return (T)obj;
        }

        private void SetResource<T>(string name, T value)
        {
            //if (Resources.conti(name))
            //{
                //if (Resources[name] is T)
                //{
                    this.Resources[name] = value;
                //}
                //throw new Exception($"资源{name}不是该类型");
            //}
            //throw new Exception($"不存在资源{name}");
        }

        private int numberOfDecimals=0;

        public int NumberOfDecimals
        {
            get => numberOfDecimals;
            set
            {
                if(value>5 || value<0)
                {
                    throw new Exception("位数应大于等于0且小于等于5");
                }
                numberOfDecimals = value;
            }
        }

        public string Text { get => text; set => text = value; }

        private void Check(IDictionary<double,SolidColorBrush> dic)
        {
            if(dic==null)
            {
                throw new ArgumentNullException();
            }
            if (dic.Count < 1)
            {
                throw new Exception("不足1项");
            }
            double oldScale = 0;
            foreach (var i in dic)
            {
                if (i.Value == null)
                {
                    throw new ArgumentNullException();
                }
                if (i.Key <= oldScale)
                {
                    throw new Exception("后值小于等于前值");
                }
                if (i.Key > Maximum)
                {
                    throw new Exception("值大于最大值");
                }
                if (i.Key <Minimum)
                {
                    throw new Exception("值小于最小值");
                }
                oldScale = i.Key;
            }
         

        }

        public void LoadBarBrush(IDictionary<double, SolidColorBrush> barBrush)
        {
            Check(barBrush);
            this.barBrush = barBrush.Reverse().ToDictionary(x => x.Key, v => v.Value);
            ProgressBarValueChangedEventHandler(null, null);
        }
        public void LoadTextBrush(IDictionary<double, SolidColorBrush> textBrush)
        {
            Check(textBrush);
            this.textBrush = textBrush.Reverse().ToDictionary(x => x.Key, v => v.Value);
            ProgressBarValueChangedEventHandler(null, null);
        }
        public void Load(IDictionary<double, SolidColorBrush> barBrush, IDictionary<double, SolidColorBrush> textBrush)
        {
            //Check(barBrush);
            //Check(textBrush);
            //this.barBrush = barBrush.Reverse().ToDictionary(x=>x.Key,v=>v.Value);
            //this.textBrush = textBrush.Reverse().ToDictionary(x => x.Key, v => v.Value);
            //ProgressBarValueChangedEventHandler(null, null);
            LoadBarBrush(barBrush);
            LoadTextBrush(textBrush);
        }

        public void Load(IList<double> scale, IList<SolidColorBrush> barBrush, IList<SolidColorBrush> textBrush)
        {
            if (scale.Count != barBrush.Count || barBrush.Count != textBrush.Count)
            {
                throw new Exception("列表数量不等");
            }
            Load(scale.Zip(barBrush, (k, v) => new { k, v }).ToDictionary(x => x.k, v => v.v),
                scale.Zip(textBrush, (k, v) => new { k, v }).ToDictionary(x => x.k, v => v.v));
        }

        private const string percentagePlaceholder = "%p";
        private const string maxPlaceholder = "%max";
        private const string minPlaceholder = "%min";
        private const string valuePlaceholder = "%v";

        private string text = "%p";

        private void ProgressBarValueChangedEventHandler(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string percentage = (100 * (Value - Minimum) / (Maximum - Minimum)).ToString("0." + new string('0', NumberOfDecimals)) + "%";
            SetResource("Text", Text.Replace(percentagePlaceholder,percentage)
                .Replace(maxPlaceholder,Maximum.ToString())
                .Replace(minPlaceholder,Minimum.ToString()
                .Replace(valuePlaceholder,Value.ToString())));
            if (barBrush != null && barBrush.Count > 0)
            {
                bool yes = true;
                foreach (var i in barBrush)
                {

                    if (i.Key <= Value)
                    {
                        if (Foreground != i.Value)
                        {
                            Foreground = i.Value;
                        }
                        yes = false;
                        break;
                    }
                }
                if (yes)
                {
                    Foreground = defautBarBrush;
                }
            }

            if (textBrush != null && textBrush.Count > 0)
            {
                bool yes = true;
                foreach (var i in textBrush)
                {


                    if (i.Key <= Value)
                    {
                        if (!GetResource<SolidColorBrush>("TextColor").Equals(i.Value))
                        {
                            SetResource("TextColor", i.Value);
                        }
                        yes = false;
                        break;
                    }
                }
                if (yes)
                {
                    SetResource("TextColor", defautTextBrush);
                }
            }
        }
    }
}
