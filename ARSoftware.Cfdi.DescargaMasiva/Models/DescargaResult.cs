namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class DescargaResult
    {
        private DescargaResult()
        {
        }

        public string CodEstatus { get; private set; }
        public string Mensaje { get; private set; }
        public string Paquete { get; private set; }
        public string ResponseContent { get; private set; }

        public static DescargaResult CreateInstance(string codEstatus, string mensaje, string paquete, string responseContent)
        {
            return new DescargaResult { CodEstatus = codEstatus, Mensaje = mensaje, Paquete = paquete, ResponseContent = responseContent };
        }
    }
}
