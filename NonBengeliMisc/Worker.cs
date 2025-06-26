using NonBengeliMisc.Models;
using NonBengeliMisc.Repository;

namespace NonBengeliMisc
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IInsertCustomerRepository _repository;
        private readonly IDatabaseConfigRepository _configRepository;

        public Worker(ILogger<Worker> logger, IInsertCustomerRepository repository, IDatabaseConfigRepository configRepository)
        {
            _logger = logger;
            _repository = repository;
            _configRepository = configRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try

                {
                    var allDatabases = await _configRepository.GetAllActiveDatabaseConfigAsync();
                    var insertValueList = new List<InsertCustomerArrearModel>();
                    _logger.LogInformation($"Job Start {DateTime.Now}");
                    foreach (var database in allDatabases)
                    {
                        try
                        {
                            var data = await _repository.GetAllCustomerDataAsync(database.CODE);
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
                        await _repository.InsertCustomerArrearDataAsync(insertValueList);
                    }
                    _logger.LogInformation($"Job Completed  {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                await Task.Delay(86340000, stoppingToken);
            }
        }
    }
}