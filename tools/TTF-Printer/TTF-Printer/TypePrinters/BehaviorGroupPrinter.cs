using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using System.Reflection;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class BehaviorGroupPrinter
    {
        private static readonly ILog _log;
        static BehaviorGroupPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void PrintBehaviorGroup(WordprocessingDocument document, BehaviorGroup bg, bool book, bool isForAppendix = false)
        {
            ArtifactPrinter.AddArtifactContent(document, bg.Artifact, book,isForAppendix);
            
            _log.Info("Printing Behavior Group Properties: " + bg.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Behavior Group Details"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);

            foreach (var br in bg.Behaviors)
            {
                BehaviorPrinter.AddBehaviorReferenceProperties(document, br);
            }
            if (!book) return;
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            pbr.AppendChild(new Text(""));

            if (pageBreak.ParagraphProperties == null)
            {
                pageBreak.ParagraphProperties = new ParagraphProperties();
            }

            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", pageBreak);

        }
        
        public static void AddBehaviorGroupSpecification(WordprocessingDocument document, BehaviorGroupSpecification bg)
        {
            ArtifactPrinter.AddArtifactContent(document, bg.Artifact, false,true);
            _log.Info("Printing Behavior Group Properties: " + bg.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var cDef = body.AppendChild(new Paragraph());
            var dRun = cDef.AppendChild(new Run());
            dRun.AppendChild(new Text("The behaviors belonging to this group are included in the Behaviors section of this specification."));
            Utils.ApplyStyleToParagraph(document, "Quote", "Quote", cDef);
        }
    }
}
