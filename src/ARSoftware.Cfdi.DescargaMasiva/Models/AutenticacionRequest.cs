using System;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    /// <summary>
    ///     Peticion de autenticacion.
    /// </summary>
    public class AutenticacionRequest
    {
        private AutenticacionRequest(DateTime tokenCreatedDateUtc, DateTime tokenExpiresDateUtc, Guid uuid)
        {
            TokenCreatedDateUtc = tokenCreatedDateUtc;
            TokenExpiresDateUtc = tokenExpiresDateUtc;
            Uuid = uuid;
        }

        /// <summary>
        ///     Fecha de cuando el token fue creado en formato UTC.
        /// </summary>
        public DateTime TokenCreatedDateUtc { get; }

        /// <summary>
        ///     Fecha de cuando el token expira en formato UTC.
        /// </summary>
        public DateTime TokenExpiresDateUtc { get; }

        /// <summary>
        ///     UUID unico para asociar a la peticion.
        /// </summary>
        public Guid Uuid { get; }

        public static AutenticacionRequest CreateInstance()
        {
            DateTime tokenCreationDateUtc = DateTime.UtcNow;
            return new AutenticacionRequest(tokenCreationDateUtc, tokenCreationDateUtc.AddMinutes(5), Guid.NewGuid());
        }

        public static AutenticacionRequest CreateInstance(DateTime tokenCreatedDateUtc, DateTime tokenExpiresDateUtc, Guid uuid)
        {
            return new AutenticacionRequest(tokenCreatedDateUtc, tokenExpiresDateUtc, uuid);
        }
    }
}
