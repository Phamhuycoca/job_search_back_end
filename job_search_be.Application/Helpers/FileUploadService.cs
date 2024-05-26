using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Helpers
{
    public class FileUploadService
    {
        public static string CreatePDF(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileSQL = "";
                var allowedExtensions = new[] { ".pdf" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!Array.Exists(allowedExtensions, ext => ext.Equals(fileExtension)))
                {
                    throw new ApiException(HttpStatusCode.BAD_REQUEST, "Tệp không đúng định dạng");
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PDFs", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    fileSQL = "/PDFs/" + fileName;
                }
                return fileSQL;
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, "Tệp không được bỏ trống");
        }

        public static bool DeletePDF(string filePath)
        {
            try
            {
                string pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + filePath;
                if (!string.IsNullOrEmpty(filePath) && File.Exists(pdfPath))
                {
                    File.Delete(pdfPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string UpdatePDF(IFormFile file, string currentFilePath)
        {
            if (file != null && file.Length > 0)
            {
                string pdfPath = CreatePDF(file);
                if (!string.IsNullOrEmpty(pdfPath))
                {
                    if (DeletePDF(currentFilePath))
                    {
                        return pdfPath;
                    }
                }
            }
            return "";
        }
    }
}
