namespace Core.Application.Queries.Dbo.GetRoleByUserId;

public class GetRoleByUserIdQueryValidator : AbstractValidator<GetRoleByUserIdQuery>
{
    public GetRoleByUserIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
