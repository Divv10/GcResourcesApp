using System;
using TesseractOCR;
using TesseractOCR.Enums;

namespace GCManagementApp.Operations
{
    public static class TesseractOperations
    {
        public static string ReadText(byte[] imageBytes)
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string tessDataPath = System.IO.Path.Combine(exePath, "tessdata");
            Environment.SetEnvironmentVariable("TESSDATA_PREFIX", null);

            using (var engine = new Engine(tessDataPath, TesseractOCR.Enums.Language.English, EngineMode.Default))
            {
                using (var img = TesseractOCR.Pix.Image.LoadFromMemory(imageBytes))
                {
                    engine.DefaultPageSegMode = PageSegMode.SingleLine;
                    using (var page = engine.Process(img))
                    {
                        return page.Text;
                    }
                }
            }
        }

    }
}
