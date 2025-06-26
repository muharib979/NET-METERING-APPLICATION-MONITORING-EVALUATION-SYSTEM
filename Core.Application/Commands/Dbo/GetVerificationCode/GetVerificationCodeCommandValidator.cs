namespace Core.Application.Commands.Dbo.GetVerificationCode;

public class GetVerificationCodeCommandValidator : AbstractValidator<GetVerificationCodeCommand>
{
    public GetVerificationCodeCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
    }
}
