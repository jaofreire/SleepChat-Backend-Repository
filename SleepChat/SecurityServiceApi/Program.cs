using KeysRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebHostExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.UseServiceSDK();
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(Keys.connectionStringDb);
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPost("auth/connect", async (UserModel user, ApiDbContext db) =>
{
    var userIsValid = await db.Users.FirstOrDefaultAsync(x => x.Name == user.Name);

    if (userIsValid.Email == user.Email && userIsValid.Password == user.Password)
    {
        switch (userIsValid.Role)
        {
            case "USER":
                return Token.GenerateToken("USER");
            case "ADM":
                return Token.GenerateToken("ADM");
        }
    }

    return Results.BadRequest();

});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseServicesAuth();

app.Run();

public static class Token
{
    public static object GenerateToken(string role)
    {
        var key = Encoding.ASCII.GetBytes(Keys.secret);

        var tokenConfig = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, role)
            }),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddHours(3)

        };

        var tokenHandle = new JwtSecurityTokenHandler();
        var token = tokenHandle.CreateToken(tokenConfig);

        var tokenString = tokenHandle.WriteToken(token);

        return new
        {
            token = tokenString
        };
    }

}