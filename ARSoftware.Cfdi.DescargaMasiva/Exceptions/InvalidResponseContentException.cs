using System;

namespace ARSoftware.Cfdi.DescargaMasiva.Exceptions
{
    public class InvalidResponseContentException : Exception
    {
        private static readonly string DefaultMessage = "Response content is not in a valid format.";

        public InvalidResponseContentException(string message, string content) : base(
            $"{DefaultMessage} Message: {message} Content: {content}")
        {
            Content = content;
        }

        public string Content { get; }
    }
}
