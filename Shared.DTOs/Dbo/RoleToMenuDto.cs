namespace Shared.DTOs.Dbo
{
    public class RoleToMenuDto
    {
        public string RoleFkId { get; set; }
        public string RoleName { get; set; }
        public string MenuFkId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int IsActive { get; set; } //bool
        public int IsParent { get; set; } //bool
        public int ParentId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? ORDER_NO { get; set; }
    }
}