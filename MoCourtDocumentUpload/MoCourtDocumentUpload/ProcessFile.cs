using LinqToExcel;
using LinqToExcel.Attributes;
using MoCourtDocumentUpload.Helpers;
using MoCourtDocumentUpload.Models;
using MoCourtDocumentUpload.Repos;
using MoCourtDocumentUpload.Services;
using System;
using System.Configuration;
using System.IO;
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
			try
			{
				Process(fileName);

				HandleSuccess();
			}
			catch (Exception E)
			{
				HandleError(E);
			}
		
		}

		private static void Process(string fileName)
		{
			GetFile(fileName);

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

        private static void HandleSuccess(string fileName)
        {
		
			Logger.Info("Message Sent");
		}
		private void HandleError(Exception e)
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
	public class FileProcess
	{
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public FileProcess(string filePath)
		{
			FileName=  Path.GetFileName(filePath);
			FilePath = filePath;
		}
		public void MoveFile(string path)
		{
			File.Move(FilePath, Path.Combine(path, FileName));
		}
	}
}
