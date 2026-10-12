namespace EstimatorX.Security;

public static class AuthenticationProviders
{
    public static class Microsoft
    {
        public const string Name = "Microsoft";
        public const string ClientId = "Microsoft:ClientId";
        public const string ClientSecret = "Microsoft:ClientSecret";
    }

    public static class Google
    {
        public const string Name = "Google";
        public const string ClientId = "Google:ClientId";
        public const string ClientSecret = "Google:ClientSecret";
    }

    public static class Facebook
    {
        public const string Name = "Facebook";
        public const string AppId = "Facebook:AppId";
        public const string AppSecret = "Facebook:AppSecret";
    }

    public static class GitHub
    {
        public const string Name = "GitHub";
        public const string ClientId = "GitHub:ClientId";
        public const string ClientSecret = "GitHub:ClientSecret";
    }
}
