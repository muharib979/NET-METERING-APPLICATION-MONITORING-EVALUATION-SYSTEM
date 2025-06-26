using System.Security.Claims;

namespace CFEMS.API.Common;

public static class StaticData
{
    //public const string Role_Administrator = "Administrator";
    public const string Role_Contractor = "Contractor";
    public const string Role_Admin = "Admin";
    public const string Role_Lta = "Lta";
}

public static class ServiceExtension
{

    /// <summary>
    /// / This method will return UserId from the token. Because in the jwt claim id is bound in the token.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
   

    #region (int) UserId for token
    public static int GetUserId(this HttpContext httpContext)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
       return identity.FindFirst(ClaimTypes.NameIdentifier)?.Value != null ? int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;
    }
    #endregion
    #region (int) RoldId from token
    public static int GetRoleId(this HttpContext httpContext)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
        return identity.FindFirst("roleid").Value != null ? int.Parse(identity.FindFirst("roleid").Value) : 0;
    }
    #endregion

    #region UserName from token
    public static string GetUserName(this HttpContext httpContext)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
        return identity.FindFirst("name").Value;
    }
    #endregion
}
