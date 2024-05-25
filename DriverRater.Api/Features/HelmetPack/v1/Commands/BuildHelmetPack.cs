namespace DriverRater.Api.Features.HelmetPack.v1.Commands;

using System.IO.Compression;
using System.Reflection;
using DriverRater.Api.Entities;
using DriverRater.Api.Exceptions;
using DriverRater.Api.Helpers;
using DriverRater.Api.Plumbing.Startup.Mediator;
using DriverRater.Shared;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class BuildHelmetPack
{
    public record Command : ICommand<Response>
    {
        public IUserContext UserContext { get; set; }
    }

    public record Response
    {
        public string Filename { get; set; }
        public byte[] ZilFileData { get; set; }
    }
    
    public record FileData
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }

    public class CommandHandler(DriverRatingContext context) : ICommandHandler<Command, Response>
    {
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var user = await context.Profiles
                .Include(u => u.RankedDrivers)
                .SingleOrDefaultAsync(u => u.Id == command.UserContext.ProfileId, cancellationToken);

            if (user is null)
            {
                throw new EntityNotFoundException("User not found");
            }

            if (user.RankedDrivers.All(d => d.Rank == DriverRank.None) || !user.RankedDrivers.Any())
            {
                throw new ValidationException("Rank some drivers before downloading a helmet pack");
            }

            var helmetData = user.RankedDrivers
                .Where(d => d.Rank != DriverRank.None)
                .Select(d => new FileData
                {
                    FileName = $"helmet_{d.RacingId}.tga",
                    Data = HelmetFileData(TemplateName(d.Rank)),
                })
                .ToArray();
            
            
            var sessionId = Guid.NewGuid();
            try
            {
                await using (var fileStream = new FileStream($"C:/temp/{sessionId}.zip", FileMode.CreateNew))
                {
                    using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var helmet in helmetData)
                        {
                            var zipArchiveEntry = archive.CreateEntry(helmet.FileName, CompressionLevel.Fastest);
                            await using var zipStream = zipArchiveEntry.Open();
                            await zipStream.WriteAsync(helmet.Data, 0, helmet.Data.Length, cancellationToken);
                        }
                    }
                }

                // read file to bytes.
                var zipFileBytes = await File.ReadAllBytesAsync($"C:/temp/{sessionId}.zip", cancellationToken);
                return new Response
                {
                    Filename = $"HelmetPack-{user.RacingId}.zip",
                    ZilFileData = zipFileBytes,
                };
            }
            finally
            {
                if (File.Exists($"C:/temp/{sessionId}.zip"))
                {
                    File.Delete($"C:/temp/{sessionId}.zip");
                }
            }

            string TemplateName(DriverRank rank) => rank switch
            {
                DriverRank.Black => "helmet_black",
                DriverRank.Blue => "helmet_blue",
                DriverRank.Green => "helmet_green",
                DriverRank.Red => "helmet_red",
                DriverRank.Yellow => "helmet_yellow",
                _ => throw new Exception("Invalid Driver Rank for helmets"),
            };

            byte[] HelmetFileData(string helmetFile)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames()
                    .SingleOrDefault(s => s.Contains(helmetFile));
                using var fileStream = assembly.GetManifestResourceStream(resourceName);

                return fileStream.ReadToEnd();
            }
        }
    }
}