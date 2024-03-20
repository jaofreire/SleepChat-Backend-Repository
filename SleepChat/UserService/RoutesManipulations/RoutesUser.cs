namespace UserService.RoutesManipulations
{
    public static class RoutesUser
    {
        public static void Map(WebApplication app)
        {
            app.MapGroup("/user").MapGroupUser().WithTags("UserRoutes").RequireAuthorization("USER") ;
        }
    }
}
