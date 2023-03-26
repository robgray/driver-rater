namespace DriverRater.Features.HelmetPack.v1.Queries;

using System.Data;
using System.IO.Compression;
using System.Reflection;
using DriverRater.Entities;
using DriverRater.Exceptions;
using DriverRater.Helpers;
using DriverRater.Plumbing.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class BuildHelmetPack
{
    public class Command : ICommand<Response>
    {
        public Guid UserId { get; set; }
    }

    public class Response
    {
        public string Filename { get; set; }
        public byte[] ZilFileData { get; set; }
    }
    
    public class FileData
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }

    public class CommandHandler : ICommandHandler<Command, Response>
    {
        private DriverRatingContext context;
        
        public CommandHandler(DriverRatingContext context)
        {
            this.context = context;
        }
        
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.RankedDrivers)
                .SingleOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

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
                    Data = helmetFileData(templateName(d.Rank)),
                })
                .ToArray();
            
            // Create folder for user 
            var sessionId = Guid.NewGuid();
            var folder = $"C:/temp/{sessionId}";

            var di = Directory.CreateDirectory(folder);
            foreach (var data in helmetData)
            {
                data.Data.ToFile(data.FileName);
            }

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

            string templateName(DriverRank rank) => rank switch
            {
                DriverRank.Black => "helmet_black",
                DriverRank.Blue => "helmet_blue",
                DriverRank.Green => "helmet_green",
                DriverRank.Red => "helmet_red",
                DriverRank.Yellow => "helmet_yellow",
                _ => throw new Exception("Invalid Driver Rank for helmets"),
            };

            byte[] helmetFileData(string helmetFile)
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