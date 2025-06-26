namespace Core.Domain.Dbo;

public class Token
{
    public int ID { get; set; }
    public string VALUE { get; set; }
    public DateTime CREATED_DATE { get; set; }
    public int USER_ID { get; set; }
    public DateTime LAST_MODIFIED_DATE { get; set; }
    public DateTime EXPIRY_TIME { get; set; }
    public string SESSION_ID{ get; set; }
}
