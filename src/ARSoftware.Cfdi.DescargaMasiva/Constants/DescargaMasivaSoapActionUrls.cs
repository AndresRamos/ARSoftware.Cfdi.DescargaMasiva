namespace ARSoftware.Cfdi.DescargaMasiva.Constants
{
    public static class DescargaMasivaSoapActionUrls
    {
        public const string Autentica = "http://DescargaMasivaTerceros.gob.mx/IAutenticacion/Autentica";

        public const string SolicitaDescargaEmitidos =
            "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaEmitidos";

        public const string SolicitaDescargaRecibidos =
            "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaRecibidos";

        public const string SolicitaDescargaFolio =
            "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaFolio";

        public const string VerificaSolicitudDescarga =
            "http://DescargaMasivaTerceros.sat.gob.mx/IVerificaSolicitudDescargaService/VerificaSolicitudDescarga";

        public const string Descargar = "http://DescargaMasivaTerceros.sat.gob.mx/IDescargaMasivaTercerosService/Descargar";
    }
}
