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
		public void Process(string fileFullPath)
		{
			GetFile(fileFullPath);

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
		}

		private static void GetFile(string fileName)
		{
			var excel = new ExcelQueryFactory(fileName);
			var indianaCompanies = from c in excel.Worksheet<ExcelObject>()
								   select c;
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
	}
	public class ExcelObject
	{
		[ExcelColumn("Test1")]
		public string Test1 { get; set; }
		[ExcelColumn("Test2")]
		public string Test2 { get; set; }
	}
}
