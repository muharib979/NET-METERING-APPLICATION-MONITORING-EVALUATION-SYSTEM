namespace Shared.DTOs.Building;

public class BuildingDto
{
    public int BuildingId { get; set; }
    public string? SiteNbr { get; set; }
    public string? AddressCode { get; set; }
    public string? BuildingTitle { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? BuildingType { get; set; }
    public string? AssetNo { get; set; }
    public string? LtaId { get; set; }
    public DateTime? IssInstallDate { get; set; }
    public int? IsActive { get; set; }
    public int? IsOnTest { get; set; }
    public int TotalRowCount { get; set; }
}
