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
		public FileWatcher()
		{
			_theWatcher = new FileSystemWatcher();
			processFile = new ProcessFile();
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
				_theWatcher.Path = ConfigurationManager.AppSettings["processedPath"];
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
				processFile.processFile(e.FullPath);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}
	}
}