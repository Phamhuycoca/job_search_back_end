using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace job_search_be.Application.Helpers
{
    public class UpLoadImage
    {
        private readonly Cloudinary _cloudinary;
        public UpLoadImage(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        public string ImageUpload(IFormFile? hinhAnh)
        {
            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                using (var stream = hinhAnh.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(hinhAnh.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill"),
                        //Transformation = new Transformation().Width(500).Height(500),
                    };

                    try
                    {
                        var uploadResult = _cloudinary.Upload(uploadParams);
                        return uploadResult.SecureUrl.ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi tải lên hình ảnh: {ex.Message}");
                    }
                }
            }

            return null;
        }
        public void DeleteImage(string imageUrl)
        {
            var data = GetPublicIdFromUrl(imageUrl);

            var deletionParams = new DeletionParams(data)
            {
                Invalidate = true
            };

            try
            {
                var result = _cloudinary.Destroy(deletionParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var publicId = imageUrl.Split('/').Last().Split('.').First();
            return publicId;
        }
    }
}
