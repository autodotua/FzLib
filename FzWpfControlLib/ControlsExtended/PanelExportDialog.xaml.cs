using System;
using System.Collections.Generic;
using System.IO;
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
using static FzLib.Control.Common;

namespace FzLib.Control.ControlsExtended
{
    /// <summary>
    /// PanelExportDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PanelExportDialog : Window
    {
        public Panel Panel;

        public PanelExportDialog(Panel pnl)
        {
            InitializeComponent();
            Panel = pnl;
            sldJpeg.TextConvert = p => ((int)p).ToString() + "%";
            btnJpg.Filters = new Dictionary<string, string> { { "JPEG图片", "jpg" } ,{ "所有文件", "*" } };
            btnPng.Filters = new Dictionary<string, string> { { "PNG图片", "png" }, { "所有文件", "*" } };
            btnXps.Filters = new Dictionary<string, string> { { "XPS文档", "xps" }, { "所有文件", "*" } };

        }

        public Size Size
        {
            get
            {
                if(!chkSize.IsChecked.Value)
                {
                    return Size.Empty;
                }
                if (txtSizeHeight.DoubleNumber.HasValue && txtSizeWidth.DoubleNumber.HasValue)
                {
                    return new Size(txtSizeWidth.DoubleNumber.Value, txtSizeHeight.DoubleNumber.Value);
                }
                throw new Exception("输入的尺寸不是合法数字");
            }
            set
            {
                if (value == null)
                {
                    chkSize.IsChecked = false;
                }
                else
                {
                    chkSize.IsChecked = true;
                    txtSizeWidth.Text = value.Width.ToString();
                    txtSizeHeight.Text = value.Height.ToString();
                }
            }
        }

        public Size Magnification
        {
            get
            {
                if (txtMagnificationHeight.DoubleNumber.HasValue && txtMagnificationWidth.DoubleNumber.HasValue)
                {
                    return new Size(txtMagnificationWidth.DoubleNumber.Value, txtMagnificationHeight.DoubleNumber.Value);
                }
                throw new Exception("输入的尺寸不是合法数字");
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }

                txtMagnificationWidth.Text = value.Width.ToString();
                txtMagnificationHeight.Text = value.Height.ToString();
            }

        }

        public double Blood
        {
            get
            {
                if (txtBlood.DoubleNumber.HasValue)
                {
                    return txtBlood.DoubleNumber.Value;
                }
                throw new Exception("输入的尺寸不是合法数字");
            }
            set
            {
                txtBlood.Text = value.ToString();
            }
        }

        public SolidColorBrush PanelBackground
        {
            get
            {
                if(chkBackground.IsChecked.Value)
                {
                    return color.ColorBrush;
                }
                return null;
            }
            set
            {
                if(value==null)
                {
                    chkBackground.IsChecked = false;
                }
                else
                {
                    chkBackground.IsChecked = true;
                    color.ColorBrush = value;
                }
            }
        }
        public int JpegQuality
        {
            get => (int)sldJpeg.Value;
            set
            {
                if(value>100 || value<10)
                {
                    throw new Exception("值不再10-100范围内");
                }
                sldJpeg.Value = value;
            }
        }



        public PanelExport GetExportObject()
        {
            foreach(var btn in new FileSystem.StorageOperationButton[] { btnJpg,btnPng,btnXps})
            {
                btn.DefaultFileName = "输出 - " + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

           return new PanelExport(Panel, Size, Blood, Magnification.Width, Magnification.Height);
        }




        private void StorageOperationButton_DialogComplete_1(object sender, StorageOperationEventArgs e)
        {
            PanelExport export = GetExportObject();
            switch((sender as FileSystem.StorageOperationButton).DefaultExtension)
            {
                case "jpg":
                    export.ExportToJpg(e.Name, JpegQuality, PanelBackground);
                    break;

                case "png":
                    export.ExportToPng(e.Name, PanelBackground);
                    break;

                case "xps":
                    export.ExportToXps(e.Name, PanelBackground);
                    break;

            }
        }

        private void btnPrt_Click(object sender, RoutedEventArgs e)
        {
            PanelExport export = GetExportObject();
            export.ExportByPrinting(PanelBackground);
        }
    }
}
