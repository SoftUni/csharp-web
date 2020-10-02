using System;
using System.Threading.Tasks;

namespace SUS.HTTP
{
    public interface IHttpServer
    {
        Task StartAsync(int port);
    }
}
