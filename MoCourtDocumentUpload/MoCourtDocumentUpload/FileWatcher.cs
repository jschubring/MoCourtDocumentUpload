using MoCourtDocumentUpload.Helpers;
using System;
using System.Configuration;
using System.IO;

namespace MoCourtDocumentUpload
{
	public class FileWatcher
	{		
		private FileSystemWatcher _theWatcher;
		private ProcessFile processFile;
		private string _fileDirectory;
		public FileWatcher()
		{
			_theWatcher = new FileSystemWatcher();
			_fileDirectory = ConfigurationManager.AppSettings["filePDirectory"];
			processFile = new ProcessFile(new DocumentProcessor());
		}

		public void stopFileWatch()
		{
			_theWatcher.Created -= new FileSystemEventHandler(OnChanged);
			_theWatcher.Dispose();
		}

		public void StartFileWatch()
		{
			try
			{
				_theWatcher.Path = _fileDirectory + "Unprocessed";
				_theWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName;
				_theWatcher.Filter = "*.xls";
				_theWatcher.Created += new FileSystemEventHandler(OnChanged);
				_theWatcher.EnableRaisingEvents = true;
				_theWatcher.Error += OnError;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
			}
		}
		private void OnError(object sender, ErrorEventArgs ex)
		{
			Logger.Error(ex);
		}
		private void OnChanged(object source, FileSystemEventArgs e)
		{
			try
			{
				processFile.Process(new CaseFile(e.FullPath, _fileDirectory));
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}
	}
}