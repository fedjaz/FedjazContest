using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace FedjazContest.Tools
{
    public class ImageConverter
    {
        public static MemoryStream Base64ToImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image = Image.Load(bytes);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, PngFormat.Instance);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static string ImageToBase64(string file)
        {
            Image image = Image.Load(file);
            MemoryStream stream = new MemoryStream();
            image.SaveAsPng(stream);
            return Convert.ToBase64String(stream.ToArray());
        }

        public static bool TryImageToBase64(IFormFile formFile, out string result)
        {
            if (TryConvert(formFile, out Image? image) && image != null)
            {
                result = ImageToBase64(image);
                return true;
            }
            else
            {
                result = "";
                return false;
            }
        }

        public static bool TryImageToBase64(IFormFile formFile, int size, out string result)
        {
            if (TryConvert(formFile, out Image? image) && image != null)
            {
                image.Mutate(op =>
                {
                    int min = Math.Min(image.Width, image.Height);
                    op.Crop(min, min);
                    op.Resize(size, size);
                });

                result = ImageToBase64(image);

                return true;
            }
            else
            {
                result = "";
                return false;
            }
        }

        public static void SaveToDisk(string base64, string path)
        {
            MemoryStream stream = Base64ToImage(base64);
            byte[] bytes = stream.ToArray();

            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            fileStream.Write(bytes);
            fileStream.Close();
        }

        private static bool TryConvert(IFormFile formFile, out Image? image)
        {
            try
            {
                Stream stream = formFile.OpenReadStream();
                image = Image.Load(stream);
                return true;
            }
            catch
            {
                image = null;
                return false;
            }
        }

        private static string ImageToBase64(Image image)
        {
            MemoryStream saveStream = new MemoryStream();
            image.SaveAsPng(saveStream);
            return Convert.ToBase64String(saveStream.ToArray());
        }
    }
}
