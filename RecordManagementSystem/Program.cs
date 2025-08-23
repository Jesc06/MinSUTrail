using RecordManagementSystem.Infrastructure.Persistence.Data;
using RecordManagementSystem.Application.Features.Account.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RecordManagementSystem.Infrastructure.Repository.Features.Account;
using RecordManagementSystem.Application.Features.Account.Service;
using RecordManagementSystem.Infrastructure.Persistence.Seeder;
using RecordManagementSystem.Infrastructure.Services;   
using RecordManagementSystem.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("default")
));

builder.Services.AddIdentity<UserIdentity, IdentityRole>(option => 
    {
        option.Password.RequireDigit = false;
        option.Password.RequiredLength = 3;
        option.Password.RequireLowercase = false;
        option.Password.RequireUppercase = false;
        option.Password.RequireNonAlphanumeric = false;

        option.User.RequireUniqueEmail = true;
        option.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();

builder.Services.AddScoped<IAddStudentUserData, AddStudentUserAccountRepository>();
builder.Services.AddScoped<AddStudentUserAccountServices>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthServices>();

builder.Services.AddScoped<IGenerateTokenService, GenerateTokenService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://localhost:7253")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
     
});


var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context => 
            {
                if (context.Request.Cookies.ContainsKey("Jwt"))
                {
                    context.Token = context.Request.Cookies["Jwt"];
                }
                return Task.CompletedTask;
            }
        };


    });

   
builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();


app.MapGet("/", context => 
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserIdentity>>();
    await RoleSeeder.Roles(roleManager, userManager);
}


app.Run();
