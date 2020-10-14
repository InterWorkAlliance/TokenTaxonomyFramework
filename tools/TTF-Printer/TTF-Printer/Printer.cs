using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using log4net;
using Microsoft.Extensions.Configuration;
using Grpc.Core;
using IWA.TTF.Taxonomy.TypePrinters;
using IWA.TTF.Taxonomy.Model;

namespace IWA.TTF.Taxonomy
{
    internal static class Printer
    {
        private static IConfigurationRoot _config;
        private static ILog _log;
        private static string _taxonomyService;
        private static int _taxonomyPort;
        private static string _printerHost;
        private static int _printerPort;
        internal static Service.ServiceClient TaxonomyClient;
        private static string _printToPath;
        private static Server _apiServer;

        private static void Main(string[] args)
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

            _log.Info("TTF-Printer boot...");
            
            _printerHost = _config["printerHost"];
            _printerPort = Convert.ToInt32(_config["printerPort"]);

            _taxonomyService = _config["taxonomyHost"];
            _taxonomyPort = Convert.ToInt32(_config["taxonomyPort"]);
            _printToPath = _config["printToPath"];

            #region TCP Loop

            var tcpScan = new TcpClient();
            var open = false;
            while (!open)
                try
                {
                    tcpScan.Connect(_taxonomyService, _taxonomyPort);
                    open = true;
                }
                catch
                {
                    _log.Info("Waiting on Taxonomy Service on port: " + _taxonomyPort);
                    Thread.Sleep(2500);
                }

            _log.Info("Connected to Taxonomy Service");
            tcpScan.Close();

            #endregion
            
            _log.Info("Connection to TaxonomyService: " + _taxonomyService + " port: " + _taxonomyPort);
            TaxonomyClient = new Service.ServiceClient(
                new Channel(_taxonomyService, _taxonomyPort, ChannelCredentials.Insecure));

            ModelManager.Taxonomy = (TaxonomyClient.GetFullTaxonomy(new TaxonomyVersion
            {
                Version = "1.0"
            }));

            ModelMap.WaterMark = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + ModelMap.FolderSeparator
                                                                                              + "images" +
                                                                                              ModelMap.FolderSeparator +
                                                                                              "TTF-bw.jpg";
            
            ModelMap.DraftWaterMark = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + ModelMap.FolderSeparator
                                                                                              + "images" +
                                                                                              ModelMap.FolderSeparator +
                                                                                              "TTF-bw-draft.jpg";

            ModelMap.StyleSource =
                Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + ModelMap.FolderSeparator
                                                                             + "templates" +
                                                                             ModelMap.FolderSeparator +
                                                                             "savon.docx";

            //If running locally, not in a container or k8s, it will have a relative path instead of a volume path.
            if(_printToPath.Contains(".."))
                ModelMap.FilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                                    ModelMap.FolderSeparator +
                                    _printToPath + ModelMap.FolderSeparator;
            else
            {
                ModelMap.FilePath = _printToPath + ModelMap.FolderSeparator;
            }
            _log.Info("Printer PrintToPath: " + ModelMap.FilePath);

            if (args.Length>0 && args[0].Equals("/noserve")) {
                PrintController.BuildTtfBook(draft: false);
                PrintController.BuildTtfBook(draft: true);
            } else {
                _apiServer = new Server
                {
                    Services = {PrinterService.BindService(new Host())},
                    Ports = {new ServerPort(_printerHost, _printerPort, ServerCredentials.Insecure)}
                };
                _log.Info("Taxonomy Printer listening on: " + _printerHost + " Port: " + _printerPort);

                _apiServer.Start();
                _log.Info("Printer Api open on host: " + _printerHost + " port: " + _printerPort);
                Console.WriteLine("Printer Ready");
                var exitEvent = new ManualResetEvent(false);

                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    eventArgs.Cancel = true;
                    exitEvent.Set();
                };

                Console.WriteLine("Press \'^C\' to close the Taxonomy.Printer.Service");
                // TODO: Instructions for /noserve mode
                Console.WriteLine("To immediately print, re-run with the /noserve parameter");

                exitEvent.WaitOne();
                _apiServer.ShutdownAsync();
            }
        }
    }
}