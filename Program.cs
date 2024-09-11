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
using System;

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
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales Management API", Version = "v1" });
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

        // Add JWT-based Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        // CORS Policy for React
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactPolicy", opts =>
            {
                opts.WithOrigins("http://localhost:3000", "null")
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader();
            });
        });

        // Configure Database Context (SQL Server or MySQL based on your choice)
        builder.Services.AddDbContext<ApplicationDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnectionString"));
        });

        // Register Repositories and Services for Dependency Injection
        builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        builder.Services.AddScoped<IRepository<int, Sale>, Repository<int, Sale>>();
        builder.Services.AddScoped<IRepository<string, User>, Repository<string, User>>();
        builder.Services.AddScoped<ISaleService, SaleService>();
       

        var app = builder.Build();

        // Configure the HTTP request pipeline.
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
