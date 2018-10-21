using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace FzLib.Wpf.Control.Text
{
    public class NumberTextBox : FlatStyle.TextBox
    {
        public NumberTextBox()
        {
            //InputMethod.SetPreferredImeState(this, InputMethodState.Off);

            // PreviewKeyDown += PreviewKeyDownEventHandler;
            //PreviewTextInput += PreviewTextInputEventHandler;
            normalBrush = BorderBrush;
            TextChanged += TextChangedEventHandler;
        }

        private void TextChangedEventHandler(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (IsAllowed())
            {
                BorderBrush = normalBrush;
            }
            else
            {
                BorderBrush = errorBrush;
            }
        }

        private SolidColorBrush errorBrush = new SolidColorBrush(Colors.Red);

        private Brush normalBrush;

        private static readonly Regex rPositiveInteger = new Regex(@"^\+?[0-9]+\.?$");
        private static readonly Regex rNegativeInteger = new Regex(@"^-[0-9]+\.?$");
        private static readonly Regex rPositive = new Regex(@"^\+?([0-9]+)?\.?([0-9]+)?$");
        private static readonly Regex rNegative = new Regex(@"^-([0-9]+)?\.?([0-9]+)?$");
        private static readonly Regex rInteger = new Regex(@"^[0-9]+$");
        private static readonly Regex rScience = new Regex(@"^[\+\-]?[\d]+([\.][\d]*)?([Ee][+-]?[\d]+)?$");



        //private void PreviewTextInputEventHandler(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = true;
        //    return;
        //    if (e.Text.Length>1)
        //    {
        //        e.Handled = true;
        //        return;
        //    }
        //    e.Handled = !IsAllowed();

        //}

        public static bool AreAllowed(Mode mode, params System.Windows.Controls.TextBox[] textboxs)
        {
            foreach (var textbox in textboxs)
            {
                if (mode.HasFlag(Mode.IntegerNumber))
                {
                    if (rInteger.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (mode.HasFlag(Mode.NegativeIntegerNumber))
                {
                    if (rNegativeInteger.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (mode.HasFlag(Mode.NegativeNumber))
                {
                    if (rNegative.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (mode.HasFlag(Mode.PositiveIntegerNumber))
                {
                    if (rPositiveInteger.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (mode.HasFlag(Mode.PositiveNumber))
                {
                    if (rPositive.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (mode.HasFlag(Mode.Scinece))
                {
                    if (rScience.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }

                return false;
            }
            return true;
        }

        public static bool AreAllowed(params NumberTextBox[] textboxs)
        {
            foreach (var textbox in textboxs)
            {
                if (textbox.MatchMode.HasFlag(Mode.IntegerNumber))
                {
                    if (rInteger.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (textbox.MatchMode.HasFlag(Mode.NegativeIntegerNumber))
                {
                    if (rNegativeInteger.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (textbox.MatchMode.HasFlag(Mode.NegativeNumber))
                {
                    if (rNegative.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (textbox.MatchMode.HasFlag(Mode.PositiveIntegerNumber))
                {
                    if (rPositiveInteger.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (textbox.MatchMode.HasFlag(Mode.PositiveNumber))
                {
                    if (rPositive.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }
                if (textbox.MatchMode.HasFlag(Mode.Scinece))
                {
                    if (rScience.IsMatch(textbox.Text))
                    {
                        continue;
                    }
                }

                return false;
            }
            return true;
        }

        private bool IsAllowed()
        {
            return AreAllowed(this);
            //if (MatchMode.HasFlag(Mode.IntegerNumber))
            //{
            //    if (rInteger.IsMatch(Text))
            //    {
            //        return true;
            //    }
            //}
            //if (MatchMode.HasFlag(Mode.NegativeIntegerNumber))
            //{
            //    if (rNegativeInteger.IsMatch(Text))
            //    {
            //        return true;
            //    }
            //}
            //if (MatchMode.HasFlag(Mode.NegativeNumber))
            //{
            //    if (rNegative.IsMatch(Text))
            //    {
            //        return true;
            //    }
            //}
            //if (MatchMode.HasFlag(Mode.PositiveIntegerNumber))
            //{
            //    if (rPositiveInteger.IsMatch(Text))
            //    {
            //        return true;
            //    }
            //}
            //if (MatchMode.HasFlag(Mode.PositiveNumber))
            //{
            //    if (rPositive.IsMatch(Text))
            //    {
            //        return true;
            //    }
            //}
            //if (MatchMode.HasFlag(Mode.Scinece))
            //{
            //    if (rScience.IsMatch(Text))
            //    {
            //        return true;
            //    }
            //}

            //return false;
        }

        public int? IntNumber
        {
            get
            {
                if(int.TryParse(Text,out int result))
                {
                    return result;
                }
                return null;
            }
        }
        public long? LongNumber
        {
            get
            {
                if (long.TryParse(Text, out long result))
                {
                    return result;
                }
                return null;
            }
        }

        public double? DoubleNumber
        {
            get
            {
                if (double.TryParse(Text, out double result))
                {
                    return result;
                }
                return null;
            }
        }


        public Mode MatchMode { get; set; }
        public SolidColorBrush ErrorBrush { get => errorBrush; set => errorBrush = value; }

        [Flags]
        public enum Mode
        {
            PositiveIntegerNumber = 0x01,
            NegativeIntegerNumber = 0x02,
            PositiveNumber = 0x04,
            NegativeNumber = 0x08,
            IntegerNumber = 0x10,
            Scinece = 0x20,
            All = PositiveIntegerNumber | NegativeIntegerNumber | PositiveNumber | NegativeNumber | IntegerNumber | Scinece,
        }
    }
}
