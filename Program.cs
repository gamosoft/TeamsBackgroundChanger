using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace TeamsBackgroundChanger
{
    public class Program
    {
        private static readonly Random _rand = new();

        public static void Main(string[] args)
        {
            var destFolder = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                destFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Teams\Backgrounds\Uploads");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                destFolder = @"Library/Application Support/Microsoft/Teams/Backgrounds/Uploads";

            var configuration = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile($"appsettings.json")
                         .Build();
            var settings = configuration.GetSection("ImageConfig").Get<ImageConfig>();
            settings.DestinationFolder = destFolder;

            var validationErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(settings, new ValidationContext(settings), validationErrors, true))
            {
                Console.WriteLine("Configuration errors:");
                validationErrors.ForEach(e => Console.WriteLine(e));
                return;
            }
            ProcessImages(settings);
        }

        private static void ProcessImages(ImageConfig settings)
        {
            var files = Directory.GetFiles(settings.SourceFolder).Where(f => settings.SourceExtensions.Any(e => f.ToLower().EndsWith(e))).ToArray();
            var sourceFileName = files[_rand.Next(files.Length)];
            Console.WriteLine(sourceFileName);
            ApplyBackground(sourceFileName, settings);
        }

        private static void ApplyBackground(string sourceFileName, ImageConfig settings)
        {
            try
            {
                File.Copy(sourceFileName, Path.Combine(settings.DestinationFolder, $"{settings.TeamsImagePrefix}.png"), overwrite: true);
                File.Copy(sourceFileName, Path.Combine(settings.DestinationFolder, $"{settings.TeamsImagePrefix}_thumb.png"), overwrite: true);
                Console.WriteLine("Suceeded!");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed, probably you are in a meeting sharing your camera? Please stop sharing your camera and try again...");
                Console.WriteLine($"Error: {exc.Message}");
            }
        }
    }
}