using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Svg;
using System.IO;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Xml;
using System.Xml.Linq;
using System.Drawing.Drawing2D;
using Zen.Barcode;
using KeepAutomation.Barcode.Bean;
using Neodynamic.SDK.Barcode;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Drawing.Layout;
using System.Collections;

namespace SVGtoIMG.Converter
{
    public class Class1
    {
    }

    public class Converter
    {
        BarcodeLib.Barcode b = new BarcodeLib.Barcode();

        private Hashtable _encoding;
        private SVGtoIMG.BarCode.BarCode barCodeClass;

        public Converter()
        {
            barCodeClass = new SVGtoIMG.BarCode.BarCode();
            this._encoding = new Hashtable();
            _encoding.Add("*", "bWbwBwBwb");
            _encoding.Add("-", "bWbwbwBwB");
            _encoding.Add("$", "bWbWbWbwb");
            _encoding.Add("%", "bwbWbWbWb");
            _encoding.Add(" ", "bWBwbwBwb");
            _encoding.Add(".", "BWbwbwBwb");
            _encoding.Add("/", "bWbWbwbWb");
            _encoding.Add("+", "bWbwbWbWb");
            _encoding.Add("0", "bwbWBwBwb");
            _encoding.Add("1", "BwbWbwbwB");
            _encoding.Add("2", "bwBWbwbwB");
            _encoding.Add("3", "BwBWbwbwb");
            _encoding.Add("4", "bwbWBwbwB");
            _encoding.Add("5", "BwbWBwbwb");
            _encoding.Add("6", "bwBWBwbwb");
            _encoding.Add("7", "bwbWbwBwB");
            _encoding.Add("8", "BwbWbwBwb");
            _encoding.Add("9", "bwBWbwBwb");
            _encoding.Add("A", "BwbwbWbwB");
            _encoding.Add("B", "bwBwbWbwB");
            _encoding.Add("C", "BwBwbWbwb");
            _encoding.Add("D", "bwbwBWbwB");
            _encoding.Add("E", "BwbwBWbwb");
            _encoding.Add("F", "bwBwBWbwb");
            _encoding.Add("G", "bwbwbWBwB");
            _encoding.Add("H", "BwbwbWBwb");
            _encoding.Add("I", "bwBwbWBwb");
            _encoding.Add("J", "bwbwBWBwb");
            _encoding.Add("K", "BwbwbwbWB");
            _encoding.Add("L", "bwBwbwbWB");
            _encoding.Add("M", "BwBwbwbWb");
            _encoding.Add("N", "bwbwBwbWB");
            _encoding.Add("O", "BwbwBwbWb");
            _encoding.Add("P", "bwBwBwbWb");
            _encoding.Add("Q", "bwbwbwBWB");
            _encoding.Add("R", "BwbwbwBWb");
            _encoding.Add("S", "bwBwbwBWb");
            _encoding.Add("T", "bwbwBwBWb");
            _encoding.Add("U", "BWbwbwbwB");
            _encoding.Add("V", "bWBwbwbwB");
            _encoding.Add("W", "BWBwbwbwb");
            _encoding.Add("X", "bWbwBwbwB");
            _encoding.Add("Y", "BWbwBwbwb");
            _encoding.Add("Z", "bWBwBwbwb");
        }

        //public Image getBarCode2(string text)
        //{
        //    BarCode barcode = new BarCode();
        //    barcode.Symbology = KeepAutomation.Barcode.Symbology.Code39;
        //    barcode.CodeToEncode = text;
        //    barcode.ChecksumEnabled = true;
        //    barcode.X = 1;
        //    barcode.Y = 50;
        //    barcode.BarCodeWidth = 1500;
        //    barcode.BarCodeHeight = 300;
        //    barcode.Orientation = KeepAutomation.Barcode.Orientation.Degree0;
        //    barcode.BarcodeUnit = KeepAutomation.Barcode.BarcodeUnit.Inch;
        //    barcode.DPI = 300;
        //    barcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Gif;

        //    return barcode.generateBarcodeToBitmap();

