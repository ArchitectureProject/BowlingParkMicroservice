namespace BowlingParkMicroService.Models.DataObjectModels;

public record BowlingParkRequest(string? Adress, string? ManagerId);
public record BowlingParkResponse(string Id, string Adress, string ManagerId, List<BowlingAlleyResponse> BowlingAlleys);