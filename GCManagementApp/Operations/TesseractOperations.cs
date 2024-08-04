using TesseractOCR.Enums;
using TesseractOCR;

namespace GCManagementApp.Operations
{
    public static class TesseractOperations
    {
        public static string ReadText(byte[] imageBytes)
        {
            using (var engine = new Engine(@"./tessdata", TesseractOCR.Enums.Language.English, EngineMode.Default))
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
