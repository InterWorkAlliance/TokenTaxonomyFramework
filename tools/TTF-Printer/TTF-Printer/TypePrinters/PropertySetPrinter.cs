using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using System.Reflection;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;
using static DocumentFormat.OpenXml.Wordprocessing.TableWidthUnitValues;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class PropertySetPrinter
    {
        private static readonly ILog _log;
        static PropertySetPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void AddPropertySetProperties(WordprocessingDocument document, PropertySet ps, bool book, bool isForAppendix = false)
        {
            ArtifactPrinter.AddArtifactContent(document, ps.Artifact, book,isForAppendix);
            _log.Info("Printing Property Set Properties: " + ps.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Property Set"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);

            var baseProps = new[,]
            {
                {"Representation Type:", ps.RepresentationType.ToString()}
            };

            var pDef = body.AppendChild(new Paragraph());
            var pRun = pDef.AppendChild(new Run());
            pRun.AppendChild(new Text("For each token instance this property is represented as:"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef, JustificationValues.Center);
            Utils.AddTable(document, baseProps);
            
            CommonPrinter.BuildPropertiesTable(document, ps.Properties, book);
            
            if(!book) return;
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            pbr.AppendChild(new Text(""));

            if (pageBreak.ParagraphProperties == null)
            {
                pageBreak.ParagraphProperties = new ParagraphProperties();
            }

            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
        }
        
        public static void AddPropertySetSpecification(WordprocessingDocument document, PropertySetSpecification ps)
        {

            _log.Info("Printing Property Set Specification Properties: " + ps.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Specification Property Set"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);
            
            ArtifactPrinter.AddArtifactContent(document, ps.Artifact, false, true);
            
            var claPara = body.AppendChild(new Paragraph());
            var claRun = claPara.AppendChild(new Run());
            claRun.AppendChild(PresentRepresentationType(document, ps.RepresentationType));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", claPara);
            
            if(ps.Properties.Count > 0)
                CommonPrinter.BuildPropertySpecificationTable(document, ps.Properties);
            
        }
        
        private static Table PresentRepresentationType(WordprocessingDocument document, RepresentationType representationType)
        {
            var classificationDetails = ModelMap.GetPropertySetRepresentationType(representationType);
            var classificationTable = new Table();
           
            //representationType
            var repTypeRow = new TableRow();
            var repTypeHeader = new TableCell();
            var repTypeHeader2 = new TableCell();

            var newRow1 = new TableRow();
            var repTypeValue = new TableCell();
            var repTypeExplained = new TableCell();

            repTypeHeader.Append(new Paragraph(new Run(new Text("Property Set Representation Type"))));
            repTypeHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "35"}));
            repTypeRow.Append(repTypeHeader);
            
            repTypeHeader2.Append(new Paragraph(new Run(new Text("Description"))));
            repTypeHeader2.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));
            repTypeRow.Append(repTypeHeader2);
            
            classificationTable.Append(repTypeRow);
            
            repTypeValue.Append(new Paragraph(new Run(new Text(representationType.ToString()))));
            repTypeValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "35"}));

            repTypeExplained.Append(new Paragraph(new Run(
                new Text(classificationDetails[representationType.ToString()].ToString()))));

            repTypeExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            newRow1.Append(repTypeValue);
            newRow1.Append(repTypeExplained);
            classificationTable.Append(newRow1);

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", classificationTable);
            return classificationTable;
        }
    }
}
