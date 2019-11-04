using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using USC.GISResearchLab.Common.Utils.Colors;
using USC.GISResearchLab.Common.Utils.Files;

namespace USC.GISResearchLab.Common.Utils.Web.Images
{
    public class ImagesUtils
    {

        public static ImageFormat GetImageFormat(string url)
        {
            switch (FileUtils.GetExtension(url))
            {
                case "JPG":
                case "JPEG":
                    return ImageFormat.Jpeg;
                case "PNG":
                    return ImageFormat.Png;
                case "GIF":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Bmp;
            }
        }

        public static Bitmap CreateColorOpenBoxWhiteBitmap(int width, int height, string colorName)
        {
            Bitmap ret = null;
            Color color = Color.FromName(colorName);
            if (color != null)
            {
                ret = CreateColorOpenBoxWhiteBitmap(width, height, color);
            }
            return ret;
        }

        public static Bitmap CreateHexColorOpenBoxWhiteBitmap(int width, int height, string hexColor)
        {
            Bitmap ret = null;
            Color color = ColorUtils.HexToColor(hexColor);
            if (color != null)
            {
                ret = CreateColorOpenBoxWhiteBitmap(width, height, color);
            }
            return ret;
        }

        public static Bitmap CreateColorOpenBoxWhiteBitmap(int width, int height, Color boxColor)
        {
            Bitmap ret = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ret);
            g.Clear(Color.White);

            Color color = boxColor;

            Pen p = new Pen(color, 2);
            Point topLeft = new Point(3, 3);
            Point topRight = new Point(width - 3, 3);
            Point bottomLeft = new Point(3, height - 3);
            Point bottomRight = new Point(width - 3, height - 3);
            g.DrawLine(p, topLeft, bottomLeft);
            g.DrawLine(p, topLeft, topRight);
            g.DrawLine(p, bottomLeft, bottomRight);
            g.Flush();
            return ret;
        }

        public static Bitmap CreateColorBoxWhiteBitmap(int width, int height, string colorName)
        {
            Bitmap ret = null;
            Color color = Color.FromName(colorName);
            if (color != null)
            {
                ret = CreateColorBoxWhiteBitmap(width, height, color);
            }
            return ret;
        }

        public static Bitmap CreateHexColorBoxWhiteBitmap(int width, int height, string hexColor)
        {
            Bitmap ret = null;
            Color color = ColorUtils.HexToColor(hexColor);
            if (color != null)
            {
                ret = CreateColorBoxWhiteBitmap(width, height, color);
            }
            return ret;
        }

        public static Bitmap CreateColorBoxWhiteBitmap(int width, int height, Color boxColor)
        {
            Bitmap ret = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ret);
            g.Clear(Color.White);

            Color color = boxColor;

            Pen p = new Pen(color, 2);
            Rectangle rect = new Rectangle(3, 3, width - 6, height - 6);
            g.DrawRectangle(p, rect);
            g.Flush();


            //// below is from: http://www.bobpowell.net/giftransparency.htm
            //Image gifImage = (Image)bitmap;
            //ColorPalette cp = gifImage.Palette;
            //Bitmap ret = null;

            //// just making up a color here
            //int CurrentEntry = 0;
            ////int yPos = (int)(((float)5) / (((float)144) / 16f));
            ////int xPos = (int)(((float)5) / (((float)144) / 16f));

            ////CurrentEntry = (int)((16 * yPos) + xPos);

            //if (cp != null)
            //{

            //        //if (CurrentEntry >= cp.Entries.Length)
            //        //    CurrentEntry = cp.Entries.Length - 1;

            //    ret = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            //    ColorPalette ncp = ret.Palette;
            //    //copy all the entries from the old palette removing any transparency
            //    int n = 0;
            //    foreach (Color c in cp.Entries)
            //        ncp.Entries[n++] = Color.FromArgb(255, c);

            //    //Set the newly selected transparency
            //    ncp.Entries[CurrentEntry] = Color.FromArgb(0, cp.Entries[CurrentEntry]);
            //    //re-insert the palette
            //    ret.Palette = ncp;

