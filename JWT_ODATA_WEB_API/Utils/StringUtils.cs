namespace JWT_ODATA_WEB_API.Utils
{
    public class StringUtils
    {
        public static string TruncateAt(string text, int maxWidth)
        {
            var retVal = text;
            if (text.Length > maxWidth)
                retVal = text.Substring(0, maxWidth);

            return retVal;
        }
    }
}