using DocumentFormat.OpenXml.Packaging;
using log4net;
using System.Reflection;
using DocumentFormat.OpenXml.Wordprocessing;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class FormulaPrinter
    {
        private static readonly ILog _log;
        static FormulaPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void PrintFormula(WordprocessingDocument document, TemplateFormula formula, bool book, bool isForAppendix = false)
        {
            ArtifactPrinter.AddArtifactContent(document, formula.Artifact,book, isForAppendix);
            _log.Info("Printing Template Formula Properties: " + formula.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Template Formula"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);
            
            var tDef = body.AppendChild(new Paragraph());
            var tRun = tDef.AppendChild(new Run());
            tRun.AppendChild(new Text("Template Type: " + formula.TemplateType));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", tDef);
            
            var bDef = body.AppendChild(new Paragraph());
            var bRun = bDef.AppendChild(new Run());
            bRun.AppendChild(new Text("Base Token"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bDef, JustificationValues.Center);
            
            ArtifactPrinter.GenerateArtifactSymbol(document, formula.TokenBase.Base);

            var beDef = body.AppendChild(new Paragraph());
            var beRun = beDef.AppendChild(new Run());
            beRun.AppendChild(new Text("Behaviors"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", beDef, JustificationValues.Center);
            foreach (var b in formula.Behaviors)
            {
                ArtifactPrinter.GenerateArtifactSymbol(document, b.Behavior);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbDef, JustificationValues.Center);
            }
            
            var bgDef = body.AppendChild(new Paragraph());
            var bgRun = bgDef.AppendChild(new Run());
            bgRun.AppendChild(new Text("Behavior Groups"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bgDef, JustificationValues.Center);
            foreach (var b in formula.BehaviorGroups)
            {
                ArtifactPrinter.GenerateArtifactSymbol(document, b.BehaviorGroup);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbDef, JustificationValues.Center);
            }
            
            var pDef = body.AppendChild(new Paragraph());
            var pRun = pDef.AppendChild(new Run());
            pRun.AppendChild(new Text("Property Sets"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", pDef, JustificationValues.Center);
            foreach (var p in formula.PropertySets)
            {
                ArtifactPrinter.GenerateArtifactSymbol(document, p.PropertySet);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbDef, JustificationValues.Center);
            }
            
            var cDef = body.AppendChild(new Paragraph());
            var cRun = cDef.AppendChild(new Run());
            cRun.AppendChild(new Text("Child Tokens"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", cDef, JustificationValues.Center);
            foreach (var c in formula.ChildTokens)
            {
                PrintFormula(document, c, false);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbDef, JustificationValues.Center);
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
    }
}
