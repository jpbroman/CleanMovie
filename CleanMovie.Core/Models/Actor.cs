using System.ComponentModel.DataAnnotations;
using CleanMovie.Core.Models.DTOs;
namespace CleanMovie.Core.Models;


public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    public ICollection<MovieActor> MovieActors { get; set; } = [];
}