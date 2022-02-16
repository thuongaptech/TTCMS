using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace TTCMS.Core.Images
{
    public class ResizeImage
    {
        /// <summary>
        /// Gets an image from the specified URL.
        /// </summary>
        /// <param name="url">The URL containing an image.</param>
        /// <returns>The image as a bitmap.</returns>
       public static Bitmap GetImageFromUrl(string url)
        {
            var buffer = 1024;
            Bitmap image = null;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return image;

            using (var ms = new MemoryStream())
            {
                var req = WebRequest.Create(url);

                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        var bytes = new byte[buffer];
                        var n = 0;

                        while ((n = stream.Read(bytes, 0, buffer)) != 0)
                            ms.Write(bytes, 0, n);
                    }
                }

                image = Bitmap.FromStream(ms) as Bitmap;
            }

            return image;
        }

        /// <summary>
        /// Gets the filename that is placed under a certain URL.
        /// </summary>
        /// <param name="url">The URL which should be investigated for a file name.</param>
        /// <returns>The file name.</returns>
        public static string GetUrlFileName(string url)
        {
            var parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var last = parts[parts.Length - 1];
            return Path.GetFileNameWithoutExtension(last);
        }

        /// <summary>
        /// Creates a small image out of a larger image.
        /// </summary>
        /// <param name="original">The original image which should be cropped (will remain untouched).</param>
        /// <param name="x">The value where to start on the x axis.</param>
        /// <param name="y">The value where to start on the y axis.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <returns>The cropped image.</returns>
        public static Bitmap CreateImage(Bitmap original, int x, int y, int width, int height)
        {
            var img = new Bitmap(width, height);

            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }

            return img;
        }
        public static Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                g.DrawImage(b, 0, 0, nWidth, nHeight);
            return result;
        }
        public static Bitmap ResizeBitmapIsMax(Bitmap b, int max_width, int max_height)
        {
            int new_width = b.Width;
            int new_height = b.Height;
            if (new_width > max_width)
            {
                float ratio = (float)max_width / (float)new_width;
                new_width = (int)(new_width * ratio);
                new_height = (int)(new_height * ratio);
            }
            else if (new_height > max_height)
            {
                float ratio = (float)max_height / (float)new_height;
                new_width = (int)(new_width * ratio);
                new_height = (int)(new_height * ratio);
            }
            Bitmap result = new Bitmap(max_width, max_height);

            using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                g.DrawImage(b, 0, 0, max_width, max_height);
            return result;
        }
        public static string ResizeByMaxWidth(string dir, HttpPostedFileBase file, int max_width, int max_height)
        {
            string filename = file.FileName;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string tmp = Path.GetRandomFileName().Substring(0, 3);
            filename = EscapeName.Renamefile(tmp + "-" + filename);
            System.Drawing.Image bm = System.Drawing.Image.FromStream(file.InputStream);
            // calculate new width and height
            int new_width = bm.Width;
            int new_height = bm.Height;
            if (new_width > max_width)
            {
                float ratio = (float)max_width / (float)new_width;
                new_width = (int)(new_width * ratio);
                new_height = (int)(new_height * ratio);
            }
            else if (new_height > max_height)
            {
                float ratio = (float)max_height / (float)new_height;
                new_width = (int)(new_width * ratio);
                new_height = (int)(new_height * ratio);
            }
            bm = ResizeBitmap((Bitmap)bm, new_width, new_height);
            bm.Save(Path.Combine(dir, filename));
            return filename;
        }

        public static void Resize(string path, string subfolder, HttpPostedFileBase file, string filenamefinal, int nWidth, int nHeight)
        {
            string dir = string.Empty;
            string filename = file.FileName;
            if (!String.IsNullOrEmpty(subfolder))
            {
                dir = path + "\\" + subfolder;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            if (!String.IsNullOrEmpty(filenamefinal))
            {
                filename = filenamefinal;
            }
            System.Drawing.Image bm = System.Drawing.Image.FromStream(file.InputStream);
            bm = ResizeBitmap((Bitmap)bm, nWidth, nHeight);
            bm.Save(Path.Combine(dir, filename));
        }
        public static void UploadImage(string url, string filename)
        {
            if (!String.IsNullOrEmpty(url))
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, filename);
                }
            }

        }
        public static void ChenLogoVideo(string imagesfile, string logo, string savelink)
        {
            Bitmap myBitmap = new Bitmap(imagesfile);
            Graphics myGraphics = Graphics.FromImage(myBitmap);
            Bitmap myGraphicsLogo = new Bitmap(logo);
            myGraphics.DrawImage(myGraphicsLogo, new Point(280, 200));
            //save
            myBitmap.Save(savelink);
        }
    }
}
