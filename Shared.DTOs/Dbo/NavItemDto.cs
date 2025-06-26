namespace Shared.DTOs.Dbo;

public class NavItemDto
{
    public int ItemId { get; set; }
    public string? ParentId { get; set; }
    public string ItemName { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int IsActive { get; set; } //bool
    public int IsCreated { get; set; }//bool
    public int IsEdited { get; set; }//bool

    public int IsDeleted { get; set; }//bool
    public List<NavItemDto>? Children { get; set; }
}

public class SideBarItemDto
{
    public int ItemId { get; set; }
    public string? ParentId { get; set; }
    public string Title { get; set; }
    public string? Link { get; set; }
    public int Group { get; set; } //bool
    public string GroupId { get; set; }
    public int Home { get; set; } //bool
    public IconSideBar? Icon { get; set; }
    public BadgeSideBar? Badge { get; set; }
    public int IsActive { get; set; } //bool
    public List<SideBarItemDto>? Children { get; set; }
}

public class IconSideBar
{
    public string Icon { get; set; }
    public string Pack { get; set; }
}

public class BadgeSideBar
{
    public string Text { get; set; } = "214";
}
