using AdminSystem.Job.Jobs;
using AdminSystem.Job.Models;
using AdminSystem.Job.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustoemerInsertController : ControllerBase
    {

        private ILogger<CustomerJob> _logger;
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly IInsertCustomerRepository _insertCustomerRepo;
        //private readonly IMinistryRepository _ministryRepo;

        public CustoemerInsertController(ILogger<CustomerJob> logger, IDatabaseConfigRepository dbConfigRepo, IInsertCustomerRepository insertCustomerRepo)
        {
            _logger = logger;
            _dbConfigRepo = dbConfigRepo;
            _insertCustomerRepo = insertCustomerRepo;
            // _ministryRepo = ministryRepo;
        }

        [HttpGet("CustomerArrearInsert")]
        public async Task<IActionResult> CustomerArrearInsert()
        {
            using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
            {
                writer.WriteLine($"Notify User start at {DateTime.Now}");
            }

            try

            {
               // string bill_cycle = DateTime.Now.AddMonths(-2).ToString("yyyyMM");
                // string bill_cycle = "201201";
                var allDatabases = await _dbConfigRepo.GetAllActiveDatabaseConfigAsync();
                var insertValueList = new List<InsertCustomerArrearModel>();
                foreach (var database in allDatabases)
                {
                    ExceuteStartStop(database.NAME + "Start at "); // start db job
                    try
                    {
                        var data = await _insertCustomerRepo.GetAllCustomerDataAsync(database.CODE);
                        if (data.Count > 0)
                        {
                            insertValueList.AddRange(data);

                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    ExceuteStartStop(database.NAME + "End at "); // End db job
                }
                if (insertValueList.Count > 0)
                {
                    await _insertCustomerRepo.InsertCustomerArrearDataAsync(insertValueList);
                }

                return Ok("Job complete");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("CustIdUpdate")]
        public async Task<IActionResult> CustIdUpdate()
        {
            using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
            {
                writer.WriteLine($"Notify User start at {DateTime.Now}");
            }

            try

            {
                string bill_cycle = DateTime.Now.AddMonths(-2).ToString("yyyyMM");
                // string bill_cycle = "201201";
                var allDatabases = await _dbConfigRepo.GetAllActiveDatabaseConfigAsync();
                var insertValueList = new List<CustIdInsertModel>();
                foreach (var database in allDatabases)
                {
                    try
                    {
                        var data = await _insertCustomerRepo.GetAllCustIdDataAsync(bill_cycle, database.CODE);
                        if (data.Count > 0)
                        {
                            insertValueList.AddRange(data);

                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                if (insertValueList.Count > 0)
                {
                    await _insertCustomerRepo.InsertCustIdDataAsync(insertValueList);
                }

                return Ok("Job complete");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #region utility

        private void ExceuteStartStop(string message = " ")
        {
            using (StreamWriter writer = System.IO.File.AppendText("JobLogs/log.txt"))
            {
                writer.WriteLine($" {message}" + $"{DateTime.Now.ToString()}");
            }
        }

        #endregion

    }
}
