using System;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using GCManagementApp.Static;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using GCManagementApp.Helpers;

namespace GCManagementApp.Operations
{
    public static class ImageOperations
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public async static Task<BitmapSource> GetBitmapSourceFromScreen()
        {
            var img = await AdbOperations.ScreenShot();
            return img.ToImageSource();
        }

        public async static Task<BitmapSource> GetBitmapSourceFromRegion(int left, int top, int width, int height)
        {
            var img = await AdbOperations.ScreenShot();
            return img.CropImage(new Rectangle(left, top, width, height)).ToImageSource();
        }
    }
}
