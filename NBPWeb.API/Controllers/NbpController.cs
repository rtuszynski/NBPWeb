using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NBPWeb.API.Models;

namespace NBPWeb.API.Controllers;

public class NbpController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;

    public NbpController(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
    }

    [HttpGet("exchange-rates")]
    public async Task<IActionResult> GetExchangeRates()
    {
        var httpClient = _httpClientFactory.CreateClient("NbpApi");
        var response = await httpClient.GetAsync("tables/A");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Failed to retrieve data from NBP.");
        }

        var content = await response.Content.ReadFromJsonAsync<NbpApiResponse[]>();
        return Ok(content);
    }

    [HttpGet("exchange-rate/{code}")]
    public async Task<IActionResult> GetExchangeRate(string code)
    {
        var httpClient = _httpClientFactory.CreateClient("NbpApi");
        var response = await httpClient.GetAsync($"rates/A/{code}");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, $"No exchange rate found for currency {code}.");
        }

        var content = await response.Content.ReadFromJsonAsync<NbpApiResponse>();
        return Ok(content);
    }
}
