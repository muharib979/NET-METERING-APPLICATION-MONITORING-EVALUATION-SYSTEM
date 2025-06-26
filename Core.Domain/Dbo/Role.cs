namespace Core.Domain.Dbo;

public class Role
{
    public int ID { get; set; }
    public string ROLE_NAME { get; set; }
    public int IS_SMS_ROLE { get; set; }// bool
    public int MENU_ID_FK { get; set; }
    public int IS_ACTIVE { get; set; }
    public int PRIORITY { get; set; }
    public int TOTAL_ROW_COUNT { get; set; }
}
