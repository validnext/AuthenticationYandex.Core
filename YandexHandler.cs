using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Authentication.Yandex.Core
{
    internal class YandexHandler : OAuthHandler<YandexOptions>
    {
        public YandexHandler(IOptionsMonitor<YandexOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            Microsoft.AspNetCore.Authentication.AuthenticationProperties properties,
            OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("Произошла ошибка при получении профиля пользователя: удаленный сервер " +
                                "вернул {Status} ответ со следующей информацией: {Headers} {Body}.",
                                response.StatusCode,
                                response.Headers.ToString(),
                                await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("Произошла ошибка при получении профиля пользователя.");
            }

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            identity.AddOptionalClaim(ClaimTypes.NameIdentifier, payload.Value<string>("id"), Options.ClaimsIssuer)
                   .AddOptionalClaim(ClaimTypes.Name, payload.Value<string>("login"), Options.ClaimsIssuer)
                   .AddOptionalClaim(ClaimTypes.Surname, payload.Value<string>("last_name"), Options.ClaimsIssuer)
                   .AddOptionalClaim(ClaimTypes.GivenName, payload.Value<string>("first_name"), Options.ClaimsIssuer)
                   .AddOptionalClaim(ClaimTypes.Email, payload.Value<JArray>("emails")?[0]?.Value<string>(), Options.ClaimsIssuer);


            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload);

            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}
