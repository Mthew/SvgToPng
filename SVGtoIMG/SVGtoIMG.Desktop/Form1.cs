using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SVGtoIMG.Data;
using SVGtoIMG.Converter;

namespace SVGtoIMG.Desktop
{
    public partial class Form1 : Form
    {
        private SVGtoIMG.Converter.Converter c = null;
        private Entities db = null;
        private TicketBase plantilla = null;
        private int k = 0;

        public Form1()
        {
            InitializeComponent();
            c = new Converter.Converter();
            db = new Entities();
            //c.DrawFromSvg();
            //c.SvgToImg();
            //c.DrawPDF();
            //c.example();
            //example();
        }

        private PrintDocument m_PrintDocument;
        private Bitmap barcodeCanvas = null;
        private PaperSize papersize = null;

        public void doPrintActive()
        {
            m_PrintDocument = new PrintDocument();
            m_PrintDocument.PrinterSettings = PrintDialog1.PrinterSettings;
            m_PrintDocument.DefaultPageSettings.Color = true;
            //m_PrintDocument.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(8, 8, 35, 35);
            papersize = m_PrintDocument.DefaultPageSettings.PaperSize;
            m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
            m_PrintDocument.Print();
        }

        private void m_PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            plantilla.ticketType = TicketType.small;

            Bitmap thisTicket = null;
            Image bk = null;
            Rectangle rect = new Rectangle();
            const int aumentoX = 10, aumentoY = 5;
            int w = papersize.Width, h = papersize.Height;
            plantilla.InicioNumeracion += 1;
            plantilla.numericBarcode = DateTime.Now.ToFileTime().ToString();

