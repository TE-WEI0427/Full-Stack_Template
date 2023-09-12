namespace BaseLib
{
    public class CommonUtil
    {
        /// <summary>
        /// 產生亂數
        /// </summary>
        /// <param name="len">亂數長度</param>
        /// <param name="isIncludeNumber">是否包含數字</param>
        /// <param name="isIncludeUpperLetter">是否包含大寫字母</param>
        /// <param name="isIncludeLowerLetter">是否包含小寫字母</param>
        /// <returns></returns>
        public static string GenRandomStr(int len, bool isIncludeNumber = true, bool isIncludeUpperLetter = true, bool isIncludeLowerLetter = true)
        {
            string val = "";
            string str = "";

            str += (isIncludeNumber) ? "1234567890" : "";
            str += (isIncludeUpperLetter) ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ" : "";
            str += (isIncludeLowerLetter) ? "abcdefghijklmnopqrstuvwxyz" : "";

            Random rnd = new();
            for (int i = 0; i < len; i++)
            {
                val += str[rnd.Next(str.Length)].ToString();
            }
            return val;
        }

        /// <summary>
        /// key val 轉成 Json格式的 字串
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string GetStrJsonFormat(Dictionary<string, string> dict)
        {
            string log = "{";
            foreach (var item in dict)
            {
                log += "\"" + item.Key + "\": ";
                log += "\"" + item.Value + "\", ";
            }
            log += "}";
            return log;
        }
    }
}
