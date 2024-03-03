using BowlingParkMicroService.Helpers;
using BowlingParkMicroService.Models.DataObjectModels;
using BowlingParkMicroService.Models.Entities;

namespace BowlingParkMicroService.Extensions;

public static class BowlingParkExtensions
{
    public static BowlingParkResponse ToBowlingParkResponse(this BowlingPark park) =>
        new(
            park.Id, 
            park.Adress, 
            park.ManagerId, 
            park.BowlingAlleys
                .Select(a => a.ToBowlingAlleyResponse())
                .ToList());
    
    public static BowlingPark ToEntity(this BowlingParkCreationRequest model)
    {
        var id = Guid.NewGuid().ToString();
        return new BowlingPark
        {
            Id = id,
            Adress = model.Adress,
            ManagerId = model.ManagerId,
            // init list with exactly 20 elements, each with id from 0 to 19
            BowlingAlleys = Enumerable.Range(0, 20)
                .Select(i => new BowlingAlley { AlleyNumber = i, QrCode = "alley_qr_code_" + i + "_" + id })
                .ToList()
        };
    }
}