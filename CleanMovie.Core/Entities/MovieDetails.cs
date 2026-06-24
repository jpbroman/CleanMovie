namespace CleanMovie.Core.Entities;
public class MovieDetails
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Synopsis { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public int Budget { get; set; }
    public Movie Movie { get; set; } = null!;
}
