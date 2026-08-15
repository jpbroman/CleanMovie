using System.ComponentModel.DataAnnotations;
using CleanMovie.Core.Entities.DTOs;
namespace CleanMovie.Core.Entities;


public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? BirthDate { get; set; }

    public ICollection<MovieActor> MovieActors { get; set; } = [];
}