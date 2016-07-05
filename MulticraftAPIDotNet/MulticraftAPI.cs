using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MulticraftAPIDotNet
{
     public partial class MulticraftAPI
    {
        public string url { get; set; }
        public string user { get; set; }
        public string key { get; set; }
        public int timeout { get; set; } = 10000;

        public McApiResponse Call(string method, Dictionary<string,string> parameters)
        {
            var str = "";
            var query = "";

            parameters.Add("_MulticraftAPIMethod", method);
            parameters.Add("_MulticraftAPIUser", user);

            var keys = parameters.Keys.ToArray();
            foreach (var parameter in parameters)
            {
                str += parameter.Key + parameter.Value;
                query += "&" + parameter.Key + "=" + parameter.Value;
            }

            //Console.WriteLine("str: " + str);

            return Send(url, query + "&_MulticraftAPIKey=" + GetHMACSHA256(str, key));
        }

        private McApiResponse Send(string url, string query)
        {
            Console.WriteLine("Query: " + query + "\n");
            
            var data = Encoding.ASCII.GetBytes(query);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Timeout = timeout;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //Console.WriteLine("==> " + responseString);

                if (string.IsNullOrEmpty(responseString))
                    throw new Exception("Empty response (wrong API URL or connection problem)");

                return new McApiResponse((JObject)JsonConvert.DeserializeObject(responseString));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetHMACSHA256(string str, string key)
        {
            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();
        }
    }

    public class McApiResponse
    {
        public bool success { get; }
        public List<string> errors { get; }
        public object data { get; }

        public McApiResponse(JObject response)
        {
            success = response["success"].ToObject<bool>();
            errors = response["errors"].ToObject<List<string>>();
            data = new Dictionary<string, string>();

            if (response["data"].HasValues)
            {
                var name = response["data"].First.ToObject<JProperty>().Name;
                switch (name)
                {
                    case "User":
                    case "Users":
                    case "Servers":
                        data = response["data"][name].ToObject<Dictionary<string, string>>();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
