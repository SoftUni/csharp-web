namespace SIS.MvcFramework
{
    public interface IView
    {
        string GetHtml(object model, string user);
    }
}
