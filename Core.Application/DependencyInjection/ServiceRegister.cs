using Core.Application.Commands.Dbo.UserAdd;
using Core.Application.Common.Mapper;
using Core.Application.Interfaces.Agriculture.ServiceInterfaces;
using Core.Application.Interfaces.Building.ServiceInterfaces;
using Core.Application.Interfaces.Cutomers.ServiceInterfaces;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.Location;
using Core.Application.Interfaces.OfficeStuff.ServiceInterface;
using Core.Application.Interfaces.ProsoftDataSync.ServiceInterface;
using Core.Application.Services.Agriculture;
using Core.Application.Services.Customers;
using Core.Application.Services.DatabaseConfig;
using Core.Application.Services.Location;
using Core.Application.Services.PaymentGateway;
using Core.Application.Services.ProsoftDataSync;
using FluentValidation.AspNetCore;

namespace Core.Application.DependencyInjection;

public static class ServiceRegister
{
    public static void AddApllicationServices(this IServiceCollection services) =>
        services.AddTransient<IUserService, UserService>()
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IRoleService, RoleService>()
                .AddTransient<IMenuService, MenuService>()
                .AddTransient<IRoleToMenuService, RoleToMenuService>()
                .AddTransient<IUserToMenuService, UserToMenuService>()
                .AddTransient<ITokenService, TokenService>()

                .AddTransient<IForgotPasswordService, ForgotPasswordService>()

                .AddTransient<IProsoftDataSyncService, ProsoftDataSyncService>()
                .AddTransient<IProsoftTokenService, ProsoftTokenService>()
                .AddTransient<IProsoftUserService, ProsoftUserService>()
                .AddTransient<IDatabaseConfigService, DatabaseConfigService>()
                .AddTransient<ILocationService, LocationService>()
                .AddTransient<IAgricultureService, AgricultureService>()
                .AddTransient<ICustomerTypeServices, CustomerTypeService>()
                .AddTransient<ICustomerCategoryServices, CustomerCategoryServices>()
                .AddTransient<PaymentGatewayService>()

                .AddMediatR(typeof(UserAddCommand).Assembly)
                .AddAutoMapper(typeof(AdminMapperProfile).Assembly)
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<UserAddCommand>());
}
