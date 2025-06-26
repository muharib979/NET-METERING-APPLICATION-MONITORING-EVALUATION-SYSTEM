namespace AdminSystem.Job.Models
{
    public class CronSettings
    {
        public string CustomerInsertCronExpression { get; set; }
        public string JobLogCronExpression { get; set; }
        public string MinistryCronExpression { get; set; }
    }
}
