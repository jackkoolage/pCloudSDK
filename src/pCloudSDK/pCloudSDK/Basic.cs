using Newtonsoft.Json;
using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pCloudSDK
{
    public static class Basic
    {

        public static string APIbase = "https://api.pcloud.com";
        public static TimeSpan m_TimeOut = System.Threading.Timeout.InfiniteTimeSpan;
        public static bool m_CloseConnection = true;
        public static JsonSerializerSettings JSONhandler = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
        public static string authToken = null;
        public static string Username = null;
        public static string Password = null;
        public static ConnectionSettings ConnectionSetting = null;
        public static string TheUserID = null;


        private static ProxyConfig _proxy;
        public static ProxyConfig m_proxy
        {
            get
            {
                return _proxy ?? new ProxyConfig();
            }
            set
            {
                _proxy = value;
            }
        }


        public class HCHandler : System.Net.Http.HttpClientHandler
        {
            public HCHandler() : base()
            {
                if (m_proxy.SetProxy)
                {
                    base.MaxRequestContentBufferSize = 1 * 1024 * 1024;
                    base.Proxy = new System.Net.WebProxy($"http://{m_proxy.ProxyIP}:{m_proxy.ProxyPort}", true, null, new System.Net.NetworkCredential(m_proxy.ProxyUsername, m_proxy.ProxyPassword));
                    base.UseProxy = m_proxy.SetProxy;
                }
            }
        }

        public class HtpClient : System.Net.Http.HttpClient
        {
            public HtpClient(HCHandler HCHandler) : base(HCHandler)
            {
                //base.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                base.DefaultRequestHeaders.UserAgent.ParseAdd("pCloudSDK");
                base.DefaultRequestHeaders.ConnectionClose = m_CloseConnection;
                base.Timeout = m_TimeOut;
            }
            public HtpClient(System.Net.Http.Handlers.ProgressMessageHandler progressHandler) : base(progressHandler)
            {
                //base.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                base.DefaultRequestHeaders.UserAgent.ParseAdd("pCloudSDK");
                base.DefaultRequestHeaders.ConnectionClose = m_CloseConnection;
                base.Timeout = m_TimeOut;
            }
        }

        public class AuthDictionary : Dictionary<string, string>
        {
            public AuthDictionary() : base()
            {
                base.Add("username", Username);
                base.Add("password", Password);
            }
        }

        public class pUri : Uri
        {
            public pUri(string ApiAction, Dictionary<string, string> Parameters) : base(APIbase + ApiAction + Utilitiez.AsQueryString(Parameters)) { }
            public pUri(string ApiAction) : base(APIbase + ApiAction) { }
        }

        public static void ShowError(string result)
        {
            JSON_Error errorInfo = JsonConvert.DeserializeObject<JSON_Error>(result, JSONhandler);
            throw new pCloudException(errorInfo._ErrorMessage, errorInfo._ERRORCODE);
        }

        public static System.Net.Http.HttpContent JsonContent(this object JsonCls)
        {
            return new System.Net.Http.StringContent(JsonConvert.SerializeObject(JsonCls, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
        }

        public static bool ResponseStatus(this string Response)
        {
            return Newtonsoft.Json.Linq.JObject.Parse(Response).Value<int>("result").Equals(0);
        }

        public static AuthDictionary RemoveEmptyValues(this AuthDictionary dictionary)
        {
            var badKeys = dictionary.Where(P => string.IsNullOrEmpty(P.Value)).Select(P => P.Key).ToList();
            badKeys.ForEach(k => dictionary.Remove(k));
            return dictionary;
        }

        public static string GetParameterInUrl(this Uri url,string parameter)
        {
            return System.Net.Http.UriExtensions.ParseQueryString(url).Get(parameter);
            //HttpUtility.ParseQueryString(myUri.Query).Get("param1")
        }
    }
}