        //    //barcode.generateBarcodeToImageFile("C://barcode-code39-csharp.gif");
        //}


        public Image getBarcode(int type, string n)
        {
            //Bitmap temp = new Bitmap(1, 1);
            //temp.SetPixel(0, 0, Color.White);
            //Image img = (Image)temp;

            //int W = Convert.ToInt32(1500);
            //int H = Convert.ToInt32(300);
            short W = 3900;
            short H = 300;



            //return new Bitmap(barCodeClass.GenerateBarcodeImage(W, H, n.ToString()));

            //'Draw the barcode onto the barcode printing page canvas
            //dc.DrawImage(thisBarCode, New PointF(pgX, pgY))

            //BarcodeLib.TYPE tipo = BarcodeLib.TYPE.UNSPECIFIED;
            //tipo = BarcodeLib.TYPE.CODE39;

            //try
            //{
            //    b.IncludeLabel = false;
            //    img = b.Encode(tipo, n.ToString().Trim(), this.b.ForeColor, this.b.BackColor, W, H);
            //}
            //catch (Exception ex)
            //{
            //}

            //return img;


            //__________________________________________________________
            //Zen.Barcode.BarcodeSymbology s = Zen.Barcode.BarcodeSymbology.Code39C;
            //BarcodeDraw drawObject = BarcodeDrawFactory.GetSymbology(s);

            //var metrics = drawObject.GetDefaultMetrics(40);
            //metrics.Scale = 1;
            //var barcodeImage = drawObject.Draw(n, metrics);

            //return barcodeImage;
            //__________________________________________________________
            //return ResizeImage(barcodeImage, 4500, 500);//523, 82
            //Bitmap bm = ResizeImage(barcodeImage, 4500, 500);//523, 82

            //using (MemoryStream ms = new MemoryStream())
            //{   
            //    //barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //    barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    //bm.Save(@"C:\test\BarCode(3).jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //    //byte[] imageBytes = ms.ToArray();
            //    return Image.FromStream(ms);
            //    //return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageBytes));
            //}
            //______________________________________________________________
            Zen.Barcode.BarcodeSymbology s = Zen.Barcode.BarcodeSymbology.Code128;
            BarcodeDraw drawObject = BarcodeDrawFactory.GetSymbology(s);

            var metrics = drawObject.GetDefaultMetrics(40);
            metrics.Scale = 1;
            var barcodeImage = drawObject.Draw(n.ToString(), metrics);

            return barcodeImage;

            //Bitmap bm = ResizeImage(barcodeImage, 398, 60);

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    //barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //    bm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    bm.Save(@"C:\test\BarCode(3).jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //    byte[] imageBytes = ms.ToArray();
            //    //return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageBytes));
            //}
        }

        private Bitmap CreateBitmapImage(string sImageText)
        {
            Bitmap objBmpImage = new Bitmap(2, 2);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            System.Drawing.Font objFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));


            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color

