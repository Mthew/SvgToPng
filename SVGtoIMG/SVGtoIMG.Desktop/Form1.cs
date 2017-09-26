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
using System.IO;

namespace SVGtoIMG.Desktop
{
    public partial class Form1 : Form
    {
        private SVGtoIMG.Converter.Converter c = null;
        private Entities db = null;
        private TicketBase plantilla = null;
        private TransaccionPullTicketsImpresion InfTickets = null;
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
            Bitmap thisTicket = null;
            Rectangle rect = new Rectangle();
            const int aumentoX = 10, aumentoY = 5;
            int w = papersize.Width, h = papersize.Height, lastTicketNumber = 0;
            string numeroText = string.Empty;

            var tickets = db.TicketsEventos.FirstOrDefault(o => o.Id == plantilla.IdPullTickets).Tickets.Where(o => o.Numero > plantilla.InicioNumeracion && o.IdEstado == EstadoTicket.DISPONIBLE).OrderBy(o=>o.Numero).Take(plantilla.Cantidad);
            if (tickets.Any())
            {
                foreach (var ticket in tickets)
                {
                    numeroText = string.Format("TICKET {0}", ticket.Numero);

                    ticket.NumericBarcode = DateTime.Now.ToFileTime();

                    var black = new SolidBrush(Color.Black);

                    string imageText = plantilla.ImagenFondo;
                    //string fromBase = String.Format("data:image/png;base64,{0}", imageText);

                    byte[] imageBytes = Convert.FromBase64String(imageText);
                    using (MemoryStream ms = new MemoryStream(imageBytes, 0,
                      imageBytes.Length))
                    {
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        thisTicket = new Bitmap(ms,true);
                    }

                    // Set world transform of graphics object to translate.
                    e.Graphics.TranslateTransform(w, h);
                    // Then to rotate, prepending rotation matrix.
                    e.Graphics.RotateTransform(180.0F, System.Drawing.Drawing2D.MatrixOrder.Prepend);

                    Bitmap bm = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
                    bm.SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
                    Graphics g = Graphics.FromImage(bm);

                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                    drawFormat.Alignment = StringAlignment.Center;

                    Font fuenteResponsable = new Font("Arial Narrow", 4, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    Font fuenteNumero = new Font("Arial Narrow", 8, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    Font fuenteNumericBarcode = new Font("Arial Narrow", 9, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

                    int posXnumero = Convert.ToInt32((w / 2) - (e.Graphics.MeasureString(numeroText, fuenteNumero).Width / 2));
                    int posYResponsable = Convert.ToInt32((h / 2));
                    int posXResponsable = Convert.ToInt32((w / 2));
                    int posXNumericBarcode = Convert.ToInt32((w / 2) - (e.Graphics.MeasureString(ticket.NumericBarcode.ToString(), fuenteNumericBarcode).Width / 2));

                    Image barcode = c.getBarcode(1, ticket.NumericBarcode.ToString());
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
                                //thisTicket = new Bitmap(@"C:\test\plantilla.png", false);
                                //bk = Image.FromFile(@"C:\test\plantilla.png");
                                e.Graphics.DrawImage(thisTicket, rect);

                                //e.Graphics.DrawImage(bk, rect);

                                e.Graphics.DrawImage(barcode, new PointF(posXBarCode, 420));
                                e.Graphics.DrawString(ticket.NumericBarcode.ToString(), fuenteNumericBarcode, black, posXNumericBarcode, 460 + aumentoY);

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
                                e.Graphics.DrawString(ticket.NumericBarcode.ToString(), fuenteNumericBarcode, black, posXNumericBarcode, 446 + aumentoY);

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

                    //e.Graphics.DrawRectangle(new Pen(Color.Black), rect);
                    ticket.IdEstado = EstadoTicket.IMPRESO;                    
                    lastTicketNumber = (int)ticket.Numero;
                    db.SaveChanges();
                    e.HasMorePages = true;
                    return;
                }
            }

            //thisTicket.Dispose();
            this.InfTickets.NumeroFinal = lastTicketNumber;
            this.InfTickets.Fecha = DateTime.Now;
            db.SaveChanges();
            
            e.HasMorePages = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.InfTickets = db.TransaccionPullTicketsImpresions.Where(o => o.Token == "7530A39A-169A-4B83-B476-9AF84A7CECA2" && o.IdEstado == EstadoTicket.DISPONIBLE).OrderByDescending(o => o.Id).FirstOrDefault();

            if (this.InfTickets != null)
            {
                plantilla = new TicketBase();

                plantilla.IdPullTickets = (int)this.InfTickets.IdTicketsEvento;

                var responsable = this.InfTickets.TicketsEvento.Evento.Empresario;
                plantilla.Responsable = string.Format("RESPONSABLE: {0} NIT: {1}\nTICKETEVENTO NO SE HACE RESPONSABLE DE LA COMERCIALIZACIÓN DE ESTA ENTRADA", responsable.RazonSocial, responsable.Identificacion);
                plantilla.ticketType = plantilla.getTicketType(this.InfTickets.TicketsEvento.TicketType);
                plantilla.Cantidad = (int)this.InfTickets.Cantidad;
                plantilla.InicioNumeracion = (int)this.InfTickets.NumeroInicial;
                plantilla.ImagenFondo = InfTickets.ImagenTicket;

                var localidad = this.InfTickets.TicketsEtapa.Localidad;
                if (localidad != null)
                {
                    plantilla.Localidad = string.Format("Localidad: {0}", localidad.Label);
                }

                var Etapa = this.InfTickets.TicketsEtapa.EtapasVentaEvento;
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
    }



}

