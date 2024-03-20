using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LoginService.RouteManipulation
{
    public static class RoutesExtensions
    {
        public static RouteGroupBuilder MapGroupLogin(this RouteGroupBuilder group)
        {
            group.MapPost("/register", async (UserModel newUser , ApiDbContext db) =>
            {
                newUser.Role = "USER";
                await db.Users.AddAsync(newUser);
                await db.SaveChangesAsync();

                return Results.Ok(newUser);
            });

            group.MapGet("/byname/{name}", GetByName);

            group.MapPost("/loginConfirm/{name}", (ApiDbContext db, string? name, string? password, string? email) =>
            {
                if (ValidateUser(db ,name, password, email))
                {
                    return Results.Ok("SUCESS LOGIN!!");
                }

                return Results.BadRequest("INVALID CREDENTIALS!!");
            });

            return group;
        }

        public static UserModel GetByName(ApiDbContext db, string? name)
        {
            var user =  db.Users.FirstOrDefault(x => x.Name == name) ??
                throw new Exception("User not found");

            return user;
        }

        public static bool ValidateUser(ApiDbContext db, string? name, string? password, string? email)
        {
            var user = GetByName(db, name);

            if (password == user.Password && email == user.Email)
            {
                return true;
            }

            return false;
        }

        

    }
}
