using LinqToExcel;
using LinqToExcel.Attributes;
using MoCourtDocumentUpload.Helpers;
using MoCourtDocumentUpload.Models;
using MoCourtDocumentUpload.Repos;
using MoCourtDocumentUpload.Services;
using System;
using System.Configuration;
using System.Linq;

namespace MoCourtDocumentUpload
{
	public class ProcessFile
	{
		private static MoEcfServiceRepo _service;

		public ProcessFile()
		{
			_service = new MoEcfServiceRepo();
		}
		public void processFile(string fileName)
		{		
			var excel = new ExcelQueryFactory(fileName);
			var indianaCompanies = from c in excel.Worksheet<ExcelObject>()								
								   select c;

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
			Logger.Info("Message Sent");
		}
	}
	public class ExcelObject
	{
		[ExcelColumn("Test1")]
		public string Test1 { get; set; }
		[ExcelColumn("Test2")]
		public string Test2 { get; set; }
	}
}
