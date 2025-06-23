using System;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de autenticacion.
    /// </summary>
    /// <param name="TokenCreatedDateUtc">Fecha de cuando el token fue creado en formato UTC.</param>
    /// <param name="TokenExpiresDateUtc">Fecha de cuando el token expira en formato UTC.</param>
    /// <param name="Uuid">UUID unico para asociar a la peticion.</param>
    public sealed record AutenticacionRequest(DateTime TokenCreatedDateUtc, DateTime TokenExpiresDateUtc, Guid Uuid)
    {
        public static AutenticacionRequest CreateDefault()
        {
            DateTime tokenCreationDateUtc = DateTime.UtcNow;
            return new AutenticacionRequest(tokenCreationDateUtc, tokenCreationDateUtc.AddMinutes(5), Guid.NewGuid());
        }
    }
}
