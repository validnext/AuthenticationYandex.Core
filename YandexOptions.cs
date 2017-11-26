using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Authentication.Yandex.Core
{
    public class YandexOptions : OAuthOptions
    {
        public YandexOptions()
        {
            ClaimsIssuer =YandexDefaults.ClaimsIssuer;
            CallbackPath = new PathString(YandexDefaults.CallbackPath);
            AuthorizationEndpoint = YandexDefaults.AuthorizationEndpoint;
            TokenEndpoint = YandexDefaults.TokenEndpoint;
            UserInformationEndpoint = YandexDefaults.UserInformationEndpoint;
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(AppId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Отсутствует {0}", nameof(AppId)), nameof(AppId));
            }

            if (string.IsNullOrEmpty(AppSecret))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Отсутствует {0}", nameof(AppSecret)), nameof(AppSecret));
            }

            base.Validate();
        }

        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        public string AppSecret
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }

        public bool SendAppSecretProof { get; set; }

        public ICollection<string> Fields { get; } = new HashSet<string>();
    }
}
