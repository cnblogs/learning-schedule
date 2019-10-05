using System;
using System.Collections.Generic;
using System.Linq;
using APIMock.Data;
using Cnblogs.Academy.DTO;
using Cnblogs.Domain.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APIMock.Controllers
{
    [Route("api/[controller]")]
    public class FeedController : ControllerBase
    {
        private readonly ILogger<FeedController> _logger;

        public FeedController(ILogger<FeedController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FeedDto>> GetFeeds(int page = 1, int size = 30)
        {
            return Ok(FakeFeeds.Feeds().Take(size));
        }

        [HttpGet("concern")]
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<FeedDto>> GetPagedFeeds(
            int page = 1,
            int size = 30)
        {
            return Ok(new PagedResult<FeedDto>(500, FakeFeeds.Feeds().Take(size).ToList()));
        }

        [HttpPost]
        public IActionResult Post(FeedInputModel input)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(input));
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put(FeedUpdateModel model)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(model));
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(FeedDeletedInput model)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(model));
            return NoContent();
        }

    }
}
