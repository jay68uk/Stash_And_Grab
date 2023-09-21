﻿namespace Stash_And_Grab.Api.Startup
{
    public static class SwaggerConfiguration
    {
        public static WebApplication ConfigureSwagger(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment()) return app;
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}