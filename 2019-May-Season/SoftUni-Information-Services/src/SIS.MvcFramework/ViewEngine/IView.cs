using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.ViewEngine
{
    using Validation;

    public interface IView
    {
        string GetHtml(object model, ModelStateDictionary modelState, Principal user);
    }
}
