namespace Authentication.Yandex.Core
{
    public static class YandexDefaults
    {
        public const string ClaimsIssuer = "Yandex";
        public const string AuthenticationScheme = "Yandex";
        public static readonly string DisplayName = "Yandex";
        public static readonly string AuthorizationEndpoint = "https://oauth.yandex.ru/authorize";
        public static readonly string TokenEndpoint = "https://oauth.yandex.ru/token";
        public static readonly string UserInformationEndpoint = "https://login.yandex.ru/info";
        public static readonly string CallbackPath = "/signin-yandex";
    }
}
