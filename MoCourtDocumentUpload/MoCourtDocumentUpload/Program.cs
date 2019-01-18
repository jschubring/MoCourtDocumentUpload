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
using MoCourtDocumentUpload.Repos;
using MoCourtDocumentUpload.Services;


namespace MoCourtDocumentUpload
{
    class Program
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static MoEcfServiceRepo _service;

        public Program()
        {
            _service = new MoEcfServiceRepo();
        }

        static void Main(string[] args)
        {
            //log4net.Config.XmlConfigurator.Configure();
            var doc = BuildTheDocument();
           
            var isValid = ValidateTheDocument(doc);

            if (isValid)
            {
                var response = SendTheDocument(doc);
                ProcessResponse(response);
            }
            else
            {
                HandleInvalidDocument();
            }

            SendSuccessMessage();
        }

        private static void ProcessResponse(MoExchangeServiceReference.MoExchangeResponsePayloadType response)
        {
            throw new NotImplementedException();
        }

        private static void HandleInvalidDocument()
        {
            throw new NotImplementedException();
        }

        private static MoExchangeServiceReference.MoExchangeResponsePayloadType SendTheDocument(string doc)
        {
            return _service.SendRequest(doc);
        }

        private static bool ValidateTheDocument(string doc)
        {
            var validator = new Validator();
            return validator.ValidateDocument(doc);
        }
        private static string BuildTheDocument()
        {
            return new BuildDocument().ReturnDocumentXML(new RootObject());
        }

        private static void SendSuccessMessage()
        {
            //log.Info("Email Sent");
        }
    }   
}
