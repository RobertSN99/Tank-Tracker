using Microsoft.AspNetCore.Authentication;
using Server.Data;

public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;

    public SessionValidationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var sessionIdClaim = context.User.FindFirst("SessionId")?.Value;

                if (!Guid.TryParse(sessionIdClaim, out var sessionId))
                {
                    await context.SignOutAsync("Identity.Application");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var session = await dbContext.UserSessions.FindAsync(sessionId);
                var now = DateTime.UtcNow;

                if (session == null || session.LogoutTime != null || session.ExpirationTime <= now)
                {
                    await context.SignOutAsync("Identity.Application");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
        }
        catch
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An error occurred while validating session.");
            return;
        }

        await _next(context);
    }
}
