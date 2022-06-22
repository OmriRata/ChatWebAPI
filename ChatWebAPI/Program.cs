using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChatWebAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql;
using System.Text.Json.Serialization;
using ChatWebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = "server=localhost;user=root;password=;database=ef";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

builder.Services.AddDbContext<ChatWebAPIContext>(options =>
    //options.UseMySql(connectionString, serverVersion)
    options.UseSqlite("Data Source=chat.db")
    );

//options.UseSqlServer(builder.Configuration.GetConnectionString("ChatWebAPIContext") ?? throw new InvalidOperationException("Connection string 'ChatWebAPIContext' not found.")));


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwtparams:Audience"],
        ValidIssuer = builder.Configuration["Jwtparams:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwtparams:SecretKey"]))

    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});



builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseSession();
app.UseCors("Allow All");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MessageHub>("/messageHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Rating}/{action=Index}/{id?}");
});

app.Run();
