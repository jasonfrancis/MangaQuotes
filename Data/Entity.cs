namespace MangaQuotes.Data
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public new string ToString() => Id.ToString();
        public abstract string GetDisplayName();
    }
}