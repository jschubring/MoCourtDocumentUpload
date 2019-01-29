using System;
using System.ServiceProcess;
using MoCourtDocumentUpload.Helpers;

namespace MoCourtDocumentUpload
{
	class Program
    {    
		static void Main(string[] args)
		{
			try
			{
				if (!Environment.UserInteractive)
				{
					ServiceBase[] ServicesToRun;
					ServicesToRun = new ServiceBase[]
					{
				new FileProcessor()
					};
					ServiceBase.Run(ServicesToRun);
				}
				else
				{
					var fileWatcher = new FileWatcher();
					fileWatcher.StartFileWatch();
					Console.ReadLine();
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}	
		}
    }   
}
