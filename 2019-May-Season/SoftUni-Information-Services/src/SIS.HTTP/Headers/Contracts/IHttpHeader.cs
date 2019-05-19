namespace SIS.HTTP.Headers.Contracts
{
    public interface IHttpHeader
    {
        string Key { get; }

        string Value { get; }
    }
}
