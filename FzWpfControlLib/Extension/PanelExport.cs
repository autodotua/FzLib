using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace FzLib.Control.Extension
{
    public class PanelExport
    {
        public PanelExport(Panel pnl) : this(pnl, Size.Empty)
        {

        }
        public PanelExport(Panel pnl, Size exportSize) : this(pnl, exportSize, 0)
        {

        }
        public PanelExport(Panel pnl, Size exportSize, double blood) : this(pnl, exportSize, blood, 1)
        {

        }
        public PanelExport(Panel pnl, Size exportSize, double blood, double magnification) : this(pnl, exportSize, blood, magnification, magnification)
        {

        }

        public PanelExport(Panel pnl, Size exportSize, double blood, double horizontalMagnification, double verticalMagnification)
        {
            PanelRawBrush = pnl.Background ?? Brushes.Transparent;
            this.pnl = pnl;
            if(exportSize.Equals(Size.Empty))
            {
                ExportSize = pnl.RenderSize;
            }
            else
            {
                ExportSize = exportSize;
            }
            Blood = blood;
            HorizontalMagnification = horizontalMagnification;
            VerticalMagnification = verticalMagnification;
        }
        public Size ExportSize { get; set; }

        private Panel pnl;
        private Brush PanelRawBrush;

        public double HorizontalMagnification { get; set; } = 10;
        public double VerticalMagnification { get; set; } = 10;


        public double Blood { get; set; }

        private Transform ArrangeSize()
        {
            Transform transform = pnl.LayoutTransform;
            pnl.LayoutTransform = null;
            pnl.Measure(ExportSize);
            pnl.Arrange(new Rect(ExportSize));
            return transform;
        }

        public void ExportByPrinting(Brush background = null)
        {
            Before(background);
            ArrangeSize();
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            dialog.PrintVisual(pnl, "打印");
            After();
        }
        public void ExportToXps(string path, Brush background = null)
        {
            if (path == null)
            {
                return;
            }
            Before(background);
            ArrangeSize();

            Package package = Package.Open(path, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            writer.Write(pnl);
            doc.Close();
            package.Close();
            After();
            //cvs.LayoutTransform = transform;
        }
        public void ExportToJpg(string path, int quality, Brush background = null)
        {
            if (path == null)
            {
                return;
            }
            if(background==null)
            {
                background = Brushes.White;
            }
            Before(background);
            pnl.Background = new SolidColorBrush(Colors.White);
            ArrangeSize();

            RenderTargetBitmap renderBitmap =
            new RenderTargetBitmap(
            (int)(ExportSize.Width * HorizontalMagnification),
            (int)(ExportSize.Height * VerticalMagnification),
            96 * HorizontalMagnification,
            96 * VerticalMagnification,
            PixelFormats.Pbgra32);
            renderBitmap.Render(pnl);

            using (FileStream outStream = new FileStream(path, FileMode.Create))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = quality;
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }
            // cvs.LayoutTransform = transform;
            After();

        }
        public void ExportToPng(string path, Brush  background=null)
        {
            if (path == null)
            {
                return;
            }
            Before(background);
            ArrangeSize();
            RenderTargetBitmap renderBitmap =
       new RenderTargetBitmap(
            (int)(ExportSize.Width * HorizontalMagnification),
            (int)(ExportSize.Height * VerticalMagnification),
            96 * HorizontalMagnification,
            96 * VerticalMagnification,
            PixelFormats.Pbgra32);
            renderBitmap.Render(pnl);

            using (FileStream outStream = new FileStream(path, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }
            //cvs.LayoutTransform = transform;
            After();

        }

        private void After()
        {
            pnl.Background = PanelRawBrush;
        }

        private void Before(Brush background)
        {
            if (!(background == null || background == Brushes.Transparent))
            {
                pnl.Background = background;
            }
           
        }



        public static void ShowExportWindow(Panel pnl,Window owner=null)
        {
            new PanelExportDialog(pnl) { Owner = owner }.Show();
        }
        public static void ShowExportWindowDialog(Panel pnl, Window owner = null)
        {
            new PanelExportDialog(pnl) { Owner = owner } .ShowDialog();
        }
    }
}
