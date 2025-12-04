using System.Text.Json;
using LeadsSaas.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("external")]
public class ExternalController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExternalController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("ip-info")]
    public async Task<ActionResult<IpInfoDto>> GetIpInfo()
    {
        var client = _httpClientFactory.CreateClient("ExternalIpClient");

        // ipify devuelve la IP como texto plano si no se le pasa format=json
        var response = await client.GetAsync("?format=json");
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Error llamando a API externa");
        }

        using var stream = await response.Content.ReadAsStreamAsync();
        var payload = await JsonSerializer.DeserializeAsync<IpifyResponse>(stream);

        return Ok(new IpInfoDto
        {
            Ip = payload?.Ip ?? "unknown"
        });
    }
}
