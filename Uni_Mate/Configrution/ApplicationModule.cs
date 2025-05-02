
using Autofac;
using Uni_Mate.Common.BaseHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;
using Uni_Mate.Common.helper;
using Uni_Mate.Common.Views;
using Uni_Mate.Common;
using Uni_Mate.Domain;
using MediatR;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrelloCopy.Common.BaseEndpoints;
using Uni_Mate.Common.BaseEndpoints;
using Uni_Mate.Domain.Repository;
using Uni_Mate.Common.BaseHandlers;


namespace Uni_Mate.Configrution
{
    public class ApplicationModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();
                var connectionString = config.GetConnectionString("HossamConnection");
                var optionsBuilder = new DbContextOptionsBuilder<Context>().UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                ).Options;

                return new Context(optionsBuilder);
            }).As<Context>().InstancePerLifetimeScope();

            //#region JWT Authentication Registration
            //// Register JWT authentication
            //builder.Register(context =>
            //{
            //    var config = context.Resolve<IConfiguration>();
            //    var jwtSettings = config.GetSection("JwtSettings");
            //    var secretKey = jwtSettings.GetValue<string>("SecretKey");
            //    if (string.IsNullOrEmpty(secretKey))
            //    {
            //        throw new InvalidOperationException("SecretKey is not configured properly in appsettings.json");
            //    }

            //    var key = Encoding.UTF8.GetBytes(secretKey);

            //    return new JwtBearerOptions
            //    {
            //        TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //        {
            //            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            //            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateAudience = true,
            //            ValidateIssuer = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidateLifetime = true,
            //        }
            //    };
            ////}).As<JwtBearerOptions>().SingleInstance();
            //#endregion

            #region Services Registration
            builder.RegisterType<UserInfo>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TokenHelper>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserInfoProvider>().AsSelf().InstancePerLifetimeScope();
            #endregion


            #region MediatR Handlers Registration
            // Register MediatR request handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            #endregion

            #region FluentValidation Registration
            // Register FluentValidation validators
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            #endregion

            #region Endpoint Registration
            // Register specific endpoint parameters
            builder.RegisterGeneric(typeof(BaseEndpointParameters<>))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof(BaseWithoutTRequestEndpointParameters))
                .AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            #region Repository Registration
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryUser<>)).As(typeof(IRepositoryIdentity<>)).InstancePerLifetimeScope();
            #endregion

            builder.RegisterGeneric(typeof(BaseRequestHandlerParameter<>))
                 .AsSelf()
                 .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseWithoutRepositoryRequestHandlerParameters<>))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();


        }
    }
}