            objGraphics.Clear(System.Drawing.Color.White);
            objGraphics.SmoothingMode = SmoothingMode.HighQuality;



            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //  <-- This is the correct value to use. ClearTypeGridFit is better yet!
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(System.Drawing.Color.Black), 0, 0, StringFormat.GenericDefault);

            objGraphics.Flush();

            return (objBmpImage);
        }

        public void example()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create a font
            XFont font = new XFont("Times", 25, XFontStyle.Bold);

            PageSize[] pageSizes = (PageSize[])Enum.GetValues(typeof(PageSize));
            foreach (PageSize pageSize in pageSizes)
            {
                if (pageSize == PageSize.Undefined)
                    continue;

                // One page in Portrait...
                PdfPage page = document.AddPage();
                page.Size = pageSize;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize.ToString(), font, XBrushes.DarkRed,
                  new XRect(0, 0, page.Width, page.Height),
                  XStringFormat.Center);

                // ... and one in Landscape orientation.
                page = document.AddPage();
                page.Size = pageSize;
                page.Orientation = PageOrientation.Landscape;
                gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize.ToString() + " (landscape)", font,
                  XBrushes.DarkRed, new XRect(0, 0, page.Width, page.Height),
                  XStringFormat.Center);
            }

            // Save the document...
            string filename = @"C:\test\Sizes.pdf";
            document.Save(filename);
        }

        public void DrawPDF()
        {
            string path = @"C:\test\ImageGenerated(10).pdf";
            using (PdfDocument doc = new PdfDocument())
            {
                PdfPage page = new PdfPage();
                page = doc.AddPage();
                page.Width = 606;
                page.Height = 1669;


                var n = 630670420;
                //Image barcode = getBarcode(1, n);

                //Bitmap bm = new Bitmap(4857, 12893);

                //bm.SetResolution(600, 600);
                //Graphics g = Graphics.FromImage(bm);

                //PointF pf = new PointF(0f, 0f);
                //XPoint pt = new XPoint(pf);
                //XSize size = new XSize(pt);

                //XGraphics gfx = XGraphics.FromGraphics(g, size, XGraphicsUnit.Inch);
                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {

                    XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                    //XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.WinAnsi, PdfFontEmbedding.Default);
                    //XFont fontRegular = new XFont(facename, 20, XFontStyle.Regular, options);

                    XGraphicsState state;
                    gfx.Clear(Color.White);
                    gfx.SmoothingMode = XSmoothingMode.HighQuality;

                    gfx.DrawString("Ticket 99".ToUpper(),
                     new XFont("Arial", 16, XFontStyle.Regular, options),
                     XBrushes.Black,
                     new XRect(0, 0, page.Width, page.Height),
                     XStringFormat.TopCenter);

                    //gfx.DrawString(630670420.ToString(), new Font("Bar-Code 39", 2.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Inch), new SolidBrush(Color.Black), 2000f, 5000f, drawFormat);

                    gfx.DrawString(n.ToString().ToUpper(),
                       new XFont("Arial", 25, XFontStyle.Italic, options),
                       XBrushes.Black,
                       new XRect(0, 0, page.Width, page.Height),
                       XStringFormat.Center);

                    //gfx.DrawImage(barcode, new Point(80, 1200));

                    state = gfx.Save();

                    gfx.RotateAtTransform(90, new XPoint(350, 500));

                    gfx.DrawString("Empacadorea y empaque SAS TICKETEVENTO NO SE HACE RESPONSABLE DE COMERCIALIZACIÓN DE ESTA ENTRADA".ToUpper(),
                        new XFont("Arial", 10, XFontStyle.Italic, options),
                        XBrushes.Black,
                        new XRect(100, 0, page.Width, page.Height),
                        XStringFormat.Center);

                    //XFont font = new XFont("Arial", 10, XFontStyle.Regular);
                    //XTextFormatter tf = new XTextFormatter(gfx);

                    //XRect rect = new XRect(40, 100, 250, 220);
                    //gfx.DrawRectangle(XBrushes.SeaShell, rect);
                    //tf.DrawString("Empacadorea y empaque SAS TICKETEVENTO NO SE HACE RESPONSABLE DE COMERCIALIZACIÓN DE ESTA ENTRADA".ToUpper(), font, XBrushes.Black, rect, XStringFormats.TopLeft);

                    gfx.Restore(state);

                    //gfx.DrawString("Empacadorea y empaque SAS\nTICKETEVENTO NO SE HACE RESPONSABLE DE COMERCIALIZACIÓN DE ESTA ENTRADA".ToUpper(), new Font("Arial", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World), new SolidBrush(Color.Black), 229f, 1032f);


                    gfx.Save();
                    doc.Save(path);
                }
            }
        }



        public MemoryStream DrawFromSvg(int Width, int Heigth)
        {
            float wCenter = Width / 2;
            //int Width = 4857, Heigth = 12893;
            //int Width = 606 / 2, Heigth = 1669 / 2;
            string pngPath = @"C:\test\ImageGenerated(10).bmp";

            //var n = DateTime.Now.ToFileTime();
            var n = DateTime.Now.ToFileTime();
            //Image barcode = getBarcode(1, n);

            Image bk = Image.FromFile(@"C:\test\Image(1).png");

            //string imageText = System.IO.File.ReadAllText(@"C:\test\Image.txt");
            ////string fromBase = String.Format("data:image/png;base64,{0}", imageText);

            //byte[] imageBytes = Convert.FromBase64String(imageText);
            //Image bk = null;
            //using (MemoryStream ms1 = new MemoryStream(imageBytes, 0,
            //  imageBytes.Length))
            //{
            //    ms1.Write(imageBytes, 0, imageBytes.Length);
            //    bk = System.Drawing.Image.FromStream(ms1, true);
            //}

            //Bitmap bm = new Bitmap(606, 1669);
            //Bitmap bm = new Bitmap(Width, Heigth, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);
            Bitmap bm = new Bitmap(Width, Heigth, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);//saca menos cuadros
            //Bitmap bm = new Bitmap(Width, Heigth, System.Drawing.Imaging.PixelFormat.Format32bppArgb);//Original

            Rectangle canvas = new Rectangle(0, 0, Width, Heigth);

            //Image bk2 = ResizeImage(bk, bm.Width, bm.Height);

            //bm.SetResolution(600, 600);
            Graphics g = Graphics.FromImage(bm);
            g.FillRectangle(Brushes.White, 0, 0, Width, Heigth);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //g.DrawString("Ticket 99".ToUpper(), new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 45f, 14f);
            //g.DrawString("Empacadorea y empaque SAS\nTICKETEVENTO NO SE HACE RESPONSABLE DE COMERCIALIZACIÓN DE ESTA ENTRADA".ToUpper(), new Font("Arial", 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 26f, 112f);
            //g.DrawString(n.ToString().ToUpper(), new Font("Arial", 12f, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), 45f, 474f);

            g.DrawImage(bk, 0, 0, bm.Width, bm.Height - 10);


            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;

            string UseCode = string.Format("{0}{1}{0}", "*", n);

            float XPosition = 20;
            int YPosition = 410;
            const int _barHeight = 40;
            const float aumento = 1;

            string CurrentSymbol = string.Empty;

            //for (int j = 0; j < UseCode.Length - 1; j++)
            //{
            //    CurrentSymbol = UseCode.Substring(j, 1);

            //    if (this._encoding[CurrentSymbol] != null)
            //    {
            //        string EncodedSymbol = _encoding[CurrentSymbol].ToString();

            //        for (int i = 0; i < EncodedSymbol.Length - 1; i++)
            //        {
            //            string CurrentCode = EncodedSymbol.Substring(i, 1);

            //            g.FillRectangle(barCodeClass.getBCSymbolColor(CurrentCode), XPosition, YPosition, barCodeClass.getBCSymbolWidth(CurrentCode) * aumento, _barHeight);

            //            XPosition = XPosition + barCodeClass.getBCSymbolWidth(CurrentCode) * aumento;
            //        }

            //        g.FillRectangle(barCodeClass.getBCSymbolColor("w"), XPosition, YPosition, barCodeClass.getBCSymbolWidth("w") * aumento, _barHeight);

            //        XPosition = XPosition + barCodeClass.getBCSymbolWidth("w") * aumento;
            //    }
            //}

            //g.RotateTransform(-90);
            //g.Save();

            g.FillRectangle(Brushes.White, 5, YPosition - 5, 190, 50);

            Image barcode = getBarcode(1, n.ToString());

            XPosition = (Width / 2) - (barcode.Width / 2);

            g.DrawImage(barcode, new PointF(XPosition, YPosition));

            MemoryStream ms = new MemoryStream();

            EncoderParameters encodingParams = new EncoderParameters();
            encodingParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);

            //'Define the encoding details of "how" for the image
            //'We will use PNG because, well it's got the best image quality for it's footprint
            ImageCodecInfo encodingInfo = barCodeClass.FindCodecInfo("PNG");

            //bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
            bm.RotateFlip(RotateFlipType.Rotate180FlipNone);

            //'Save the drawing directly into the stream
            bm.Save(ms, encodingInfo, encodingParams);
            //bm.Save(pngPath, encodingInfo, encodingParams);

            //'Clean-up!  Nobody likes a possible memory leaking application!
            g.Dispose();
            b.Dispose();
            bk.Dispose();

            return ms;

            //g.DrawImage(getBarcode(1, n), new PointF(100, 10000));

            #region MyRegion


            //System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            //g.DrawString(630670420.ToString(), new Font("Bar-Code 39", 2.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Inch), new SolidBrush(Color.Black), 2000f, 5000f, drawFormat);

            //var Encoded_Value = "1001011011010101100110101011011001010101010011011010101100110101010100101101101010011011010101001101011010110010101101010011011010100101101101";
            //int Width = Convert.ToInt32(1500);
            //int Height = Convert.ToInt32(300);

            //var b = new Bitmap(Width, Height);
            //b.SetPixel(0, 0, Color.White);
            //int iBarWidth = (bm.Width / 2) / Encoded_Value.Length;
            //int shiftAdjustment = (Width % Encoded_Value.Length) / 2;

            //if (iBarWidth <= 0)
            //    throw new Exception("EGENERATE_IMAGE-2: Image size specified not large enough to draw image. (Bar size determined to be less than 1 pixel)");

            ////draw image
            //int pos = 0;

            ////using (Graphics gr = Graphics.FromImage(b))
            ////{
            ////clears the image and colors the entire background
            ////g.Clear(Color.White);
            //g.CompositingMode = CompositingMode.SourceCopy;
            ////lines are fBarWidth wide so draw the appropriate color line vertically
            //using (Pen pen = new Pen(Color.Black, iBarWidth))
            //{
            //    pen.Alignment = PenAlignment.Right;

            //    while (pos < Encoded_Value.Length)
            //    {
            //        if (Encoded_Value[pos] == '1')
            //            g.DrawLine(pen, new Point(pos * iBarWidth + shiftAdjustment, 10000), new Point(pos * iBarWidth + shiftAdjustment, 10500 + Height));

            //        //g.DrawLine(pen, new Point(1000, pos * iBarWidth + shiftAdjustment), new Point(1050 + Height, pos * iBarWidth + shiftAdjustment));

            //        pos++;
            //    }//while
            //}//using
            ////}//using
            #endregion

            #region MyRegion
            ////using (
            //BarcodeProfessional br = new BarcodeProfessional();
            ////)
            ////{
            //br.Symbology = Neodynamic.SDK.Barcode.Symbology.Code39;
            //br.BarcodeUnit = Neodynamic.SDK.Barcode.BarcodeUnit.Inch;
            //br.BarWidth = 0.04;
            //br.BarHeight = 0.75;
            //br.Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 16);
            //br.CodeAlignment = Neodynamic.SDK.Barcode.Alignment.BelowJustify;
            //using (Font fnt = new Font("Courier New", 10f))
            //{
            //    br.TextFont = fnt;
            //}
            //br.DrawOnCanvas(g, new Point(0, 0));
            ////}


            //g.DrawImage(b, new Point(100, 10000));

            //g.DrawImage(getBarCode2("6380670420"), new Point(100, 10000));
            #endregion

            //g.Flush();

            //bm.Save(pngPath, encodingInfo, encodingParams);

            //g.Dispose();
            //bm.Dispose();
            //Bitmap bmp = new Bitmap("filename.bmp");

            //RectangleF rectf = new RectangleF(70, 90, 90, 50);

            //Graphics g = Graphics.FromImage(bmp);

            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.DrawString("yourText", new Font("Tahoma", 8), Brushes.Black, rectf);

            //g.Flush();

            //bmp.Save(pngPath, ImageFormat.Jpeg); ;
        }

        //public void DrawFromSvg(Graphics g, Bitmap bm)
        //{
        //    string pngPath = @"C:\test\ImageGenerated(10).bmp";

        //    //var n = DateTime.Now.ToFileTime();
        //    var n = 630670420;
        //    Image barcode = getBarcode(1, n);

        //    //Bitmap bm = new Bitmap(606, 1669);
        //    //Bitmap bm = new Bitmap(4857, 12893);

        //    //bm.SetResolution(600, 600);
        //    //Graphics g = Graphics.FromImage(bm);
        //    g.Clear(Color.White);

        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //    //g.CompositingMode = CompositingMode.SourceCopy;
        //    //g.CompositingQuality = CompositingQuality.HighQuality;


        //    g.DrawString("Ticket 99".ToUpper(), new Font("Arial", 0.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Inch), new SolidBrush(Color.Black), 229f, 532f);
        //    g.DrawString("Empacadorea y empaque SAS\nTICKETEVENTO NO SE HACE RESPONSABLE DE COMERCIALIZACIÓN DE ESTA ENTRADA".ToUpper(), new Font("Arial", 0.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Inch), new SolidBrush(Color.Black), 229f, 1032f);
        //    g.DrawString(n.ToString().ToUpper(), new Font("Arial", 0.9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Inch), new SolidBrush(Color.Black), 229f, 1432f);

        //    #region MyRegion



        //    //System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
        //    //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

        //    //g.DrawString(630670420.ToString(), new Font("Bar-Code 39", 2.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Inch), new SolidBrush(Color.Black), 2000f, 5000f, drawFormat);

        //    //var Encoded_Value = "1001011011010101100110101011011001010101010011011010101100110101010100101101101010011011010101001101011010110010101101010011011010100101101101";
        //    //int Width = Convert.ToInt32(1500);
        //    //int Height = Convert.ToInt32(300);

        //    //var b = new Bitmap(Width, Height);
        //    //b.SetPixel(0, 0, Color.White);
        //    //int iBarWidth = (bm.Width / 2) / Encoded_Value.Length;
        //    //int shiftAdjustment = (Width % Encoded_Value.Length) / 2;

        //    //if (iBarWidth <= 0)
        //    //    throw new Exception("EGENERATE_IMAGE-2: Image size specified not large enough to draw image. (Bar size determined to be less than 1 pixel)");

        //    ////draw image
        //    //int pos = 0;

        //    ////using (Graphics gr = Graphics.FromImage(b))
        //    ////{
        //    ////clears the image and colors the entire background
        //    ////g.Clear(Color.White);
        //    //g.CompositingMode = CompositingMode.SourceCopy;
        //    ////lines are fBarWidth wide so draw the appropriate color line vertically
        //    //using (Pen pen = new Pen(Color.Black, iBarWidth))
        //    //{
        //    //    pen.Alignment = PenAlignment.Right;

        //    //    while (pos < Encoded_Value.Length)
        //    //    {
        //    //        if (Encoded_Value[pos] == '1')
        //    //            g.DrawLine(pen, new Point(pos * iBarWidth + shiftAdjustment, 10000), new Point(pos * iBarWidth + shiftAdjustment, 10500 + Height));

        //    //        //g.DrawLine(pen, new Point(1000, pos * iBarWidth + shiftAdjustment), new Point(1050 + Height, pos * iBarWidth + shiftAdjustment));

        //    //        pos++;
        //    //    }//while
        //    //}//using
        //    ////}//using
        //    #endregion

        //    //using (
        //    BarcodeProfessional br = new BarcodeProfessional();
        //    //)
        //    //{
        //    br.Symbology = Neodynamic.SDK.Barcode.Symbology.Code39;
        //    br.BarcodeUnit = Neodynamic.SDK.Barcode.BarcodeUnit.Inch;
        //    br.BarWidth = 0.04;
        //    br.BarHeight = 0.75;
        //    br.Code = Guid.NewGuid().ToString().ToUpper().Substring(0, 16);
        //    br.CodeAlignment = Neodynamic.SDK.Barcode.Alignment.BelowJustify;
        //    using (Font fnt = new Font("Courier New", 10f))
        //    {
        //        br.TextFont = fnt;
        //    }
        //    br.DrawOnCanvas(g, new Point(0, 0));
        //    //}


        //    //g.DrawImage(b, new Point(100, 10000));

        //    //g.DrawImage(getBarCode2("6380670420"), new Point(100, 10000));

        //    g.Flush();

        //    bm.Save(pngPath, ImageFormat.Bmp);

        //    //Bitmap bmp = new Bitmap("filename.bmp");

        //    //RectangleF rectf = new RectangleF(70, 90, 90, 50);

        //    //Graphics g = Graphics.FromImage(bmp);

        //    //g.SmoothingMode = SmoothingMode.AntiAlias;
        //    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //    //g.DrawString("yourText", new Font("Tahoma", 8), Brushes.Black, rectf);

        //    //g.Flush();

        //    //bmp.Save(pngPath, ImageFormat.Jpeg); ;
        //}



        public XmlDocument ToXmlDocument(XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(300, 300);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public void SvgToImg()
        {
            //using (var stream = new MemoryStream())
            //{
            //    string svgText = "data here";
            //    //var sampleDoc = SvgDocument.OpenFromMemoryStream(stream);
            //    var sampleDoc = SvgDocument.Open(stream,);

            //    var Image = sampleDoc.Draw().ToPng();
            //}

            string path = @"C:\test\svg3.svg", pngPath = @"C:\test\CorrectSize(1)(ResizedTo300dpi).png";

            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();

            string imageText = System.IO.File.ReadAllText(path);

            XDocument document = XDocument.Load(path);
            XElement svg_Element = document.Root;

            //IEnumerable<XElement> test = from e1 in svg_Element.Elements("{http://www.w3.org/2000/svg}g")
            //                             select e1;
            //StringBuilder sb = new StringBuilder();
            //foreach (XElement ee in test)
            //{
            //    // Get "Central" and "Capital"
            //    sb.AppendLine(ee.Attribute("id").Value);

            //    IEnumerable<XElement> test2 = from e2 in ee.Elements("{http://www.w3.org/2000/svg}text")
            //                                  select e2;
            //    foreach (XElement ee2 in test2)
            //    {
            //        sb.AppendLine("     Block No :" + ee2.Attribute("id").Value);
            //        IEnumerable<XElement> test3 = from ee3 in ee2.Elements("{http://www.w3.org/2000/svg}tspan")
            //                                      select ee3;
            //        foreach (XElement epath in test3)
            //        {
            //            sb.AppendLine("     sPath  :" + epath.Attribute("d").Value);
            //        }
            //    }
            //}
            //string text = sb.ToString();


            xml = ToXmlDocument(document);


            //xml.LoadXml(imageText);

            EncoderParameters encodingParams = new EncoderParameters();
            encodingParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
            ImageCodecInfo encodingInfo = barCodeClass.FindCodecInfo("PNG");

            //Bitmap bm = ResizeImage(smallBitmap, 4857, 12893);
            //Bitmap bm = new Bitmap(200, 550, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //    //var width = smallBitmap.Width;
            //    //var height = smallBitmap.Height;

            var svgDocument = SvgDocument.Open(xml);
            //svgDocument.Draw(bm);
            //bm.SetResolution(300, 300);
            //bm.Save(pngPath, encodingInfo, encodingParams);

            //bm.Dispose();

            using (var smallBitmap = svgDocument.Draw())
            {

                //EncoderParameters encodingParams = new EncoderParameters();
                //encodingParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
                //ImageCodecInfo encodingInfo = barCodeClass.FindCodecInfo("PNG");




                ////Bitmap bm = ResizeImage(smallBitmap, 4857, 12893);
                Bitmap bm = ResizeImage(smallBitmap, 200, 550);
                bm.Save(pngPath, encodingInfo, encodingParams);

                //var width = smallBitmap.Width;
                //var height = smallBitmap.Height;
                ////if (width != 2000)// I resize my bitmap
                ////{
                //    //width = 2000;
                //    //height = 2000 / smallBitmap.Width * height;
                ////}

                //width = width * 3;
                //height = height * 3;

                ////Graphics g = new Graphics();



                //using (var bitmap = svgDocument.Draw(width, height))//I render again
                //{
                //    bitmap.Save(pngPath, ImageFormat.Png);
                //}


            }



            //try
            //{

            //    ImageMagick.MagickImage image = new ImageMagick.MagickImage(@"c:\1.svg");
            //    image.Format = ImageMagick.MagickFormat.Png;
            //    ImageMagick.Density de = new ImageMagick.Density(400, 400);
            //    image.Density = de;

            //    ImageMagick.Percentage pe = new ImageMagick.Percentage(25);
            //    image.Resize(pe);

            //    image.Settings.TextAntiAlias = true;

            //    ImageMagick.MagickColor mc = new ImageMagick.MagickColor(Color.Transparent);
            //    image.Transparent(mc);

            //    ImageMagick.QuantizeSettings a = new ImageMagick.QuantizeSettings();
            //    a.DitherMethod = ImageMagick.DitherMethod.Riemersma;

            //    image.Write(@"c:\Magick.png");
            //}
            //catch (Exception ex)
            //{

            //}
        }
    }
}

