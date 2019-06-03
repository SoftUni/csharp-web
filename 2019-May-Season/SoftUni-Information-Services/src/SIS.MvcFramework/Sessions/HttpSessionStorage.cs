using System.Collections.Concurrent;
using SIS.HTTP.Sessions;

namespace SIS.MvcFramework.Sessions
{
    public class HttpSessionStorage : IHttpSessionStorage
    {
        public const string SessionCookieKey = "SIS_ID";

        private readonly ConcurrentDictionary<string, IHttpSession> httpSessions;

        public HttpSessionStorage()
        {
            this.httpSessions = new ConcurrentDictionary<string, IHttpSession>();
        }

        public IHttpSession GetSession(string id)
        {
            return httpSessions.GetOrAdd(id, _ => new HttpSession(id));
        }

        public bool ContainsSession(string id)
        {
            return httpSessions.ContainsKey(id);
        }
    }
}
