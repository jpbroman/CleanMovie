namespace CleanMovie.Core.Entities.DTOs;

public record MovieDto(
    int Id,
    string Title,
    int Year,
    string Genre,
    int Duration);
    
public record MovieDetailsDto(
    int? MovieId,
    string? Synopsis,
    string? Language,
    int? Budget);

public record ActorDto(
    int Id,
    string Name,
    DateTime BirthDate);

public record ReviewDto(
    int Id,
    string Reviewer,
    string Comment,
    int Rating);

public record CreateActorDto(
    string Name,
    DateTime BirthDate);
    
public record CreateReviewDto(
    string Reviewer,
    string Comment,
    int Rating);

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