/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace DoctaJonez.Drawing.Imaging
{
    /// <summary>
    /// Provides various image untilities, such as high quality resizing and the ability to save a JPEG.
    /// </summary>
    public static class ImageUtilities
    {    
        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> encoders = null;

        /// <summary>
        /// A lock to prevent concurrency issues loading the encoders.
        /// </summary>
        private static object encodersLock = new object();

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    //protect against concurrency issues
                    lock (encodersLock)
                    {
                        //check again, we might not have been the first person to acquire the lock (see the double checked lock pattern)
                        if (encoders == null)
                        {
                            encoders = new Dictionary<string, ImageCodecInfo>();

                            //get all the codecs
                            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                            {
                                //add each codec to the quick lookup
                                encoders.Add(codec.MimeType.ToLower(), codec);
                            }
                        }
                    }
                }

                //return the lookup
                return encoders;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);
            //set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        /// <param name="quality">An integer from 0 to 100, with 100 being the 
        /// highest quality</param> 
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(path, jpegCodec, encoderParams);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        } 
    }
}*/


//<RL><RC360,10><F3><HW1,1>GHOSTWRITER WORLD
//<RC380,76><F6><HW1,1><BS26,44>ALL<F2>
//<F6><BS26,44>THREE<F2>  <F6><BS26,44>PARKS
//<RC348,130><F6><HW1,1><BS42,44>PASSPORT
//<RC324,240><RL><F6><HW2,2>6
//<RC210,240><HW1,1>DAY
//<RC230,290><F3><HW1,1>ADMIT ONE
//<RC230,320><F6><HW1,1>GUEST
//<F1><RC230,370><F1><HW1,1>VERY SMALL PRINT
//<RC24,530><LT2><BX340,50>
//<RC25,528><LT2><VX338>
//<RC216,550><HW1,1><F2>DAY  1
//<RC24,580><LT2><BX340,50>
//<RC216,600>DAY  2
//<RC24,630><LT2><BX340,50>
//<RC216,650>DAY  3
//<RC24,680><LT2><BX340,50>
//<RC216,700>DAY  4
//<RC24,730><LT2><BX340,50>
//<RC25,780><LT2><VX338>
//<RC216,760>DAY  5
//<RC340,400><RL><F6><BS36,44><HW1,1>DAY GUEST
//<RC260,450><F3><HW1,1>$112.00
//<RC240,482><F3>PLUS TAX
//<RC280,1010><F3><HW1,1>12345678
//<RC60,990><NL10><X2>*01000407*
//<RC360,820><F9><HW1,1>VALID ONLY ON DATE STAMPED
//NONTRANSFERABLE NONREFUNDABLE
//<RC280,870><F3><HW1,1>01000407
//<RC20,1079><RR><F3><HW1,1>GHOSTWRITER WORLD
//<p>