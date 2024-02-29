using System.ComponentModel.DataAnnotations.Schema;

namespace BowlingParkMicroService.Models.Entities;

[Table("bowlingAlleys")]
public class BowlingAlley
{
    public int AlleyNumber { get; set; }
    public string QrCode { get; set; }

    public string BowlingParkId { get; set; }
    public BowlingPark BowlingPark { get; set; }
}
