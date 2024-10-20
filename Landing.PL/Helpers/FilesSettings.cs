using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Landing.PL.Helpers
{
    public class FilesSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            try
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("File upload failed.", ex);
            }
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", folderName, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File deletion failed.", ex);
            }
        }
    }
}
