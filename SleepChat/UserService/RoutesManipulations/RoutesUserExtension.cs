using Microsoft.EntityFrameworkCore;
using LoginService;

namespace UserService.RoutesManipulations
{
    public static class RoutesUserExtension
    {
        public static RouteGroupBuilder MapGroupUser(this RouteGroupBuilder group)
        {
            group.MapPut("/updateById/{id}", async (ApiDbContext db, int id, UserModel newUser) =>
            {
                var userUpdate = GetById(id, db);

                userUpdate.Name = newUser.Name;
                userUpdate.Email = newUser.Email;
                userUpdate.Password = newUser.Password;

                db.Users.Update(userUpdate);
                await db.SaveChangesAsync();

                return Results.Ok(newUser);
            });

            group.MapGet("/getAll", async (ApiDbContext db) =>
            {
                var users = await db.Users.ToListAsync();

                return Results.Ok(users);
            });


            return group;
        }

        public static UserModel GetById(int id, ApiDbContext db)
        {
            var user = db.Users.Find(id) ??
                throw new Exception("User not found");

            return user;

        }
    }
}
