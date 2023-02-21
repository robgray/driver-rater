namespace HelmetRanker.Features.Drivers.v1.Commands;

using HelmetRanker.Entities;
using HelmetRanker.Plumbing.Mediator;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

[UsedImplicitly]
public class UpdateDriverRank
{
    [UsedImplicitly]
    public class Command : ICommand
    {
        public Guid DriverId { get; set; }
        public DriverRank NewRank { get; set; }
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
            // TODO Get userId from authentication.
            Guid userId = Guid.Empty;
            
            var driver = await context.Drivers.SingleAsync(d => d.Id == command.DriverId && d.RankedBy.Id == userId, cancellationToken);
            driver.UpdateRank(command.NewRank);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}