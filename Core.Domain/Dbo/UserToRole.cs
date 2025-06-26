namespace Core.Domain.Dbo;

public class UserToRole
{
    public int ROLE_ID_FK { get; set; }
    public int USER_ID_FK { get; set; }
    public int IS_ACTIVE { get; set; }//bool
}
