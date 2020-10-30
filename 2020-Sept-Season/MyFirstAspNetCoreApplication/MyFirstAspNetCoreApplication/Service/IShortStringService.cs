namespace MyFirstAspNetCoreApplication.Service
{
    public interface IShortStringService
    {
        string GetShort(string str, int maxLength);
    }
}
