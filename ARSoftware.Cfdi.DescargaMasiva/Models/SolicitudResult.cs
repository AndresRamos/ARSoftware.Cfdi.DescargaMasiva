namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class SolicitudResult
    {
        public string CodEstatus { get; private set; }
        public string IdSolicitud { get; private set; }
        public string Mensaje { get; private set; }
        public string WebResponse { get; private set; }

        public static SolicitudResult CreateInstance(string codEstatus, string idSolicitud, string mensaje, string webResponse)
        {
            return new SolicitudResult
            {
                CodEstatus = codEstatus,
                IdSolicitud = idSolicitud,
                Mensaje = mensaje,
                WebResponse = webResponse
            };
        }
    }
}