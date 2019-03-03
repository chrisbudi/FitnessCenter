using System;
using System.IO;
using System.Web;

namespace Services.Helpers
{
    public class ConverterImage
    {
        HttpPostedFile postFile;
        static string strFile_Name;
        static byte[] bt;

        # region Converting Image to Byte

        public static byte[] ReadImageFile(string PostedFileName, string[] filetype)
        {
            bool isAllowedFileType = false;
            try
            {
                FileInfo file = new FileInfo(PostedFileName);
                strFile_Name = file.Name;
                foreach (string strExtensionType in filetype)
                {
                    if (strExtensionType == file.Extension)
                    {
                        isAllowedFileType = true;
                        break;
                    }
                }
                if (isAllowedFileType)
                {
                    FileStream fs = new FileStream(PostedFileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    byte[] image = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();
                    return image;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



        #region Converting Btye to Image

        public void byteArrayToImage(byte[] byteArrayIn)
        {
            System.Drawing.Image newImage;
            string strFileName = GetTempFolderName() + strFile_Name;
            using (MemoryStream stream = new MemoryStream(byteArrayIn))
            {
                newImage = System.Drawing.Image.FromStream(stream);
                newImage.Save(strFileName);
            }
        }

        #endregion
        #region Getting Temporary Folder Name
        
        public static string GetTempFolderName()
        {
            string strTempFolderName = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + @"\";
            if (Directory.Exists(strTempFolderName))
            {
                return strTempFolderName;
            }
            else
            {
                Directory.CreateDirectory(strTempFolderName);
                return strTempFolderName;
            }
        }
        #endregion
    }
}