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
        /// <summary>
        /// Lists the users.
        /// </summary>
        /// <returns></returns>
        public McApiResponse ListUsers()
        {
            return Call("listUsers", new Dictionary<string, string>());
        }

        /// <summary>
        /// Finds the users.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Invlaid parameters</exception>
        public McApiResponse FindUsers(object field, object value)
        {
            if ((field is string || field is Array) && (value is string || value is Array))
            {
                return Call("findUsers", new Dictionary<string, string>
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

        public McApiResponse GetUser(object id)
        {
            if (id is string || id is int)
            {
                return Call("getUser", new Dictionary<string, string>
                {
                    { "id", id.ToString() }
                });
            }
            else
            {
                throw new Exception("Invlaid parameters");
            }
        }

        public McApiResponse GetCurrentUser()
        {
            return Call("getCurrentUser", new Dictionary<string, string>());
        }
    }
}
