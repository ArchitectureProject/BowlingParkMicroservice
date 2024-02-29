using BowlingParkMicroService.Extensions;
using BowlingParkMicroService.Helpers;
using BowlingParkMicroService.Models.DataObjectModels;
using Microsoft.EntityFrameworkCore;

namespace BowlingParkMicroService.Services;

public interface IBowlingParkService
{
    IEnumerable<BowlingParkResponse> GetAll();
    BowlingParkResponse GetById(string id);
    QrCodeResponse GetByQrCode(string qrCode);
    BowlingParkResponse Create(BowlingParkRequest model);
    BowlingParkResponse Update(string id, BowlingParkRequest model);
    void Delete(string id);
}
public class BowlingParkService : IBowlingParkService
{
    private readonly DataContext _context;

    public BowlingParkService(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<BowlingParkResponse> GetAll()
        => _context.BowlingParks
            .Include(park => park.BowlingAlleys)
            .Select(x => x.ToBowlingParkResponse());

    public BowlingParkResponse GetById(string id) 
        => _context.BowlingParks
               .Where(park => park.Id == id)
               .Include(park => park.BowlingAlleys)
               .FirstOrDefault()?
               .ToBowlingParkResponse() 
           ?? throw new AppException("Bowling Park not found", 404);

    public QrCodeResponse GetByQrCode(string qrCode)
    {
        var bowlingAlley = _context.BowlingAlleys
                              .Where(alley => alley.QrCode == qrCode)
                              .Include(alley => alley.BowlingPark)
                              .FirstOrDefault() 
                          ?? throw new AppException("Bowling Alley not found", 404);

        return new QrCodeResponse(
            bowlingAlley.BowlingParkId,
            bowlingAlley.AlleyNumber);
    }

    public BowlingParkResponse Create(BowlingParkRequest model)
    {
        var bowlingPark = model.ToEntity();
        _context.BowlingParks.Add(bowlingPark);
        _context.SaveChanges();
        return bowlingPark.ToBowlingParkResponse();
    }

    public BowlingParkResponse Update(string id, BowlingParkRequest model)
    {
        var bowlingPark = _context.BowlingParks
                              .Where(park => park.Id == id)
                              .Include(park => park.BowlingAlleys)
                              .FirstOrDefault() 
                          ?? throw new AppException("Bowling Park not found", 404);

        bowlingPark.Adress = model.Adress ?? bowlingPark.Adress;
        bowlingPark.ManagerId = model.ManagerId ?? bowlingPark.ManagerId;

        _context.BowlingParks.Update(bowlingPark);
        _context.SaveChanges();
        
        return bowlingPark.ToBowlingParkResponse();
    }

    public void Delete(string id) 
    {
        var bowlingPark = _context.BowlingParks.Find(id) 
                          ?? throw new AppException("Bowling Park not found", 404);
        
        _context.BowlingParks.Remove(bowlingPark);
        _context.SaveChanges();
    }
}