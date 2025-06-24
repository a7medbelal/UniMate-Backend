
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data.SqlClient;
using System.Text;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Configrution;
using Uni_Mate.Domain;
using Uni_Mate.Features.ApartmentManagment.GetApartment.Queries;
using Uni_Mate.Middlewares;
using Uni_Mate.Middlewares;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

         

            builder.Services.AddDbContext<Context>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            #region Identity Configration
            builder.Services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                  

                })
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });
            #endregion

            #region configure AutoFac

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(container =>
            {
                container.RegisterModule(new ApplicationModule());
            });

            #endregion

            #region JwtSettings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddAuthentication(options =>
            {
                // Use JWT as the default authentication method
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
          {
              var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

              // Set Token Validation Rules
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,

                  ValidIssuer = jwtSettings.Issuer,
                  ValidAudience = jwtSettings.Audience,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
              };
          });
            #endregion
           
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddControllersWithViews(opt => opt.Filters.Add<UserInfoFilter>());


            builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();


            var coonfig = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build(); 

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(coonfig)
               .WriteTo.File("Logs/unimate-log.txt", rollingInterval: RollingInterval.Day)
            
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                   .MinimumLevel.Override("System", LogEventLevel.Warning)
                   .MinimumLevel.Information()
               .WriteTo.MSSqlServer(
                    connectionString: coonfig.GetConnectionString("HostConnection"),
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    sinkOptions : new MSSqlServerSinkOptions { TableName = "Logs"   , AutoCreateSqlTable = true})
                .CreateLogger();



            builder.Host.UseSerilog();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            app.UseMiddleware<TransactionMiddleware>();
            app.UseSerilogRequestLogging();
            app.MapControllers();

            app.Run();
        }
    }
}
