using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sales_Management.Context;
using Sales_Management.Interface;
using Sales_Management.Repository;
using Sales_Management.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Sales_Management.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opts.JsonSerializerOptions.WriteIndented = true;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactPolicy", opts =>
            {
                opts.WithOrigins("http://localhost:3000", "null").AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
            });
        });

        builder.Services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnectionString"));
        });

        // Register Repositories and Services
        builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        builder.Services.AddScoped<IRepository<int, Sale>, Repository<int, Sale>>();
        builder.Services.AddScoped<SaleService>();
        builder.Services.AddScoped<UserService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("ReactPolicy");
        app.MapControllers();
        app.Run();
    }
}
