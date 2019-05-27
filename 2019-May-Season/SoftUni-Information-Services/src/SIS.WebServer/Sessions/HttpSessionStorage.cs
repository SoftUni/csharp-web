using System.Collections.Concurrent;
using SIS.HTTP.Sessions;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.WebServer.Sessions
{
    public class HttpSessionStorage
    {
        public const string SessionCookieKey = "SIS_ID";

        private static readonly ConcurrentDictionary<string, IHttpSession> httpSessions =
            new ConcurrentDictionary<string, IHttpSession>();

        public static IHttpSession GetSession(string id)
        {
            return httpSessions.GetOrAdd(id, _ => new HttpSession(id));
        }

        public static bool ContainsSession(string id)
        {
            return httpSessions.ContainsKey(id);
        }

        public static IHttpSession AddOrUpdateSession(string id)
        {
            return httpSessions
                .AddOrUpdate(id, _ => new HttpSession(id), (key, val) => new HttpSession(id));
        }
    }
}
