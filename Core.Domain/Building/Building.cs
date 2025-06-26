namespace Core.Domain.Building;

public class Building
{
    public int ID { get; set; }
    public string? SITE_NBR { get; set; }
    public string? ADDRESS_CODE { get; set; }
    public string? BUILDING_TITLE { get; set; }
    public string? ADDRESS { get; set; }
    public string? POSTAL_CODE { get; set; }
    public string? BUILDING_TYPE { get; set; }
    public string? ASSET_NO { get; set; }
    public string? LTA_ID { get; set; }
    public DateTime? ISS_INSTALL_DATE { get; set; }
    public int? IS_ACTIVE { get; set; }
    public int? IS_ON_TEST { get; set; }
    public int? TOTAL_ROW_COUNT { get; set; }
}
