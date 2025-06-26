namespace Core.Application.Queries.Dbo.GetAllRoleToMenuByRoleNUser;

public class GetAllRoleToMenuByRoleNUserQueryValidator : AbstractValidator<GetAllRoleToMenuByRoleNUserQuery>
{
    public GetAllRoleToMenuByRoleNUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull();
        RuleFor(x => x.RoleId).NotEmpty().NotNull();
    }
}
