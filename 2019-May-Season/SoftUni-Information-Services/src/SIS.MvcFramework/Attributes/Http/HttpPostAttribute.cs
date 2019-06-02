using SIS.HTTP.Enums;

namespace SIS.MvcFramework.Attributes
{
    public class HttpPostAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Post;
    }
}
