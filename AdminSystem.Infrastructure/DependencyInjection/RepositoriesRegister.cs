
using CFEMS.Infrastructure.Persistence.Repositories.Dbo;

using Core.Application.Interfaces.Building.RepositoryInterfaces;
using CFEMS.Infrastructure.Persistence.Repositories.BuildingRepositories;
using Core.Application.Interfaces.ProsoftDataSync.RepositoryInterface;
using AdminSystem.Infrastructure.Persistence.Repositories.ProsoftDataSyncRepositories;
using Core.Application.Interfaces.OfficeStuff.RepositoryInterface;
using AdminSystem.Infrastructure.Persistence.Repositories.OfficeStuffRepositories;
using Core.Application.Interfaces.DatabaseConfig;
using AdminSystem.Infrastructure.Persistence.Repositories.DatabaseConfig;
using AdminSystem.Infrastructure.Persistence.Repositories.Location;
using Core.Application.Interfaces.Location;
using Core.Application.Interfaces.Agriculture.RepositoryInterfaces;
using AdminSystem.Infrastructure.Persistence.Repositories.Agriculture;
using Core.Application.Interfaces.Cutomers.RepositoryInterfaces;
using AdminSystem.Infrastructure.Persistence.Repositories.Customer;
using Core.Application.Interfaces.MiscBilling;
using Core.Application.Interfaces.Ministry;
using AdminSystem.Infrastructure.Persistence.Repositories.Ministry;
using AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling;
using Core.Application.Interfaces.CityCorporation;
using AdminSystem.Infrastructure.Persistence.Repositories.CityCorporationRepositorie;
using Core.Application.Interfaces.ZoneCircle;
using AdminSystem.Infrastructure.Persistence.Repositories.ZoneCircle;
using Shared.DTOs.MISCBILL;
using Core.Application.Interfaces;
using AdminSystem.Infrastructure.Persistence.Repositories.FileSave;
using Core.Application.Interfaces.Common.Repository;
using AdminSystem.Infrastructure.Persistence.Repositories.Common;
using AdminSystem.Infrastructure.Persistence.Repositories.PaymentGateway;
using Core.Application.Interfaces.PaymentGateway;
using AdminSystem.Infrastructure.Persistence.Repositories.MrsGenarateRepository;
using AdminSystem.Infrastructure.Persistence.Repositories.BillCycleRepository;
using AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.Meter;
using AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.Schedule;
using AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.BookBillGroup;
using AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.Location;
using AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling.BusinessTypes;
using AdminSystem.Infrastructure.Persistence.Repositories.MiscChargeRepository;
using Core.Application.Interfaces.NonBengali;
using AdminSystem.Infrastructure.Persistence.Repositories.NonBengali;
using Core.Application.Interfaces.Religious;
using AdminSystem.Infrastructure.Persistence.Repositories.Religious;
using Core.Application.Interfaces.MISReport;
using AdminSystem.Infrastructure.Persistence.Repositories.MISReport;
using AdminSystem.Infrastructure.Persistence.Repositories.MinistryCustomer;
using Core.Application.Interfaces.MinistryCustomer;
using Core.Application.Interfaces.ChannelAPI;
using AdminSystem.Infrastructure.Persistence.Repositories.ChannelAPI;
using Core.Application.Interfaces.UnionPorishod;
using AdminSystem.Infrastructure.Persistence.Repositories.UnionPorishodRepository;
using Core.Application.Interfaces.APA;
using AdminSystem.Infrastructure.Persistence.Repositories.APA;
using Core.Application.Interfaces.ReplicationStatus;
using AdminSystem.Infrastructure.Persistence.Repositories.ReplicationStatus;
using Core.Application.Interfaces.Reconciliation;
using AdminSystem.Infrastructure.Persistence.Repositories.Reconciliation;
using Core.Application.Interfaces.ConsumerBill;
using AdminSystem.Infrastructure.Persistence.Repositories.ConsumerBill;
using Core.Application.Interfaces.ReligiousSetup;
using AdminSystem.Infrastructure.Persistence.Repositories.ReligiousSetup;
using Core.Application.Interfaces.AppUserManagement;
using AdminSystem.Infrastructure.Persistence.Repositories.AppuserManagement;
using AdminSystem.Infrastructure.Persistence.Repositories.Dbo;
using Core.Application.Interfaces.Police;
using AdminSystem.Infrastructure.Persistence.Repositories.Police;
using AdminSystem.Infrastructure.Persistence.Repositories.Fire_service;
using Core.Application.Interfaces.VisitorDetails;
using AdminSystem.Infrastructure.Persistence.Repositories.UniqueVistiorDetails;
using Core.Application.Services;
using Core.Application.Interfaces.Consumer;
using AdminSystem.Infrastructure.Persistence.Repositories.Consumer;
using Core.Application.Interfaces.PaymentConfirmation;
using AdminSystem.Infrastructure.Persistence.Repositories.PaymentConfirmation;

