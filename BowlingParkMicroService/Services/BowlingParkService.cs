using BowlingParkMicroService.Extensions;
using BowlingParkMicroService.Helpers;
using BowlingParkMicroService.Models.DataObjectModels;

namespace BowlingParkMicroService.Services;

public interface IBowlingParkService
{
    IEnumerable<BowlingParkResponse> GetAll();
    BowlingParkResponse GetById(int id);
    BowlingParkResponse Create(BowlingParkRequest model);
    BowlingParkResponse Update(int id, BowlingParkRequest model);
    void Delete(int id);
}
public class BowlingParkService : IBowlingParkService
{
    private readonly DataContext _context;

    public BowlingParkService(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<BowlingParkResponse> GetAll()
        => _context.BowlingParks.Select(x => x.ToBowlingParkResponse());

    public BowlingParkResponse GetById(int id) 
        => _context.BowlingParks.Find(id)?.ToBowlingParkResponse() 
           ?? throw new AppException("Bowling Park not found", 404);

    public BowlingParkResponse Create(BowlingParkRequest model)
    {
        var bowlingPark = model.ToEntity();
        _context.BowlingParks.Add(bowlingPark);
        _context.SaveChanges();
        return bowlingPark.ToBowlingParkResponse();
    }

    public BowlingParkResponse Update(int id, BowlingParkRequest model)
    {
        var bowlingPark = _context.BowlingParks.Find(id) 
                          ?? throw new AppException("Bowling Park not found", 404);
        
        bowlingPark.Adress = model.Adress;
        bowlingPark.ManagerId = model.ManagerId;
        
        _context.BowlingParks.Update(bowlingPark);
        _context.SaveChanges();
        
        return bowlingPark.ToBowlingParkResponse();
    }

    public void Delete(int id) 
    {
        var bowlingPark = _context.BowlingParks.Find(id) 
                          ?? throw new AppException("Bowling Park not found", 404);
        
        _context.BowlingParks.Remove(bowlingPark);
        _context.SaveChanges();
    }
}