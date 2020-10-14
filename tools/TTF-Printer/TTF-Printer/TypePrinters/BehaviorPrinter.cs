using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using log4net;
using System.Reflection;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class BehaviorPrinter
    {
        private static readonly ILog _log;
        static BehaviorPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void PrintBehavior(WordprocessingDocument document, Model.Core.Behavior behavior, bool book, bool isForAppendix = false)
        {
            
            ArtifactPrinter.AddArtifactContent(document, behavior.Artifact, book, isForAppendix);
            
            _log.Info("Printing Behavior Properties: " + behavior.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Behavior Details"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);

            var basicProps = new[,]
            {
                {"Is External:", behavior.IsExternal.ToString()},
                {"Constructor:", behavior.ConstructorType}
            };
            Utils.AddTable(document, basicProps); //"PlainTable3"

            CommonPrinter.BuildInvocationsTable(document, behavior.Invocations);
            if(behavior.Properties.Any())
                CommonPrinter.BuildPropertiesTable(document, behavior.Properties, book);

        }
        
        public static void AddBehaviorReferenceProperties(WordprocessingDocument document, BehaviorReference behavior)
        {
            _log.Info("Printing Behavior Properties: " + behavior.Reference.Id);
            var body = document.MainDocumentPart.Document.Body;
            var name = Utils.GetNameForId(behavior.Reference.Id, ArtifactType.Behavior);
            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Behavior Reference: " + name));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);

            var rDef = body.AppendChild(new Paragraph());
            var rRun = rDef.AppendChild(new Run());
            rRun.AppendChild(new Text("Reference Notes: " + behavior.Reference.ReferenceNotes));
            Utils.ApplyStyleToParagraph(document, "Quote", "Quote", rDef);

            var basicProps = new[,]
            {
                {"Is External:", behavior.IsExternal.ToString()},
                {"Constructor:", behavior.ConstructorType}
            };
            Utils.AddTable(document, basicProps); //"PlainTable3"

            var appDef = body.AppendChild(new Paragraph());
            var appRun = appDef.AppendChild(new Run());
            appRun.AppendChild(new Text("Applies To"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", appDef);
            foreach (var a in behavior.AppliesTo)
            {
                ArtifactPrinter.GenerateArtifactSymbol(document, a);
            }

            CommonPrinter.BuildInvocationsTable(document, behavior.Invocations);

            CommonPrinter.BuildInfluenceBindings(document, behavior.InfluenceBindings, ArtifactType.Behavior);
           
            if(behavior.Properties.Any())
                CommonPrinter.BuildPropertiesTable(document, behavior.Properties, false);

        }
        
        public static void AddBehaviorSpecification(WordprocessingDocument document, BehaviorSpecification behavior)
        {
            _log.Info("Printing Behavior Properties: " + behavior.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Specification Behavior"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);

            ArtifactPrinter.AddBehaviorArtifactSpecification(document, behavior.Artifact);
            ArtifactPrinter.AddArtifactContent(document, behavior.Artifact, false,false, true);
            
            var basicProps = new[,]
            {
                {"Is External:", behavior.IsExternal.ToString()},
                {"Constructor:", behavior.ConstructorType}
            };
            Utils.AddTable(document, basicProps); //"PlainTable3"

            CommonPrinter.BuildInvocationBindingsTable(document, behavior.Invocations, behavior.Artifact.Name);

            var bProps = behavior.Properties.ToArray();
            if(bProps.Length > 0)
                CommonPrinter.BuildPropertiesTable(document, behavior.Properties, false);

        }

    }
}
