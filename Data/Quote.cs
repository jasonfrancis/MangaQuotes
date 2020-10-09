using System.ComponentModel.DataAnnotations;

namespace MangaQuotes.Data
{
    public class Quote : Entity
    {
        public Quote ParentQuote { get; set; }

        [Required]
        public Character Character { get; set; }

        [Required]
        public string Chapter { get; set; }

        [Required]
        public int Page { get; set; }

        [Required]
        public string Text { get; set; }

        public override string GetDisplayName()
        {
            var truncatedQuote = Text.Length > 100 ? $"{Text.Substring(0, 100)}..." : Text;
            return $"Ch. {Chapter} Pg. {Page} {Character.Name} {truncatedQuote}";
        }
    }
}