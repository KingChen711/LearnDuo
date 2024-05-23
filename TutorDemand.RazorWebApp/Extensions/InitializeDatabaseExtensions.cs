using TutorDemand.Data;

namespace TutorDemand.RazorWebApp.Extensions
{
    public static class InitializeDatabaseExtension
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();

                await initializer.InitializeAsync();
                await initializer.SeedAsync();
            }
        }
    }
}
