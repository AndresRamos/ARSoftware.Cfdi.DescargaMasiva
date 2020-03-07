namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class TipoSolicitud : Enumeration
    {
        public static readonly TipoSolicitud Cfdi = new TipoSolicitud(0, "CFDI");
        public static readonly TipoSolicitud Metadata = new TipoSolicitud(1, "Metadata");

        public TipoSolicitud(int id, string name) : base(id, name)
        {
        }
    }
}