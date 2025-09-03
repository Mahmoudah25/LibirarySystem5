
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagmentSystem.Models;
using System.Text;

namespace StudentManagmentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();


            builder.Services.AddDbContext<LibiraryContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            //-------------------auth

            builder.Services.AddIdentity<ApplicationUser,IdentityRole>().
                AddEntityFrameworkStores<LibiraryContext>();


            builder.Services.AddAuthentication(options => 
            {
                //Check JWT Token
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => //VeryfiedKey
            {
                option.SaveToken=true;
                option.RequireHttpsMetadata= true;
                option.TokenValidationParameters = new TokenValidationParameters()

                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIp"],
                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"]))
                };
            });

            //-------------


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();


            //#region swagger setting

            builder.Services.AddSwaggerGen(swagger =>
            {
                // This is to generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Libirary Managment System ",
                    Description = "API Project "
                });

                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token.\r\nExample: \"Bearer 12345abcdef\""
                });

                swagger.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            //#region endRegion




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
