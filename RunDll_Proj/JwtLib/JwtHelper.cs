using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace JwtLib
{
    public class JwtHelper
    {
        /// <summary>
        /// 創建 Token
        /// </summary>
        /// <typeparam name="T">Class 物件</typeparam>
        /// <param name="value">物件</param>
        /// <param name="ExpiredDays">過期日</param>
        /// <returns>回傳一個Token字串(內容包含創建與過期的時間)</returns>
        public static string CreateToken<T>(T value, double ExpiredDays) where T : class
        {
            if (value == null) return "";
            if (string.IsNullOrEmpty(JwtSettings.Issuer)) return "";

            List<Claim> claims = new();

            var jToken = JToken.FromObject(value);

            // 1. 要加入 token 的資料
            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (FieldInfo info in fieldInfos)
            {
                if (jToken?[info.Name] != null)
                {
                    Claim claim = new(info.Name, jToken?[info.Name]!.ToString() ?? "");

                    if (!claims.Contains(claim))
                        claims.Add(new Claim(info.Name, jToken?[info.Name]!.ToString() ?? ""));
                }
            }

            // 1. 要加入 token 的資料
            PropertyInfo[] infos = typeof(T).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (jToken?[info.Name] != null)
                {
                    Claim claim = new(info.Name, jToken?[info.Name]!.ToString() ?? "");

                    if (!claims.Contains(claim))
                        claims.Add(new Claim(info.Name, jToken?[info.Name]!.ToString() ?? ""));
                }
            }

            // 寫入創建與過期時間
            DateTimeOffset Issued = DateTimeOffset.Now;
            DateTimeOffset Expired = Issued.AddDays(ExpiredDays); // AddDays
            claims.Add(new Claim("Issued", Issued.ToUnixTimeMilliseconds().ToString()));
            claims.Add(new Claim(ClaimTypes.Expired, Expired.ToUnixTimeMilliseconds().ToString()));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // JWT ID

            var userClaimsIdentity = new ClaimsIdentity(claims);

            // 2. SecretKey
            var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JwtSettings.SecretKey));

            // 3. 加密演算法
            var algorithm = SecurityAlgorithms.HmacSha256Signature;

            // 4.生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根據以上，生成 token
            var jwtSecurityToken = new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = JwtSettings.Issuer,
                Audience = JwtSettings.Audience,
                Subject = userClaimsIdentity,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddDays(ExpiredDays), // AddDays
                SigningCredentials = signingCredentials,
            });

            // Generate a JWT securityToken, than get the serialized Token result (string)
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }

        /// <summary>
        /// 驗證Token
        /// </summary>
        /// <param name="Token">Token字串</param>
        /// <returns></returns>
        public static JwtSecurityToken? VerifyToken(string Token)
        {

            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(JwtSettings.SecretKey)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));

            try
            {
                tokenHandler.ValidateToken(Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = (!string.IsNullOrEmpty(JwtSettings.SecretKey)),
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = (!string.IsNullOrEmpty(JwtSettings.Issuer)),
                    ValidIssuer = JwtSettings.Issuer,
                    ValidateAudience = (!string.IsNullOrEmpty(JwtSettings.Audience)),
                    ValidAudience = JwtSettings.Audience,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IDX10223"))
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 取得Token內的資料
        /// </summary>
        /// <typeparam name="T">Class 物件</typeparam>
        /// <param name="jwtSecurityToken"></param>
        /// <returns>必須先進行驗證(VerifyToken)，並回傳物件(包含創建與過期的時間)</returns>
        public static JObject? GetTokenData<T>(JwtSecurityToken jwtSecurityToken) where T : class
        {
            if (jwtSecurityToken == null) return null;

            JObject jo = new();

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (FieldInfo info in fieldInfos)
            {
                jo.Add(info.Name, jwtSecurityToken.Claims.First(x => x.Type == info.Name).Value);
            }

            PropertyInfo[] infos = typeof(T).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                jo.Add(info.Name, jwtSecurityToken.Claims.First(x => x.Type == info.Name).Value);
            }

            jo.Add("Issued", jwtSecurityToken.Claims.First(x => x.Type == "Issued").Value);
            jo.Add("exp", jwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.Expired).Value);

            return jo;
        }

        /// <summary>
        /// 取得Token內的資料
        /// </summary>
        /// <typeparam name="T">Class 物件</typeparam>
        /// <param name="jwtSecurityToken"></param>
        /// <returns>必須先進行驗證(VerifyToken)，並回傳Class物件</returns>
        public static T? GetTokenDataV2<T>(JwtSecurityToken jwtSecurityToken) where T : class
        {
            if (jwtSecurityToken == null) return null;

            JObject jo = new();

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (FieldInfo info in fieldInfos)
            {
                jo.Add(info.Name, jwtSecurityToken.Claims.First(x => x.Type == info.Name).Value);
            }

            PropertyInfo[] infos = typeof(T).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                jo.Add(info.Name, jwtSecurityToken.Claims.First(x => x.Type == info.Name).Value);
            }

            jo.Add("Issued", jwtSecurityToken.Claims.First(x => x.Type == "Issued").Value);
            jo.Add("exp", jwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.Expired).Value);

            string value = JsonConvert.SerializeObject(jo);

            if (value == "") return null;

            T? t = JsonConvert.DeserializeObject<T>(value);

            return t;
        }
    }
}