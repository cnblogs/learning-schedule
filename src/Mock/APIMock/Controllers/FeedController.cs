using System;
using System.Collections.Generic;
using System.Linq;
using APIMock.Data;
using Cnblogs.Academy.DTO;
using Cnblogs.Domain.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace APIMock.Controllers
{
    public class FeedController : ControllerBase
    {
        [HttpGet("/api/feed/")]
        public ActionResult<IEnumerable<FeedDto>> GetFeeds(int page = 1, int size = 30)
        {
            return Ok(FakeFeeds.Feeds().Take(size));
        }

        [HttpGet("/api/feed/concern")]
        [HttpGet("/api/feed/{userId}")]
        public ActionResult<IEnumerable<FeedDto>> GetPagedFeeds(
            int page = 1,
            int size = 30)
        {
            return Ok(new PagedResults<FeedDto>(500, FakeFeeds.Feeds().Take(size).ToList()));
        }
    }
}
