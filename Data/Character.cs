using System.ComponentModel.DataAnnotations;

namespace MangaQuotes.Data
{
    public class Character
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}