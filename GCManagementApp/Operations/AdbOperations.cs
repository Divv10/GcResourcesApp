using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Models;
using GCManagementApp.Models;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GCManagementApp.Operations
{
    public sealed class AdbOperations
    {
        private static AdbClient client;

        private static DeviceData device;

        private static BlueStackProcess instance;

        private static Random random { get; } = new Random();

        public static async Task Connect(BlueStackProcess _instance)
        {
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                AdbServer server = new AdbServer();
                var adbPath = "adb//adb.exe";// System.Configuration.ConfigurationManager.AppSettings["AdbPath"];
                StartServerResult result = server.StartServer(adbPath, false);
                if (result != StartServerResult.Started)
                {
                    Console.WriteLine("Can't start adb server");
                }
            }

            if (client == null)
            {
                client = new AdbClient();
            }

            await client.ConnectAsync(_instance.InstanceIpAddress);
            device = client.GetDevices().FirstOrDefault();
            instance = _instance;
        }

        public static async Task Disconnect()
        {
            if (device.State != DeviceState.Online)
            {
                return;
            }

            await client.DisconnectAsync(instance.InstanceIpAddress);
            instance = null;
        }

        public static async Task Click(Point coordinates)
        {
            if (client == null || device == null)
            {
                return;
            }

            var randomPt = new Point(coordinates.X + random.Next(-5, 5), coordinates.Y + random.Next(-5, 5));
            await Task.Delay(random.Next(0, 100));
            await client.ClickAsync(device, randomPt);
        }

        public static async Task ClickInRepeat(Point coordinates, int repeats)
        {
            if (client == null || device == null)
            {
                return;
            }

            for (int i = 0; i < repeats; i++)
            {
                var randomPt = new Point(coordinates.X + random.Next(-5, 5), coordinates.Y + random.Next(-5, 5));
                await Task.Delay(random.Next(200, 300));
                await client.ClickAsync(device, randomPt);
            }
        }

        public static async Task Swipe(Point from, Point to, long speed, bool useBatchCommand = false)
        {
            if (client == null || device == null)
            {
                return;
            }

            if (!useBatchCommand)
            {
                await client.SwipeAsync(device, from, to, speed);
                return;
            }

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb");
            startInfo.FileName = "cmd.exe";
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = $"/C {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb")}/adb shell input swipe {from.X} {from.Y} {to.X} {to.Y} {speed}";
            process.StartInfo = startInfo;
            process.Start();
        }

        public static async Task<Bitmap> ScreenShot()
        {
            if (client == null || device == null)
            {
                return null;
            }

            Framebuffer fm = await client.GetFrameBufferAsync(device);
            return ToImage(fm);
        }

        public static Bitmap ToImage(byte[] buffer, Framebuffer fb)
        {
            // This happens, for example, when DRM is enabled. In that scenario, no screenshot is taken on the device and an empty
            // framebuffer is returned; we'll just return null.
            if (fb.Header.Width == 0 || fb.Header.Height == 0 || fb.Header.Bpp == 0)
            {
                return null;
            }

            // The pixel format of the framebuffer may not be one that .NET recognizes, so we need to fix that
            PixelFormat pixelFormat = StandardizePixelFormat(ref buffer, fb);

            Bitmap bitmap = new((int)fb.Header.Width, (int)fb.Header.Height, pixelFormat);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, pixelFormat);
            Marshal.Copy(buffer, 0, bitmapData.Scan0, (int)fb.Header.Size);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        private static PixelFormat StandardizePixelFormat(ref byte[] buffer, Framebuffer fb)
        {
            if (buffer.Length < fb.Header.Width * fb.Header.Height * (fb.Header.Bpp / 8))
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), $"The buffer length {buffer.Length} is less than expected buffer " +
                    $"length ({fb.Header.Width * fb.Header.Height * (fb.Header.Bpp / 8)}) for a picture of width {fb.Header.Width}, height {fb.Header.Height} and pixel depth {fb.Header.Bpp}");
            }

            if (fb.Header.Width == 0 || fb.Header.Height == 0 || fb.Header.Bpp == 0)
            {
                throw new InvalidOperationException("Cannot cannulate the pixel format of an empty framebuffer");
            }

            // By far, the most common format is a 32-bit pixel format, which is either
            // RGB or RGBA, where each color has 1 byte.
            if (fb.Header.Bpp == 8 * 4)
            {
                // Require at least RGB to be present; and require them to be exactly one byte (8 bits) long.
                if (fb.Header.Red.Length != 8 || fb.Header.Blue.Length != 8 || fb.Header.Green.Length != 8)
                {
                    throw new ArgumentOutOfRangeException($"The pixel format with with RGB lengths of {fb.Header.Red.Length}:{fb.Header.Blue.Length}:{fb.Header.Green.Length} is not supported");
                }

                // Alpha can be present or absent, but must be 8 bytes long
                if (fb.Header.Alpha.Length is not (0 or 8))
                {
                    throw new ArgumentOutOfRangeException($"The alpha length {fb.Header.Alpha.Length} is not supported");
                }

                // Gets the index at which the red, bue, green and alpha values are stored.
                int redIndex = (int)fb.Header.Red.Offset / 8;
                int blueIndex = (int)fb.Header.Blue.Offset / 8;
                int greenIndex = (int)fb.Header.Green.Offset / 8;
                int alphaIndex = (int)fb.Header.Alpha.Offset / 8;

                byte[] array = new byte[(int)fb.Header.Size * 4];
                // Loop over the array and re-order as required
                for (int i = 0; i < (int)fb.Header.Size; i += 4)
                {
                    byte red = buffer[i + redIndex];
                    byte blue = buffer[i + blueIndex];
                    byte green = buffer[i + greenIndex];
                    byte alpha = buffer[i + alphaIndex];

                    // Convert to ARGB. Note, we're on a little endian system,
                    // so it's really BGRA. Confusing!
                    if (fb.Header.Alpha.Length == 8)
                    {
                        array[i + 3] = alpha;
                        array[i + 2] = red;
                        array[i + 1] = green;
                        array[i + 0] = blue;
                    }
                    else
                    {
                        array[i + 3] = 0;
                        array[i + 2] = red;
                        array[i + 1] = green;
                        array[i + 0] = blue;
                    }
                }
                buffer = array;

                // Returns RGB or RGBA, function of the presence of an alpha channel.
                return fb.Header.Alpha.Length == 0 ? PixelFormat.Format32bppRgb : PixelFormat.Format32bppArgb;
            }
            else if (fb.Header.Bpp == 8 * 3)
            {
                // For 24-bit image depths, we only support RGB.
                if (fb.Header.Red.Offset == 0
                    && fb.Header.Red.Length == 8
                    && fb.Header.Green.Offset == 8
                    && fb.Header.Green.Length == 8
                    && fb.Header.Blue.Offset == 16
                    && fb.Header.Blue.Length == 8
                    && fb.Header.Alpha.Offset == 24
                    && fb.Header.Alpha.Length == 0)
                {
                    return PixelFormat.Format24bppRgb;
                }
            }
            else if (fb.Header.Bpp == 5 + 6 + 5
            && fb.Header.Red.Offset == 11
                     && fb.Header.Red.Length == 5
                     && fb.Header.Green.Offset == 5
                     && fb.Header.Green.Length == 6
                     && fb.Header.Blue.Offset == 0
                     && fb.Header.Blue.Length == 5
                     && fb.Header.Alpha.Offset == 0
                     && fb.Header.Alpha.Length == 0)
            {
                // For 16-bit image depths, we only support Rgb565.
                return PixelFormat.Format16bppRgb565;
            }

            // If not caught by any of the statements before, the format is not supported.
            throw new NotSupportedException($"Pixel depths of {fb.Header.Bpp} are not supported");
        }

        public static Bitmap ToImage(Framebuffer fb)
        {
            return fb.Data == null ? throw new InvalidOperationException($"Call {nameof(fb.Refresh)} first") : ToImage(fb.Data, fb);
        }
    }
}
