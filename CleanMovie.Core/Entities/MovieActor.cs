using System.Text.Json.Serialization;

namespace CleanMovie.Core.Entities;

public class MovieActor
{
    public int MovieId { get; set; }

    [JsonIgnore]
    public Movie Movie { get; set; } = null!;

    public int ActorId { get; set; }
    public Actor Actor { get; set; } = null!;
}

