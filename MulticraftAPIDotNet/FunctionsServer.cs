using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulticraftAPIDotNet
{
    public partial class MulticraftAPI
    {
        public McApiResponse ListServers()
        {
            return Call("listServers", new Dictionary<string, string>());
        }

        public McApiResponse FindServers(object field, object value)
        {
            if ((field is string || field is Array) && (value is string || value is Array))
            {
                return Call("findServers", new Dictionary<string, string>
                {
                    { "field",  new JArray(field).ToString() },
                    { "value", new JArray(value).ToString() }
                });
            }
            else
            {
                throw new Exception("Invlaid parameters");
            }
        }
    }
}