namespace CFEMS.Infrastructure.DependencyInjection;

public static class RepositoriesRegister
{
    public static void AddRepositoryServices(this IServiceCollection services) =>
        services
        .AddSingleton<DapperContext>()
        .AddTransient<IUserRepository, UserRepository>()
        .AddTransient<ITokenRepository, TokenRepository>()
        .AddTransient<IRoleRepository, RoleRepository>()
        .AddTransient<IMenuRepository, MenuRepository>()
        .AddTransient<IRoleToMenuRepository, RoleToMenuRepository>()
        .AddTransient<IUserToMenuRepository, UserToMenuRepository>()
        .AddTransient<IBuildingRepository, BuildingRepository>()
        .AddTransient<IProsoftDataSyncRepository, ProsoftDataSyncRepository>()
        .AddTransient<IProsoftTokenRepository, ProsoftTokenRepository>()
        .AddTransient<IProsoftUserRepository, ProsoftUserRepository>()
        .AddTransient<IDatabaseConfigRepository, DatabaseConfigRepository>()
        .AddTransient<ILocationRepository, LocationRepository>()
        .AddTransient<IOfficeStuffRepository, OfficeStuffRepository>()
        .AddTransient<IAgricultureRepository, AgricultureRepository>()
        .AddTransient<ICustomerCategoryRepository, CustomerCategoryRepository>()
        .AddTransient<ICustomerTariffRepository, CustomerTariffRepository>()
        .AddTransient<ICustomerTypeRepository, CustomerTypeRepository>()
        .AddTransient<IBillingReasonRepository, BillingReasonRepository>()
        .AddTransient<IImposedByRepository, ImposedByRepository>()
        .AddTransient<ICustomerDetailsRepository, CustomerDetailsRepository>()
        .AddTransient<ICalculateBillGenerateRepository, CalculateBillGenerateRepository>()
        .AddTransient<IPenaltyBillGenerateRepository, PenaltyBillGenerateRepository>()
        .AddTransient<ISupplementaryGenarateRepository, SupplementaryBillGenarateRepository>()
        .AddTransient<IInstallmentPlanRepository<InstallmentPlanDTO>, InstallmentPlanRepository>()
        .AddTransient<IBillPrintRepository<PenaltyBillPrintDTO>, BillPrintRepository>()
        .AddTransient<IMinistryRepository, MinistryRepository>()
        .AddTransient<ICityCorporationRepository, CityCorporationRepository>()
        .AddTransient<IZoneCircleRepository, ZoneCircleRepository>()
        .AddTransient<IFileSaveRepository, FileSaveRepository>()
        .AddTransient<IDcRcBillGenerateRepository, DcRcBillGenerateRepository>()
        .AddTransient<ITemporaryBillRepository, TemporaryBillRepository>()
        .AddTransient<ICommonRepository, CommonRepository>()
        .AddTransient<ITarrifRepository, TarrifRepository>()
        .AddTransient<IMeterRepository, MeterRepository>()
        .AddTransient<IScheduleRepository, ScheduleRepository>()
        .AddTransient<IBookBillGroupRepository, BookBillGroupRepository>()
        .AddTransient<IMscLocationRepository, MiscLocationRepository>()
        .AddTransient<IBusinessTypeRepository, BusinessTypesRepository>()
        .AddTransient<IPaymentGatewayRepository, PaymentGatewayRepository>()
        .AddTransient<ILogEventRepository, LogEventRepository>()
        .AddTransient<IPaymentDetailsRepository, PaymentDetailsRepository>()
        .AddTransient<IMrsGenarateRepository, MrsGenarateRepository>()
        .AddTransient<IBillCycleRepository, BillCyCleRepository>()
        .AddTransient<IVatLpsRepository, VatLpsRepository>()
        .AddTransient<INonBengaliRepository, NonBengaliRepository>()
        .AddTransient<ICustomerInstallmentRepository, CustomerInstallmentRepository>()
        .AddTransient<IMiscChargeRepository, MiscChargeRepository>()
        .AddTransient<IDueDateExtendRepository, DueDateExtendRepository>()
        .AddTransient<IBillDetailsRepository<CustomerBillDetailsDTO>, BillDetailsRepository>()
        .AddTransient<IReligiousRepository,ReligiousRepository>()
        .AddTransient<IModBillRepository, ModBillRepository>()
        .AddTransient<IMisReportRepository, MisReportRepository>()
        .AddTransient<IPostpaidCustomerRepository, PostpaidCustomerRepository>()
        .AddTransient<IMinistryCustomerRepository,MinistryCustomerRepository>()
        .AddTransient<IChannelApiRepository,ChannelApiRepository>()
        .AddTransient<IUnionPorishodRepository, UnionPorishodRepository>()
        .AddTransient<IStrategicObjectiveRepository, StrategicObjectiveRepository>()
        .AddTransient<IProgramRepository, ProgramRepository>()
        .AddTransient<IPerformanceIndexRepository, PerformanceIndexRepository>()
        .AddTransient<IIndexUnitRepository, IndexUnitRepository>()
        .AddTransient<IUnitIndexTargetRepository, UnitIndexTargetRepository>()
        .AddTransient<IReplicationStatusRepository, ReplicationStatusRepository>()
        .AddTransient<IApaRepository, ApaRepository>()
        .AddTransient<ITargetRepository, TargetRepository>()
        .AddTransient<IPerformanceIndexRepository, PerformanceIndexRepository>()
        .AddTransient<IProgramRepository, ProgramRepository>()
        .AddTransient<IIndexUnitRepository, IndexUnitRepository>()
        .AddTransient<IUntracedConsumerRepository, UntracedConsumerRepository>()
        .AddTransient<IReconciliation, ReconciliationRepository>()
        .AddTransient<IUntraceableCustomerRepository, UntraceableCustomerRepository>()
        .AddTransient<IConsumerBillRepository, ConsumerBillRepository>()
        .AddTransient<IReligiousSetupRepository, ReligiouSetupRepository>()
        .AddTransient<IAppUserManagement, AppUserManagementRepository>()
        .AddTransient<IUserAccessMenuRepository, UserAccessMenuRepository>()
        .AddTransient<IPoliceRepository, PoliceRepository>()
        .AddTransient<IFireServiceRepository, FireServiceRepository>()
        .AddTransient<IVisitorDetails, VisitorDetails>()
        .AddTransient<IBkashAppRepository, BkashGatewayRepository>()
        .AddTransient<IConsumerRepository, ConsumerRepository>()
        .AddTransient<IPaymentConfirmationRepository, PaymentRepositories>()
        .AddTransient<IBillPaymentMiscConfirmationRepository, BillPaymentMiscRepositories>()
        //.AddSingleton<SmsService>()
        ;



}
