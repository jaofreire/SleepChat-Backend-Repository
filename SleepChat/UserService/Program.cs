using KeysRepository;
using Microsoft.EntityFrameworkCore;
using UserService.RoutesManipulations;
using WebHostExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(Keys.connectionStringDb);
});
builder.Services.UseServiceSDK();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

RoutesUser.Map(app);

app.UseServicesAuth();

app.Run();


