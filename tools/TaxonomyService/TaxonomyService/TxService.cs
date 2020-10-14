using System;
using System.Reflection;
using System.Threading;
using Grpc.Core;
using log4net;
using Microsoft.Extensions.Configuration;

namespace IWA.TTF.Taxonomy
{
	internal static class TxService
	{
		private static IConfigurationRoot _config;
		private static ILog _log;
		internal static string ArtifactPath { get; private set; }
		internal static string Latest { get; private set; }
		private static string _gRpcHost;
		private static int _gRpcPort;
		internal static string GitId { get; private set; }
		private static string _gitPwd;
		internal static bool ReadOnlyMode { get; private set; }
		private static Server _apiServer;
		internal static string FolderSeparator { get; private set; }
		private static void Main()
		{
			#region config
			_config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.AddEnvironmentVariables()
				.Build();
			#endregion
			
			#region logging

			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

			#endregion
			
			ArtifactPath = _config["artifactPath"];
			_gRpcHost = _config["gRpcHost"];
			_gRpcPort = Convert.ToInt32(_config["gRpcPort"]);
			
			GitId = _config["gitId"];
			_gitPwd = _config["gitPwd"];
			ReadOnlyMode = Convert.ToBoolean(_config["readOnlyMode"]);

			FolderSeparator = Os.IsWindows() ? "\\" : "/";
			Latest = FolderSeparator + "latest";
			
			ModelManager.Init();
			
			_apiServer = new Server
			{
				Services = {Service.BindService(new Host())},
				Ports = {new ServerPort(_gRpcHost, _gRpcPort, ServerCredentials.Insecure)}
			};
			_log.Info("TaxonomyService listening on: " + _gRpcHost + " Port: " + _gRpcPort );
			
			_apiServer.Start();
			_log.Info("Api open on host: " + _gRpcHost + " port: " + _gRpcPort);
			Console.WriteLine("Taxonomy Ready");
			var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eventArgs) => {
                                         eventArgs.Cancel = true;
                                         exitEvent.Set();
                                       };

			Console.WriteLine("Press \'^C\' to close the Taxonomy.Service");

			exitEvent.WaitOne();
			_apiServer.ShutdownAsync();
		}
	}
}