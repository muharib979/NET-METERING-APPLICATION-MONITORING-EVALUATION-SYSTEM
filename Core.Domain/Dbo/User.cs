namespace Core.Domain.Dbo;

public class User 
{
    public int ID { get; set; }
    public string USER_NAME { get; set; }
    public string FULL_NAME { get; set; }
    public string EMAIL { get; set; }
    
    public string? SMS_PHONE_NBR { get; set; }
    public string PASSWORD { get; set; }
    public string PASSWORD_SALT { get; set; }
    public int? OTP { get; set; }
    public DateTime? OTP_EXPIRY_TIMIE { get; set; }
    public int IS_ACTIVE { get; set; } //bool
    public string ENTRY_BY { get; set; }
    public DateTime ENTRY_DATE { get; set; }
    public string? UPDATED_BY { get; set; }
    public DateTime? UPDATED_DATE { get; set; }
    public int IS_DELETED{ get; set; } //bool
    public string? DELETED_BY { get; set; }

    public int IS_CREATED { get; set; }

    public int IS_EDITED { get; set; }
}

public class UserCreateByCenterLocation : User
{
    public int? ROLE_ID { get; set; }
    public string? Role_Name { get; set; }
    public string? Status { get; set; }
    public List<int>? Location { get; set; }
    public List<int>? DB { get; set; }
}

public class GetUserCreateByCenterLocationModel
{
    public int ROLE_ID { get; }
    public int USERID { get; set; }
    public string USER_NAME { get;}
    public int FULLACCESS { get; set; }
    public string DB_CODE { get; set; }
    public string STATUS { get; set;}
    public int LOCATION_ID { get; set; }
}
