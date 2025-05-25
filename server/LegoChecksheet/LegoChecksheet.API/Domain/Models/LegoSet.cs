namespace LegoChecksheet.API.Domain.Models;

public class LegoSet
{
    public int LegoSetId { get; set; }

    public string Name { get; set; }

    public ICollection<LegoPiece> LegoPieces { get; set; } = new List<LegoPiece>();
}