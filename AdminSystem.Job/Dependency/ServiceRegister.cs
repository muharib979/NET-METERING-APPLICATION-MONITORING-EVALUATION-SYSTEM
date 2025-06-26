

using AdminSystem.Job.Interface;
using AdminSystem.Job.Repository;
using AdminSystem.Job.Service;

namespace AdminSystem.Job.Dependency;
public static class ServiceRegister
{
    public static void AddApllicationServices(this IServiceCollection services) =>
        services.AddTransient<IDapperService,DapperService>()
                .AddTransient<IDatabaseConfigRepository, DatabaseConfigRepository>()
                .AddTransient<ICommonRepository, CommonRepository>()
                .AddTransient<IInsertCustomerRepository, InsertCustomerRepository>();
}
