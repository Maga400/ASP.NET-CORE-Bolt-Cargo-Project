using BoltCargo.Business.Services.Abstracts;
using BoltCargo.Business.Services.Concretes;
using BoltCargo.DataAccess.Data;
using BoltCargo.DataAccess.Repositories.Abstracts;
using BoltCargo.DataAccess.Repositories.Concretes;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Hubs;
using BoltCargo.WebUI.Middlewares;
using BoltCargo.WebUI.Services.Abstracts;
using BoltCargo.WebUI.Services.Concretes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000","http://localhost:3001")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllApp",
//        policy => policy.AllowAnyOrigin()
//                        .AllowAnyHeader()
//                        .AllowAnyMethod()
                        //.AllowCredentials());

//});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddScoped<IFeedBackDAL, FeedBackDAL>();
builder.Services.AddScoped<IFeedBackService, FeedBackService>();

builder.Services.AddScoped<IOrderDAL, OrderDAL>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<ICustomIdentityUserDAL, CustomIdentityUserDAL>();
builder.Services.AddScoped<ICustomIdentityUserService, CustomIdentityUserService>();

builder.Services.AddScoped<IChatDAL, ChatDAL>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<IMessageDAL, MessageDAL>();
builder.Services.AddScoped<IMessageService,MessageService>();

builder.Services.AddScoped<IRelationShipDAL,RelationShipDAL>();
builder.Services.AddScoped<IRelationShipService,RelationShipService>();

builder.Services.AddScoped<IRelationShipRequestDAL,RelationShipRequestDAL>();
builder.Services.AddScoped<IRelationShipRequestService,RelationShipRequestService>();

var connection = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<CargoDbContext>(option =>
{
    option.UseSqlServer(connection);
});


builder.Services.AddIdentity<CustomIdentityUser, CustomIdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<CargoDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(Options =>
{
    //Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    Options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    Options.AddPolicy("DriverPolicy", policy => policy.RequireRole("Driver"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.WebHost.UseUrls("https://*":10604);

var app = builder.Build();

app.UseCors("AllowReactApp");
//app.UseCors("AllowAllApp");

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MessageHub>("/messageHub");

app.Run();
