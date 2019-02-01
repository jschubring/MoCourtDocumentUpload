using MoCourtDocumentUpload.Helpers;
using System;
using System.Configuration;
using System.IO;

namespace MoCourtDocumentUpload
{
	public class ExistingFileProcessor
	{
		private ProcessFile processFile;	
		private bool _stopProcessing = false;
		private string _fileDirectory;
		public ExistingFileProcessor()
		{
			_fileDirectory = ConfigurationManager.AppSettings["filePDirectory"];
			processFile = new ProcessFile(new DocumentProcessor());
		}
		public void StartProcessing()
		{
			var filePaths = Directory.EnumerateFiles(_fileDirectory + "Unprocessed", "*.xls", SearchOption.TopDirectoryOnly);
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
				processFile.Process(new CaseFile(path, _fileDirectory));
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
