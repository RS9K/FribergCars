using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using FribergCars.Data;
using FribergCars.Repositories.Interfaces;
using FribergCars.Repositories.Implementations;

namespace FribergCarsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            // Add services to the container.

            // 1. DbContext connection string.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FribergCars;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")));

            // 2. Dependency Injection for Repositories.
            builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            // 3. Controllers
            builder.Services.AddControllers();
            
            // 4. JWT Authentication configuration.
            var jwtSection = builder.Configuration.GetSection("Jwt");
            var jwtKey = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing in configuration");
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSection["Issuer"],
                        ValidAudience = jwtSection["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                    };
                });
            builder.Services.AddAuthorization();

            // 5. Swagger/OpenAPI with JWT support.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });
            });

            var app = builder.Build();

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

            app.Run();
        }
    }
}
