using Microsoft.EntityFrameworkCore;
using UserService.RoutesManipulations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseSql"));
});
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

app.Run();


