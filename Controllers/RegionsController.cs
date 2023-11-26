using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPWalksUI.Models.DTO;

namespace NPWalksUI.Controllers
{
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly ILogger<RegionsController> _logger;

        public IHttpClientFactory HttpClient { get; }

        public RegionsController(ILogger<RegionsController> logger, IHttpClientFactory httpClient)
        {
            _logger = logger;
            HttpClient = httpClient;
        }

        // Get all regions from the web api
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                var client = HttpClient.CreateClient();
                var httpResponseMessage = await client.GetAsync("http://localhost:5282/api/Regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (System.Exception)
            {
                throw;
            }
            return View(response);
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}