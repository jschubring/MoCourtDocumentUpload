using System;
using System.IO;
using System.Threading;

namespace MoCourtDocumentUpload
{
	public class CaseFile
	{
		public string FullPath { get; set; }
		private string _errorFolder;
		private string _proccessedFolder;
		public CaseFile(string fileName, string fileDirectory)
		{
			FullPath = fileName;
			_errorFolder = fileDirectory + "Error\\";
			_proccessedFolder = fileDirectory + "Processed\\";
		}

		public void MoveToProcessedFolder()
		{
			var pathFolder = Path.GetFileName(FullPath);
			MoveFileWithRetry(FullPath, _proccessedFolder + pathFolder);
		}

		public void MoveToErrorFolder()
		{
			var pathFolder = Path.GetFileName(FullPath);
			MoveFileWithRetry(FullPath, _errorFolder + pathFolder);
		}
		private void MoveFileWithRetry(string sourceFullPath, string destinationFullPath)
		{

			int attempts = 0;
			const int maxAttmempts = 30;
			while (attempts < maxAttmempts)
			{
				try
				{
					File.Move(sourceFullPath, destinationFullPath);
					return;
				}
				catch (IOException e)
				{
					if (e.HResult != -2147024864)
					{
						throw;
					}
					else
					{
						attempts++;
						Thread.Sleep(1000);
					}

				}
			}
			throw new Exception($"Failed to move file with {maxAttmempts} attempts.");
		}
	}
}
