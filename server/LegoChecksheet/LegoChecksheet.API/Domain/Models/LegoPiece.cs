using System.Text.Json.Serialization;

namespace LegoChecksheet.API.Domain.Models;

public class LegoPiece
{
    public int LegoPieceId { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public string Color { get; set; }

    public int Quantity { get; set; }

    public string? ElementId { get; set; }

    public bool Owned { get; set; }

    public int NumberMissing { get; set; }

    public int LegoSetId { get; set; }

    [JsonIgnore]
    public LegoSet LegoSet { get; set; }
}