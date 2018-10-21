﻿using System;
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

namespace FzLib.Wpf.Control.Picker
{
    /// <summary>
    /// ColorChooserTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPickerTextBox : UserControl
    {
        public ColorPickerTextBox()
        {
            InitializeComponent();
        }


        private bool strictMode;

        public SolidColorBrush ColorBrush
        {
            get
            {
                if(StrictMode)
                {
                    SolidColorBrush brush;
                    try
                    {
                        brush=new BrushConverter().ConvertFrom(txt.Text) as SolidColorBrush;
                    }
                    catch
                    {
                        return null;
                    }
                    if(brush != colorPicker.CurrentColor)
                    {
                        return null;
                    }
                }

                return colorPicker.CurrentColor;
            }
            set
            {
                colorPicker.CurrentColor = value;
            }
        }

        public void SetColor(string color)
        {
          ColorBrush=  new BrushConverter().ConvertFrom(color) as SolidColorBrush;
        }
        /// <summary>
        /// 如果启动严格模式，输入框的内容也必须正确
        /// </summary>
        public bool StrictMode { get => strictMode; set => strictMode = value; }
    }
}
