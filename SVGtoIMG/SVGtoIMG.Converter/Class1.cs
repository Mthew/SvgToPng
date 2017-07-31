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

namespace SVGtoIMG.Converter
{
    public class Class1
    {
    }

    public class Converter
    {

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
            string path = @"C:\test\svg2.svg", pngPath = @"C:\test\barcode(1).jpeg";

            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();

            string imageText = System.IO.File.ReadAllText(path);

            XDocument document = XDocument.Load(path);
            xml = ToXmlDocument(document);
            

            //xml.LoadXml(imageText);

            var svgDocument = SvgDocument.Open(xml);
            using (var smallBitmap = svgDocument.Draw())
            {
                //Bitmap bm = ResizeImage(smallBitmap, 4857, 12893);
                Bitmap bm = ResizeImage(smallBitmap, 606, 1669);
                bm.Save(pngPath, ImageFormat.Jpeg);

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
