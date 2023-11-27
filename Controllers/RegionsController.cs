using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPWalksUI.Models;
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
        [HttpGet]
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

        // Add new region page
        [HttpGet]
        [Route("Add")]
        public IActionResult Add()
        {
            return View();
        }

        // Add new region to the database
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(AddRegionViewModel addRegion)
        {
            var client = HttpClient.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5282/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegion), Encoding.UTF8, "application/json")

            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}