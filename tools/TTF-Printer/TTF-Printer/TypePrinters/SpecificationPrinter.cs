using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using log4net;
using System.Reflection;
using DocumentFormat.OpenXml.Wordprocessing;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class SpecificationPrinter
    {
        private static readonly ILog _log;
        static SpecificationPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void PrintSpecification(WordprocessingDocument document, TokenSpecification spec, bool isForAppendix = false)
        {
            var classification = ModelMap.GetClassification(spec);
            ArtifactPrinter.AddArtifactSpecification(document, spec.Artifact, classification, true);
            _log.Info("Printing Token Specification Properties: " + spec.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;
            
            var beDef = body.AppendChild(new Paragraph());
            var beRun = beDef.AppendChild(new Run());
            beRun.AppendChild(new Text(spec.Artifact.Name + " is:"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", beDef);
            
            var behaviorList = spec.Behaviors.Select(b => b.Artifact.Name).ToList();
            Utils.AddBulletList(document, behaviorList);

            if (spec.PropertySets.Count > 0)
            {
                var bgDef = body.AppendChild(new Paragraph());
                var bgRun = bgDef.AppendChild(new Run());
                bgRun.AppendChild(new Text("It includes the following Property Sets:"));
                Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", bgDef, JustificationValues.Center);

                var propSetList = spec.PropertySets.Select(b => b.Artifact.Name).ToList();
                Utils.AddBulletList(document, propSetList);
            }

            if (spec.ChildTokens.Count > 0)
            {
                var cDef = body.AppendChild(new Paragraph());
                var cRun = cDef.AppendChild(new Run());
                cRun.AppendChild(new Text("As a Hybrid " + spec.Artifact.Name + " has the following Child Tokens:"));
                Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", cDef, JustificationValues.Center);
                foreach (var c in spec.ChildTokens)
                {
                    ArtifactPrinter.AddChildArtifactSpecification(document, c);
                    var bbDef = body.AppendChild(new Paragraph());
                    var bbRun = bbDef.AppendChild(new Run());
                    bbRun.AppendChild(new Text(""));
                    Utils.ApplyStyleToParagraph(document, "Normal", "Normal", bbDef, JustificationValues.Center);
                }
            }

            var detailsDef = body.AppendChild(new Paragraph());
            var detailsRun = detailsDef.AppendChild(new Run());
            detailsRun.AppendChild(new Text(spec.Artifact.Name + " Details"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", detailsDef, JustificationValues.Center);
            
            BasePrinter.AddBaseSpecification(document, spec.TokenBase);

            var dDef = body.AppendChild(new Paragraph());
            var dRun = dDef.AppendChild(new Run());
            dRun.AppendChild(new Text("Behaviors"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", dDef);
            
            foreach (var b in spec.Behaviors)
            {
                BehaviorPrinter.AddBehaviorSpecification(document, b);
            }
            
            foreach (var bg in spec.BehaviorGroups)
            {
               BehaviorGroupPrinter.AddBehaviorGroupSpecification(document, bg);
            }
            
            foreach (var ps in spec.PropertySets)
            {
                PropertySetPrinter.AddPropertySetSpecification(document, ps);
            }
            
            foreach (var c in spec.ChildTokens)
            {
                PrintSpecification(document, c, false);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", bbDef, JustificationValues.Center);
            }
            
            if (isForAppendix) return;
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
