using BowlingParkMicroService.Models.DataObjectModels;
using BowlingParkMicroService.Models.Entities;

namespace BowlingParkMicroService.Extensions;

public static class BowlingAlleyExtensions
{
    public static BowlingAlleyResponse ToBowlingAlleyResponse(this BowlingAlley alley) =>
        new(
            alley.AlleyNumber, 
            alley.QrCode);
}