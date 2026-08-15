namespace CleanMovie.Core.Entities;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


public class Movie{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Duration { get; set; }
    public MovieDetails? Details { get; set; } = null;

    public ICollection<MovieActor>? MovieActors { get; set; } = [];
    public ICollection<Review>? Reviews { get; set; } = [];
}

public class MovieAllDetails{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Duration { get; set; }
    public MovieDetails? Details { get; set; } = null;

    public ICollection<MovieActor>? MovieActors { get; set; } = [];
    public ICollection<Review>? Reviews { get; set; } = [];
}
