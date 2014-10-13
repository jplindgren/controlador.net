using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Gerenciador.Web.UI.Models;

namespace Gerenciador.Web.UI {
    public static class AuthConfig {
        public static void RegisterAuth() {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: "abc",
                clientSecret: "abc");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "abc",
                consumerSecret: "abc");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "abc",
                appSecret: "abc");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
