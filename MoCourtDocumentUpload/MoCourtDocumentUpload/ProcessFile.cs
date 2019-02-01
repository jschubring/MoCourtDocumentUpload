using MoCourtDocumentUpload.Helpers;
using System;

namespace MoCourtDocumentUpload
{
	public class ProcessFile
	{
		private readonly DocumentProcessor _documentWorkflow;
		public ProcessFile(DocumentProcessor documentWorkflow)
		{
			_documentWorkflow = documentWorkflow;
		}

		public void Process(CaseFile newFile)
		{
			try
			{
				var wasSuccess = _documentWorkflow.Process(newFile.FullPath);

			    if (wasSuccess)
			    {
			        HandleSuccess(newFile);
                }
			    else
			    {
                    HandleError(newFile, new Exception("Reason why it failed."));
			    }
				
			}
			catch (Exception e)
			{
				HandleError(newFile, e);
			}		
		}
        private static void HandleSuccess(CaseFile newFile)
        {
			newFile.MoveToProcessedFolder();
			Logger.Info("Message Sent");
		}
		private static void HandleError(CaseFile newFile, Exception e)
		{
			newFile.MoveToErrorFolder();
			Logger.Error(e);
		}
	}
}
