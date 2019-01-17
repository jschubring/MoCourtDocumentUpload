using System;

namespace MoCourtDocumentUpload.Repos
{
    public class MoEcfServiceRepo
    {
        public MoExchangeServiceReference.MoExchangeResponsePayloadType SendRequest(MoEcfExchangeType doc)
        {

            MoExchangeServiceReference.MoExchangeResponsePayloadType response = null;

            try
            {
                var payload = new MoExchangeServiceReference.MoExchangeRequestPayloadType()
                {
                    MoExchangeHeader = new MoExchangeServiceReference.MoExchangeHeaderType(),
                    MoExchangeNotification = new MoExchangeServiceReference.MoExchangeNotificationType(),
                    MoExchangeStructuredDataPayload = new MoExchangeServiceReference.MoExchangeStructuredDataPayloadType
                    {
                        MoExchangeStructuredData = doc.ToString()
                    }
                };

                var x = new MoExchangeServiceReference.FilingServicePortTypeClient();
                response = x.fileNewCase(payload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: ", ex);
            }

            return response;
        }
    }
}
