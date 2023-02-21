namespace HelmetRanker.Features.Drivers.v1.Commands;

using FluentValidation;
using HelmetRanker.Entities;
using HelmetRanker.Plumbing.Mediator;
using JetBrains.Annotations;

[UsedImplicitly]
public class NewDriverRank
{
    [UsedImplicitly]
    public class Command : ICommand
    {
        public string Name { get; init; } = "";
        public int RacingId { get; init; }
        public DriverRank Rank { get; set; }
    }

    [UsedImplicitly]
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(400);
            // TODO Validate with iRacing.
            RuleFor(x => x.RacingId).GreaterThan(0);
        }
    }

    [UsedImplicitly]
    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly DriverContext context;
        
        public CommandHandler(DriverContext context)
        {
            this.context = context;
        }
        
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            // TODO get user from Authentication.
            var user = new User("Rob", 59619);
            
            Driver newDriver = new(command.Name, command.RacingId, command.Rank, user);
            context.Drivers.Add(newDriver);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

}