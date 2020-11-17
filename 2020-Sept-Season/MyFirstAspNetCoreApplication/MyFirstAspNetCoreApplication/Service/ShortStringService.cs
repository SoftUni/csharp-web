using Microsoft.AspNetCore.Http;

namespace MyFirstAspNetCoreApplication.Service
{

    public class OtherShortStringImplementation : IShortStringService
    {
        public string GetShort(string str, int maxLength)
        {
            return str;
        }
    }


    public class ShortStringService : IShortStringService
    {
        public string GetShort(string str, int maxLength)
        {
            if (str == null)
            {
                return str;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Substring(0, maxLength) + "...";
        }
    }
}
