namespace SUS.MvcFramework.ViewEngine
{
    public interface IView
    {
        string ExecuteTemplate(object viewModel, string user);
    }
}
