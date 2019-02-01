using LinqToExcel;
using LinqToExcel.Attributes;
using MoCourtDocumentUpload.Models;
using MoCourtDocumentUpload.Repos;
using MoCourtDocumentUpload.Services;
using System;
using System.Linq;

namespace MoCourtDocumentUpload
{
	public class DocumentProcessor
	{
		private static MoEcfServiceRepo _service;

		public DocumentProcessor()
		{
			_service = new MoEcfServiceRepo();
		}
		public bool Process(string fileFullPath)
		{
		    var wasSuccessfullyUploaded = false;

			GetFile(fileFullPath);

			var doc = BuildTheDocument();

			var isValid = ValidateTheDocument(doc);

			if (isValid)
			{
				var response = SendTheDocument(doc);
				wasSuccessfullyUploaded = ProcessResponse(response);
			}
			else
			{
				HandleInvalidDocument();
			}

		    return wasSuccessfullyUploaded;
		}

		private static void GetFile(string fileName)
		{
			var excel = new ExcelQueryFactory(fileName);
			var indianaCompanies = from c in excel.Worksheet<ExcelObject>()
								   select c;
		}

		private bool ProcessResponse(MoExchangeServiceReference.MoExchangeResponsePayloadType response)
		{
            //TODO: Do we need to provide user specific information regarding failure to upload?
		    return response.MoExchangeStructuredDataPayload.MoExchangeStructuredData.Contains("RECEIVED");
		}

		private static void HandleInvalidDocument()
		{
			// TODO: How do we / should we inform user of invalid document?
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
	}
	public class ExcelObject
	{
		[ExcelColumn("Test1")]
		public string Test1 { get; set; }
		[ExcelColumn("Test2")]
		public string Test2 { get; set; }
	}
}
