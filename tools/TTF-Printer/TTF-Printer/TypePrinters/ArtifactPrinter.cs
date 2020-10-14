using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Linq;
using IWA.TTF.Taxonomy.Model.Artifact;
using log4net;
using System.Reflection;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Core;
using static DocumentFormat.OpenXml.Wordprocessing.TableWidthUnitValues;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    internal static class ArtifactPrinter
    {
        private static readonly ILog _log;

        static ArtifactPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        #region generic artifact

        public static void AddArtifactContent(WordprocessingDocument document, Artifact artifact,
            bool isForBook, bool isForAppendix, bool isForSpec = false)
        {
            _log.Info("Printing Artifact: " + artifact.Name + " is top artifact " + isForAppendix);

            var body = document.MainDocumentPart.Document.Body;
            
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            if (!isForSpec)
            {
                if (!isForAppendix)
                {
                    run.AppendChild(new Text(artifact.Name) {Space = SpaceProcessingModeValues.Preserve});
                    Utils.ApplyStyleToParagraph(document, "Title", "Title", para, JustificationValues.Center);
                }
                else
                {
                    run.AppendChild(new Text(artifact.Name) {Space = SpaceProcessingModeValues.Preserve});
                    Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", para, JustificationValues.Center);
                }
            }

            GenerateArtifactSymbol(document, artifact.ArtifactSymbol);
            if (artifact.Contributors != null && artifact.Contributors.Count > 0)
            {
                var contPara = body.AppendChild(new Paragraph());
                var contRun = contPara.AppendChild(new Run());
                contRun.AppendChild(new Text("Contributors"));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", contPara);

                var conPara = body.AppendChild(new Paragraph());
                var conRun = conPara.AppendChild(new Run());
                conRun.AppendChild(AddContributorsTable(document, artifact.Contributors));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", conPara);
            }

            if (!isForSpec)
            {
                var aDef = body.AppendChild(new Paragraph());
                var adRun = aDef.AppendChild(new Run());
                adRun.AppendChild(new Text("Definition"));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);

                var bizBody = body.AppendChild(new Paragraph());
                var bRun = bizBody.AppendChild(new Run());
                bRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessDescription));
                Utils.ApplyStyleToParagraph(document, "Quote", "Quote", bizBody);

                var eDef = body.AppendChild(new Paragraph());
                var eRun = eDef.AppendChild(new Run());
                eRun.AppendChild(new Text("Example"));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", eDef);

                var exBody = body.AppendChild(new Paragraph());
                var exRun = exBody.AppendChild(new Run());
                exRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessExample));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", exBody);

                var aPara = body.AppendChild(new Paragraph());
                var aRun = aPara.AppendChild(new Run());
                aRun.AppendChild(new Text("Analogies"));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aPara);

                var arPara = body.AppendChild(new Paragraph());
                var arRun = arPara.AppendChild(new Run());
                arRun.AppendChild(BuildAnalogies(document, artifact.ArtifactDefinition.Analogies));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", arPara);

                if (!string.IsNullOrEmpty(artifact.ArtifactDefinition.Comments))
                {
                    var coPara = body.AppendChild(new Paragraph());
                    var coRun = coPara.AppendChild(new Run());
                    coRun.AppendChild(new Text("Comments"));
                    Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", coPara);

                    var cmBody = body.AppendChild(new Paragraph());
                    var cmRun = cmBody.AppendChild(new Run());
                    cmRun.AppendChild(new Text(artifact.ArtifactDefinition.Comments));
                    Utils.ApplyStyleToParagraph(document, "Normal", "Normal", cmBody);
                }
            }

            var dPara = body.AppendChild(new Paragraph());
            var dRun = dPara.AppendChild(new Run());
            dRun.AppendChild(new Text("Dependencies"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", dPara);

            var ddPara = body.AppendChild(new Paragraph());
            var ddRun = ddPara.AppendChild(new Run());
            ddRun.AppendChild(BuildDependencyTable(document, artifact.Dependencies));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ddPara);

            var iPara = body.AppendChild(new Paragraph());
            var iRun = iPara.AppendChild(new Run());
            iRun.AppendChild(new Text("Incompatible With"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", iPara);

            var iiPara = body.AppendChild(new Paragraph());
            var iiRun = iiPara.AppendChild(new Run());
            iiRun.AppendChild(BuildIncompatibleTable(document, artifact.IncompatibleWithSymbols));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iiPara);

            var fPara = body.AppendChild(new Paragraph());
            var fRun = fPara.AppendChild(new Run());
            fRun.AppendChild(new Text("Influenced By"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", fPara);

            var ffPara = body.AppendChild(new Paragraph());
            var ffRun = ffPara.AppendChild(new Run());
            ffRun.AppendChild(BuildInfluencesTable(document, artifact.InfluencedBySymbols));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ffPara);

            var fiPara = body.AppendChild(new Paragraph());
            var fiRun = fiPara.AppendChild(new Run());
            fiRun.AppendChild(new Text("Artifact Files"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", fiPara);

            var ffiPara = body.AppendChild(new Paragraph());
            var ffiRun = ffiPara.AppendChild(new Run());
            ffiRun.AppendChild(BuildFilesTable(document, artifact.ArtifactFiles));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ffiPara);

            var cPara = body.AppendChild(new Paragraph());
            var cRun = cPara.AppendChild(new Run());
            cRun.AppendChild(new Text("Code Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", cPara);

            var ccPara =
                body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId {Val = "Heading2"})));
            var ccRun = ccPara.AppendChild(new Run());
            ccRun.AppendChild(BuildCodeTable(document, artifact.Maps.CodeReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ccPara);

            var pPara = body.AppendChild(new Paragraph());
            var pRun = pPara.AppendChild(new Run());
            pRun.AppendChild(new Text("Implementation Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", pPara);

            var ppPara = body.AppendChild(new Paragraph());
            var ppRun = ppPara.AppendChild(new Run());
            ppRun.AppendChild(BuildImplementationTable(document, artifact.Maps.ImplementationReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ppPara);

            var rPara = body.AppendChild(new Paragraph());
            var rRun = rPara.AppendChild(new Run());
            rRun.AppendChild(new Text("Resource Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", rPara);

            var rrPara = body.AppendChild(new Paragraph());
            var rrRun = rrPara.AppendChild(new Run());
            rrRun.AppendChild(BuildReferenceTable(document, artifact.Maps.Resources));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rrPara);

            if (isForAppendix)
                PrintController.Save();
        }

        #endregion

        #region specification artifact

        public static void AddArtifactSpecification(WordprocessingDocument document, Artifact artifact,
            Classification classification = null, bool isTopArtifact = false)
        {
            _log.Info("Printing Artifact for Specification: " + artifact.Name + " is top artifact " + isTopArtifact);

            var body = document.MainDocumentPart.Document.Body;
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(artifact.Name) {Space = SpaceProcessingModeValues.Preserve});
            if (isTopArtifact && artifact.ArtifactSymbol.Type != ArtifactType.TemplateFormula)
                Utils.ApplyStyleToParagraph(document, "Title", "Title", para, JustificationValues.Center);
            else
                Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", para, JustificationValues.Center);

            
            if (artifact.Contributors != null && artifact.Contributors.Count > 0)
            {
                var contPara = body.AppendChild(new Paragraph());
                var contRun = contPara.AppendChild(new Run());
                contRun.AppendChild(new Text("Contributors"));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", contPara);

                var conPara = body.AppendChild(new Paragraph());
                var conRun = conPara.AppendChild(new Run());
                conRun.AppendChild(AddContributorsTable(document, artifact.Contributors));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", conPara);
            }
            
            var fDef = body.AppendChild(new Paragraph());
            var ff2Run = fDef.AppendChild(new Run());
            ff2Run.AppendChild(new Text("Taxonomy Formula: " + artifact.ArtifactSymbol.Tooling));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", fDef, JustificationValues.Center);

            if (isTopArtifact && classification != null)
            {
                var asDef = body.AppendChild(new Paragraph());
                var adsRun = asDef.AppendChild(new Run());
                adsRun.AppendChild(new Text("Token Specification Summary"));
                Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", asDef, JustificationValues.Center);

                var aDef = body.AppendChild(new Paragraph());
                var adRun = aDef.AppendChild(new Run());
                adRun.AppendChild(new Text("Token Classification"));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);

                var claPara = body.AppendChild(new Paragraph());
                var claRun = claPara.AppendChild(new Run());
                claRun.AppendChild(PresentClassification(document, classification));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", claPara);
            }

            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessDescription));
            Utils.ApplyStyleToParagraph(document, "Quote", "Quote", bizBody);

            var eDef = body.AppendChild(new Paragraph());
            var eRun = eDef.AppendChild(new Run());
            eRun.AppendChild(new Text("Example"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", eDef);

            var exBody = body.AppendChild(new Paragraph());
            var exRun = exBody.AppendChild(new Run());
            exRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessExample));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", exBody);

            if (artifact.ArtifactDefinition.Analogies.Count > 0)
            {
                var aPara = body.AppendChild(new Paragraph());
                var aRun = aPara.AppendChild(new Run());
                aRun.AppendChild(new Text("Analogies"));
                Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", aPara);

                var arPara = body.AppendChild(new Paragraph());
                var arRun = arPara.AppendChild(new Run());
                arRun.AppendChild(BuildAnalogies(document, artifact.ArtifactDefinition.Analogies));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", arPara);
            }

            if (!string.IsNullOrEmpty(artifact.ArtifactDefinition.Comments))
            {
                var coPara = body.AppendChild(new Paragraph());
                var coRun = coPara.AppendChild(new Run());
                coRun.AppendChild(new Text("Comments"));
                Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", coPara);

                var cmBody = body.AppendChild(new Paragraph());
                var cmRun = cmBody.AppendChild(new Run());
                cmRun.AppendChild(new Text(artifact.ArtifactDefinition.Comments));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", cmBody);
            }

            if (isTopArtifact)
                PrintController.Save();
        }
        
        public static void AddBehaviorArtifactSpecification(WordprocessingDocument document, Artifact artifact,
            Classification classification = null)
        {
            _log.Info("Printing Artifact for Specification: " + artifact.Name);

            var body = document.MainDocumentPart.Document.Body;
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(artifact.Name) {Space = SpaceProcessingModeValues.Preserve});
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", para, JustificationValues.Center);

            var fDef = body.AppendChild(new Paragraph());
            var ff2Run = fDef.AppendChild(new Run());
            ff2Run.AppendChild(new Text("Taxonomy Symbol: " + artifact.ArtifactSymbol.Tooling));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", fDef);
            
            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessDescription));
            Utils.ApplyStyleToParagraph(document, "Quote", "Quote", bizBody);

            var eDef = body.AppendChild(new Paragraph());
            var eRun = eDef.AppendChild(new Run());
            eRun.AppendChild(new Text("Example"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", eDef);

            var exBody = body.AppendChild(new Paragraph());
            var exRun = exBody.AppendChild(new Run());
            exRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessExample));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", exBody);

            if (artifact.ArtifactDefinition.Analogies.Count > 0)
            {
                var aPara = body.AppendChild(new Paragraph());
                var aRun = aPara.AppendChild(new Run());
                aRun.AppendChild(new Text("Analogies"));
                Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", aPara);

                var arPara = body.AppendChild(new Paragraph());
                var arRun = arPara.AppendChild(new Run());
                arRun.AppendChild(BuildAnalogies(document, artifact.ArtifactDefinition.Analogies));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", arPara);
            }

            if (string.IsNullOrEmpty(artifact.ArtifactDefinition.Comments)) return;
            var coPara = body.AppendChild(new Paragraph());
            var coRun = coPara.AppendChild(new Run());
            coRun.AppendChild(new Text("Comments"));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", coPara);

            var cmBody = body.AppendChild(new Paragraph());
            var cmRun = cmBody.AppendChild(new Run());
            cmRun.AppendChild(new Text(artifact.ArtifactDefinition.Comments));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", cmBody);
        }

        
        #endregion

        #region child specification artifact

        public static void AddChildArtifactSpecification(WordprocessingDocument document, TokenSpecification spec)
        {
            _log.Info("Printing Artifact for Child Specification: " + spec.Artifact.Name);

            var body = document.MainDocumentPart.Document.Body;
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(spec.Artifact.Name) {Space = SpaceProcessingModeValues.Preserve});
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", para, JustificationValues.Center);

            var fDef = body.AppendChild(new Paragraph());
            var ff2Run = fDef.AppendChild(new Run());
            ff2Run.AppendChild(new Text("Taxonomy Formula: " + spec.Artifact.ArtifactSymbol.Tooling));
            Utils.ApplyStyleToParagraph(document, "Heading3", "Heading3", fDef);

            var classification = ModelMap.GetClassification(spec);

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Token Classification"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);

            var claPara = body.AppendChild(new Paragraph());
            var claRun = claPara.AppendChild(new Run());
            claRun.AppendChild(PresentClassification(document, classification));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", claPara);

            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text(spec.Artifact.ArtifactDefinition.BusinessDescription));
            Utils.ApplyStyleToParagraph(document, "Quote", "Quote", bizBody);
        }

        #endregion

        private static Table PropertySetRepresentationType(WordprocessingDocument document, Classification classification)
        {
            var classificationDetails = ModelMap.GetClassificationDescription(classification);
            var classificationTable = new Table();
            //templateType
            var templateTypeRow = new TableRow();
            var templateTypeHeader = new TableCell();
            var templateTypeValue = new TableCell();
            var templateTypeExplained = new TableCell();

            templateTypeHeader.Append(new Paragraph(new Run(new Text("Template Type:"))));
            var width20 = new TableCellWidth {Type = Pct, Width = "20"};
            templateTypeHeader.Append(new TableCellProperties(width20));

            templateTypeValue.Append(new Paragraph(new Run(new Text(classification.TemplateType.ToString()))));
            var width15 = new TableCellWidth {Type = Pct, Width = "15"};
            templateTypeValue.Append(new TableCellProperties(width15));

            var width65 = new TableCellWidth {Type = Pct, Width = "65"};

            templateTypeExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.TemplateType.ToString()].ToString()))));
            templateTypeExplained.Append(new TableCellProperties(width65));

            templateTypeRow.Append(templateTypeHeader);
            templateTypeRow.Append(templateTypeValue);
            templateTypeRow.Append(templateTypeExplained);
            classificationTable.Append(templateTypeRow);

            //tokenType
            var tokenTypeRow = new TableRow();
            var tokenTypeHeader = new TableCell();
            var tokenTypeValue = new TableCell();
            var tokenTypeExplained = new TableCell();

            tokenTypeHeader.Append(new Paragraph(new Run(new Text("Token Type:"))));
            tokenTypeHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            tokenTypeValue.Append(new Paragraph(new Run(new Text(classification.TokenType.ToString()))));
            tokenTypeValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            tokenTypeExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.TokenType.ToString()].ToString()))));

            tokenTypeExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            tokenTypeRow.Append(tokenTypeHeader);
            tokenTypeRow.Append(tokenTypeValue);
            tokenTypeRow.Append(tokenTypeExplained);
            classificationTable.Append(tokenTypeRow);

            //tokenUnit
            var tokenUnitRow = new TableRow();
            var tokenUnitHeader = new TableCell();
            var tokenUnitValue = new TableCell();
            var tokenUnitExplained = new TableCell();

            tokenUnitHeader.Append(new Paragraph(new Run(new Text("Token Unit:"))));
            tokenUnitHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            tokenUnitValue.Append(new Paragraph(new Run(new Text(classification.TokenUnit.ToString()))));
            tokenUnitValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            tokenUnitExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.TokenUnit.ToString()].ToString()))));

            tokenUnitExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            tokenUnitRow.Append(tokenUnitHeader);
            tokenUnitRow.Append(tokenUnitValue);
            tokenUnitRow.Append(tokenUnitExplained);
            classificationTable.Append(tokenUnitRow);

            //valueType
            var valueTypeRow = new TableRow();
            var valueTypeHeader = new TableCell();
            var valueTypeValue = new TableCell();
            var valueTypeExplained = new TableCell();

            valueTypeHeader.Append(new Paragraph(new Run(new Text("Value Type:"))));
            valueTypeHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            valueTypeValue.Append(new Paragraph(new Run(new Text(classification.ValueType.ToString()))));
            valueTypeValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            valueTypeExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.ValueType.ToString()].ToString()))));

            valueTypeExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            valueTypeRow.Append(valueTypeHeader);
            valueTypeRow.Append(valueTypeValue);
            valueTypeRow.Append(valueTypeExplained);
            classificationTable.Append(valueTypeRow);

            //representationType
            var repTypeRow = new TableRow();
            var repTypeHeader = new TableCell();
            var repTypeValue = new TableCell();
            var repTypeExplained = new TableCell();

            repTypeHeader.Append(new Paragraph(new Run(new Text("Representation Type:"))));
            repTypeHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            repTypeValue.Append(new Paragraph(new Run(new Text(classification.RepresentationType.ToString()))));
            repTypeValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            repTypeExplained.Append(new Paragraph(new Run(
                new Text(classificationDetails[classification.RepresentationType.ToString()].ToString()))));

            repTypeExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            repTypeRow.Append(repTypeHeader);
            repTypeRow.Append(repTypeValue);
            repTypeRow.Append(repTypeExplained);
            classificationTable.Append(repTypeRow);
            
            //supply Row
            var repSupplyRow = new TableRow();
            var repSupplyHeader = new TableCell();
            var repSupplyValue = new TableCell();
            var repSupplyExplained = new TableCell();

            repSupplyHeader.Append(new Paragraph(new Run(new Text("Supply:"))));
            repSupplyHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            repSupplyValue.Append(new Paragraph(new Run(new Text(classification.Supply.ToString()))));
            repSupplyValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            repSupplyExplained.Append(new Paragraph(new Run(
                new Text(classificationDetails[classification.Supply.ToString()].ToString()))));

            repSupplyExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            repSupplyRow.Append(repSupplyHeader);
            repSupplyRow.Append(repSupplyValue);
            repSupplyRow.Append(repSupplyExplained);
            classificationTable.Append(repSupplyRow);

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", classificationTable);
            return classificationTable;
        }
        
        private static Table PresentClassification(WordprocessingDocument document, Classification classification)
        {
            var classificationDetails = ModelMap.GetClassificationDescription(classification);
            var classificationTable = new Table();
            //templateType
            var templateTypeRow = new TableRow();
            var templateTypeHeader = new TableCell();
            var templateTypeValue = new TableCell();
            var templateTypeExplained = new TableCell();

            templateTypeHeader.Append(new Paragraph(new Run(new Text("Template Type:"))));
            var width20 = new TableCellWidth {Type = Pct, Width = "20"};
            templateTypeHeader.Append(new TableCellProperties(width20));

            templateTypeValue.Append(new Paragraph(new Run(new Text(classification.TemplateType.ToString()))));
            var width15 = new TableCellWidth {Type = Pct, Width = "15"};
            templateTypeValue.Append(new TableCellProperties(width15));

            var width65 = new TableCellWidth {Type = Pct, Width = "65"};

            templateTypeExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.TemplateType.ToString()].ToString()))));
            templateTypeExplained.Append(new TableCellProperties(width65));

            templateTypeRow.Append(templateTypeHeader);
            templateTypeRow.Append(templateTypeValue);
            templateTypeRow.Append(templateTypeExplained);
            classificationTable.Append(templateTypeRow);

            //tokenType
            var tokenTypeRow = new TableRow();
            var tokenTypeHeader = new TableCell();
            var tokenTypeValue = new TableCell();
            var tokenTypeExplained = new TableCell();

            tokenTypeHeader.Append(new Paragraph(new Run(new Text("Token Type:"))));
            tokenTypeHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            tokenTypeValue.Append(new Paragraph(new Run(new Text(classification.TokenType.ToString()))));
            tokenTypeValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            tokenTypeExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.TokenType.ToString()].ToString()))));

            tokenTypeExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            tokenTypeRow.Append(tokenTypeHeader);
            tokenTypeRow.Append(tokenTypeValue);
            tokenTypeRow.Append(tokenTypeExplained);
            classificationTable.Append(tokenTypeRow);

            //tokenUnit
            var tokenUnitRow = new TableRow();
            var tokenUnitHeader = new TableCell();
            var tokenUnitValue = new TableCell();
            var tokenUnitExplained = new TableCell();

            tokenUnitHeader.Append(new Paragraph(new Run(new Text("Token Unit:"))));
            tokenUnitHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            tokenUnitValue.Append(new Paragraph(new Run(new Text(classification.TokenUnit.ToString()))));
            tokenUnitValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            tokenUnitExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.TokenUnit.ToString()].ToString()))));

            tokenUnitExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            tokenUnitRow.Append(tokenUnitHeader);
            tokenUnitRow.Append(tokenUnitValue);
            tokenUnitRow.Append(tokenUnitExplained);
            classificationTable.Append(tokenUnitRow);

            //valueType
            var valueTypeRow = new TableRow();
            var valueTypeHeader = new TableCell();
            var valueTypeValue = new TableCell();
            var valueTypeExplained = new TableCell();

            valueTypeHeader.Append(new Paragraph(new Run(new Text("Value Type:"))));
            valueTypeHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            valueTypeValue.Append(new Paragraph(new Run(new Text(classification.ValueType.ToString()))));
            valueTypeValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            valueTypeExplained.Append(new Paragraph(
                new Run(new Text(classificationDetails[classification.ValueType.ToString()].ToString()))));

            valueTypeExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            valueTypeRow.Append(valueTypeHeader);
            valueTypeRow.Append(valueTypeValue);
            valueTypeRow.Append(valueTypeExplained);
            classificationTable.Append(valueTypeRow);

            //representationType
            var repTypeRow = new TableRow();
            var repTypeHeader = new TableCell();
            var repTypeValue = new TableCell();
            var repTypeExplained = new TableCell();

            repTypeHeader.Append(new Paragraph(new Run(new Text("Representation Type:"))));
            repTypeHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            repTypeValue.Append(new Paragraph(new Run(new Text(classification.RepresentationType.ToString()))));
            repTypeValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            repTypeExplained.Append(new Paragraph(new Run(
                new Text(classificationDetails[classification.RepresentationType.ToString()].ToString()))));

            repTypeExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            repTypeRow.Append(repTypeHeader);
            repTypeRow.Append(repTypeValue);
            repTypeRow.Append(repTypeExplained);
            classificationTable.Append(repTypeRow);
            
            //supply Row
            var repSupplyRow = new TableRow();
            var repSupplyHeader = new TableCell();
            var repSupplyValue = new TableCell();
            var repSupplyExplained = new TableCell();

            repSupplyHeader.Append(new Paragraph(new Run(new Text("Supply:"))));
            repSupplyHeader.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "20"}));

            repSupplyValue.Append(new Paragraph(new Run(new Text(classification.Supply.ToString()))));
            repSupplyValue.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "15"}));

            repSupplyExplained.Append(new Paragraph(new Run(
                new Text(classificationDetails[classification.Supply.ToString()].ToString()))));

            repSupplyExplained.Append(new TableCellProperties(new TableCellWidth {Type = Pct, Width = "65"}));

            repSupplyRow.Append(repSupplyHeader);
            repSupplyRow.Append(repSupplyValue);
            repSupplyRow.Append(repSupplyExplained);
            classificationTable.Append(repSupplyRow);

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", classificationTable);
            return classificationTable;
        }
        

        public static void AddArtifactReference(WordprocessingDocument document, ArtifactReference artifactReference)
        {
            _log.Info("Printing Artifact Reference: " + artifactReference.Id);

            var body = document.MainDocumentPart.Document.Body;

            var name = Utils.GetNameForId(artifactReference.Id, artifactReference.Type);

            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text("Name: " + name) {Space = SpaceProcessingModeValues.Preserve});

            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", para);

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Id: " + artifactReference.Id));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", aDef);

            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text("Reference Notes: " + artifactReference.ReferenceNotes));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", bizBody);

            if (artifactReference.Values == null) return;

            var eDef = body.AppendChild(new Paragraph());
            var eRun = eDef.AppendChild(new Run());
            eRun.AppendChild(new Text("Reference Values"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", eDef);

            var fiPara = body.AppendChild(new Paragraph());
            var fiRun = fiPara.AppendChild(new Run());
            fiRun.AppendChild(new Text("Artifact Files"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", fiPara);

            var ffiPara = body.AppendChild(new Paragraph());
            var ffiRun = ffiPara.AppendChild(new Run());
            ffiRun.AppendChild(BuildFilesTable(document, artifactReference.Values.ArtifactFiles));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ffiPara);

            var cPara = body.AppendChild(new Paragraph());
            var cRun = cPara.AppendChild(new Run());
            cRun.AppendChild(new Text("Code Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", cPara);

            var ccPara =
                body.AppendChild(
                    new Paragraph(new ParagraphProperties(new ParagraphStyleId {Val = "Heading2"})));
            var ccRun = ccPara.AppendChild(new Run());
            ccRun.AppendChild(BuildCodeTable(document, artifactReference.Values.Maps.CodeReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ccPara);

            var pPara = body.AppendChild(new Paragraph());
            var pRun = pPara.AppendChild(new Run());
            pRun.AppendChild(new Text("Implementation Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", pPara);

            var ppPara = body.AppendChild(new Paragraph());
            var ppRun = ppPara.AppendChild(new Run());
            ppRun.AppendChild(BuildImplementationTable(document,
                artifactReference.Values.Maps.ImplementationReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ppPara);

            var rPara = body.AppendChild(new Paragraph());
            var rRun = rPara.AppendChild(new Run());
            rRun.AppendChild(new Text("Resource Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", rPara);

            var rrPara = body.AppendChild(new Paragraph());
            var rrRun = rrPara.AppendChild(new Run());
            rrRun.AppendChild(BuildReferenceTable(document, artifactReference.Values.Maps.Resources));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rrPara);
        }

        public static void GenerateArtifactSymbol(WordprocessingDocument document, ArtifactSymbol symbol)
        {
            var name = Utils.GetNameForId(symbol.Id, symbol.Type);
            var basicProps = new[,]
            {
                {"Type:", symbol.Type.ToString()},
                {"Name:", name},
                {"Id:", symbol.Id},
                {"Visual:", symbol.Visual},
                {"Tooling:", symbol.Tooling},
                {"Version:", symbol.Version}
            };
            Utils.AddTable(document, basicProps);
        }


        private static Table BuildReferenceTable(WordprocessingDocument document,
            IEnumerable<MapResourceReference> resources)
        {
            var referenceMapTable = new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var name1 = new TableCell();
            var platform1 = new TableCell();
            var link1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Map Type"))));
            var width = new TableCellWidth();
            width.Type = Pct;
            width.Width = "15";
            contentType1.Append(new TableCellProperties(
                width));
            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "15"}));
            platform1.Append(new Paragraph(new Run(new Text("Location"))));
            platform1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "20"}));
            link1.Append(new Paragraph(new Run(new Text("Description"))));
            var element = new TableCellWidth();
            element.Type = Pct;
            element.Width = "50";
            link1.Append(new TableCellProperties(
                element));
            tr1.Append(contentType1);
            tr1.Append(name1);
            tr1.Append(platform1);
            tr1.Append(link1);
            referenceMapTable.Append(tr1);

            foreach (var im in resources)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var name = new TableCell();
                var link = new TableCell();
                var description = new TableCell();

                contentType.Append(new Paragraph(new Run(new Text(im.MappingType.ToString()))));
                name.Append(new Paragraph(new Run(new Text(im.Name))));
                link.Append(new Paragraph(new Run(new Text(im.ResourcePath))));
                description.Append(new Paragraph(new Run(new Text(im.Description))));
                tr.Append(contentType);
                tr.Append(name);
                tr.Append(link);
                tr.Append(description);
                referenceMapTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", referenceMapTable);
            return referenceMapTable;
        }

        private static Table AddContributorsTable(WordprocessingDocument document,
            IEnumerable<Contributor> contributors)
        {
            var contributorsTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var org1 = new TableCell();
            name1.Append(new Paragraph(new Run(new Text("Name"))));
            var width = new TableCellWidth {Width = "15", Type = Pct};
            name1.Append(new TableCellProperties(
                width));
            org1.Append(new Paragraph(new Run(new Text("Organization"))));
            var element = new TableCellWidth {Type = Pct, Width = "15"};
            org1.Append(new TableCellProperties(
                element));
           
            tr1.Append(name1);
            tr1.Append(org1);
            contributorsTable.Append(tr1);

            foreach (var im in contributors)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var org = new TableCell();
              
                name.Append(new Paragraph(new Run(new Text(im.Name))));
                org.Append(new Paragraph(new Run(new Text(im.Organization))));
                tr.Append(name);
                tr.Append(org);
                contributorsTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", contributorsTable);
            return contributorsTable;
        }
                
        private static Table BuildImplementationTable(WordprocessingDocument document,
            IEnumerable<MapReference> references)
        {
            var implementationMapTable = new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var name1 = new TableCell();
            var platform1 = new TableCell();
            var link1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Map Type"))));
            var width = new TableCellWidth {Width = "15", Type = Pct};
            contentType1.Append(new TableCellProperties(
                width));
            name1.Append(new Paragraph(new Run(new Text("Name"))));
            var element = new TableCellWidth {Type = Pct, Width = "15"};
            name1.Append(new TableCellProperties(
                element));
            platform1.Append(new Paragraph(new Run(new Text("Platform"))));
            var elements = new TableCellWidth {Type = Pct, Width = "15"};
            platform1.Append(new TableCellProperties(
                elements));
            link1.Append(new Paragraph(new Run(new Text("Location"))));
            var cellWidth = new TableCellWidth {Type = Pct, Width = "55"};
            link1.Append(new TableCellProperties(
                cellWidth));
            tr1.Append(contentType1);
            tr1.Append(name1);
            tr1.Append(platform1);
            tr1.Append(link1);
            implementationMapTable.Append(tr1);

            foreach (var im in references)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var name = new TableCell();
                var platform = new TableCell();
                var link = new TableCell();

                contentType.Append(new Paragraph(new Run(new Text(im.MappingType.ToString()))));
                name.Append(new Paragraph(new Run(new Text(im.Name))));
                platform.Append(new Paragraph(new Run(new Text(im.Platform.ToString()))));
                link.Append(new Paragraph(new Run(new Text(im.ReferencePath))));
                tr.Append(contentType);
                tr.Append(name);
                tr.Append(platform);
                tr.Append(link);
                implementationMapTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", implementationMapTable);
            return implementationMapTable;
        }

        private static Table BuildCodeTable(WordprocessingDocument document, IEnumerable<MapReference> references)
        {
            var codeMapTable = new Table();

            var trH = new TableRow();
            var contentTypeH = new TableCell();
            var nameH = new TableCell();
            var platformH = new TableCell();
            var linkH = new TableCell();

            contentTypeH.Append(new Paragraph(new Run(new Text("Map Type"))));
            var element = new TableCellWidth {Type = Pct, Width = "15"};
            contentTypeH.Append(new TableCellProperties(
                element));
            nameH.Append(new Paragraph(new Run(new Text("Name"))));
            var width = new TableCellWidth {Type = Pct, Width = "15"};
            nameH.Append(new TableCellProperties(
                width));
            platformH.Append(new Paragraph(new Run(new Text("Platform"))));
            var elements = new TableCellWidth {Type = Pct, Width = "15"};
            platformH.Append(new TableCellProperties(
                elements));
            linkH.Append(new Paragraph(new Run(new Text("Location"))));
            var cellWidth = new TableCellWidth {Type = Pct, Width = "55"};
            linkH.Append(new TableCellProperties(
                cellWidth));
            trH.Append(contentTypeH);
            trH.Append(nameH);
            trH.Append(platformH);
            trH.Append(linkH);
            codeMapTable.Append(trH);
            foreach (var c in references)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var name = new TableCell();
                var platform = new TableCell();
                var link = new TableCell();

                contentType.Append(new Paragraph(new Run(new Text(c.MappingType.ToString()))));
                name.Append(new Paragraph(new Run(new Text(c.Name))));
                platform.Append(new Paragraph(new Run(new Text(c.Platform.ToString()))));
                link.Append(new Paragraph(new Run(new Text(c.ReferencePath))));
                tr.Append(contentType);
                tr.Append(name);
                tr.Append(platform);
                tr.Append(link);
                codeMapTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", codeMapTable);
            return codeMapTable;
        }

        private static Table BuildFilesTable(WordprocessingDocument document, IEnumerable<ArtifactFile> files)
        {
            var filesTable = new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var fileName1 = new TableCell();
            var content1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Content Type"))));
            contentType1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "10"}));
            fileName1.Append(new Paragraph(new Run(new Text("File Name"))));
            fileName1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "25"}));
            content1.Append(new Paragraph(new Run(new Text("File Content"))));
            content1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "65"}));
            tr1.Append(contentType1);
            tr1.Append(fileName1);
            tr1.Append(content1);
            filesTable.Append(tr1);
            foreach (var f in files)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var fileName = new TableCell();
                var content = new TableCell();

                var data = f.FileData == null ? "pending" : f.FileData.ToStringUtf8();
                contentType.Append(new Paragraph(new Run(new Text(f.Content.ToString()))));
                fileName.Append(new Paragraph(new Run(new Text(f.FileName))));
                content.Append(new Paragraph(new Run(new Text(data))));
                tr.Append(contentType);
                tr.Append(fileName);
                tr.Append(content);
                filesTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", filesTable);
            return filesTable;
        }

        private static Table BuildInfluencesTable(WordprocessingDocument document,
            IEnumerable<SymbolInfluence> influences)
        {
            var influenceTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();


            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "10"}));
            name1.Append(new Paragraph(new Run(new Text("Description"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "75"}));
            description1.Append(new Paragraph(new Run(new Text("Applies To"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "15"}));
            tr1.Append(name1);
            tr1.Append(symbol1);
            tr1.Append(description1);
            influenceTable.Append(tr1);
            foreach (var i in influences)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var symbol = new TableCell();
                var description = new TableCell();

                symbol.Append(new Paragraph(new Run(new Text(i.Symbol.Tooling))));
                name.Append(new Paragraph(new Run(new Text(i.Description))));
                description.Append(new Paragraph(new Run(new Text(i.AppliesTo.ToString()))));
                tr.Append(name);
                tr.Append(symbol);
                tr.Append(description);
                influenceTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", influenceTable);
            return influenceTable;
        }

        private static Table BuildIncompatibleTable(WordprocessingDocument document,
            IEnumerable<ArtifactSymbol> incompatibles)
        {
            var incompatibleTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Artifact Type"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "45"}));
            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "10"}));
            description1.Append(new Paragraph(new Run(new Text("Id"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "45"}));
            tr1.Append(name1);
            tr1.Append(symbol1);
            tr1.Append(description1);
            incompatibleTable.Append(tr1);
            foreach (var i in incompatibles)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var symbol = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(i.Type.ToString()))));
                symbol.Append(new Paragraph(new Run(new Text(i.Tooling))));
                description.Append(new Paragraph(new Run(new Text(i.Id))));
                tr.Append(name);
                tr.Append(symbol);
                tr.Append(description);
                incompatibleTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", incompatibleTable);
            return incompatibleTable;
        }

        private static Table BuildDependencyTable(WordprocessingDocument document,
            IEnumerable<SymbolDependency> dependencies)
        {
            //dependencies table should be the 4th table
            var dependencyTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Artifact Type"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "35"}));
            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "10"}));
            description1.Append(new Paragraph(new Run(new Text("Description"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "55"}));
            tr1.Append(name1);
            tr1.Append(symbol1);
            tr1.Append(description1);
            dependencyTable.Append(tr1);
            foreach (var d in dependencies)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var symbol = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(d.Symbol.Type.ToString()))));
                symbol.Append(new Paragraph(new Run(new Text(d.Symbol.Tooling))));
                description.Append(new Paragraph(new Run(new Text(d.Description))));
                tr.Append(name);
                tr.Append(symbol);
                tr.Append(description);
                dependencyTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", dependencyTable);
            return dependencyTable;
        }

        private static Table BuildAnalogies(WordprocessingDocument document, IEnumerable<ArtifactAnalogy> analogies)
        {
            var analogyTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "25"}));
            description1.Append(new Paragraph(new Run(new Text("Description"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth {Type = Pct, Width = "75"}));
            tr1.Append(name1);
            tr1.Append(description1);
            analogyTable.Append(tr1);
            foreach (var a in analogies)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(a.Name))));
                description.Append(new Paragraph(new Run(new Text(a.Description))));
                tr.Append(name);
                tr.Append(description);
                analogyTable.Append(tr);
            }

            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", analogyTable);
            return analogyTable;
        }
    }
}