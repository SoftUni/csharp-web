using System.Collections.Generic;
using SIS.Common;
using SIS.HTTP.Common;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.IsNew = true;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public bool IsNew { get; set; }

        public object GetParameter(string parameterName)
        {
            parameterName.ThrowIfNullOrEmpty(nameof(parameterName));

            // TODO: Validation for existing parameter (maybe throw exception)

            return this.sessionParameters[parameterName];
        }

        public bool ContainsParameter(string parameterName)
        {
            parameterName.ThrowIfNullOrEmpty(nameof(parameterName));

            return this.sessionParameters.ContainsKey(parameterName);
        }

        public void AddParameter(string parameterName, object parameter)
        {
            parameterName.ThrowIfNullOrEmpty(nameof(parameterName));
            parameter.ThrowIfNull(nameof(parameter));

            this.sessionParameters[parameterName] = parameter;
        }

        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }
    }
}
