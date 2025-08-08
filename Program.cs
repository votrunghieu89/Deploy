using GraphQLandEF.Controllers;
using GraphQLandEF.DAL;
using GraphQLandEF.DAL.Tasks;
using GraphQLandEF.DAL.Users;
using GraphQLandEF.Model.Users;
using GraphQLandEF.Repositories;
using GraphQLandEF.Security;
using GraphQLandEF.Services.Tasks;
using GraphQLandEF.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// === CẤU HÌNH JWT AUTHENTICATION ===
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // chỉ nên false khi dev
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// === CẤU HÌNH CÁC SERVICE KHÁC ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GoogleSettingModel>(builder.Configuration.GetSection("GoogleAuth"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<GoogleSettingModel>>().Value);

builder.Services.AddHttpClient();
builder.Services.AddScoped<AccesToken>();
builder.Services.AddScoped<GoogleServices>();
builder.Services.AddScoped<IUsersRepository, UsersDAL>();
builder.Services.AddScoped<ITaskRepository, TaskDAL>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// === CẤU HÌNH HOTCHOCOLATE GRAPHQL + AUTHORIZATION ===
builder.Services
    .AddGraphQLServer()
    .AddAuthorization() // 👈 thêm dòng này để dùng [Authorize]
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .BindRuntimeType<DateTime, DateTimeType>(); ;

var app = builder.Build();

// === PIPELINE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
// 👇 Quan trọng: thêm Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// 👇 Map các endpoint GraphQL và API
app.MapGraphQL(); // /graphql
app.MapControllers();

app.Run();
