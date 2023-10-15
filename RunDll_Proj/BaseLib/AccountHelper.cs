using System.Security.Cryptography;

namespace BaseLib
{
    public class AccountHelper
    {
        /// <summary>
        /// 創建密碼的雜湊與鹽值 (HMACSHA512)
        /// </summary>
        /// <param name="Password">密碼</param>
        /// <param name="PasswordHash">密碼雜湊值</param>
        /// <param name="PasswordSalt">密碼鹽值</param>
        public static void CreatePaswwordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            if (string.IsNullOrEmpty(Password))
            {
                PasswordHash = Array.Empty<byte>();
                PasswordSalt = Array.Empty<byte>();
            }
            else
            {
                Password.StringHash(new HMACSHA512().GetType(), out PasswordHash, out PasswordSalt);

                //using var hmac = new HMACSHA512();
                //PasswordSalt = hmac.Key;
                //PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }

        /// <summary>
        /// 驗證密碼 (HMACSHA512)
        /// </summary>
        /// <param name="Password">密碼</param>
        /// <param name="PasswordHash">密碼雜湊值</param>
        /// <param name="PasswordSalt">密碼鹽值</param>
        /// <returns>是否驗證成功，回傳一個布林值</returns>
        public static bool VerifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            if (string.IsNullOrEmpty(Password)) return false;
            if (PasswordSalt == null || PasswordSalt.Length == 0) return false;
            if (PasswordHash == null || PasswordHash.Length == 0) return false;

            return Password.VerifyStringHash(new HMACSHA512().GetType(), PasswordHash, PasswordSalt);

            //using var hmac = new HMACSHA512(PasswordSalt);
            //var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            //return computedHash.SequenceEqual(PasswordHash);
        }
    }
}
