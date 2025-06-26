namespace Core.Domain.Dbo;

public class RoleToMenu
{
    public int ROLE_ID_FK { get; set; }
    public int MENU_ID_FK { get; set; }
    public int IS_ACTIVE { get; set; } // bool
    public DateTime TIMESTAMP { get; set; }
}
