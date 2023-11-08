using Microsoft.AspNetCore.Mvc;
using MP7_progi.Models;
using System.Text.Json;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class MainController : ControllerBase
{
    private readonly ILogger<MainController> _logger;

    public MainController(ILogger<MainController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetFrontPageData")]
    public string GetFrontPageData()
    {
        return JsonSerializer.Serialize(DatabaseFunctions.read("Oglas"));
    }
}