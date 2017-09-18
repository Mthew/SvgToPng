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

namespace SVGtoIMG.Desktop
{
    public partial class Form1 : Form
    {
        private SVGtoIMG.Converter.Converter c = null;
        private Entities db = null;

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
            doPrintNow("","","","");
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;

            Rectangle rect = new Rectangle(30, -80, papersize.Width - 50, papersize.Height - 50);
            e.Graphics.DrawImage(barcodeCanvas, rect);

            var InfTickets = db.TransaccionPullTicketsImpresions.OrderByDescending(r => r.Id).FirstOrDefault();
            string nombreLocalidad = string.Empty;
            string nombreEtapa = string.Empty;
            string nombreResponsable = string.Empty;
            string ticketType = string.Empty;

            if (InfTickets != null)
            {
                var localidad = db.Localidads.Where(r => r.Id == InfTickets.IdLocalidad).FirstOrDefault();
                if (localidad != null)
                {
                    nombreLocalidad = string.Format("Localidad: {0}", localidad.Label);
                }

                var Etapa = db.EtapasVentaEventos.Where(r => r.Id == InfTickets.IdEtapa).FirstOrDefault();
                if (localidad != null)
                {
                    nombreEtapa = string.Format("Localidad: {0}", Etapa.Nombre);
                }

                var responsable = InfTickets.TicketsEvento.Evento.Empresario;
                nombreResponsable = string.Format("RESPONSABLE: {0} NIT: {1}\nTICKETEVENTO NO SE HACE RESPONSABLE DE LA COMERCIALIZACIÓN DE ESTA ENTRADA", responsable.RazonSocial, responsable.Identificacion);

                ticketType = InfTickets.TicketsEvento.TicketType;

                int maxTicketNum = InfTickets.TicketsEvento.Tickets.Max(r => (int)r.Numero);
                string numero = string.Empty;

                var black = new SolidBrush(Color.Black);

                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                drawFormat.Alignment = StringAlignment.Center;

                Font fuenteResponsable = new Font("Arial", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                Font fuenteNumero = new Font("Arial", 14, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

                

                for (int i = 0; i < InfTickets.Cantidad; i++)
                {
                    maxTicketNum += 1;
                    numero = string.Format("TICKET {0}", maxTicketNum);

                    int posXnumero = Convert.ToInt32((papersize.Width / 2) - (e.Graphics.MeasureString(numero, fuenteNumero).Width / 2));
                    switch (ticketType)
                    {
                        case "small":
                            e.Graphics.DrawString(numero, fuenteNumero, black, posXnumero, papersize.Height - 50, drawFormat);
                            break;
                        case "grande":
                            e.Graphics.DrawString(numero, fuenteNumero, black, posXnumero, 5, drawFormat);
                            e.Graphics.DrawString(numero, fuenteNumero, black, posXnumero, 5, drawFormat);
                            e.Graphics.DrawString(numero, fuenteNumero, black, posXnumero, 5, drawFormat);
                            break;
                    }
                    e.Graphics.DrawString(nombreResponsable, fuenteResponsable, black, 5, 150, drawFormat);

                }

            }





            //e.Graphics.DrawString((papersize.Height - 10).ToString() + " Puntos", new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 50, papersize.Height - 10);
            //e.Graphics.DrawString((10).ToString() + " Puntos", new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 50, 10);
            //e.Graphics.DrawString("TEST2".ToUpper(), new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), papersize.Width, papersize.Height);
            //e.Graphics.DrawRectangle(new Pen(Color.Black), rect);

            //Rectangle rect2 = new Rectangle(papersize.Width / 2, 20, papersize.Width / 3, papersize.Height);
            //e.Graphics.FillRectangle(new SolidBrush(Color.Black), rect2);

            //for (int i = 0; i < papersize.Height; i += 10)
            //{
            //    e.Graphics.DrawLine(new Pen(Color.Black), new PointF(30, i), new PointF(90, i));
            //    e.Graphics.DrawString((i).ToString(), new Font("Arial", 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 20, i);
            //}

            //e.Graphics.Transform.Rotate(90f, System.Drawing.Drawing2D.MatrixOrder.Prepend);
            //e.Graphics.Flush();
            e.HasMorePages = false;
        }


        public void doPrintNow(string localidad, string etapa, string numero, string responsable)
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
            thisBarCode = new Bitmap(c.DrawFromSvg(w, h)); // aqui se llama el canvas que se va a imprimir

            //Draw the barcode onto the barcode printing page canvas
            dc.DrawImage(thisBarCode, new PointF(pgX, pgY));

            //doPrintActive();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            doPrintActive();
            //c.DrawFromSvg(200, 550);
            //c.SvgToImg();
        }

        private void PrepareTicketsToPrint()
        {
            var InfTickets = db.TransaccionPullTicketsImpresions.OrderByDescending(e => e.Id).FirstOrDefault();
            string nombreLocalidad = string.Empty;
            string nombreEtapa = string.Empty;
            string nombreResponsable = string.Empty;

            if (InfTickets != null)
            {
                var localidad = db.Localidads.Where(e => e.Id == InfTickets.IdLocalidad).FirstOrDefault();
                if (localidad != null)
                {
                    nombreLocalidad = string.Format("Localidad: {0}", localidad.Label);
                }

                var Etapa = db.EtapasVentaEventos.Where(e => e.Id == InfTickets.IdEtapa).FirstOrDefault();
                if (localidad != null)
                {
                    nombreEtapa = string.Format("Localidad: {0}", Etapa.Nombre);
                }

                var responsable = InfTickets.TicketsEvento.Evento.Empresario;
                nombreResponsable = string.Format("RESPONSABLE: {0} NIT: {1}", responsable.RazonSocial, responsable.Identificacion);
            }


            string img = InfTickets.ImagenTicket;

        }
    }



}

