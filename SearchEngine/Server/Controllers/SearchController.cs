using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchEngine.Server.Application.Services.Search;
using SearchEngine.Server.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace SearchEngine.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        ISearchService searchService;

        ILogger<SearchController> logger;

        public SearchController(ISearchService _searchService, ILogger<SearchController> _logger)
        {
            this.searchService = _searchService;
            this.logger = _logger;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query,int startIndex, int endIndex)
        {
            try
            {
                var results = await Task.Run(() => searchService.Search(query, startIndex, endIndex));

                return Ok(results);
            }
            catch(Exception ex)
            {
                logger.LogWarning("A error searching the requested data {0}", ex.Message);

                return BadRequest(ex.Message);
            }
        }

    }
}
