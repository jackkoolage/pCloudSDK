using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static pCloudSDK.Basic;

namespace pCloudSDK
{
    public  class Authentication
    {

        #region Get Auth Token
        /// <summary>
        /// get auth token from email and passeord
        /// </summary>
        public static  async Task<string> GetAuthToken(string Email, string Password)
        {
            var parameters = new AuthDictionary();
            parameters.Add("username", WebUtility.UrlEncode(Email));
            parameters.Add("password", WebUtility.UrlEncode(Password));
            parameters.Add("getauth", "1");
            parameters.Add("logout", "1");

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/userinfo"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JObject.Parse(result).SelectToken("auth").ToString();
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region Signup
        /// <summary>
        /// return userid
        /// email: https://www.pcloud.com/track?url=aHR0cHM6Ly9teS5wY2xvdWQuY29tLyNwYWdlPXZlcmlmeW1haWwmY29kZT16OU8wN1pNUkI3TkVXQUZqam5MN08wemRzZ0ZwSWtvc3Z5&token=j7yZz9O07Z7Z87ZgG0QX2Mk5Nyh6tIdzsvnsmIwW0UV
        /// </summary>
        public static  async Task<string> Register(string Email, string Password, string inviteCode = null)
        {
            var parameters = new Dictionary<string,string>();
            parameters.Add("mail", Email);
            parameters.Add("password", Password);
            if (!string.IsNullOrEmpty(inviteCode)) { parameters.Add("invite", inviteCode); }
            parameters.Add("termsaccepted", "yes");

            using (HttpClient localHttpClient = new HttpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/register"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JObject.Parse(result).SelectToken("userid").ToString();
                    }
                    else
                    {
                        ShowError(result);
                    }

                    return null;
                }
            }
        }
        #endregion

        #region Logout
        /// <summary>
        /// Logout is the process when the user leaves your application. To make this remove your application's auth cookie and delete all user's information
        /// https://docs.pcloud.com/methods/auth/logout.html
        /// </summary>
        public async Task<bool> Logout(string AuthToken)
        {
            using (HttpClient localHttpClient = new HttpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri($"/logout?auth={ AuthToken }"));
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return Convert.ToBoolean(JObject.Parse(result).SelectToken("auth_deleted").ToString());
                    }
                    else
                    {
                        ShowError(result);
                        return false;
                    }
                }
            }
        }
        #endregion
    }
}
