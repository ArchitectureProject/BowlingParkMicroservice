using System.Security.Claims;
using BowlingParkMicroService.Models.DataObjectModels;
using BowlingParkMicroService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BowlingParkMicroService.Controllers;


[ApiController]
[Route("[controller]"), Authorize]
public class BowlingParkController : ControllerBase
{
    private readonly IBowlingParkService _bowlingParkService;

    public BowlingParkController(IBowlingParkService bowlingParkService)
    {
        _bowlingParkService = bowlingParkService;
    }
    
    [HttpGet, Authorize(Roles = "AGENT")]
    public IEnumerable<BowlingParkResponse> GetAll()
        => _bowlingParkService.GetAll();
    
    [HttpGet("{id}"), Authorize(Roles = "AGENT,CUSTOMER")]
    public BowlingParkResponse GetById(string id)
        => _bowlingParkService.GetById(id);
    
    [HttpGet("fromQrCode/{qrCode}"), Authorize(Roles = "AGENT")]
    public QrCodeResponse GetByQrCode(string qrCode)
        => _bowlingParkService.GetByQrCode(qrCode);
    
    [HttpGet("byManagerId/{userId}"), Authorize(Roles = "AGENT")]
    public IEnumerable<BowlingParkResponse> GetByManagerId(string userId)
        => _bowlingParkService.GetByManagerId(userId);
    
    [HttpPost, Authorize(Roles = "AGENT")]
    public BowlingParkResponse Create(BowlingParkRequest model)
        => _bowlingParkService.Create(model);
    
    [HttpPut("{id}"), Authorize(Roles = "AGENT")]
    public BowlingParkResponse Update(string id, BowlingParkRequest model)
        => _bowlingParkService.Update(id, model);
    
    [HttpDelete("{id}"), Authorize(Roles = "AGENT")]
    public void Delete(string id)
        => _bowlingParkService.Delete(id);
}