namespace DriverRater.Features.Drivers.v1.Commands;

using Aydsko.iRacingData;
using DriverRater.Entities;
using DriverRater.Plumbing.Mediator;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

[UsedImplicitly]
public class UpdateDriverRank
{
    [UsedImplicitly]
    public class Command : ICommand<Response>
    {
        public Guid RankedDriverId { get; set; }
    
        public int RacingId { get; set; }
        
        public DriverRank NewRank { get; set; }
    
        public Guid RankedByUserId { get; set; }
    }

    public class Response
    {
        public Guid RankedDriverId { get; set; }
        public string Name { get; set; }
        public DriverRank Rank { get; set; }
    }

    [UsedImplicitly]
    public class CommandHandler : ICommandHandler<Command, Response>
    {
        private readonly DriverRatingContext ratingContext;
        private readonly IDataClient dataClient;
        
        public CommandHandler(DriverRatingContext ratingContext, IDataClient dataClient)
        {
            this.ratingContext = ratingContext;
            this.dataClient = dataClient;
        }
        
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            RankedDriver driver;
            var user = ratingContext.Users.SingleOrDefault(u => u.Id == command.RankedByUserId) ?? throw new ValidationException("User not found");
            if (command.RankedDriverId == Guid.Empty)
            {
                var memberProfile = (await dataClient.GetMemberProfileAsync(command.RacingId, cancellationToken)).Data;

                driver = new RankedDriver
                {
                    RacingId = command.RacingId,
                    Name = memberProfile.Info.DisplayName,
                    RankedBy = user
                };
                ratingContext.Drivers.Add(driver);
            }
            else
            {
                driver = await ratingContext.Drivers.SingleAsync(d => d.Id == command.RankedDriverId && command.RankedDriverId != Guid.Empty, cancellationToken);
            }

            driver.UpdateRank(command.NewRank);
            await ratingContext.SaveChangesAsync(cancellationToken);

            return new Response
            {
                RankedDriverId = driver.Id,
                Name = driver.Name,
                Rank = driver.Rank,
            };
        }
    }
}