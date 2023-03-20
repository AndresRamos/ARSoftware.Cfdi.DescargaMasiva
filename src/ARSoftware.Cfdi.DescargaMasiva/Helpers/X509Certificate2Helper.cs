using System.Security.Cryptography.X509Certificates;

namespace ARSoftware.Cfdi.DescargaMasiva.Helpers;

public static class X509Certificate2Helper
{
    public static X509Certificate2 GetCertificate(byte[] certificate, string password)
    {
        return new X509Certificate2(certificate,
            password,
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
    }
}
