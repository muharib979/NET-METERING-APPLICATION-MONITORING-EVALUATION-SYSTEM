namespace Core.Domain.Dbo;

public class Menu
{
    public int ID { get; set; }
    public string MENU_NAME { get; set; }
    public string? URL { get; set; }
    public string? ICON { get; set; }
    public int? PARENT_ID { get; set; }
    public int IS_PARENT { get; set; }//bool
    public int IS_GROUP { get; set; } //bool
    public int? GROUP_ID { get; set; }
    public int IS_ACTIVE { get; set; }//bool
    public DateTime TIMESTAMP { get; set; }
    public int TOTAL_ROW_COUNT { get; set; }
    public int? ORDER_NO { get; set; }
    public int IsActive { get; set; } //bool
    public int IS_CREATED { get; set; }//bool
    public int IS_EDITED { get; set; }//bool
    public int IS_DELETED { get; set; }//bool
    //public int IS_CREATED { get; set; }
    //public int IS_EDITED { get; set; }
    //public int IS_DELEDTED { get; set; }
}
