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
using System.Net;
using System.Net.Sockets;
using Zen.Barcode;
using System.Drawing.Imaging;

namespace SVGtoIMG.Desktop
{
    public partial class Form1 : Form
    {
        //https://www.codeproject.com/Articles/1063910/WebSocket-Server-in-Csharp Web-Sockets Exaple

        private SVGtoIMG.Converter.Converter c = null;
        private Entities db = null;
        private TransaccionTickets plantilla = null;
        private int k = 0;

        SocketPermission permission;
        Socket sListener;
        IPEndPoint ipEndPoint;
        Socket handler;

        #region Socket
        private void Start_Click()
        {
            try
            {
                // Creates one SocketPermission object for access restrictions
                permission = new SocketPermission(
                NetworkAccess.Accept,     // Allowed to accept connections 
                TransportType.Tcp,        // Defines transport types 
                "",                       // The IP addresses of local host 
                SocketPermission.AllPorts // Specifies all ports 
                );

                // Listening Socket object 
                sListener = null;

                // Ensures the code to have permission to access a Socket 
                permission.Demand();

                // Resolves a host name to an IPHostEntry instance 
                IPHostEntry ipHost = Dns.GetHostEntry("");

                // Gets first IP address associated with a localhost 
                IPAddress ipAddr = ipHost.AddressList[0];

                // Creates a network endpoint 
                ipEndPoint = new IPEndPoint(ipAddr, 4510);

                // Create one Socket object to listen the incoming connection 
                sListener = new Socket(
                    ipAddr.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                    );

                // Associates a Socket with a local endpoint 
                sListener.Bind(ipEndPoint);

                //tbStatus.Text = "Server started.";

                //Start_Button.IsEnabled = false;
                //StartListen_Button.IsEnabled = true;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        private void Listen_Click()
        {
            try
            {
                // Places a Socket in a listening state and specifies the maximum 
                // Length of the pending connections queue 
                sListener.Listen(10);

                // Begins an asynchronous operation to accept an attempt 
                AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                sListener.BeginAccept(aCallback, sListener);

                //tbStatus.Text = "Server is now listening on " + ipEndPoint.Address + " port: " + ipEndPoint.Port;

                //StartListen_Button.IsEnabled = false;
                //Send_Button.IsEnabled = true;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = null;

            // A new Socket to handle remote host communication 
            Socket handler = null;
            try
            {
                // Receiving byte array 
                byte[] buffer = new byte[1024];
                // Get Listening Socket object 
                listener = (Socket)ar.AsyncState;
                // Create a new socket 
                handler = listener.EndAccept(ar);

                // Using the Nagle algorithm 
                handler.NoDelay = false;

                // Creates one object array for passing data 
                object[] obj = new object[2];
                obj[0] = buffer;
                obj[1] = handler;

                // Begins to asynchronously receive data 
                handler.BeginReceive(
                    buffer,        // An array of type Byt for received data 
                    0,             // The zero-based position in the buffer  
                    buffer.Length, // The number of bytes to receive 
                    SocketFlags.None,// Specifies send and receive behaviors 
                    new AsyncCallback(ReceiveCallback),//An AsyncCallback delegate 
                    obj            // Specifies infomation for receive operation 
                    );

                // Begins an asynchronous operation to accept an attempt 
                AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                listener.BeginAccept(aCallback, listener);
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Fetch a user-defined object that contains information 
                object[] obj = new object[2];
                obj = (object[])ar.AsyncState;

                // Received byte array 
                byte[] buffer = (byte[])obj[0];

                // A Socket to handle remote host communication. 
                handler = (Socket)obj[1];

                // Received message 
                string content = string.Empty;


                // The number of bytes received. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    content += Encoding.Unicode.GetString(buffer, 0,
                        bytesRead);

                    // If message contains "<Client Quit>", finish receiving
                    if (content.IndexOf("<Client Quit>") > -1)
                    {
                        // Convert byte array to string
                        string str = content.Substring(0, content.LastIndexOf("<Client Quit>"));
                        ToPrint(str);
                        //this is used because the UI couldn't be accessed from an external Thread
                        //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                        //{
                        //    tbAux.Text = "Read " + str.Length * 2 + " bytes from client.\n Data: " + str;
                        //}
                        //);
                    }
                    else
                    {
                        // Continues to asynchronously receive data
                        byte[] buffernew = new byte[1024];
                        obj[0] = buffernew;
                        obj[1] = handler;
                        handler.BeginReceive(buffernew, 0, buffernew.Length,
                            SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), obj);
                    }

                    //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                    //{
                    //    tbAux.Text = content;
                    //}
                    //);
                }
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        private void Send_Click()
        {
            try
            {
                // Convert byte array to string 
                string str = "Como estas ";

                // Prepare the reply message 
                byte[] byteData =
                    Encoding.Unicode.GetBytes(str);

                // Sends data asynchronously to a connected Socket 
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);

                //Send_Button.IsEnabled = false;
                //Close_Button.IsEnabled = true;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                // A Socket which has sent the data to remote host 
                Socket handler = (Socket)ar.AsyncState;

                // The number of bytes sent to the Socket 
                int bytesSend = handler.EndSend(ar);
                Console.WriteLine(
                    "Sent {0} bytes to Client", bytesSend);
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        private void Close_Click()
        {
            try
            {
                if (sListener.Connected)
                {
                    sListener.Shutdown(SocketShutdown.Receive);
                    sListener.Close();
                }

                //Close_Button.IsEnabled = false;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }
        #endregion

        //https://stackoverflow.com/questions/37226176/winsock-server-client-application-in-c-sharp

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
            m_PrintDocument.PrinterSettings.PrinterName = plantilla.PrinterName;
            m_PrintDocument.DefaultPageSettings.Color = true;
            //m_PrintDocument.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(8, 8, 35, 35);
            papersize = m_PrintDocument.DefaultPageSettings.PaperSize;
            m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
            m_PrintDocument.Print();
        }

        private void m_PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //papersize = new PaperSize("Prueba", 200, 550);

            Bitmap thisTicket = null;
            Image imgFondo = null;
            Rectangle rect = new Rectangle();
            const int aumentoX = 10, aumentoY = 5;
            int w = papersize.Width, h = papersize.Height, lastTicketNumber = 0;
            string numeroText = string.Empty;

            //Cargar los tickets a imprimir en una lista desde el principio e ir removiendo cada una que se valla imprimiendo

            if (plantilla.Tickets.Count() > 0)
            {
                _Ticket ticket = plantilla.Tickets[0];
                var dbTicket = db.Tickets.FirstOrDefault(o => o.Id == ticket.Id);

                numeroText = string.Format("TICKET {0}", ticket.Numero);

                ticket.NumericBarcode = DateTime.Now.ToFileTime().ToString();//"131517994256516479";.Replace("5", "1").Replace("8", "1").Replace("9", "1");// "0773911515056442422"; //

                var black = new SolidBrush(Color.Black);

                string imageText = plantilla.ImagenFondo;
                //string fromBase = String.Format("data:image/png;base64,{0}", imageText);
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(imageText.Replace("data:image/jpeg;base64,", ""));
                    using (MemoryStream ms = new MemoryStream(imageBytes, 0,
                      imageBytes.Length))
                    {
                        ms.Write(imageBytes, 0, imageBytes.Length);

                        //imgFondo = System.Drawing.Image.FromStream(ms, true);
                        thisTicket = new Bitmap(ms, true);
                        thisTicket.SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }


                //// Set world transform of graphics object to translate.
                //e.Graphics.TranslateTransform(w, h);
                //// Then to rotate, prepending rotation matrix.
                //e.Graphics.RotateTransform(180.0F, System.Drawing.Drawing2D.MatrixOrder.Prepend);

                //Bitmap bm = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
                //bm.SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
                //Graphics g = Graphics.FromImage(bm);

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





                ///__________________________
                ///

                //Image barcode = c.getBarcode(1, ticket.NumericBarcode.ToString());

                Zen.Barcode.BarcodeSymbology s = Zen.Barcode.BarcodeSymbology.Code128;
                BarcodeDraw drawObject = BarcodeDrawFactory.GetSymbology(s);

                var metrics = drawObject.GetDefaultMetrics(40);
                metrics.Scale = 1;
                Image barcode = drawObject.Draw(ticket.NumericBarcode.ToString(), metrics);


                //Image barcode = c.GenerateBarCodeTest(ticket.NumericBarcode.ToString());
                ////____________________

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
                            //e.Graphics.DrawImage(barcode, new Rectangle(new Point(posXBarCode, 420), new Size(barcode.Width / 2, barcode.Height / 2)));


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

                //using (Bitmap bmp = new Bitmap(papersize.Width, papersize.Height, e.Graphics))
                //{
                //    EncoderParameters encodingParams = new EncoderParameters();
                //    encodingParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
                //    ImageCodecInfo encodingInfo = c.getFormatData("PNG");
                //    bmp.Save(@"C:\test\enviar\print(1).png", encodingInfo, encodingParams);
                //}

                //e.Graphics.DrawRectangle(new Pen(Color.Black), rect);
                dbTicket.IdEstado = EstadoTicket.IMPRESO;
                dbTicket.NumericBarcode = ticket.NumericBarcode;
                lastTicketNumber = (int)ticket.Numero;
                plantilla.Tickets.Remove(ticket);
                db.SaveChanges();
                if (plantilla.Tickets.Count() >= 1)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            //thisTicket.Dispose();

            var InfTickets = db.TransaccionPullTicketsImpresions.FirstOrDefault(o => o.Id == plantilla.Id);
            InfTickets.NumeroFinal = lastTicketNumber;
            InfTickets.Fecha = DateTime.Now;
            InfTickets.IdEstado = EstadoTicket.IMPRESO;
            db.SaveChanges();

            //MessageBox.Show("Tickes Impresos");
            ReiniciarCoenxion();
            e.HasMorePages = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string s = "";
            //foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    s = printer;
            //}
            Start_Click();
            Listen_Click();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToPrint(Comun.GetMacAddress());

        }

        public void ToPrint(string token)
        {
            var InfTickets = db.TransaccionPullTicketsImpresions.Where(o => o.Token == token && o.IdEstado == EstadoTicket.DISPONIBLE).OrderByDescending(o => o.Id).FirstOrDefault();

            if (InfTickets != null)
            {
                plantilla = new TransaccionTickets();

                plantilla.Id = InfTickets.Id;
                plantilla.IdPullTickets = (int)InfTickets.IdTicketsEvento;
                plantilla.PrinterName = InfTickets.PrinterName;

                var responsable = InfTickets.TicketsEvento.Evento.Empresario;
                plantilla.Responsable = string.Format("RESPONSABLE: {0} NIT: {1}\nTICKETEVENTO NO SE HACE RESPONSABLE DE LA COMERCIALIZACIÓN DE ESTA ENTRADA", responsable.RazonSocial, responsable.Identificacion);
                plantilla.ticketType = plantilla.getTicketType(InfTickets.TicketsEvento.TicketType);
                plantilla.Cantidad = (int)InfTickets.Cantidad;
                plantilla.InicioNumeracion = (int)InfTickets.NumeroInicial;
                plantilla.ImagenFondo = InfTickets.ImagenTicket;

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

                var tickets = db.TicketsEventos.FirstOrDefault(o => o.Id == plantilla.IdPullTickets).Tickets.Where(o => o.IdEstado == EstadoTicket.DISPONIBLE).OrderBy(o => o.Numero).Take(plantilla.Cantidad);
                if (tickets.Any())
                {
                    plantilla.Tickets = new List<_Ticket>();
                    plantilla.Tickets.AddRange(tickets.Select(e => new _Ticket
                    {
                        Id = e.Id,
                        Numero = (int)e.Numero,
                        NumericBarcode = e.NumericBarcode
                    }).ToList());
                }
                doPrintActive();
                //ReiniciarCoenxion();
            }

            //AQui poner el escuchador e imprimir, crear un meodo para guardar los tickets(barcode, numricbarcode y analizar si se guarda el numero del ticket)


            //c.DrawFromSvg(200, 550);
            //c.SvgToImg();
        }

        private void ReiniciarCoenxion()
        {
            try
            {
                Close_Click();
                Start_Click();
                Listen_Click();
            }
            catch
            {
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Close_Click();
        }
    }



}

