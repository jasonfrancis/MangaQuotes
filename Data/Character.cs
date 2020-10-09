using System.ComponentModel.DataAnnotations;

namespace MangaQuotes.Data
{
    public class Character : Entity
    {
        [Required]
        public string Name { get; set; }

        public override string GetDisplayName()
        {
            return Name;
        }
    }
}