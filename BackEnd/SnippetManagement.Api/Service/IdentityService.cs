using Microsoft.AspNetCore.Mvc;

namespace SnippetManagement.Api.Service;

public interface IIdentityService
{
    Guid GetCurrentUserId();
}

public class IdentityService : ControllerBase, IIdentityService
{
    private readonly IHttpContextAccessor _accessor;

    public IdentityService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid GetCurrentUserId()
    {
        if (_accessor.HttpContext is null)
            throw new InvalidOperationException("Method cannot be used without HttpContext");
        
        return new Guid(_accessor.HttpContext!.User.Claims.Where(c => c.Type == "UserId")
            .Select(c => c.Value).Single());
    }
}