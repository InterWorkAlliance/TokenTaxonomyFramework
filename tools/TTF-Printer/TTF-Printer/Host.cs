using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using log4net;

namespace IWA.TTF.Taxonomy
{
    public class Host : PrinterService.PrinterServiceBase
    {
        private static ILog _log;

        public Host()
        {
            TypePrinters.Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
        
        public override Task<PrintResult> PrintTTFArtifact(ArtifactToPrint printArtifact, ServerCallContext ctx)
        {
            _log.Info("gRpc request to PrintTTFArtifact: " + printArtifact.Type + " id: " + printArtifact.Id);
            return Task.FromResult(PrintController.PrintArtifact(printArtifact));
        }
        
        public override Task<PrintResult> PrintTTF(PrintTTFOptions printTtfOptions, ServerCallContext ctx)
        {
            _log.Info("gRpc request to PrintTTF, Book = " + printTtfOptions.Book);
            return printTtfOptions.Book ? Task.FromResult(PrintController.BuildTtfBook(printTtfOptions.Draft)) 
                : Task.FromResult(PrintController.PrintAllArtifacts(printTtfOptions.Draft));
        }
    }
}