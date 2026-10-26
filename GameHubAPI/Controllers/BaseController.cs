using GameHubAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameHubAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected int CurrentUserId
        {
            get
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(claim, out var currentUserId))
                    throw new CurrentUserNotFoundException("User identifier was not found");

                return currentUserId;
            }
        }
    }
}
