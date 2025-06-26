namespace AdminSystem.Job.Models
{
    public static class Common
    {
        public static void Logs(string message, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "JobLog", "JobLogs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, fileName + ".txt");
            using FileStream stream = new FileStream(path, FileMode.Create);
            using TextWriter textWriter = new StreamWriter(stream);
            textWriter.WriteLine(message);
        }
    }
}
