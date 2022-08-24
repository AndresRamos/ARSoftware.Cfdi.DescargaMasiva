using System;

namespace ARSoftware.Cfdi.DescargaMasiva.Models
{
    public class AutenticacionRequest
    {
        private AutenticacionRequest()
        {
        }

        public DateTime TokenCreatedDateUtc { get; private set; }
        public DateTime TokenExpiresDateUtc { get; private set; }
        public Guid Uuid { get; private set; }

        public static AutenticacionRequest CreateInstance()
        {
            DateTime tokenCreationDateUtc = DateTime.UtcNow;
            return new AutenticacionRequest
            {
                TokenCreatedDateUtc = tokenCreationDateUtc,
                TokenExpiresDateUtc = tokenCreationDateUtc.AddMinutes(5),
                Uuid = Guid.NewGuid()
            };
        }

        public static AutenticacionRequest CreateInstance(DateTime tokenCreatedDateUtc, DateTime tokenExpiresDateUtc, Guid uuid)
        {
            return new AutenticacionRequest
            {
                TokenCreatedDateUtc = tokenCreatedDateUtc, TokenExpiresDateUtc = tokenExpiresDateUtc, Uuid = uuid
            };
        }
    }
}
