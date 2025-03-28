using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using NBPWeb.API.Models;

namespace NBPWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("FixedWindowPolicy")]
public class NbpController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;

    public NbpController(IHttpClientFactory httpClientFactory, IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
    }

    [HttpGet("exchange-rates")]
    public async Task<IActionResult> GetExchangeRates()
    {
        if (_cache.TryGetValue("ExchangeRates", out NbpApiResponse[] cachedData))
        {
            Response.Headers["Cache-Status"] = "HIT";
            return Ok(cachedData);
        }

        Response.Headers["Cache-Status"] = "MISS";
        var httpClient = _httpClientFactory.CreateClient("NbpApi");
        var response = await httpClient.GetAsync("tables/A");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Błąd NBP.");
        }

        var data = await response.Content.ReadFromJsonAsync<NbpApiResponse[]>();
        _cache.Set("ExchangeRates", data, TimeSpan.FromMinutes(10));

        return Ok(data);
    }

    [HttpGet("exchange-rate/{code}")]
    public async Task<IActionResult> GetExchangeRate(string code)
    {
        var cacheKey = $"ExchangeRate_{code}";

        if (_cache.TryGetValue(cacheKey, out NbpApiResponse cachedRate))
        {
            Response.Headers["Cache-Status"] = "HIT";
            return Ok(cachedRate);
        }

        Response.Headers["Cache-Status"] = "MISS";
        var httpClient = _httpClientFactory.CreateClient("NbpApi");
        var response = await httpClient.GetAsync($"rates/A/{code}");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, $"No exchange rate found for currency {code}.");
        }

        var rateData = await response.Content.ReadFromJsonAsync<NbpApiResponse>();

        _cache.Set(cacheKey, rateData, TimeSpan.FromMinutes(10));

        return Ok(rateData);
    }
}
