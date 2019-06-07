using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.ViewEngine
{
    using Validation;

    public interface IViewEngine
    {
        string GetHtml<T>(string viewContent, T model, ModelStateDictionary modelState, Principal user);
    }
}
