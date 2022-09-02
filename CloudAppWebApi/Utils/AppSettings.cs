namespace CloudApp.Utils
{
    public static class AppSettings
    {
        private static IConfiguration? _config;

        public static IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    return _config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .Build();
                }
                return _config;
            }            
        }

        public static string ConnectionString => Configuration["ConnectionString"];
    }
}
