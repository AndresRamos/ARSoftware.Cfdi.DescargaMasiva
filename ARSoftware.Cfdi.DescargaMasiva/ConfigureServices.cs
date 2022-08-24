using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ARSoftware.Cfdi.DescargaMasiva
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddCfdiDescargaMasivaServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<IHttpSoapClient, HttpSoapClient>();
            serviceCollection.AddTransient<IAutenticacionService, AutenticacionService>();
            serviceCollection.AddTransient<ISolicitudService, SolicitudService>();
            serviceCollection.AddTransient<IVerificacionService, VerificacionService>();
            serviceCollection.AddTransient<IDescargaService, DescargaService>();
            return serviceCollection;
        }
    }
}
