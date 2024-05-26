using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Infrastructure.Settings
{
    public class Contants
    {

    }
    public static class CustomClaims
    {
        public const string Permissions = "Permission";
    }
    public static class HttpStatusCode
    {
        public static int OK => 200;
        public static int BAD_REQUEST => 400;
        public static int UNAUTHORIZED => 401;
        public static int FORBIDDEN => 403;
        public static int NOT_FOUND => 404;
        public static int CONFLICT => 409;
        public static int UNPROCESSABLE_ENTITY => 442;
        public static int ITEM_NOT_FOUND => 444;
        public static int ITEM_ALREADY_EXIST => 445;
        public static int ITEM_INVALID => 446;
        public static int INTERNAL_SERVER_ERROR => 500;
        public static int SERVICE_UNAVAILABLE => 503;
    }
    public static class HttpStatusMessages
    {
        public static string OK => "Thành công";
        public static string AddedSuccesfully => "Thêm mới thông tin thành công";
        public static string AddedError => "Thêm mới thông tin không thành công";
        public static string UpdatedSuccessfully => "Cập nhập thông tin thành công";
        public static string UpdatedError=> "Cập nhập thông tin không thành công";
        public static string DeletedSuccessfully => "Xóa thông tin thành công";
        public static string DeletedError => "Xóa thông tin không thành công";
        public static string NotFound => "Không tìm thấy thông tin";
        public static string TokenOrUserNotFound => "Token của người dùng không tìm thấy";
        public static string RefreshTokenNotFound => "Refresh Token không tìm thấy";
        public static string RefreshTokenExpired => "Refresh Token đã hết hạn";
        public static string BadRequest => "Không thể hiển thị danh sách";
        public static string Forbidden => "Không có quyền thực hiện";
        public static string Unauthorized => "Chưa đăng nhập";



    }
}
