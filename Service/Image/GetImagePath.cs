using System.IO;
using System.Web;

namespace Services.Image
{
    public class  Image
    {
        public byte[] ConvertImageToByteArray(string path)
        {
            return File.ReadAllBytes(path);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

    }

}
