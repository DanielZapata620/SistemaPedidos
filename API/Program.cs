using API.Helpers;
using API.Models.Entities;
using API.Repositories;
using API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecretKey") ?? ""));
        x.TokenValidationParameters.ValidateIssuer = true;
        x.TokenValidationParameters.ValidateAudience = true;
        x.TokenValidationParameters.ValidateLifetime = true;
        x.TokenValidationParameters.ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience");
        x.TokenValidationParameters.ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
    });

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var cs = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<PlataformaalimentosContext>(x =>
{
    x.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));
builder.Services.AddScoped<TokenHelper>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ResenasService>();
builder.Services.AddScoped<ProductosService>();
builder.Services.AddScoped<PedidosService>();
builder.Services.AddScoped<SucursalesService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ExternalInfoService>();

builder.Services.AddAutoMapper(x => { }, typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PlataformaalimentosContext>();
    db.Database.EnsureCreated();
    DbInitializer.Seed(db);
}

var webRoot = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
var uploadsPath = Path.Combine(webRoot, "Uploads");
Directory.CreateDirectory(uploadsPath);
Directory.CreateDirectory(Path.Combine(webRoot, "uploads", "products"));

app.UseStaticFiles();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