            //    //now to copy the actual bitmap data
            //    //lock the source and destination bits
            //    BitmapData src = ((Bitmap)gifImage).LockBits(new Rectangle(0, 0, gifImage.Width, gifImage.Height), ImageLockMode.ReadOnly, gifImage.PixelFormat);
            //    BitmapData dst = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height), ImageLockMode.WriteOnly, ret.PixelFormat);
            //    //uses pointers so we need unsafe code.
            //    //the project is also compiled with /unsafe
            //    unsafe
            //    {
            //        //steps through each pixel
            //        for (int y = 0; y < gifImage.Height; y++)
            //            for (int x = 0; x < gifImage.Width; x++)
            //            {
            //                //transferring the bytes
            //                ((byte*)dst.Scan0.ToPointer())[(dst.Stride * y) + x] = ((byte*)src.Scan0.ToPointer())[(src.Stride * y) + x];
            //            }
            //    }

            //    //all done, unlock the bitmaps
            //    ((Bitmap)bitmap).UnlockBits(src);
            //    ret.UnlockBits(dst);
            //}

            return ret;
        }




        public static Bitmap CreateGraphicForWord(string word, int width, int height, string font, string color, string bcolor, bool btransparent)
        {
            SolidBrush brush = new SolidBrush(Color.FromName(color));
            Bitmap ret = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ret);
            //find a font size that will fit in the bitmap
            bool foundfont = false;
            int fontsize = 50;
            SizeF sizeofstring;
            g.Clear(Color.FromName(bcolor));
            //find a font size that will fit in the bitmap
            while (foundfont == false)
            {
                Font fc = new Font(font, fontsize, System.Drawing.FontStyle.Bold);
                sizeofstring = new SizeF(width, height);
                sizeofstring = g.MeasureString(word, fc);
                if (sizeofstring.Width < ret.Width)
                {
                    if (sizeofstring.Height < ret.Height)
                    {
                        foundfont = true;
                        g.DrawString(word, fc, brush, 1, 1);
                    }
                    else
                    {
                        fontsize--;
                    }
                }
                else
                {
                    fontsize--;
                }

            }
            if (btransparent == true)
            {
                ret.MakeTransparent(Color.FromName(bcolor));
            }
            return ret;
        }

        public static Image GetImage(string url)
        {
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse wRes = (HttpWebResponse)(wReq).GetResponse();
            Stream wStr = wRes.GetResponseStream();
            return Image.FromStream(wStr);
        }

        public static byte[] ImageToByteArray(Image image, ImageFormat imageFormat)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, imageFormat);
            return stream.ToArray();
        }

        public static Image ImageFromByteArray(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            Image ret = Image.FromStream(stream);
            return ret;
        }

        public static ImageFormat GetFormat(string filename)
        {
            ImageFormat ret = null;

            if (filename != null)
            {
                string extension = FileUtils.GetExtension(filename);
                if (extension != null && extension != "")
                {
                    switch (extension.ToLower())
                    {
                        case ".bmp":
                            ret = ImageFormat.Bmp;
                            break;
                        case ".emf":
                            ret = ImageFormat.Emf;
                            break;
                        case ".exif":
                            ret = ImageFormat.Exif;
                            break;
                        case ".gif":
                            ret = ImageFormat.Gif;
                            break;
                        case ".icon":
                        case ".ico":
                            ret = ImageFormat.Icon;
                            break;
                        case ".jpeg":
                        case ".jpg":
                            ret = ImageFormat.Jpeg;
                            break;
                        case ".png":
                            ret = ImageFormat.Png;
                            break;
                        case ".tiff":
                            ret = ImageFormat.Tiff;
                            break;
                        case ".wmf":
                            ret = ImageFormat.Wmf;
                            break;
                    }

                }
            }

            return ret;
        }

        public static string GetExtension(ImageFormat imageFormat)
        {
            string ret = "";

            if (imageFormat != null)
            {

                if (imageFormat == ImageFormat.Bmp)
                    ret = ".bmp";

                else if (imageFormat == ImageFormat.Emf)
                    ret = ".emf";

                else if (imageFormat == ImageFormat.Exif)
                    ret = ".exif";

                else if (imageFormat == ImageFormat.Gif)
                    ret = ".gif";

                else if (imageFormat == ImageFormat.Icon)
                    ret = ".icon";

                else if (imageFormat == ImageFormat.Jpeg)
                    ret = ".jpeg";

                else if (imageFormat == ImageFormat.Png)
                    ret = ".png";

                else if (imageFormat == ImageFormat.Tiff)
                    ret = ".tiff";

                else if (imageFormat == ImageFormat.Wmf)
                    ret = ".wmf";

            }

            return ret;
        }


        public static void fetchAndSaveImage(string url, string savePath)
        {
            if (null != url)
            {
                Image img = GetImage(url);
                img.Save(savePath, ImageFormat.Gif);
            }
        }
    }
}
