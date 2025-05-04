//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;

//namespace ChickenFilmV2.Helpers
//{
//    public class VnPayHelper
//    {
//        // Thông tin từ VNPAY
//        private static string vnp_TmnCode = "VNPAY_MERCHANT_ID";  // Tên mã cửa hàng
//        private static string vnp_HashSecret = "VNPAY_SECRET_KEY";  // Key bảo mật
//        private static string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";  // Địa chỉ API VNPAY
//        private static string vnp_Locale = "vn";  // Ngôn ngữ (vn = Tiếng Việt, en = Tiếng Anh)

//        // Tạo URL thanh toán VNPAY
//        public static string GeneratePaymentUrl(decimal totalAmount, string orderId, string returnUrl)
//        {
//            string transactionDate = DateTime.Now.ToString("yyyyMMddHHmmss");

//            var vnp_Params = new Dictionary<string, string>
//            {
//                { "vnp_Version", "2.1.0" },
//                { "vnp_TmnCode", vnp_TmnCode },
//                { "vnp_Amount", (totalAmount * 100).ToString() },  // Tổng tiền thanh toán (VNPAY yêu cầu tính bằng đồng)
//                { "vnp_Command", "pay" },
//                { "vnp_CreateDate", transactionDate },
//                { "vnp_CurrCode", "VND" },
//                { "vnp_Locale", vnp_Locale },
//                { "vnp_OrderInfo", "Thanh toán vé xem phim" },
//                { "vnp_OrderType", "other" },
//                { "vnp_ReturnUrl", returnUrl },
//                { "vnp_TxnRef", orderId },
//                { "vnp_IpAddr", GetIpAddress() },
//            };

//            var query = vnp_Params
//                .Where(p => p.Value != null)
//                .Select(p => $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}")
//                .Aggregate((p1, p2) => $"{p1}&{p2}");

//            var hashData = vnp_TmnCode + query + vnp_HashSecret;
//            var vnp_SecureHash = ComputeMD5Hash(hashData);

//            return $"{vnp_Url}?{query}&vnp_SecureHash={vnp_SecureHash}";
//        }

//        private static string GetIpAddress()
//        {
//            var context = System.Web.HttpContext.Current;
//            return context.Request.ServerVariables["REMOTE_ADDR"];
//        }

//        private static string ComputeMD5Hash(string input)
//        {
//            using (MD5 md5 = MD5.Create())
//            {
//                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
//                byte[] hashBytes = md5.ComputeHash(inputBytes);
//                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
//            }
//        }

//        // Xác minh mã bảo mật
//        public static bool VerifyPayment(Dictionary<string, string> vnp_Params, string secureHash)
//        {
//            string hashData = "vnp_TmnCode=" + vnp_TmnCode;
//            foreach (var param in vnp_Params)
//            {
//                if (!string.IsNullOrEmpty(param.Value) && param.Key != "vnp_SecureHash")
//                {
//                    hashData += "&" + param.Key + "=" + param.Value;
//                }
//            }
//            hashData += vnp_HashSecret;
//            string calculatedHash = ComputeMD5Hash(hashData);
//            return calculatedHash.Equals(secureHash, StringComparison.InvariantCultureIgnoreCase);
//        }
//    }
//}
