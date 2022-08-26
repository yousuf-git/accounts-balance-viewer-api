using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Models.Requests;
using AccountsViewer.API.Models.Responses;
using AccountsViewer.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public DebugController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            Status = "Running..."
        });
    }

    [HttpGet("config")]
    public IActionResult GetConfigs()
    {
        if (_configuration["ASPNETCORE_ENVIRONMENT"] != "Development")
        {
            return BadRequest(new
            {
                Error = "Run in development mode to view config"
            });
        }        
        
        return Ok(_configuration.AsEnumerable().ToList());
    }
}