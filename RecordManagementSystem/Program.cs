using RecordManagementSystem.Infrastructure.Persistence.Data;
using RecordManagementSystem.Application.Features.Account.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RecordManagementSystem.Infrastructure.Repository.Features.Account;
using RecordManagementSystem.Application.Features.Account.Service;
using RecordManagementSystem.Map;
using RecordManagementSystem.Application.Common.Mappings;
using RecordManagementSystem.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("default")
));

builder.Services.AddIdentity<UserIdentity, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();

builder.Services.AddScoped<IAddStudentUserData, AddStudentUserAccountRepository>();
builder.Services.AddScoped<AddStudentUserAccountServices>();

builder.Services.AddAutoMapper(typeof(MappingProfileApp), typeof(MappingProfile));


builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

});

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


app.Run();
