using System;
using System.Web.Mvc;

namespace Services.Helpers
{

    public static class ImageConveter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html">this</param>
        /// <param name="image">byte untuk di konversi ke base 64</param>
        /// <param name="imageId">id object image</param>
        /// <param name="stringClass">class object image</param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper html, byte[] image, string imageId, string stringClass)
        {
            var img = $"data:image/jpg;base64,{Convert.ToBase64String(image)}";
            return new MvcHtmlString("<img src='" + img + "' name='" + imageId.Replace("_", ".") + "' id='" + imageId + "' class='" + stringClass + "'/>");
        }

        public static byte[] ConvertImageToByteArray(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }
    }
}
