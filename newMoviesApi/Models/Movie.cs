namespace newMoviesApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set;}
        public string ? Gender { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
