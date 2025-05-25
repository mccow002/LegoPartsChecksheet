using LegoChecksheet.API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegoChecksheet.API.Controllers;

[ApiController]
[Route("lego-pieces")]
public class LegoPieceController : ControllerBase
{
    private readonly LegoContext _context;

    public LegoPieceController(LegoContext context)
    {
        _context = context;
    }

    [HttpPut("toggle-owned")]
    public async Task<IActionResult> ToggleOwned([FromBody] OwnedModel model)
    {
        var piece = await _context.LegoPieces
            .FirstAsync(x => x.LegoPieceId == model.LegoPieceId);

        piece.Owned = model.Owned;

        if (model.Owned)
            piece.NumberMissing = 0;
        
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("number-missing")]
    public async Task<IActionResult> NumberMissing([FromBody] NumberMissingModel model)
    {
        var piece = await _context.LegoPieces
            .FirstAsync(x => x.LegoPieceId == model.LegoPieceId);

        piece.NumberMissing = model.NumberMissing;
        await _context.SaveChangesAsync();

        return Ok();
    }
}

public class OwnedModel
{
    public int LegoPieceId { get; set; }

    public bool Owned { get; set; }
}

public class NumberMissingModel
{
    public int LegoPieceId { get; set; }

    public int NumberMissing { get; set; }
}