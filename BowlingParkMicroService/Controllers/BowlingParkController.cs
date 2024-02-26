using BowlingParkMicroService.Models.DataObjectModels;
using BowlingParkMicroService.Services;
using Microsoft.AspNetCore.Mvc;
namespace BowlingParkMicroService.Controllers;


[ApiController]
[Route("[controller]")]
public class BowlingParkController : ControllerBase
{
    private readonly IBowlingParkService _bowlingParkService;

    public BowlingParkController(IBowlingParkService bowlingParkService)
    {
        _bowlingParkService = bowlingParkService;
    }
    
    [HttpGet]
    public IEnumerable<BowlingParkResponse> GetAll()
        => _bowlingParkService.GetAll();
    
    [HttpGet("{id}")]
    public BowlingParkResponse GetById(int id)
        => _bowlingParkService.GetById(id);
    
    [HttpPost]
    public BowlingParkResponse Create(BowlingParkRequest model)
        => _bowlingParkService.Create(model);
    
    [HttpPut("{id}")]
    public BowlingParkResponse Update(int id, BowlingParkRequest model)
        => _bowlingParkService.Update(id, model);
    
    [HttpDelete("{id}")]
    public void Delete(int id)
        => _bowlingParkService.Delete(id);
}