using System;

namespace MoCourtDocumentUpload.Helpers
{
	public static class Logger
	{
		private static readonly log4net.ILog _log;

		static Logger()
		{
			_log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			log4net.Config.XmlConfigurator.Configure();
		}
		public static void Error(object message)
		{
			_log.Error(message);
		}

		internal static void Info(string message)
		{
			_log.Info(message);
		}
	}
}
