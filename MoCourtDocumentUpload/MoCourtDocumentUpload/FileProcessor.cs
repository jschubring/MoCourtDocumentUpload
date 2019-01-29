using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MoCourtDocumentUpload
{
	partial class FileProcessor : ServiceBase
	{
		private FileWatcher _FileWatcher;
		private ExistingFileProcessor _existingFileProcessor;

		public FileProcessor()
		{
			InitializeComponent();
			_existingFileProcessor = new ExistingFileProcessor();
			_FileWatcher = new FileWatcher();
		}

		protected override void OnStop()
		{
			_existingFileProcessor.StopProcessing();
			_FileWatcher.stopFileWatch();
		}

		protected override void OnStart(string[] args)
		{
			_existingFileProcessor.StartProcessing();
			_FileWatcher.StartFileWatch();
		}
	}
}
