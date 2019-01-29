using System;

namespace MoCourtDocumentUpload.Repos
{
    public class MoEcfServiceRepo
    {
        public MoExchangeServiceReference.MoExchangeResponsePayloadType SendRequest(string doc)
        {

            MoExchangeServiceReference.MoExchangeResponsePayloadType response = null;

            try
            {
                var payload = new MoExchangeServiceReference.MoExchangeRequestPayloadType()
                {
                    MoExchangeHeader = new MoExchangeServiceReference.MoExchangeHeaderType
                    {
                        PayloadFormat = "text/xml",
                        CreationTimestamp = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString()
                    },
                    MoExchangeNotification = new MoExchangeServiceReference.MoExchangeNotificationType(),
                    MoExchangeStructuredDataPayload = new MoExchangeServiceReference.MoExchangeStructuredDataPayloadType
                    {
                        MoExchangeStructuredData = doc
                    }
                };

                var x = new MoExchangeServiceReference.FilingServicePortTypeClient();
				x.ClientCredentials.UserName.UserName = "kryptonite_autofile";
				x.ClientCredentials.UserName.Password = "#123mizzou";
				response = x.fileNewCase(payload);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: ", ex);
            }

            return response;
        }
    }
}
