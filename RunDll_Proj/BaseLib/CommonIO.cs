namespace BaseLib
{
    public class CommonIO
    {
        /// <summary>
        /// 寫 log
        /// </summary>
        /// <param name="filePath">資料夾路徑(不應包含要建立的資料夾)</param>
        /// <param name="fileName">資料夾名稱</param>
        /// <param name="logStr">log字串</param>
        /// <returns></returns>
        public static bool WriteLog(string filePath, string fileName, string logStr)
        {
            try
            {
                if (CreateDirectory(filePath))
                {
                    // Use StreamWriter to create or append to the text file.
                    using StreamWriter writer = new(Path.Combine(filePath, fileName), true);
                    // Write the content to the file.
                    writer.WriteLine(logStr);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 建立資料夾
        /// </summary>
        /// <param name="directoryPath">資料夾路徑</param>
        /// <returns></returns>
        public static bool CreateDirectory(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath) == false)
                {
                    // Use Directory.CreateDirectory to create the directory.
                    Directory.CreateDirectory(directoryPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
