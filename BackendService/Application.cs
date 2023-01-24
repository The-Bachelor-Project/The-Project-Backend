namespace BackendService;

class Application
{
    static WebApplicationBuilder builder = WebApplication.CreateBuilder();
    public static WebApplication app = builder.Build();
}