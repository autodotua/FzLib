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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FzLib.Control.Text
{
    /// <summary>
    /// RradualChangedNumberBlock.xaml 的交互逻辑
    /// </summary>
    public partial class GradualChangedNumberBlock : UserControl
    {
        private static readonly string l = Environment.NewLine;

        private long number = 0;
        private TimeSpan animationDuration=TimeSpan.FromSeconds(1);
        private UIElementCollection scrollTextBlockList;
        public GradualChangedNumberBlock()
        {
            InitializeComponent();
            scrollTextBlockList = stkScroll.Children;
            JumpTo("0");
            storyboard.Completed += NumberStoryboardCompletedEventHandler;
            Loaded+=(p1,p2) => g.Height = FontSize * FontFamily.LineSpacing;
        }
        bool storyboarding = false;
        private void NumberStoryboardCompletedEventHandler(object sender, EventArgs e)
        {
            foreach (var tbk in scrollTextBlockList.Cast<TextBlock>())
            {
                tbk.Text = ((int)tbk.Tag).ToString();
                tbk.Margin = new Thickness(0);
            }

            int count = scrollTextBlockList.Count;
            if (lastNum.Length < count)
            {
                for (int i = 0; i < count - lastNum.Length; i++)
                {
                    scrollTextBlockList.RemoveAt(0);
                }
            }

            stkScroll.Margin = new Thickness(0);

            storyboarding = false;
        }
        public long Number
        {
            set => JumpTo(value.ToString());
            get => number;
        }
        private void AddNewNumberColumn(char ch)
        {
            if (ch < '0' || ch > '9')
            {
                throw new Exception("输入并非数字");
            }
            TextBlock tbk = new TextBlock()
            {
                Text = ch.ToString(),// $"0{l}1{l}2{l}3{l}4{l}5{l}6{l}7{l}8{l}9",

                RenderTransform = new TranslateTransform(),
                Tag = ch - '0',
                //  VerticalAlignment=VerticalAlignment.Stretch,
                //Height = FontSize * FontFamily.LineSpacing * 10,
            };

            //RegisterName(tbk.Name, tbk);
            scrollTextBlockList.Add(tbk);
        }
        private void AddNewNumberColumn(char ch, int position)
        {
            if (ch < '0' || ch > '9')
            {
                throw new Exception("输入并非数字");
            }
            TextBlock tbk = new TextBlock()
            {
                Text = ch.ToString(),// $"0{l}1{l}2{l}3{l}4{l}5{l}6{l}7{l}8{l}9",
                RenderTransform = new TranslateTransform(),
                Tag = ch - '0',
                //  VerticalAlignment=VerticalAlignment.Stretch,
                //Height = FontSize * FontFamily.LineSpacing * 10,
            };

            //RegisterName(tbk.Name, tbk);
            scrollTextBlockList.Insert(position, tbk);
        }
        Storyboard storyboard = new Storyboard() { FillBehavior = FillBehavior.Stop };

        public bool IsChanging => storyboarding;

        public Direction AnimationDirection { get => animationDirection; set => animationDirection = value; }
        public TimeSpan AnimationDuration { get => animationDuration; set => animationDuration = value; }

        private string lastNum = "";

        public void ChangeTo(string num)
        {
            if (IsChanging)
            {
                throw new Exception("正在改变数字时无法改变数字");
            }
            foreach (var i in num)
            {
                if (i < '0' || i > '9')
                {
                    throw new Exception("不是数字");
                }
            }
            lastNum = num;
            if (num == "")
            {
                scrollTextBlockList.Clear();
                return;
            }

            if(long.TryParse(num,out long result))
            {
                number = result;
            }
            else
            {
                number = -1;
            }


            storyboard.Children.Clear();
            int start = 0;
            if (num.Length > scrollTextBlockList.Count)
            {
                TextBlock tbk = new TextBlock() { Text = "0" };
                //tbk.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                //tbk.Arrange(new Rect(tbk.DesiredSize));
                double needWidth = (scrollTextBlockList.Count - num.Length) * (scrollTextBlockList[0] as TextBlock).ActualWidth; //* tbk.ActualWidth;
                stkScroll.Margin = new Thickness(needWidth, 0, 0, 0);
                int count = scrollTextBlockList.Count;
                for (int i = 0; i < num.Length - count; i++)
                {
                    AddNewNumberColumn('0', 0);
                }

                StackPanelAnimation(0);
            }
            else if (num.Length < scrollTextBlockList.Count)
            {
                start = scrollTextBlockList.Count - num.Length;
                StackPanelAnimation(-start * (scrollTextBlockList[0] as TextBlock).ActualWidth);
            }
            int n = 0;
            for (int i = start; i < scrollTextBlockList.Count; i++)
            {
                TextBlock tbk = scrollTextBlockList[i] as TextBlock;
                //string l = Environment.NewLine;

                //tbk.Text = $"0{l}1{l}2{l}3{l}4{l}5{l}6{l}7{l}8";
                int targetNum = num[n++] - '0';
                int oldNum = (int)tbk.Tag;
                if (targetNum == oldNum)
                {
                    continue;
                }
                double height = 0;



                if (AnimationDirection == Direction.ZeroToNine)
                {
                    height = -FontSize * FontFamily.LineSpacing * targetNum;
                    SetText(tbk, 0, 9);
                }
                else
                {
                    int mode=0;
                    if (AnimationDirection == Direction.SmallToBig)
                    {
                        mode = 1;

                    }
                   
                    else if (animationDirection != Direction.BigToSmall)
                    {
                        if ((targetNum < oldNum?targetNum+10:targetNum) - oldNum <=( AnimationDirection == Direction.ChooseCloseSmallToBig ? 5 : 4) )
                        {
                            mode = 1;
                        }
                    }

                    if (mode==1)
                    {
                        height = -FontSize * FontFamily.LineSpacing * (targetNum > oldNum ? targetNum - oldNum : targetNum - oldNum + 10);
                        SetText(tbk, oldNum, targetNum);
                    }
                    else
                    {
                        height = -FontSize * FontFamily.LineSpacing * (oldNum > targetNum ? oldNum - targetNum : oldNum - targetNum + 10);
                        SetText(tbk, targetNum, oldNum);
                        tbk.Margin = new Thickness(0, height, 0, 0);
                        height = 0;
                    }
                }

                tbk.Tag = targetNum;

                //string transfromName = "tbkTrans" + tbk.RenderTransform.GetHashCode().ToString();
                //RegisterName(transfromName, tbk.RenderTransform as TranslateTransform);
                //DoubleAnimation ani = new DoubleAnimation()
                //{
                //    To = height,
                //    Duration = TimeSpan.FromSeconds(1),
                //    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseInOut },
                //};
                //Storyboard.SetTargetName(ani, transfromName);
                //Storyboard.SetTargetProperty(ani, new PropertyPath("Y"));
                //storyboard.Children.Add(ani);

                ThicknessAnimation ani = new ThicknessAnimation()
                {
                    To = new Thickness(0, height, 0, 0),
                    Duration = AnimationDuration,
                    EasingFunction = new CubicEase() { EasingMode = (storyboarding ? EasingMode.EaseOut : EasingMode.EaseInOut) },
                    FillBehavior = FillBehavior.Stop,
                };
                Storyboard.SetTarget(ani, tbk);
                Storyboard.SetTargetProperty(ani, new PropertyPath(MarginProperty));
                storyboard.Children.Add(ani);

                //var group = new TransformGroup();
                //group.Children.Add(new TranslateTransform(0, height));
                //scrollTextBlockList[0].RenderTransform = group;
                //tbk.RenderTransform = group;
                //if(storyboarding)
                //{
                //    storyboard.Stop();
                //}

            }
            if (storyboard.Children.Count != 0)
            {
                storyboarding = true;
                storyboard.Begin(this);
            }
            
        }

        private void SetText(TextBlock tbk, int from, int to)
        {
            if (to - from >= 10)
            {
                throw new Exception("起止相差不小于10");
            }
            if (to < 0)
            {
                throw new Exception();
            }
            if (to < from)
            {
                to += 10;
            }
            StringBuilder str = new StringBuilder(20);
            for (int i = from; i <= to; i++)
            {
                str.AppendLine((i % 10).ToString());
            }
            tbk.Text = str.ToString();
        }

        private Direction animationDirection = Direction.ChooseCloseSmallToBig;

        public void JumpTo(string num)
        {
            scrollTextBlockList.Clear();
            foreach (var i in num)
            {
                AddNewNumberColumn(i);
            }
            if (long.TryParse(num, out long result))
            {
                number = result;
            }
            else
            {
                number = -1;
            }
        }

        public void StackPanelAnimation(double to)
        {
            ThicknessAnimation ani = new ThicknessAnimation()
            {
                To = new Thickness(to, 0, 0, 0),
                Duration = AnimationDuration,
                EasingFunction = new CubicEase() { EasingMode = (storyboarding ? EasingMode.EaseOut : EasingMode.EaseInOut) },
                FillBehavior = FillBehavior.Stop,
            };
            Storyboard.SetTarget(ani, stkScroll);
            Storyboard.SetTargetProperty(ani, new PropertyPath(MarginProperty));
            storyboard.Children.Add(ani);
        }

        public enum Direction
        {
            SmallToBig,//0-9:0123456789
            BigToSmall,//9-0:9876543210
            ChooseCloseSmallToBig,//0-5:0123456
            ChooseCloseBigToSmall,//0-5:098765
            ZeroToNine,//9-0:9876543210
        }
    }
}
