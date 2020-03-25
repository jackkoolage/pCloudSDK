using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static pCloudSDK.Basic;

namespace pCloudSDK
{
    public class AccountClient : IAccount
    {

        #region UserInfo
        public async Task<JSON_UserInfo> UserInfo()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/userinfo", new AuthDictionary()), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_UserInfo>(result, JSONhandler);
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

        #region GetCurrentServer
        public async Task<JSON_GetCurrentServer> GetCurrentServer()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/currentserver", new AuthDictionary()), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_GetCurrentServer>(result, JSONhandler);
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

        #region ChangeMail
        public async Task<bool> ChangeMail(string NewEmail)
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/sendchangemail", new AuthDictionary() { { "newmail", NewEmail } }), System.Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return true;
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

        #region 
        // #Region "ActiveNewMail"
        // ''https://my.pcloud.com/#page=choosenewemail&code=l24FU7ZAL2UZyPvmuFpdhxHAJsVm1elbXzZIiieWwqgY78aGwrIF97JTfIzSMlX
        // Public Async Function GET_ActiveNewMail(ActvationUrl As Uri) As Task(Of Boolean) Implements IAccount.ActiveNewMail
        // Dim parameters = New AuthDictionary
        // parameters.Add("code", ActvationUrl.Get para code)

        // Using localHttpClient As NewHtpClient(New HCHandler)
        // Dim HtpReqMessage As New Net.Http.HttpRequestMessage(Post, New pUri("/changemail"))
        // HtpReqMessage.Content = New FormUrlEncodedContent(parameters)

        // Using response As Net.Http.HttpResponseMessage = Await localHttpClient.SendAsync(HtpReqMessage, Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(False)
        // Dim result As String = Await response.Content.ReadAsStringAsync()

        // If response.StatusCode = Net.HttpStatusCode.OK AndAlso Linq.JObject.Parse(result).Value(Of Integer)("result").Equals(0) Then
        // Return True
        // Else
        // ShowError(result)
        // End If
        // End Using
        // End Using
        // End Function
        // #End Region
        #endregion

        #region ListTokens
        public async Task<List<JSON_TokenMetadata>> ListTokens()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/listtokens", new AuthDictionary()), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_TokenMetadata>>(JObject .Parse(result).SelectToken ("tokens").ToString(), JSONhandler);
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

        #region DeleteToken
        public async Task<bool> DeleteToken(string TokenID)
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/deletetoken", new AuthDictionary() { { "tokenid", TokenID } }), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return true;
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

        #region GetRegistrationPageUrl
        public async Task<Uri> GetRegistrationPageUrl()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/invite", new AuthDictionary()), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return new Uri(JObject.Parse(result).SelectToken("url").ToString());
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

        #region ListInvites
        public async Task<List<JSON_InviteMetadata>> ListInvites()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/userinvites", new AuthDictionary()), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_InviteMetadata>>(JObject.Parse(result).SelectToken ("invites").ToString(), JSONhandler);
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

        #region IP
        public async Task<JSON_IP> IP()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/getip", new AuthDictionary()), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_IP>(result, JSONhandler);
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

        #region Resend Activation Mail
        public async Task<bool> ResendActivationMail()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/sendverificationemail", new AuthDictionary()), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return true ;
                    }
                    else
                    {
                        ShowError(result);
                        return false ;
                    }
                }
            }
        }
        #endregion


    }
}
