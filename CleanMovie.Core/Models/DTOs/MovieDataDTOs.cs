namespace CleanMovie.Core.Models.DTOs;

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