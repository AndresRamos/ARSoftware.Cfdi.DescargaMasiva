﻿using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ARSoftware.Cfdi.DescargaMasiva.Models;

namespace ARSoftware.Cfdi.DescargaMasiva.Interfaces
{
    public interface IVerificacionService
    {
        string GenerateSoapRequestEnvelopeXmlContent(VerificacionRequest verificacionRequest, X509Certificate2 certificate);

        Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent,
                                                     AccessToken accessToken,
                                                     CancellationToken cancellationToken);

        Task<VerificacionResult> SendSoapRequestAsync(VerificacionRequest verificacionRequest,
                                                      X509Certificate2 certificate,
                                                      CancellationToken cancellationToken);

        VerificacionResult GetSoapResponseResult(SoapRequestResult soapRequestResult);
    }
}
