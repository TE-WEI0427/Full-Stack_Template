using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace BaseLib
{
    public static class Extension
    {
        #region DataTable

        public static bool IsNullOrEmpty(this DataTable tbl)
        {
            return (tbl == null || 0 == tbl.Rows.Count);
        }

        /// <summary>
        /// DataTable to JsonArray
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="isRowNumDisplay"></param>
        /// <returns></returns>
        public static JsonArray ConverToJsonArray(this DataTable tbl, bool isRowNumDisplay = false)
        {
            JsonArray jArray = new();
            foreach (DataRow row in tbl.Rows)
            {
                JsonObject jObject = new();
                foreach (DataColumn column in tbl.Columns)
                {
                    if (column.ColumnName == "RowNum" && !isRowNumDisplay)
                    {
                        continue;
                    }

                    string text = column.DataType.ToString();
                    string a = text;
                    if (!(a == "System.DateTime"))
                    {
                        if (!(a == "System.Boolean"))
                        {
                            // no action
                        }

                        jObject.Add(column.ColumnName, row[column].ToString());
                        continue;
                    }

                    string text2 = row[column].ToString() ?? "";
                    if (text2.IndexOf("00:00:00") != -1 || text2.IndexOf("上午 12:00:00") != -1)
                    {
                        jObject.Add(column.ColumnName, DateTime.Parse(row[column].ToString() ?? DateTime.Now.ToString()).ToString("yyyy-MM-dd"));
                    }
                    else if (row[column] == DBNull.Value)
                    {
                        jObject.Add(column.ColumnName, "");
                    }
                    else
                    {
                        jObject.Add(column.ColumnName, Convert.ToDateTime(row[column]).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }

                jArray.Add(jObject);
            }

            return jArray;
        }

        #endregion

        #region List

        /// <summary>
        /// List to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null) ?? "";
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #endregion

        #region Number

        /// <summary>
        /// 取得設定長度的亂數
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandoms(this int length)
        {
            string str = "";
            Random random = new();
            for (int i = 0; i < length; i++)
            {
                str += random.Next(0, 10);
            }
            return str;
        }

        #endregion

        #region String
        /// <summary>
        /// 字串的雜湊與鹽值
        /// </summary>
        /// <param name="Value">字串值</param>
        /// <param name="Type">雜湊類型</param>
        /// <param name="HashData">字串雜湊值</param>
        /// <param name="SaltData">字串鹽值</param>
        public static void StringHash(this string Value, Type Type, out byte[] HashData, out byte[] SaltData)
        {
            if (string.IsNullOrEmpty(Value))
            {
                HashData = Array.Empty<byte>();
                SaltData = Array.Empty<byte>();
            }
            else
            {
                if (Type.ToString().Contains("HMACSHA1"))
                {
                    using var hmac = new HMACSHA1();
                    SaltData = hmac.Key;
                    HashData = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value));
                }
                else if (Type.ToString().Contains("HMACSHA256"))
                {
                    using var hmac = new HMACSHA256();
                    SaltData = hmac.Key;
                    HashData = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value));
                }
                else if (Type.ToString().Contains("HMACSHA384"))
                {
                    using var hmac = new HMACSHA384();
                    SaltData = hmac.Key;
                    HashData = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value));
                }
                else
                {
                    using var hmac = new HMACSHA512();
                    SaltData = hmac.Key;
                    HashData = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value));
                }
            }
        }

        /// <summary>
        /// 驗證雜湊字串值
        /// </summary>
        /// <param name="Value">字串值</param>
        /// <param name="Type">雜湊類型</param>
        /// <param name="HashData">字串雜湊值</param>
        /// <param name="SaltData">字串鹽值</param>
        public static bool VerifyStringHash(this string Value, Type Type, byte[] HashData, byte[] SaltData)
        {
            if (string.IsNullOrEmpty(Value)) return false;
            if (SaltData == null || SaltData.Length == 0) return false;
            if (HashData == null || HashData.Length == 0) return false;

            byte[] computedHash = Type.ToString() switch
            {
                "System.Security.Cryptography.HMACSHA1" => new HMACSHA1(SaltData).ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value)),
                "System.Security.Cryptography.HMACSHA256" => new HMACSHA256(SaltData).ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value)),
                "System.Security.Cryptography.HMACSHA384" => new HMACSHA384(SaltData).ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value)),
                "System.Security.Cryptography.HMACSHA512" => new HMACSHA512(SaltData).ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value)),
                _ => new HMACSHA512(SaltData).ComputeHash(System.Text.Encoding.UTF8.GetBytes(Value)),
            };

            return computedHash.SequenceEqual(HashData);
        }

        /// <summary>
        /// Base64Url 字串解碼
        /// </summary>
        /// <param name="base64UrlEncodedString">Base64Url 字串</param>
        /// <returns></returns>
        public static string Base64UrlDecode(this string base64UrlEncodedString)
        {
            // Step 1: Replace URL-specific characters
            string base64String = base64UrlEncodedString.Replace('-', '+').Replace('_', '/');

            // Step 2: Pad with '=' characters if needed
            int padding = (4 - (base64String.Length % 4)) % 4;
            base64String = base64String.PadRight(base64String.Length + padding, '=');

            // Step 3: Decode the Base64 string
            byte[] bytes = Convert.FromBase64String(base64String);
            string decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }

        /// <summary>
        /// 字串進行 Base64Url 編碼
        /// </summary>
        /// <param name="originalString">字串值</param>
        /// <returns></returns>
        public static string Base64UrlEncode(this string originalString)
        {
            // Step 1: Convert the string to Base64
            byte[] bytes = Encoding.UTF8.GetBytes(originalString);
            string base64String = Convert.ToBase64String(bytes);

            // Step 2: Replace characters for URL safety
            string base64UrlEncodedString = base64String
                .Replace('+', '-')
                .Replace('/', '_');

            // Step 3: Remove padding '=' characters
            base64UrlEncodedString = base64UrlEncodedString.TrimEnd('=');

            return base64UrlEncodedString;
        }
        #endregion

        #region class

        /// <summary>
        /// class to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string JsonData<T>(this object t) where T : class
        {
            return JsonConvert.SerializeObject(t);
        }

        #endregion
    }
}
