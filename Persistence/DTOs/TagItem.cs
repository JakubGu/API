namespace Persistence.DTOs
{
    public class TagItem
    {
        public bool has_synonyms { get; set; }
        public bool is_moderator_only { get; set; }
        public bool is_required { get; set; }
        public int count { get; set; }
        public required string name { get; set; }
    }
}