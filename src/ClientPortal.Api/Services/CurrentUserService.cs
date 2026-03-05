using System.Security.Claims;
using ClientPortal.Application.Abstractions;

namespace ClientPortal.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId != null ? Guid.Parse(userId) : null;
        }
    }

    public Guid? ClientId
    {
        get
        {
            // Assuming we store ClientId in a claim called "client_id"
            var clientId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("client_id");
            return clientId != null ? Guid.Parse(clientId) : null;
        }
    }
}
