
using iTextSharp.text.pdf;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace populat.io
{
    class PDFHelper
    {
        public static RenderTargetBitmap GetImage(UIElement view)
        {
            Size size = new Size(view.RenderSize.Width, view.RenderSize.Height);
            RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual drawingvisual = new DrawingVisual();
            using (DrawingContext context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(0, 0, (int)size.Width, (int)size.Height));
                context.Close();
            }
            result.Render(drawingvisual);
            return result;
        }

        public static string SaveAsPng(RenderTargetBitmap src)
        {
            string fileName = (Directory.GetCurrentDirectory() + "/temp.png");
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));
            using (var stm = File.Create(fileName))
            {
                encoder.Save(stm);
            }
            return fileName;
        }

        public static void CreatePdf(UIElement view, string fileName)
        {
            string imageFileName = SaveAsPng(GetImage(view));
            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER.Rotate(), 0, 0, 0, 0);
                PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();
                using (FileStream fs = new FileStream(imageFileName, FileMode.Open))
                {
                    var image = iTextSharp.text.Image.GetInstance(fs);
                    image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                    document.Add(image);
                    document.Close();                   
                }
                File.Delete(imageFileName);
            }
        }
    }
}
