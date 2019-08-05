using System;
using System.ComponentModel.DataAnnotations;
using Cnblogs.Academy.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class AcademyControllerBase : ControllerBase
    {
        protected bool IsAuthenticated
        {
            get => User.Identity.IsAuthenticated;
        }
        protected Guid UserId
        {
            get => User.UCenter().UserId;
        }

        protected UserDto UCenterUser
        {
            get
            {
                var user = User.UCenter();
                if (user == null)
                    throw new ValidationException("无法识别当前用户");
                return user;
            }
        }
    }
}
