namespace Domain
{
    public class AppTag
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool HasSynonyms { get; set; }
        public bool IsModeratorOnly { get; set; }
        public bool IsRequired { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
}