            if (k < plantilla.Cantidad)
            {
            //e.Graphics.Transform.Rotate(90f, System.Drawing.Drawing2D.MatrixOrder.Prepend);


            // Set world transform of graphics object to translate.
            e.Graphics.TranslateTransform(w, h);

            // Then to rotate, prepending rotation matrix.
            e.Graphics.RotateTransform(180.0F, System.Drawing.Drawing2D.MatrixOrder.Prepend);

            Bitmap bm = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            bm.SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
            Graphics g = Graphics.FromImage(bm);
            




            //e.Graphics.DrawImage(barcode, new PointF(posXBarCode, 420));

            //Bitmap ticketBitmap = new Bitmap(w - 10, h, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            //Graphics dc = Graphics.FromImage(ticketBitmap);

            //dc.Clear(Color.White);

            //thisTicket = new Bitmap(c.DrawFromSvg(w,h)); // aqui se llama el canvas que se va a imprimir

            //dc.DrawImage(thisTicket, new PointF(posXBarCode, 420));



            //e.Graphics.DrawImage(ticketBitmap, rect);




            string numeroText = string.Empty;
            //e.Graphics.DrawImage(barcodeCanvas, rect);

            var black = new SolidBrush(Color.Black);

            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            drawFormat.Alignment = StringAlignment.Center;

            Font fuenteResponsable = new Font("Arial Narrow", 4, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            Font fuenteNumero = new Font("Arial Narrow", 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            Font fuenteNumericBarcode = new Font("Arial Narrow", 9, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            numeroText = string.Format("TICKET {0}", plantilla.InicioNumeracion);

            int posXnumero = Convert.ToInt32((w / 2) - (e.Graphics.MeasureString(numeroText, fuenteNumero).Width / 2));
            int posYResponsable = Convert.ToInt32((h / 2));
            int posXResponsable = Convert.ToInt32((w / 2));
            int posXNumericBarcode = Convert.ToInt32((w / 2) - (e.Graphics.MeasureString(plantilla.numericBarcode, fuenteNumericBarcode).Width / 2));

            Image barcode = c.getBarcode(1, plantilla.numericBarcode);
            int posXBarCode = (w / 2) - (barcode.Width / 2);

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;

            switch (plantilla.ticketType)
            {
                case TicketType.small:
                    {
                        rect = new Rectangle(aumentoX, aumentoY, w - 10, h - 5);

                        //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                        //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        //e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        thisTicket = new Bitmap(@"C:\test\plantilla.png", false);
                        //bk = Image.FromFile(@"C:\test\plantilla.png");
                        e.Graphics.DrawImage(thisTicket, rect);

                        //e.Graphics.DrawImage(bk, rect);

                        e.Graphics.DrawImage(barcode, new PointF(posXBarCode, 420));
                        e.Graphics.DrawString(plantilla.numericBarcode, fuenteNumericBarcode, black, posXNumericBarcode, 460 + aumentoY);

                        e.Graphics.DrawString(numeroText, fuenteNumero, black, posXnumero, 10);
                        e.Graphics.DrawString(numeroText, fuenteNumero, black, posXnumero, h - 25);

                        e.Graphics.DrawString(plantilla.Responsable, fuenteResponsable, black, 8, posYResponsable, drawFormat);
                        break;
                    }
                case TicketType.grande:
                    {
                        rect = new Rectangle(aumentoX, 2, w - 10, h - 5);//larger

                        //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                        //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        //e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                        //bk = Image.FromFile(@"C:\test\plantillaGrande.png");
                        //e.Graphics.DrawImage(thisTicket, rect);
                        e.Graphics.DrawImage(thisTicket, new Rectangle(aumentoX, 2, w - 10, h));
                        //e.Graphics.DrawImage(bk, rect);

                        e.Graphics.DrawImage(barcode, new PointF(posXBarCode, 406));
                        e.Graphics.DrawString(plantilla.numericBarcode, fuenteNumericBarcode, black, posXNumericBarcode, 446 + aumentoY);

                        e.Graphics.DrawString(numeroText, fuenteNumero, black, posXnumero, 8);
                        e.Graphics.DrawString(numeroText, fuenteNumero, black, posXnumero, 496);
                        e.Graphics.DrawString(numeroText, fuenteNumero, black, posXnumero, 563);
                        e.Graphics.DrawString(numeroText, fuenteNumero, black, posXnumero, 674);

                        e.Graphics.DrawString(plantilla.Responsable, fuenteResponsable, black, 8, 210, drawFormat);

                        drawFormat = new StringFormat();
                        drawFormat.Alignment = StringAlignment.Center;
                        e.Graphics.DrawString(plantilla.Responsable, fuenteResponsable, black, posXResponsable, 690, drawFormat);
                        break;
                    }
            }

            //e.Graphics.DrawString((papersize.Height - 10).ToString() + " Puntos", new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 50, papersize.Height - 10);
            //e.Graphics.DrawString((10).ToString() + " Puntos", new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 50, 10);
            //e.Graphics.DrawString("TEST2".ToUpper(), new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), papersize.Width, papersize.Height);
            //e.Graphics.DrawRectangle(new Pen(Color.Black), rect);

            //Rectangle rect2 = new Rectangle(papersize.Width / 2, 20, papersize.Width / 3, papersize.Height);
            //e.Graphics.FillRectangle(new SolidBrush(Color.Black), rect2);

            //for (int i = -20; i < papersize.Height; i += 10)
            //{
            //    e.Graphics.DrawLine(new Pen(Color.Black), new PointF(30, i), new PointF(90, i));
            //    e.Graphics.DrawString((i).ToString(), new Font("Arial", 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 20, i);
            //}

            //e.Graphics.Transform.Rotate(90f, System.Drawing.Drawing2D.MatrixOrder.Prepend);
            //e.Graphics.Flush();

            k += 1;
            e.HasMorePages = true;
            return;
            }
            else
            {
                e.HasMorePages = false;
            }
        }


        public void doPrintNow()
        {
            Bitmap thisBarCode = null;

            //Define a new barcode canvas drawing object with a 8.5" x 11" A4 paper (around there -- we add padding later)
            //barcodeCanvas = New Bitmap(1275, 1650)
            int w = papersize.Width;
            int h = papersize.Height;
            int rk = papersize.RawKind;
            barcodeCanvas = new Bitmap(w - 10, h);

            //Create our trusty graphics object for this canvas
            Graphics dc = Graphics.FromImage(barcodeCanvas);

            //    //Make it white! (the background of the canvas)
            dc.Clear(Color.White);

            //    //Declare our placeholders for coordinates used on drawing the barcode object
            int pgX = 0;
            int pgXc = 0;

            int pgY = 0;

            //Request that the barcode be rendered and exported via memorystream into our new barcode holder
            //thisBarCode = new Bitmap(c.DrawFromSvg(w, h, )); // aqui se llama el canvas que se va a imprimir

            //Draw the barcode onto the barcode printing page canvas
            dc.DrawImage(thisBarCode, new PointF(pgX, pgY));
            //doPrintActive();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            var InfTickets = db.TransaccionPullTicketsImpresions.Where(o => o.Token == "7530A39A-169A-4B83-B476-9AF84A7CECA2" && o.IdEstado == EstadoTicket.DISPONIBLE).OrderByDescending(o => o.Id).FirstOrDefault();

            if (InfTickets != null)
            {
                plantilla = new TicketBase();

                var responsable = InfTickets.TicketsEvento.Evento.Empresario;
                plantilla.Responsable = string.Format("RESPONSABLE: {0} NIT: {1}\nTICKETEVENTO NO SE HACE RESPONSABLE DE LA COMERCIALIZACIÓN DE ESTA ENTRADA", responsable.RazonSocial, responsable.Identificacion);
                plantilla.ticketType = plantilla.getTicketType(InfTickets.TicketsEvento.TicketType);
                plantilla.Cantidad = (int)InfTickets.Cantidad;
                plantilla.InicioNumeracion = (int)InfTickets.NumeroInicial;
                plantilla.numericBarcode = DateTime.Now.ToFileTime().ToString();

                var localidad = InfTickets.TicketsEtapa.Localidad;
                if (localidad != null)
                {
                    plantilla.Localidad = string.Format("Localidad: {0}", localidad.Label);
                }

                var Etapa = InfTickets.TicketsEtapa.EtapasVentaEvento;
                if (localidad != null)
                {
                    plantilla.Etapa = string.Format("Etapa: {0}", Etapa.Nombre);
                }
            }

            //AQui poner el escuchador e imprimir, crear un meodo para guardar los tickets(barcode, numricbarcode y analizar si se guarda el numero del ticket)

            doPrintActive();
            //c.DrawFromSvg(200, 550);
            //c.SvgToImg();
        }

        private void PrepareTicketsToPrint()
        {
            var InfTickets = db.TransaccionPullTicketsImpresions.Where(e => e.Token == "7530A39A-169A-4B83-B476-9AF84A7CECA2" && e.IdEstado == EstadoTicket.DISPONIBLE).OrderByDescending(e => e.Id).FirstOrDefault();
            string nombreLocalidad = string.Empty;
            string nombreEtapa = string.Empty;
            string nombreResponsable = string.Empty;

            if (InfTickets != null)
            {
                var localidad = InfTickets.TicketsEtapa.Localidad;
                if (localidad != null)
                {
                    nombreLocalidad = string.Format("Localidad: {0}", localidad.Label);
                }

                var Etapa = InfTickets.TicketsEtapa.EtapasVentaEvento;
                if (localidad != null)
                {
                    nombreEtapa = string.Format("Etapa: {0}", Etapa.Nombre);
                }

                var responsable = InfTickets.TicketsEvento.Evento.Empresario;
                nombreResponsable = string.Format("RESPONSABLE: {0} NIT: {1}", responsable.RazonSocial, responsable.Identificacion);
            }
        }
    }



}

