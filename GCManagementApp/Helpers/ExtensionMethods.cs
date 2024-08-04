using GCManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using GCManagementApp.Windows;

namespace GCManagementApp.Helpers
{
    public static class ExtensionMethods
    {
        public static T[] MoveToFront<T>(this T[] mos, Predicate<T> match)
        {
            if (mos.Length == 0)
            {
                return mos;
            }
            var idx = Array.FindIndex(mos, match);
            if (idx == -1)
            {
                return mos;
            }
            var tmp = mos[idx];
            Array.Copy(mos, 0, mos, 1, idx);
            mos[0] = tmp;
            return mos;
        }

        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static void ToFile(this Image image, string path, ImageFormat format)
        {
            using (FileStream fs = File.OpenWrite(path))
            {
                image.Save(fs, format);
            }
        }

        public static Bitmap CropImage(this Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        public static BitmapImage ToImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmapimage.Freeze();

                return bitmapimage;
            }
        }

        public static Bitmap ToBitmap(this BitmapSource source)
        {
            Bitmap bmp = new Bitmap(source.PixelWidth, source.PixelHeight, PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static void DisplayImage(this BitmapSource image, string title = "Hero")
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                ImageWindow wnd = new ImageWindow(image);
                wnd.Title = title;
                wnd.Show();
            });
        }

        public static string GetNumbers(this string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
    }
}
