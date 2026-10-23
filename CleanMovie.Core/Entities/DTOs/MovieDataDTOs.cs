using System.Collections.Generic;
namespace CleanMovie.Core.Entities.DTOs;

public record MovieDto(
    int Id,
    string Title,
    int Year,
    string Genre,
    int Duration,
    MovieDetailsDto? Details = null,
    IEnumerable<ActorDto>? Actors = null,
    IEnumerable<ReviewDto>? Reviews = null);
    
public record MovieDetailsDto(
    int? MovieId,
    string? Synopsis,
    string? Language,
    int? Budget);


public record ActorDto(
    int Id,
    string Name,
    string BirthDate);

public record ReviewDto(
    int Id,
    int MovieId,
    string Reviewer,
    string Comment,
    int Rating,
    Movie? Movie)
{
    public ReviewDto(int id, string reviewer, string comment, int rating)
        : this(id, 0, reviewer, comment, rating, null!)
    {
    }
}


public record CreateActorDto(
    string Name,
    string BirthDate);
    
public record CreateReviewDto(
    int MovieId,
    string Reviewer,
    string Comment,
    int Rating,
    Movie? Movie);

public record CreateMovieDto(
    string Title,
    int Year,
    string Genre,
    int Duration);
 
 public record CreateMovieDetailsDto(
    int? MovieId,
    string? Synopsis = "",
    string? Language = "",
    int? Budget = 0,
    Movie? Movie = null);

public record MovieAllDataDto(
    MovieDto Movie,
    MovieDetailsDto? Details,
    IEnumerable<ActorDto>? Actors,
    IEnumerable<ReviewDto>? Reviews);