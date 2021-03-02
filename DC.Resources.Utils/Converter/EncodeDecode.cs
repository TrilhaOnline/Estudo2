namespace DC.Resources.Utils.Converter
{
    public class EncodeDecode
    {
        public static string CrashIn(string pass)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(pass);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string CrashOut(string pass)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(pass);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
