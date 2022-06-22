namespace StoredProcedureApi.Models
{
    public class SConnection
    {
        public string? DConnection { get; set; }
        
    }

    public static class StartupExtention
    {
        public static void ConfigureSQL(this IServiceCollection service, IConfiguration config)
        {
            service.Configure<SConnection>(config.GetSection("ConnectionStrings"));
        }
    }


}

