using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FzLib.Wpf.Control.Dialog
{
    public class DialogHelper
    {

        public static Window DefaultDialogOwner { get; set; } = null;

        public static int ShowPrompt(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Information);
        }
        public static int ShowError(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Error);
        }
        public static int ShowWarn(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Warn);
        }
        public static bool ShowYesNo(string message, Window owner = null)
        {
            return ShowMessage(message, DialogType.Warn,MessageBoxButton.YesNo)==0;
        }

        public static int ShowException(Exception ex, bool onlyShowMessage = false, Window owner = null)
        {
            return ShowException("程序发生异常", ex, onlyShowMessage, owner);
        }
        public static int ShowException(string text, Exception ex, bool onlyShowMessage = false, Window owner = null)
        {
            return ShowMessage(text + "：" + Environment.NewLine  , onlyShowMessage?ex.Message: ex.ToString(), DialogType.Error,owner);
        }

        public static int ShowMessage(string message, DialogType type, Window owner = null)
        {
            return ShowMessage(message, null, type, owner);
        }
        public static int ShowMessage(string message, string detial, DialogType type, Window owner = null)
        {
            return ShowMessage(message, detial, type, new string[] { "确定" });
        }
        public static int ShowMessage(string message, DialogType type, IEnumerable<string> buttonTexts, Window owner = null)
        {
            return ShowMessage(message, null, type, buttonTexts, owner);
        }
        public static int ShowMessage(string message, string detial, DialogType type, IEnumerable<string> buttonTexts, Window owner = null)
        {
            DialogBox box = new DialogBox(type, owner ?? DefaultDialogOwner);
            box.SetMessage(message);
            box.SetDetail(detial);
            foreach (var i in buttonTexts)
            {
                box.AddButton(i);
            }
            box.ShowDialog();
            return box.ResultIndex;
        }
        public static int ShowMessage(string message, DialogType type, MessageBoxButton buttons, Window owner = null)
        {
            
            return ShowMessage(message,"", type, buttons, owner);
        }

        public static int ShowMessage(string message,string detail, DialogType type, MessageBoxButton buttons, Window owner = null)
        {
            //DialogBox box = new DialogBox(type, owner ?? DefautDialogOwner);
            List<string> str = new List<string>();
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    str.Add("确定");
                    break;
                case MessageBoxButton.OKCancel:
                    str.Add("确定");
                    str.Add("关闭");
                    break;
                case MessageBoxButton.YesNo:
                    str.Add("是");
                    str.Add("否");
                    break;
                case MessageBoxButton.YesNoCancel:
                    str.Add("是");
                    str.Add("否");
                    str.Add("取消");
                    break;
            }
            return ShowMessage(message,detail, type, str, owner);
        }

        public static bool GetInput(string message, out string text, SolidColorBrush color = null, string defaultText = "", string regex = ".*", bool allowEmpty = true, Window owner = null)
        {
            var box = new InputBox(message, owner ?? DefaultDialogOwner, color, defaultText, regex) { AllowEmpty = allowEmpty };
            box.AddButton("取消");
            box.AddButton("确定", true, true);
            box.ShowDialog();
            text = box.ResultText;
            if (box.ResultIndex == 1)
            {
                return true;
            }
            return false;
        }
    }
}
