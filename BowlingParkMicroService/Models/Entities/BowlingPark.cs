using System.ComponentModel.DataAnnotations.Schema;

namespace BowlingParkMicroService.Models.Entities;

[Table("bowlingPark")]
public class BowlingPark
{
    public string Id { get; init; }
    public string Adress { get; set; }
    public string ManagerId { get; set; }
    public IEnumerable<BowlingAlley> BowlingAlleys { get; init; }
}