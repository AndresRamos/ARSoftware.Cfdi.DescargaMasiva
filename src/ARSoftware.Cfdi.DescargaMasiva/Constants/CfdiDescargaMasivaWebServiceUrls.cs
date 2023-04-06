namespace ARSoftware.Cfdi.DescargaMasiva.Constants
{
    public static class CfdiDescargaMasivaWebServiceUrls
    {
        public const string AutenticacionUrl = "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/Autenticacion/Autenticacion.svc";

        public const string AutenticacionSoapActionUrl = "http://DescargaMasivaTerceros.gob.mx/IAutenticacion/Autentica";

        public const string SolicitudUrl = "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/SolicitaDescargaService.svc";

        public const string SolicitudSoapActionUrl = "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescarga";

        public const string VerificacionUrl = "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/VerificaSolicitudDescargaService.svc";

        public const string VerificacionSoapActionUrl =
            "http://DescargaMasivaTerceros.sat.gob.mx/IVerificaSolicitudDescargaService/VerificaSolicitudDescarga";

        public const string DescargaUrl = "https://cfdidescargamasiva.clouda.sat.gob.mx/DescargaMasivaService.svc";

        public const string DescargaSoapActionUrl = "http://DescargaMasivaTerceros.sat.gob.mx/IDescargaMasivaTercerosService/Descargar";
    }
}
