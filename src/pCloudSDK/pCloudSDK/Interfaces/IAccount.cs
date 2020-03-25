using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pCloudSDK
{
   public  interface IAccount
    {

        /// <summary>
        /// Get the IP address of the remote device from which the user connects to the API
        /// https://docs.pcloud.com/methods/general/getip.html
        /// </summary>
        Task<JSON_IP> IP();

        /// <summary>
        /// Get a list of the invitations.
        /// https://docs.pcloud.com/methods/auth/userinvites.html
        /// </summary>
        Task<List<JSON_InviteMetadata>> ListInvites();

        /// <summary>
        /// Get url of a registration page with a referrer code that credits free space to user account upon user registration
        /// https://docs.pcloud.com/methods/auth/invite.html
        /// </summary>
        Task<Uri> GetRegistrationPageUrl();

        /// <summary>
        /// returns [ip] and [hostname] of the server you are currently connected to. The hostname is guaranteed to resolve only to the IP address(es) pointing to the same server. This call is useful when you need to track the upload progress
        /// https://docs.pcloud.com/methods/general/currentserver.html
        /// </summary>
        Task<JSON_GetCurrentServer> GetCurrentServer();

        /// <summary>
        /// Delete (invalidate) an authentication token
        /// https://docs.pcloud.com/methods/auth/deletetoken.html
        /// </summary>
        /// <param name="TokenID">This is recieved from [ListTokens]</param>
        Task<bool> DeleteToken(string TokenID);

        /// <summary>
        /// Get a list with the currently active tokens associated with the current user
        /// https://docs.pcloud.com/methods/auth/listtokens.html
        /// </summary>
        Task<List<JSON_TokenMetadata>> ListTokens();

        /// <summary>
        /// Sends email to the logged in user with link
        /// https://docs.pcloud.com/methods/auth/sendchangemail.html
        /// </summary>
        Task<bool> ChangeMail(string NewEmail);

        /// <summary>
        /// returns information about the current user. As there is no specific login method as credentials can be passed to any method, this is an especially good place for logging in with no particular action in mind
        /// https://docs.pcloud.com/methods/general/userinfo.html
        /// </summary>
        Task<JSON_UserInfo> UserInfo();

        /// <summary>
        /// Sends email to the logged in user with email activation link
        /// https://docs.pcloud.com/methods/auth/sendverificationemail.html
        /// </summary>
        Task<bool> ResendActivationMail();

    }
}
