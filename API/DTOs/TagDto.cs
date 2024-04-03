namespace API.DTOs
{
    public class TagDto
    {
        public required string Name { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
}