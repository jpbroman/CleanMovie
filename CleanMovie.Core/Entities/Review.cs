namespace CleanMovie.Core.Entities;
public class Review
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string Reviewer { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
    
    public Movie Movie { get; set; } = null!;
}