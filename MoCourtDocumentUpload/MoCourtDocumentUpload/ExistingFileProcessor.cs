using MoCourtDocumentUpload.Helpers;
using System;
using System.Configuration;
using System.IO;

namespace MoCourtDocumentUpload
{
	public class ExistingFileProcessor
	{
		private ProcessFile processFile;	
		private string FileDirectory;
		private bool _stopProcessing = false;
		public ExistingFileProcessor()
		{
			processFile = new ProcessFile();
		}
		public void StartProcessing()
		{
			var filePaths = Directory.EnumerateFiles(ConfigurationManager.AppSettings["processedPath"], "*.xls", SearchOption.TopDirectoryOnly);
			foreach (var path in filePaths)
			{
				if (_stopProcessing == true) break;

				ProcessFile(path);
			}
		}

		private void ProcessFile(string path)
		{
			try
			{
				processFile.processFile();
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		public void StopProcessing()
		{
			_stopProcessing = true;
		}
	}
}
