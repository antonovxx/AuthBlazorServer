using AuthTutorial.Http;

namespace AuthTutorial;

public class AppSettings : IAdminAuthHttpClientOptions
{
    public AppSettings(IConfiguration configuration)
    {
        AdminAuthClientOptions = configuration.GetSection("SsoHttpClient").Get<HttpClientOptions>();
    }
    
    public HttpClientOptions AdminAuthClientOptions { get ; set ; }
}