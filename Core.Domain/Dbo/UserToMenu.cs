namespace Core.Domain.Dbo;

public class UserToMenu
{
    public int USER_ID_FK { get; set; }
    public int MENU_ID_FK { get; set; }
    public int IS_ACTIVE { get; set; } //bool
    public DateTime TIMESTAMP { get; set; }

    public int IS_CREATED { get; set; }
    public int IS_EDITED { get; set; }

    public int IS_DELEDTED { get; set; }
}
