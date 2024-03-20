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

                newUser.Id = id;
                userUpdate.Name = newUser.Name;
                userUpdate.Email = newUser.Email;
                userUpdate.Password = newUser.Password;

                db.Users.Update(userUpdate);
                await db.SaveChangesAsync();

                return Results.Ok(newUser);
            });

            group.MapPut("/updateByName/{name}", async (ApiDbContext db, string? name, UserModel newUser) =>
            {
                var userUpdate = await db.Users.FirstOrDefaultAsync(x => x.Name == name) ??
                 throw new Exception("User not found");

                newUser.Id = userUpdate.Id;
                userUpdate.Name = newUser.Name;
                userUpdate.Email = newUser.Email;
                userUpdate.Password = newUser.Password;

                db.Users.Update(userUpdate);
                await db.SaveChangesAsync();

                return Results.Ok(newUser);
            });

            group.MapGet("/byId/{id}", GetById);

            group.MapGet("/getAll", async (ApiDbContext db) =>
            {
                var users = await db.Users.ToListAsync();

                return Results.Ok(users);
            });

            group.MapDelete("/remove/{id}", async (int id, ApiDbContext db) =>
            {
                var user = await db.Users.FindAsync(id) ??
                 throw new Exception("User not found");

                db.Users.Remove(user);
                await db.SaveChangesAsync();
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
