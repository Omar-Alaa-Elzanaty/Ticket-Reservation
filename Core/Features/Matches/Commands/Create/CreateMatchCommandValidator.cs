using FluentValidation;

namespace Core.Features.Matches.Commands.Create
{
    public class CreateMatchCommandValidator:AbstractValidator<CreateMatchCommand>
    {
        public CreateMatchCommandValidator()
        {
            RuleFor(x => x.TeamA).NotEqual(x => x.TeamB);
            RuleFor(x => x.MatchDate).GreaterThan(DateTimeOffset.Now);
        }
    }
}
