namespace LoginService.RouteManipulation
{
    public static class Routes
    {
        public static void Map(WebApplication app)
        {
            app.MapGroup("/login").MapGroupLogin().WithTags("LoginRoutes");
        }
    }
}
