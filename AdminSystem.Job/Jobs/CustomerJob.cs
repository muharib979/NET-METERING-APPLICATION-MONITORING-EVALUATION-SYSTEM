using AdminSystem.Job.Repository;
using Quartz;

namespace AdminSystem.Job.Jobs
{
    public class CustomerJob : IJob
    {
        private ILogger<CustomerJob> _logger;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly IInsertCustomerRepository _insertCustomerRepo;
        //private readonly IMinistryRepository _ministryRepo;
        private readonly IConfiguration _config;

        public CustomerJob(ILogger<CustomerJob> logger, IDatabaseConfigRepository dbConfigRepo, IInsertCustomerRepository insertCustomerRepo, IConfiguration config)
        {
            _logger = logger;
            _dbConfigRepo = dbConfigRepo;
            _insertCustomerRepo = insertCustomerRepo;
            _config = config;
            // _ministryRepo = ministryRepo;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
            //{
            //    writer.WriteLine($"Notify User start at {DateTime.Now} and Jobtype: {context.JobDetail.JobType}");
            //}

            //try

            //{
            //    string bill_cycle = DateTime.Now.AddMonths(-2).ToString("yyyyMM");
            //   // string bill_cycle = "201201";
            //    var allDatabases = await _dbConfigRepo.GetAllActiveDatabaseConfigAsync();
            // var a = _config["CornSetting:CustomerInsertCronExpression"];
            //    var insertValueList = new List<InsertCustomerArrearModel>();
            //   foreach (var database in allDatabases)
            //   {
            //        try
            //        {
            //            var data = await _insertCustomerRepo.GetAllCustomerDataAsync(bill_cycle, database.CODE);
            //            if (data.Count > 0)
            //            {
            //                insertValueList.AddRange(data);

            //            }
            //        }
            //        catch (Exception)
            //        {
            //            continue;
            //        }
            //    }
            //    if (insertValueList.Count > 0)
            //    {
            //        await _insertCustomerRepo.InsertCustomerArrearDataAsync(insertValueList);
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
            //using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
            //{
            //    writer.WriteLine($"Notify User end at {DateTime.Now} and Jobtype: {context.JobDetail.JobType}");
            //}
            await Task.CompletedTask;
        }
    }
}
