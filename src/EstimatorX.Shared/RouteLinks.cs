namespace EstimatorX;

public static class RouteLinks
{
    public static string Home() => "/";

    public static class Account
    {
        public static string Login() => "/account/login";
        public static string Login(string message) => $"/account/login?message={message}";

        public static string Challenge(string provider, string? returnUrl = null)
            => $"/account/challenge/{provider}?returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}";

        public static string Logout() => "/account/logout";

        public static string Profile() => "/account/profile";
        public static string Claims() => "/account/claims";
        public static string Logins() => "/account/logins";

        public static string AccessDenied() => "/account/denied";
    }

    public static class Projects
    {
        public static string List() => "/projects";
        public static string Create() => "/projects/create";

        public static string Summary(long id) => $"/projects/{id}";
        public static string General(long id) => $"/projects/{id}/general";
        public static string Estimation(long id) => $"/projects/{id}/estimation";
        public static string Details(long id) => $"/projects/{id}/details";
        public static string Settings(long id) => $"/projects/{id}/settings";

        public static string Report(string securityKey) => $"/projects/report/{Uri.EscapeDataString(securityKey)}";
    }

    public static class Templates
    {
        public static string List() => "/templates";
        public static string Create() => "/templates/create";

        public static string General(long id) => $"/templates/{id}";
        public static string Estimation(long id) => $"/templates/{id}/estimation";
        public static string Settings(long id) => $"/templates/{id}/settings";
    }

    public static class Organizations
    {
        public static string List() => "/organizations";
        public static string Create() => "/organizations/0";
        public static string Edit(long id) => $"/organizations/{id}";

        public static string? Members(long id) => $"/organizations/{id}/members";
        public static string? Invites(long id) => $"/organizations/{id}/invites";
    }

    public static class Invites
    {
        public static string Redeem(string token) => $"/redeem/{Uri.EscapeDataString(token)}";
    }

    public static class Administrative
    {
        public static string Organizations() => "/administrative/organizations";
        public static string Users() => "/administrative/users";
        public static string Logs() => "/administrative/logs";
    }
}
