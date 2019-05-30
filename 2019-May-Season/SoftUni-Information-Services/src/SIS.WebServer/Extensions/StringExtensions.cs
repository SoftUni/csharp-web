namespace SIS.MvcFramework.Extensions
{
    using System.Web;

    public static class StringExtensions
    {
        public static string Decode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }
    }
}