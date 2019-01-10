using log4net.Layout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoCourtDocumentUpload.Models;
using System.Xml.Serialization;
using System.Xml;


namespace MoCourtDocumentUpload
{
    class Program
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            //log4net.Config.XmlConfigurator.Configure();

            // build the ecf exchange document
            //MoEcfExchangeType mo = null;
            //var x = new MoEcfExchangeType().
          
                var result = new BuildDocument().ReturnDocumentXML();
            try
            {
                var client = new MoExchangeServiceReference.FilingServicePortTypeClient();
                client.ClientCredentials.UserName.UserName = "kryptonite_autofile";
                client.ClientCredentials.UserName.Password = "#123mizzou";
                var resulltz = client.fileNewCase(new MoExchangeServiceReference.MoExchangeRequestPayloadType()
                {
                    MoExchangeHeader = new MoExchangeServiceReference.MoExchangeHeaderType()
                    {
                        PayloadFormat = "text/xml",
                        CreationTimestamp = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString()
                    },
                    MoExchangeStructuredDataPayload = new MoExchangeServiceReference.MoExchangeStructuredDataPayloadType()
                    {
                        MoExchangeStructuredData = result
                    }
                });
                var test = resulltz;
            }
            catch(Exception e)
            {
                var error = e;
            }
           
            // validate the document before we send it
            Console.ReadLine();
            // send document
            // serialize document to text
            // build request
            // send request and get response

            // process response

            SendSuccessMessage();
        }

        private static void SendSuccessMessage()
        {
            //log.Info("Email Sent");
        }
    }   
}
