namespace API.DTOs
{
    public class TagParamsDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set;} = 10;
        public string SortBy { get; set; } = "Name";
        public string OrderBy { get; set; } = "asc";
    }
}