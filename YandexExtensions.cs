using Authentication.Yandex.Core;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class YandexAuthenticationOptionsExtensions
    {
        public static AuthenticationBuilder AddYandex(this AuthenticationBuilder builder)
            => builder.AddYandex(YandexDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddYandex(this AuthenticationBuilder builder, Action<YandexOptions> configureOptions)
            => builder.AddYandex(YandexDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddYandex(this AuthenticationBuilder builder, string authenticationScheme, Action<YandexOptions> configureOptions)
            => builder.AddYandex(authenticationScheme, YandexDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddYandex(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<YandexOptions> configureOptions)
            => builder.AddOAuth<YandexOptions, YandexHandler>(authenticationScheme, displayName, configureOptions);
    }
}
