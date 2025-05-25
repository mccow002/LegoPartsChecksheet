using System.Text.Json;
using System.Text.Json.Serialization;
using LegoChecksheet.API.Domain;
using LegoChecksheet.API.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegoChecksheet.API.Controllers;

[ApiController]
[Route("lego-set")]
public class LegoSetController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    private readonly LegoContext _context;

    public LegoSetController(IHttpClientFactory factory, LegoContext context)
    {
        _factory = factory;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var set = await _context.LegoSets
            .Include(x => x.LegoPieces.OrderBy(lp => lp.Color))
            .FirstAsync(x => x.LegoSetId == 70917);

        return Ok(set);
    } 
    
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var client = _factory.CreateClient();
        var rsp = await client.GetAsync("https://rebrickable.com/api/v3/lego/sets/70917-1/parts?page_size=1500&key=5902077d180d3326e2a803d34d2d1885");
        var json = await rsp.Content.ReadAsStringAsync();

        var legoSetRsp = JsonSerializer.Deserialize<LegoSetResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var legoSet = new LegoSet
        {
            Name = "Ultimate Batmobile",
            LegoSetId = 70917,
            LegoPieces = legoSetRsp.Results
                .Where(x => !x.IsSpare)
                .Select(x => new LegoPiece
                {
                    Name = x.Part.Name,
                    LegoPieceId = x.Id,
                    Quantity = x.Quantity,
                    ImageUrl = x.Part.PartImgUrl,
                    Color = x.Color.Name,
                    ElementId = x.ElementId
                })
                .ToList()
        };

        _context.Add(legoSet);
        await _context.SaveChangesAsync();

        return Ok();
    } 
}

public class LegoSetResponse
{
    public List<LegoPieceResponse> Results { get; set; }
}

public class LegoPieceResponse
{
    public int Id { get; set; }

    public LegoPartInfo Part { get; set; }

    public PieceColorInfo Color { get; set; }

    public int Quantity { get; set; }

    [JsonPropertyName("is_spare")]
    public bool IsSpare { get; set; }

    [JsonPropertyName("element_id")]
    public string ElementId  { get; set; }
}

public class LegoPartInfo
{
    public string Name { get; set; }

    [JsonPropertyName("part_img_url")]
    public string PartImgUrl { get; set; }
}

public class PieceColorInfo
{
    public string Name { get; set; }
}