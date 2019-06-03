using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.ViewEngine
{
    public interface IView
    {
        string GetHtml(object model, Principal user);
    }
}
