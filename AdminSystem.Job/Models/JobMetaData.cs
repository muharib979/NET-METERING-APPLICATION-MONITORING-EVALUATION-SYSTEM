using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminSystem.Job.Models
{
    public class JobMetaData
    {
        public Guid JobId { get; set; }
        public Type JobType { get; set; }
        public string JobName { get; set; }
        public string CronExpression { get; set; }

        public JobMetaData(Guid Id, Type jobType, string jobName, string cronExpression)
        {
            JobId = Id;
            JobType = jobType;
            JobName = jobName;
            CronExpression = cronExpression;
        }
    }
}
