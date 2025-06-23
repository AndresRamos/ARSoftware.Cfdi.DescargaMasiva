using ARSoftware.Cfdi.DescargaMasiva.Interfaces;
using ARSoftware.Cfdi.DescargaMasiva.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ARSoftware.Cfdi.DescargaMasiva
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCfdiDescargaMasivaServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<IHttpSoapClient, HttpSoapClient>();
            serviceCollection.AddTransient<IAutenticacionService, AutenticacionService>();
            serviceCollection.AddTransient<ISolicitudDescargaEmitidosService, SolicitudDescargaEmitidosService>();
            serviceCollection.AddTransient<ISolicitudDescargaFolioService, SolicitudDescargaFolioService>();
            serviceCollection.AddTransient<ISolicitudDescargaRecibidosService, SolicitudDescargaRecibidosService>();
            serviceCollection.AddTransient<IVerificacionService, VerificacionService>();
            serviceCollection.AddTransient<IDescargaService, DescargaService>();
            return serviceCollection;
        }
    }
}
