using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCManagementApp.Helpers;
using Serilog.Core;
using Serilog;
using System.Net;

namespace GCManagementApp.Operations
{
    public static class EmguImageOperations
    {
        public static bool FindImage(string pathToImageToFind, Bitmap image, double confidence)
        {
            Image<Bgr, byte> source = image.ToImage<Bgr, byte>(); // Image B
            Mat template = CvInvoke.Imread(pathToImageToFind, Emgu.CV.CvEnum.ImreadModes.AnyColor); // Image A
            Mat output = new Mat();
            CvInvoke.MatchTemplate(source, template, output, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            double[] maxValues;
            output.MinMax(out _, out maxValues, out _, out _);

            if (maxValues[0] > confidence)
            {
                return true;
            }

            return false;
        }

        public async static Task<Rectangle> FindImage(string pathToImageToFind, Rectangle region, double confidence)
        {
            var game = (await ImageOperations.GetBitmapSourceFromRegion(region.X, region.Y, region.Width, region.Height)).ToBitmap();

            Image<Bgr, byte> source = game.ToImage<Bgr, byte>(); // Image B
            Mat template = CvInvoke.Imread(pathToImageToFind, Emgu.CV.CvEnum.ImreadModes.AnyColor); // Image A
            Mat output = new Mat();
            CvInvoke.MatchTemplate(source, template, output, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            output.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            if (maxValues[0] > confidence)
            {
                // This is a match. Do something with it, for example draw a rectangle around it.
                return new Rectangle(maxLocations[0], template.Size);
            }

            return Rectangle.Empty;
        }

        public static Rectangle FindImageRegion(string pathToImageToFind, Bitmap image, double confidence)
        {
            Image<Bgr, byte> source = image.ToImage<Bgr, byte>(); // Image B
            Mat template = CvInvoke.Imread(pathToImageToFind, Emgu.CV.CvEnum.ImreadModes.AnyColor); // Image A
            Mat output = new Mat();
            CvInvoke.MatchTemplate(source, template, output, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            double[] maxValues;
            Point[] maxLocations;
            output.MinMax(out _, out maxValues, out _, out maxLocations);

            if (maxValues[0] > confidence)
            {
                // This is a match. Do something with it, for example draw a rectangle around it.
                return new Rectangle(maxLocations[0], template.Size);
            }

            return Rectangle.Empty;
        }

        public static bool FindImageWithMask(string pathToImageToFind, Bitmap image, double confidence)
        {
            Image<Bgr, byte> source = image.ToImage<Bgr, byte>(); // Image B
            Mat template = CvInvoke.Imread(pathToImageToFind, Emgu.CV.CvEnum.ImreadModes.AnyColor); // Image A
            Mat output = new Mat();
            CvInvoke.MatchTemplate(source, template, output, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed, template);

            double[] maxValues;
            output.MinMax(out _, out maxValues, out _, out _);

            if (maxValues[0] > confidence)
            {
                Debug.WriteLine($"Result: {maxValues[0]}");
                Log.Logger.Verbose($"Image found with result: {maxValues[0]} / {confidence}");
                return true;
            }

            return false;
        }

        public static double FindImageWithMaskScore(string pathToImageToFind, Bitmap image)
        {
            Image<Bgr, byte> source = image.ToImage<Bgr, byte>(); // Image B
            Mat template = CvInvoke.Imread(pathToImageToFind, Emgu.CV.CvEnum.ImreadModes.AnyColor); // Image A
            Mat output = new Mat();
            CvInvoke.MatchTemplate(source, template, output, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed, template);

            double[] maxValues;
            output.MinMax(out _, out maxValues, out _, out _);

            return maxValues[0];
        }

        public static async Task<Rectangle> FindImageInRepeat(string pathToImageToFind, Rectangle region, double confidence, int repetitions = 1000)
        {
            for (int i = 0; i < repetitions; i++)
            {
                var result = await FindImage(pathToImageToFind, region, confidence);
                if (!result.IsEmpty)
                    return result;

                await Task.Delay(100);

            }
            return Rectangle.Empty;
        }
    }
}
