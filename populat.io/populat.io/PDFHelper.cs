
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace populat.io
{
    class PDFHelper
    {
        private static RenderTargetBitmap GetImage(UIElement view)
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

        private static string SaveAsPng(RenderTargetBitmap src)
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

        private static PdfPTable GetPdfPTable(City city)
        {
            PdfPTable t = new PdfPTable(9);
            BaseColor color = new BaseColor(66, 182, 244);
            t.AddCell(new PdfPCell(new Phrase("Year")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Population in 1000's")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Male rate")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Female rate")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Birth rate")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Death rate")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Immigration rate")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Emigration rate")) { BackgroundColor = color });
            t.AddCell(new PdfPCell(new Phrase("Average age")) { BackgroundColor = color });
            foreach (Population p in city.PopulationThroughYears)
            {
                t.AddCell(new PdfPCell(new Phrase(p.Year.ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.PopulationNr, 2).ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.MaleRate, 2).ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.FemaleRate, 2).ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.BirthRate, 2).ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.DeathRate, 2).ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.ImmigrationRate, 2).ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.EmigrationRate, 2).ToString())));
                t.AddCell(new PdfPCell(new Phrase(Math.Round(p.AverageAge, 2).ToString())));
            }
            return t;
        }

        public static void CreatePdf(UIElement view, string fileName, City city)
        {
            string imageFileName = SaveAsPng(GetImage(view));
            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.LETTER.Rotate(), 0, 0, 0, 0);
                PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();
                using (FileStream fs = new FileStream(imageFileName, FileMode.Open))
                {
                    var image = Image.GetInstance(fs);
                    image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                    document.Add(image);
                    document.NewPage();
                    document.Add(GetPdfPTable(city));
                    document.Close();                   
                }
                File.Delete(imageFileName);
            }
        }
    }
}
