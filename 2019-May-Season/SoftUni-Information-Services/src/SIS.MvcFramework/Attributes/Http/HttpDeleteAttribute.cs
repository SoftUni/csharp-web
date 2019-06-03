using SIS.HTTP.Enums;

namespace SIS.MvcFramework.Attributes
{
    public class HttpDeleteAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Delete;
    }
}
