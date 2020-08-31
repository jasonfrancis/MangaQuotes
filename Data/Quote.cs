using System.ComponentModel.DataAnnotations;

namespace MangaQuotes.Data
{
    public class Quote
    {
        public int Id { get; set; }

        public Quote ParentQuote { get; set; }

        [Required]
        public Character Character { get; set; }

        [Required]
        public string Chapter { get; set; }

        [Required]
        public int Page { get; set; }

        [Required]
        public string Text { get; set; }
    }
}