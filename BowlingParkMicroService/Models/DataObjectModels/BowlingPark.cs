namespace BowlingParkMicroService.Models.DataObjectModels;

public record BowlingParkRequest(string Adress, int ManagerId);
public record BowlingParkResponse(string Id, string Adress, int ManagerId, List<BowlingAlleyResponse> BowlingAlleys);