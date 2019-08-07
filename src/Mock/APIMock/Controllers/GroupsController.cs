using System;
using System.Collections.Generic;
using APIMock.Data;
using Cnblogs.Academy.ServiceAgent.RelationService;
using Microsoft.AspNetCore.Mvc;

namespace APIMock.Controllers
{
    public class GroupsController : ControllerBase
    {
        [HttpGet("api/group/{userId}")]
        public ActionResult<IEnumerable<RelationGroupDto>> GetGroups(Guid userId)
        {
            return Ok(FakeGroups.Groups(userId));
        }
    }
}
