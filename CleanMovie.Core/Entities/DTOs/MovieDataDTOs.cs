namespace CleanMovie.Core.Entities.DTOs;

public record MovieDto(
    int Id,
    string Title,
    int Year,
    string Genre,
    int Duration,
    MovieDetailsDto? Details,
    IEnumerable<ActorDto> Actors,
    IEnumerable<ReviewDto> Reviews);
    
public record MovieDetailsDto(
    string Synopsis,
    string Language,
    int Budget);

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
    int Duration,
    MovieDetailsDto Details,
    IEnumerable<CreateActorDto> Actors,
    IEnumerable<CreateReviewDto> Reviews);
    
public record CreateMovieDetailsDto(
    string Synopsis,
    string Language,
    int Budget);