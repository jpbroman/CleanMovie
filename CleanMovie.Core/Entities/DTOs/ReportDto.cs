namespace CleanMovie.Core.Entities.DTOs;

public record GenreRatingDto(string Genre, double AverageRating);
public record ActorActivityDto(string ActorName, int MovieCount);

public record DashboardReportDto(
    List<GenreRatingDto> TopGenresByRating,
    List<ActorActivityDto> MostActiveActors
);
