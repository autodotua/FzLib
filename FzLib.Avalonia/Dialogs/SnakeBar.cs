using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    //public class SnakeBar
    //{
    //    public Window Window { get; private set; }
    //    public Grid MainGrid { get; private set; }

    //    public SnakeBar(Window window)
    //    {
    //        Window = window ?? throw new ArgumentNullException();
    //        if (!(window.Content is Grid))
    //        {
    //            throw new Exception("窗体的内容必须是Grid");
    //        }
    //        MainGrid = window.Content as Grid;

    //        window.SizeChanged += (p1, p2) =>
    //          {
    //              if (currentBars.Count > 0)
    //              {
    //                  foreach (var bar in currentBars)
    //                  {
    //                      if (double.IsNaN(Width))
    //                      {
    //                          bar.Width = MainGrid.Width + MainGrid.Margin.Left + MainGrid.Margin.Right;
    //                      }
    //                  }
    //              }
    //          };
    //    }

    //    public Grid ShowMessage(string text)
    //    {
    //        Grid grd = GetControl(text);
    //        currentBars.Add(grd);
    //        DoubleAnimation ani = new DoubleAnimation(0, AnimationDuration);
    //        ani.EasingFunction = EaseFunction;
    //        Storyboard.SetTarget(ani, grd);
    //        Storyboard.SetTargetProperty(ani, new PropertyPath("(Grid.RenderTransform).(TranslateTransform.Y)"));
    //        Storyboard storyboard = new Storyboard() { Children = { ani } };
    //        storyboard.Completed += async (p1, p2) =>
    //          {
    //              if (Duration == Duration.Forever || !Duration.HasTimeSpan)
    //              {
    //                  return;
    //              }
    //              await Task.Delay(Duration.TimeSpan);
    //              if (grd.IsMouseOver)
    //              {
    //                  needToClose.Add(grd);
    //              }
    //              else
    //              {
    //                  Hide(grd);
    //              }
    //          };
    //        storyboard.Begin();
    //        return grd;
    //    }

    //    private List<Grid> needToClose = new List<Grid>();

    //    private Grid GetControl(string text)
    //    {
    //        Grid grd = new Grid
    //        {
    //            Width = MainGrid.Width + MainGrid.Margin.Left + MainGrid.Margin.Right,
    //            Height = Height,
    //            VerticalAlignment = VerticalAlignment.Bottom,
    //            Margin = new Thickness(-MainGrid.Margin.Left, 0, -MainGrid.Margin.Right, -MainGrid.Margin.Bottom),
    //            Background = Background,
    //            RenderTransform = new TranslateTransform(0, Height),
    //            Effect = new DropShadowEffect()
    //            {
    //                BlurRadius = 12,
    //                Color = Colors.Black,
    //                ShadowDepth = 0,
    //                Opacity = 0.4,
    //            },
    //        };
    //        if (!double.IsNaN(Width))
    //        {
    //            grd.HorizontalAlignment = PanelHorizontalAlignment;
    //            grd.Width = Width;
    //        }
    //        grd.MouseLeave += (p1, p2) =>
    //          {
    //              if (needToClose.Contains(grd))
    //              {
    //                  Hide(grd);
    //              }
    //          };
    //        Grid.SetColumnSpan(grd, int.MaxValue);
    //        Grid.SetRowSpan(grd, int.MaxValue);

    //        TextBlock txt = new TextBlock
    //        {
    //            Text = text,
    //            VerticalAlignment = VerticalAlignment.Center,
    //            HorizontalAlignment = TextHorizontalAlignment,
    //            Margin = new Thickness(TextLeftMargin, 0, 72, 0),
    //            Foreground = TextForeground,
    //        };
    //        if (ShowButton)
    //        {
    //            Label btn = new Label()
    //            {
    //                Background = Background,
    //                HorizontalAlignment = HorizontalAlignment.Right,
    //                VerticalAlignment = VerticalAlignment.Center,
    //                Foreground = ButtonForeground,
    //                Cursor = Cursors.Hand,
    //                Content = ButtonContent,
    //                Margin = new Thickness(0, 0, ButtonRightMargin, 0),
    //            };
    //            btn.MouseLeave += (p1, p2) =>
    //              {
    //                  btn.Foreground = ButtonForeground;
    //              };
    //            btn.MouseEnter += (p1, p2) =>
    //            {
    //                btn.Foreground = ButtonMouseOverForeground;
    //            };
    //            btn.PreviewMouseLeftButtonDown += (p1, p2) =>
    //            {
    //                ButtonClick?.Invoke(p1, p2);
    //                if (clickButtonToClose)
    //                {
    //                    Hide(grd);
    //                }
    //            };

    //            grd.Children.Add(btn);
    //        }
    //        grd.Children.Add(txt);
    //        MainGrid.Children.Add(grd);
    //        return grd;
    //    }

    //    public void Hide()
    //    {
    //        foreach (var bar in currentBars.ToArray())
    //        {
    //            Hide(bar);
    //        }
    //    }

    //    public void Hide(Grid grd)
    //    {
    //        if (!currentBars.Contains(grd))
    //        {
    //            return;
    //        }
    //        currentBars.Remove(grd);
    //        DoubleAnimation ani = new DoubleAnimation(Height, AnimationDuration);
    //        ani.EasingFunction = EaseFunction;
    //        Storyboard.SetTarget(ani, grd);
    //        Storyboard.SetTargetProperty(ani, new PropertyPath("(Grid.RenderTransform).(TranslateTransform.Y)"));
    //        Storyboard storyboard = new Storyboard() { Children = { ani } };
    //        storyboard.Completed += (p3, p4) =>
    //        {
    //            MainGrid.Children.Remove(grd);
    //        };
    //        storyboard.Begin();
    //    }

    //    private List<Grid> currentBars = new List<Grid>();
    //    public double Height { get; set; } = 32;
    //    public double Width { get; set; } = 320;
    //    public HorizontalAlignment PanelHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
    //    public HorizontalAlignment TextHorizontalAlignment { get; set; } = HorizontalAlignment.Left;
    //    public double TextLeftMargin { get; set; } = 8;
    //    public Duration Duration { get; set; } = TimeSpan.FromSeconds(2);
    //    public TimeSpan AnimationDuration { get; set; } = TimeSpan.FromSeconds(0.5);
    //    public EasingFunctionBase EaseFunction { get; set; } = new CubicEase() { EasingMode = EasingMode.EaseInOut };
    //    public Brush Background { get; set; } = new SolidColorBrush(Color.FromRgb(0x32, 0x32, 0x32));
    //    public Brush TextForeground { get; set; } = Brushes.White;
    //    public Brush ButtonMouseOverForeground { get; set; } = Brushes.LightGray;
    //    public Brush ButtonForeground { get; set; } = Brushes.White;
    //    public double ButtonRightMargin { get; set; } = 8;

    //    public bool ShowButton { get; set; } = false;
    //    public object ButtonContent { get; set; } = "确定";
    //    private bool clickButtonToClose = false;

    //    public void EnableOkButton()
    //    {
    //        ShowButton = true;
    //        clickButtonToClose = true;
    //    }

    //    public event RoutedEventHandler ButtonClick;

    //    public static WindowOwner DefaultOwner { get; set; } = new WindowOwner();

    //    public static void ShowError(string message)
    //    {
    //        ShowError(DefaultOwner.Owner, message);
    //    }

    //    public static void ShowError(Window owner, string message)
    //    {
    //        SnakeBar snake = new SnakeBar(owner);
    //        snake.EnableOkButton();
    //        snake.Background = new SolidColorBrush(Color.FromRgb(183, 28, 28));
    //        snake.ShowMessage(message);
    //    }

    //    public static void Show(string message)
    //    {
    //        Show(DefaultOwner.Owner, message);
    //    }

    //    public static void Show(Window owner, string message)
    //    {
    //        SnakeBar snake = new SnakeBar(owner);
    //        snake.ShowMessage(message);
    //    }
    //}
}