using Stash_And_Grab.Api.Endpoints;

namespace Stash_And_Grab.Api.Startup
{
    /// <summary>
    ///     Used to register all the API endpoints in program.cs
    ///     Each call is to a static class that has the endpoint mapping in it
    /// </summary>
    public static class RegisterEndpoints
    {
        public static void RegisterServiceEndpoints(this WebApplication app)
        {
            MapEndpoints.MapServiceEndpoints(app);
        }
    }
}