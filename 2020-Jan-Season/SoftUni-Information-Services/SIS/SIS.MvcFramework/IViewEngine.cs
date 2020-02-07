namespace SIS.MvcFramework
{
    public interface IViewEngine
    {
        string GetHtml(string templateHtml, object model, string user);
    }
}
