using System.Collections.Generic;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class VerificacionResult
    {
        private VerificacionResult()
        {
        }

        public string CodEstatus { get; private set; }
        public string CodigoEstadoSolicitud { get; private set; }
        public string EstadoSolicitud { get; private set; }
        public string NumeroCfdis { get; private set; }
        public string Mensaje { get; private set; }
        public List<string> IdsPaquetes { get; private set; }
        public string WebResponse { get; private set; }

        public static VerificacionResult CreateInstance(string codEstatus, string codigoEstadoSolicitud, string estadoSolicitud, string numeroCfdis, string mensaje, List<string> idsPaquetes, string webResponse)
        {
            return new VerificacionResult
            {
                CodEstatus = codEstatus,
                CodigoEstadoSolicitud = codigoEstadoSolicitud,
                EstadoSolicitud = estadoSolicitud,
                NumeroCfdis = numeroCfdis,
                Mensaje = mensaje,
                IdsPaquetes = idsPaquetes,
                WebResponse = webResponse
            };
        }
    }
}