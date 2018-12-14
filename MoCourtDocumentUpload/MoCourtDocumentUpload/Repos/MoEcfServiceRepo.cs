using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoCourtDocumentUpload.MoExchangeServiceReference;
using MoExchangeRequestPayloadType = MoCourtDocumentUpload.MoExchangeServiceReference.MoExchangeRequestPayloadType;

namespace MoCourtDocumentUpload.Repos
{
    public class MoEcfServiceRepo
    {
        public void Something()
        {
            var document = "we need MoEcfExchangeDocument";
            string theData = "the data";

            var x = new MoExchangeServiceReference.FilingServicePortTypeClient();
            x.fileNewCase(new MoExchangeServiceReference.MoExchangeRequestPayloadType()
            {
                MoExchangeHeader = new MoExchangeServiceReference.MoExchangeHeaderType(),
                MoExchangeNotification = new MoExchangeServiceReference.MoExchangeNotificationType(),
                MoExchangeStructuredDataPayload = new MoExchangeServiceReference.MoExchangeStructuredDataPayloadType
                {
                    MoExchangeStructuredData = theData
                }
            });
        }
    }
}
