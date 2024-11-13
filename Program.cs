using System.Text;
using HypotheticalRetailStore.DATA;
using HypotheticalRetailStore.DTOs;
using HypotheticalRetailStore.EndPoints;
using HypotheticalRetailStore.Helper;
using HypotheticalRetailStore.Interfaces;
using HypotheticalRetailStore.Models;
using HypotheticalRetailStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nelibur.ObjectMapper;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
string databaseconnection =
    @"Data Source=SQL8011.site4now.net;Initial Catalog=db_a8e840_nass;User Id=db_a8e840_nass_admin;Password=nn@db24@77";


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
// Configure services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = JWTCredentials.xAudidance,
            ValidIssuer = JWTCredentials.xIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTCredentials.xKey))
        };
    });
//
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy =>
    {
        policy.RequireRole("User");
    });

    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });
});

// Swagger and API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization: 'Bearer Generated-JWT-Token'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(databaseconnection));

//config endpoints services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockTransactionService, StockTransactionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
//
//mapping
TinyMapper.Bind<DTOProduct, Product>();
TinyMapper.Bind<DTOSupplier, Supplier>();
TinyMapper.Bind<DTOUser, User>();
TinyMapper.Bind<DTOStockTransaction, StockTransaction>();
//

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureEndPointsSuppliers();
app.ConfigureEndPointsUsers();
app.ConfigureEndPointsProduct();
app.ConfigureEndPointsStockTransaction();
app.ConfigureEndPointsLogin();

app.Run();

