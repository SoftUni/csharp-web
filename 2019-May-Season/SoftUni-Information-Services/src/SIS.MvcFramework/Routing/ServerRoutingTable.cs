using System;
using System.Collections.Generic;
using SIS.Common;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace SIS.MvcFramework.Routing
{
    public class ServerRoutingTable : IServerRoutingTable
    {
        private readonly Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>> routingTable;

        public ServerRoutingTable()
        {
            this.routingTable = new Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>>
            {
                [HttpRequestMethod.Get] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Post] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Put] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Delete] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>()
            };
        }

        public void Add(HttpRequestMethod method, string path, Func<IHttpRequest, IHttpResponse> func)
        {
            method.ThrowIfNull(nameof(method));
            path.ThrowIfNullOrEmpty(nameof(path));
            func.ThrowIfNull(nameof(func));

            this.routingTable[method].Add(path, func);
        }

        public bool Contains(HttpRequestMethod method, string path)
        {
            method.ThrowIfNull(nameof(method));
            path.ThrowIfNullOrEmpty(nameof(path));

            return this.routingTable.ContainsKey(method) && this.routingTable[method].ContainsKey(path);
        }

        public Func<IHttpRequest, IHttpResponse> Get(HttpRequestMethod method, string path)
        {
            method.ThrowIfNull(nameof(method));
            path.ThrowIfNullOrEmpty(nameof(path));

            return this.routingTable[method][path];
        }
    }
}
