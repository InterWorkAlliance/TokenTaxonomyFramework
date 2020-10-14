using DocumentFormat.OpenXml.Packaging;
using log4net;
using System.Reflection;
using DocumentFormat.OpenXml.Wordprocessing;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class DefinitionPrinter
    {
        private static readonly ILog _log;
        static DefinitionPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void PrintDefinition(WordprocessingDocument document, TemplateDefinition definition, bool book, bool isForAppendix = false)
        {
            
            ArtifactPrinter.AddArtifactContent(document, definition.Artifact, book,isForAppendix);
            _log.Info("Printing Template Definition Properties: " + definition.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Template Definition"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);
            
            var tDef = body.AppendChild(new Paragraph());
            var tRun = tDef.AppendChild(new Run());

            var formulaName =
                Utils.GetNameForId(definition.FormulaReference.Id, ArtifactType.TemplateFormula);
            
            tRun.AppendChild(new Text("Template Formula Reference: " + formulaName));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", tDef);
            
            ArtifactPrinter.AddArtifactReference(document, definition.FormulaReference);
            
            BasePrinter.AddBaseReference(document, definition.TokenBase);

            var beDef = body.AppendChild(new Paragraph());
            var beRun = beDef.AppendChild(new Run());
            beRun.AppendChild(new Text("Behaviors"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", beDef, JustificationValues.Center);
            foreach (var b in definition.Behaviors)
            {
                BehaviorPrinter.AddBehaviorReferenceProperties(document, b);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", bbDef, JustificationValues.Center);
            }
            
            var bgDef = body.AppendChild(new Paragraph());
            var bgRun = bgDef.AppendChild(new Run());
            bgRun.AppendChild(new Text("Behavior Groups"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bgDef, JustificationValues.Center);
            foreach (var bg in definition.BehaviorGroups)
            {
                ArtifactPrinter.AddArtifactReference(document, bg.Reference);
                foreach (var b in bg.BehaviorArtifacts)
                {
                    var bbeDef = body.AppendChild(new Paragraph());
                    var bbeRun = bbeDef.AppendChild(new Run());
                    bbeRun.AppendChild(new Text("Behavior"));
                    Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbeDef);

                    BehaviorPrinter.AddBehaviorReferenceProperties(document, b);
                }
            }
            
            var pDef = body.AppendChild(new Paragraph());
            var pRun = pDef.AppendChild(new Run());
            pRun.AppendChild(new Text("Property Sets"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", pDef, JustificationValues.Center);
            foreach (var p in definition.PropertySets)
            {
                ArtifactPrinter.AddArtifactReference(document, p.Reference);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbDef, JustificationValues.Center);
                CommonPrinter.BuildPropertiesTable(document, p.Properties, false);
            }
            
            var cDef = body.AppendChild(new Paragraph());
            var cRun = cDef.AppendChild(new Run());
            cRun.AppendChild(new Text("Child Tokens"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", cDef, JustificationValues.Center);
            foreach (var c in definition.ChildTokens)
            {
                PrintDefinition(document, c, false);
                Utils.InsertSpacer(document);
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
