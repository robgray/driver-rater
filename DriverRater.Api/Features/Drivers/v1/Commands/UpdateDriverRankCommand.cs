namespace DriverRater.Api.Features.Drivers.v1.Commands;

using Aydsko.iRacingData;
using DriverRater.Api.Entities;
using DriverRater.Api.Plumbing.Startup.Mediator;
using DriverRater.Shared;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

[UsedImplicitly]
public class UpdateDriverRank
{
    [UsedImplicitly]
    public record Command : ICommand<Response>
    {
        public Guid RankedDriverId { get; set; }
    
        public int RacingId { get; set; }
        
        public DriverRank NewRank { get; set; }
    
        public IUserContext Profile { get; set; }
    }

    public record Response(Guid RankedDriverId, string Name, DriverRank Rank);
   
    [UsedImplicitly]
    public class CommandHandler(DriverRatingContext ratingContext, IDataClient dataClient) 
        : ICommandHandler<Command, Response>
    {   
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            RankedDriver driver;
            var profile = ratingContext.Profiles.SingleOrDefault(u => u.Id == command.Profile.ProfileId) ?? throw new ValidationException("User not found");
            if (command.RankedDriverId == Guid.Empty)
            {
                var memberProfile = (await dataClient.GetMemberProfileAsync(command.RacingId, cancellationToken)).Data;

                driver = new RankedDriver(profile)
                {
                    RacingId = command.RacingId,
                    Name = memberProfile.Info.DisplayName
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