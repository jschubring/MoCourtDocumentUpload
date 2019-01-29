using MoCourtDocumentUpload.Helpers;
using System;

namespace MoCourtDocumentUpload
{
	public class ProcessFile
	{
		private DocumentProcessor _documentWorkflow;
		public ProcessFile(DocumentProcessor documentWorkflow)
		{
			_documentWorkflow = documentWorkflow;
		}
		public void processFile(CaseFile newFile)
		{
			try
			{
				_documentWorkflow.Process(newFile.FullPath);
				HandleSuccess(newFile);
			}
			catch (Exception E)
			{
				HandleError(newFile, E);
			}		
		}
        private static void HandleSuccess(CaseFile newFile)
        {
			newFile.MoveToProcessedFolder();
			Logger.Info("Message Sent");
		}
		private void HandleError(CaseFile newFile, Exception e)
		{
			newFile.MoveToErrorFolder();
			Logger.Error(e);
		}
	}
}
