using LinqToExcel;
using LinqToExcel.Attributes;
using MoCourtDocumentUpload.Models;
using MoCourtDocumentUpload.Repos;
using MoCourtDocumentUpload.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using MoCourtDocumentUpload.Models.Mappers;

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

			var file = GetFile(fileFullPath);

		    foreach (var excelObject in file)
		    {
		        var doc = BuildTheDocument(excelObject);

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
            }

		    return wasSuccessfullyUploaded;
		}

		private List<ExcelObject> GetFile(string fileName)
		{
			var excel = new ExcelQueryFactory(fileName);
		    return excel.Worksheet<ExcelObject>().ToList();
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
		private static string BuildTheDocument(ExcelObject file)
		{
            return new BuildDocument().ReturnDocumentXML(file.ToRootObject());
		}
	}
}
