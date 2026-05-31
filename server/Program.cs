using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PedidoApi.Data;
using PedidoApi.Helpers;
using PedidoApi.Models.Dtos;
using PedidoApi.Profiles;
using PedidoApi.Repositories;
using PedidoApi.Services;
using PedidoApi.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton(provider =>
{
    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
    var config = new MapperConfiguration(cfg => cfg.AddProfile<AppProfile>(), loggerFactory);
    return config.CreateMapper();
});

builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddSingleton<TokenHelper>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ExternalInfoService>();
builder.Services.AddScoped<BranchService>();

builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
builder.Services.AddScoped<IValidator<ProductUpdateDto>, ProductUpdateDtoValidator>();
builder.Services.AddScoped<IValidator<OrderCreateDto>, OrderCreateDtoValidator>();
builder.Services.AddScoped<IValidator<BranchCreateDto>, BranchCreateDtoValidator>();
builder.Services.AddScoped<IValidator<BranchUpdateDto>, BranchUpdateDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularClient");
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(db);
}

app.Run();
