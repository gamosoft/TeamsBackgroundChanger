using System.ComponentModel.DataAnnotations;

namespace TeamsBackgroundChanger
{
    public class ImageConfig
    {
        [Required]
        public string TeamsImagePrefix { get; set; }

        [Required]
        public string SourceFolder { get; set; }

        [Required]
        public IEnumerable<string> SourceExtensions { get; set; }

        public string DestinationFolder { get; set; }
    }
}