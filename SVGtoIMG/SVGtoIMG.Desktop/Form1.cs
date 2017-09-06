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

namespace SVGtoIMG.Desktop
{
    public partial class Form1 : Form
    {
        private SVGtoIMG.Converter.Converter c = null;

        public Form1()
        {
            InitializeComponent();
            c = new Converter.Converter();
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
            doPrintNow();
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;
            e.Graphics.DrawImage(barcodeCanvas, 0, 0, papersize.Width, papersize.Height);
            //e.Graphics.DrawString("TEST".ToUpper(), new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 0, 0);
            //e.Graphics.DrawString("TEST1".ToUpper(), new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), 100, 100);
            //e.Graphics.DrawString("TEST2".ToUpper(), new Font("Arial", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), papersize.Width, papersize.Height);
            //e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, papersize.Width - 10, papersize.Height - 10));
            e.HasMorePages = false;
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
            thisBarCode = new Bitmap(c.DrawFromSvg(w, h)); // aqui se llama el canvas que se va a imprimir

            //Draw the barcode onto the barcode printing page canvas
            dc.DrawImage(thisBarCode, new PointF(pgX, pgY));

            //doPrintActive();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //doPrintActive();
            c.DrawFromSvg(200, 550);
            //c.SvgToImg();
        }



    }
}
