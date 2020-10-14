using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using System.Reflection;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class BasePrinter
    {
        private static readonly ILog _log;
        static BasePrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }
        public static void PrintTokenBase(WordprocessingDocument document, Base tokenBase, bool book, bool isForAppendix = false)
        {
            //print artifact stuff 1st
            ArtifactPrinter.AddArtifactContent(document, tokenBase.Artifact, book, isForAppendix);
            
            _log.Info("Printing Base Properties: " + tokenBase.TokenType);
            var body = document.MainDocumentPart.Document.Body;
            var baseProps = new[,]
            {
                {"Token Name:", tokenBase.Name},
                {"Token Type:", tokenBase.TokenType.ToString() },
                {"Representation Type:", tokenBase.RepresentationType.ToString()},
                {"Value Type:", tokenBase.ValueType.ToString()},
                {"Token Unit:",tokenBase.TokenUnit.ToString() },
                {"Symbol:", tokenBase.Symbol},
                {"Owner:", tokenBase.Owner},
                {"Quantity:", tokenBase.Quantity.ToString()},
                {"Decimals:", tokenBase.Decimals.ToString()},
                {"Constructor Name:",  tokenBase.ConstructorName}
            };

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Base Details"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);
            Utils.AddTable(document, baseProps);

            if (tokenBase.TokenProperties.Count > 0)
            {
                var propDef = body.AppendChild(new Paragraph());
                var propRun = propDef.AppendChild(new Run());
                propRun.AppendChild(new Text("Properties:"));
                Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", propDef);
                
                var propsPara = body.AppendChild(new Paragraph());
                var propsRun = propsPara.AppendChild(new Run());
                propsRun.AppendChild(
                    Utils.GetGenericPropertyTable(document, "Name", "Value", tokenBase.TokenProperties));
            }

            if (!book) return;
            
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            if (pageBreak.ParagraphProperties == null)
            {
                pageBreak.ParagraphProperties = new ParagraphProperties();
            }

            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
            pbr.AppendChild(new Text(""));
        }
        
        public static void AddBaseReference(WordprocessingDocument document, BaseReference tokenBase)
        {
            _log.Info("Printing Base Properties: " + tokenBase.Reference.Id);
            var body = document.MainDocumentPart.Document.Body;
            var baseProps = new[,]
            {
                {"Token Name:", tokenBase.Name},
                {"Symbol:", tokenBase.Symbol},
                {"Owner:", tokenBase.Owner},
                {"Quantity:", tokenBase.Quantity.ToString()},
                {"Decimals:", tokenBase.Decimals.ToString()},
                {"Constructor Name:",  tokenBase.ConstructorName}
            };

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Base Details"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);
            Utils.AddTable(document, baseProps);

            if (tokenBase.TokenProperties.Count <= 0) return;
            
            var propDef = body.AppendChild(new Paragraph());
            var propRun = propDef.AppendChild(new Run());
            propRun.AppendChild(new Text("Properties:"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", propDef);

            var propsPara = body.AppendChild(new Paragraph());
            var propsRun = propsPara.AppendChild(new Run());
            propsRun.AppendChild(Utils.GetGenericPropertyTable(document, "Name", "Value", tokenBase.TokenProperties));
        }
        
        public static void AddBaseSpecification(WordprocessingDocument document,  Base tokenBase)
        {
            ArtifactPrinter.AddArtifactContent(document, tokenBase.Artifact, false, true);
            _log.Info("Printing Biz Base Properties: " + tokenBase.TokenType);
            var body = document.MainDocumentPart.Document.Body;
            var baseProps = new[,]
            {
                {"Token Name:", tokenBase.Name},
                {"Token Type:", tokenBase.TokenType.ToString() },
                {"Representation Type:", tokenBase.RepresentationType.ToString()},
                {"Value Type:", tokenBase.ValueType.ToString()},
                {"Token Unit:",tokenBase.TokenUnit.ToString() },
                {"Symbol:", tokenBase.Symbol},
                {"Owner:", tokenBase.Owner},
                {"Quantity:", tokenBase.Quantity.ToString()},
                {"Decimals:", tokenBase.Decimals.ToString()},
                {"Constructor Name:",  tokenBase.ConstructorName}
            };

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Base Details"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);
            Utils.AddTable(document, baseProps);

            if (tokenBase.TokenProperties.Count <= 0) return;
            var propDef = body.AppendChild(new Paragraph());
            var propRun = propDef.AppendChild(new Run());
            propRun.AppendChild(new Text("Properties:"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", propDef);

            var propsPara = body.AppendChild(new Paragraph());
            var propsRun = propsPara.AppendChild(new Run());
            propsRun.AppendChild(
                Utils.GetGenericPropertyTable(document, "Name", "Value", tokenBase.TokenProperties));
        }
    }
}
