using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Protobuf.Collections;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Org.BouncyCastle.Crypto.Digests;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;
using V = DocumentFormat.OpenXml.Vml;

namespace IWA.TTF.Taxonomy.TypePrinters
{
    public static class Os
    {
        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static bool IsMacOs()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static string WhatIs()
        {
            var os = (IsWindows() ? "win" : null) ??
                     (IsMacOs() ? "mac" : null) ??
                     (IsLinux() ? "gnu" : null);
            return os;
        }
    }
    public static class Utils
    {
        private static readonly ILog _log;

        static Utils()
        {
            #region logging

            InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }
        
        public static void InitLog()
        {
            var xmlDocument = new XmlDocument();
            try
            {
                if (Os.IsWindows())
                    xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                                   "\\log4net.config"));
                else
                    xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                                   "/log4net.config"));
            }
            catch (Exception)
            {
                if (Os.IsWindows())
                    xmlDocument.Load(File.OpenRead(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\log4net.config"));
                else
                    xmlDocument.Load(File.OpenRead(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log4net.config"));
            }

            XmlConfigurator.Configure(
                LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy)), xmlDocument["log4net"]);
        }
        
        internal static string GetNameForId(string id, ArtifactType artifactType)
        {
            string name;
            switch (artifactType)
            {
                case ArtifactType.Base:
                    name = ModelManager.GetArtifactById<Base>(id).Artifact.Name;
                    break;
                case ArtifactType.Behavior:
                    name = ModelManager.GetArtifactById<Model.Core.Behavior>(id).Artifact.Name;
                    break;
                case ArtifactType.BehaviorGroup:
                    name = ModelManager.GetArtifactById<BehaviorGroup>(id).Artifact.Name;
                    break;
                case ArtifactType.PropertySet:
                    name = ModelManager.GetArtifactById<PropertySet>(id).Artifact.Name;
                    break;
                case ArtifactType.TemplateFormula:
                    name = ModelManager.GetArtifactById<TemplateFormula>(id).Artifact.Name;
                    break;
                case ArtifactType.TemplateDefinition:
                    name = ModelManager.GetArtifactById<TemplateDefinition>(id).Artifact.Name;
                    break;
                case ArtifactType.TokenTemplate:
                    name = ModelManager.GetArtifactById<TemplateDefinition>(id).Artifact.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return name;
        }
        
        internal static Invocation GetInvocationById(string invokedId, string invocationId, ArtifactType artifactType)
        {
            switch (artifactType)
            {
                case ArtifactType.Base:
                   break;
                case ArtifactType.Behavior:
                    var b = ModelManager.GetArtifactById<Model.Core.Behavior>(invokedId);
                    var bi = b.Invocations.SingleOrDefault(e => e.Id == invocationId);
                    if(bi == null) return new Invocation
                    {
                        Id = invocationId
                    };
                    else
                    {
                        return bi;
                    }
                case ArtifactType.BehaviorGroup:
                   
                    break;
                case ArtifactType.PropertySet:
                    var p = ModelManager.GetArtifactById<PropertySet>(invokedId);
                    foreach (var pp in p.Properties)
                    {
                        var pi = pp.PropertyInvocations.SingleOrDefault(e => e.Id == invocationId);
                        if(pi == null) return new Invocation
                        {
                            Id = invocationId
                        };
                        return pi;
                    }
                    
                    break;
                case ArtifactType.TemplateFormula:
                    
                    break;
                case ArtifactType.TemplateDefinition:
                   
                    break;
                case ArtifactType.TokenTemplate:
                   
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Invocation{ Id = invocationId};
        }

        public static Table GetNewTable(int columns)
        {
            var table = new Table();

            var props = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 12
                    }));

            table.AppendChild(props);

            for (var j = 0; j <= columns; j++)
            {
                var tc = new TableCell();
                // Code removed here…
                table.Append(tc);
            }

            return table;
        }

        #region formatting


        // Take the data from a two-dimensional array and build a table at the 
        // end of the supplied document.
        public static void AddTable(WordprocessingDocument document, string[,] data, string styleName = "GridTable4-Accent1")
        {
            var table = new Table();
            GetFormattedTable(table);

            for (var i = 0; i <= data.GetUpperBound(0); i++)
            {
                var tr = new TableRow();
                for (var j = 0; j <= data.GetUpperBound(1); j++)
                {
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text(data[i, j]))));

                    if (IsLabel(data[i, j]))
                    {
                        var width = new TableCellWidth();
                        width.Type = TableWidthUnitValues.Pct;
                        width.Width = "30";
                        tc.Append(new TableCellProperties(
                            width));
                    }
                    else
                    {
                        var width = new TableCellWidth();
                        width.Type = TableWidthUnitValues.Pct;
                        width.Width = "70";
                        tc.Append(new TableCellProperties(
                            width));
                    }

                    tr.Append(tc);
                }

                table.Append(tr);
            }
            ApplyStyleTable(document, styleName, styleName, table);
            document.MainDocumentPart.Document.Body.Append(table);
        }

        public static Table GetTable(WordprocessingDocument document, string[,] data)
        {
            var table = new Table();
            GetFormattedTable(table);

            for (var i = 0; i <= data.GetUpperBound(0); i++)
            {
                var tr = new TableRow();
                for (var j = 0; j <= data.GetUpperBound(1); j++)
                {
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text(data[i, j]))));

                    if (IsLabel(data[i, j]))
                    {
                        var width = new TableCellWidth();
                        width.Type = TableWidthUnitValues.Pct;
                        width.Width = "30";
                        tc.Append(new TableCellProperties(
                            width));
                    }
                    else
                    {
                        var width = new TableCellWidth();
                        width.Type = TableWidthUnitValues.Pct;
                        width.Width = "70";
                        tc.Append(new TableCellProperties(
                            width));
                    }

                    tr.Append(tc);
                }

                table.Append(tr);
            }
            ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", table);
            return table;
        }

        // Take the data from a two-dimensional array and build a table at the 
        // end of the supplied document.
        public static Table GetParameterTable(WordprocessingDocument document, string col1Name, string col2Name, 
            IEnumerable<InvocationParameter> data)
        {
            var table = new Table();

            var tr = new TableRow();
            var tch = new TableCell();
            tch.Append(new Paragraph(new Run(new Text(col1Name))));
            var element = new TableCellWidth();
            element.Type = TableWidthUnitValues.Pct;
            element.Width = "30";
            tch.Append(new TableCellProperties(
                            element));
            var tch2 = new TableCell();
            tch2.Append(new Paragraph(new Run(new Text(col2Name))));
            var width = new TableCellWidth();
            width.Type = TableWidthUnitValues.Pct;
            width.Width = "70";
            tch2.Append(new TableCellProperties(
                            width));
            tr.Append(tch);
            tr.Append(tch2);
            table.Append(tr);
            foreach (var r in data)
            {
                var trr = new TableRow();
                var tc = new TableCell();
                tc.Append(new Paragraph(new Run(new Text(r.Name))));
                var tc2 = new TableCell();
                tc2.Append(new Paragraph(new Run(new Text(r.ValueDescription))));

                trr.Append(tc);
                trr.Append(tc2);
                table.Append(trr);
            }
            ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", table);
            return table;
        }

        // Take the data from a two-dimensional array and build a table at the 
        // end of the supplied document.
        public static Table GetGenericPropertyTable(WordprocessingDocument document, string col1Name, string col2Name,
            MapField<string, string> data, string styleName = "GridTable4-Accent1")
        {
            var table = new Table();

            var tr = new TableRow();
            var tch = new TableCell();
            tch.Append(new Paragraph(new Run(new Text(col1Name))));
            var width = new TableCellWidth();
            width.Type = TableWidthUnitValues.Pct;
            width.Width = "30";
            tch.Append(new TableCellProperties(
                            width));
            var tch2 = new TableCell();
            tch2.Append(new Paragraph(new Run(new Text(col2Name))));
            var element = new TableCellWidth();
            element.Type = TableWidthUnitValues.Pct;
            element.Width = "70";
            tch2.Append(new TableCellProperties(
                            element));
            tr.Append(tch);
            tr.Append(tch2);
            table.Append(tr);
            foreach (var r in data)
            {
                var trr = new TableRow();
                var tc = new TableCell();
                tc.Append(new Paragraph(new Run(new Text(r.Key))));
                var tc2 = new TableCell();
                tc2.Append(new Paragraph(new Run(new Text(r.Value))));

                trr.Append(tc);
                trr.Append(tc2);
                table.Append(trr);
            }
            ApplyStyleTable(document, styleName, styleName, table);
            return table;
        }

        internal static bool ValidateWordDocument()
        {
            throw new NotImplementedException();
        }

        private static bool IsLabel(string s)
        {
            return s.Contains(":");
        }


        private static void GetFormattedTable(Table table)
        {
            var props = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    }));

            table.AppendChild(props);
        }

        //These methods do not work with .Net Core unless the XML file is closed and reopened.
        public static bool ValidateWordDocument(WordprocessingDocument document)
        {
            try
            {
                var validator = new OpenXmlValidator();
                var count = 0;
                foreach (var error in
                    validator.Validate(document))
                {
                    count++;
                    _log.Info("Error " + count);
                    _log.Info("Description: " + error.Description);
                    _log.Info("ErrorType: " + error.ErrorType);
                    _log.Info("Node: " + error.Node);
                    _log.Info("Path: " + error.Path.XPath);
                    _log.Info("Part: " + error.Part.Uri);
                    _log.Info("-------------------------------------------");
                }

                _log.Info("count=" + count);
                return true;
            }

            catch (Exception ex)
            {
                _log.Info(ex.Message);
                return false;
            }
        }

        public static void ValidateCorruptedWordDocument(WordprocessingDocument document)
        {
            // Insert some text into the body, this would cause Schema Error

            // Insert some text into the body, this would cause Schema Error
            var body = document.MainDocumentPart.Document.Body;
            var run = new Run(new Text("some text"));
            body.Append(run);

            try
            {
                var validator = new OpenXmlValidator();
                var count = 0;
                foreach (var error in
                    validator.Validate(document))
                {
                    count++;
                    _log.Info("Error " + count);
                    _log.Info("Description: " + error.Description);
                    _log.Info("ErrorType: " + error.ErrorType);
                    _log.Info("Node: " + error.Node);
                    _log.Info("Path: " + error.Path.XPath);
                    _log.Info("Part: " + error.Part.Uri);
                    _log.Info("-------------------------------------------");
                }

                _log.Info("count=" + count.ToString());
            }

            catch (Exception ex)
            {
                _log.Info(ex.Message);
            }
        }

        internal static void AddFooter(WordprocessingDocument document, string name)
        {
            var footerPart = document.MainDocumentPart.AddNewPart<FooterPart>();

            var footerPartId = document.MainDocumentPart.GetIdOfPart(footerPart);
            
            var footer1 = new Footer(
                new Paragraph(
                    new ParagraphProperties(
                        new ParagraphStyleId() { Val = "Footer" }),
                    new Run(
                        new Text(name),
                        // *** Adaptation: This will output the page number dynamically ***
                        new SimpleField() { Instruction = "PAGE" })
                ));
            footerPart.Footer = footer1;

            var sections = document.MainDocumentPart.Document.Body.Elements<SectionProperties>();
            document.MainDocumentPart.Document.Body.PrependChild(new FooterReference { Id = footerPartId });
            foreach (var section in sections)
            {
                section.RemoveAllChildren<FooterReference>();

                // Create the new header and footer reference node
                section.PrependChild(new FooterReference { Id = footerPartId });
            }
            footer1.Save();
            PrintController.Save();
        }

        internal static void InsertCustomWatermark(WordprocessingDocument document, string imagePath)
        {
            SetWaterMarkPicture(imagePath);
            var mainDocumentPart1 = document.MainDocumentPart;
            if (mainDocumentPart1 == null) return;
            mainDocumentPart1.DeleteParts(mainDocumentPart1.HeaderParts);
            var headPart1 = mainDocumentPart1.AddNewPart<HeaderPart>();
            GenerateHeaderPart1Content(headPart1);
            var rId = mainDocumentPart1.GetIdOfPart(headPart1);
            var image = headPart1.AddNewPart<ImagePart>("image/jpeg", "rId999");
            GenerateImagePart1Content(image);
            var sectPrs = mainDocumentPart1.Document.Body.Elements<SectionProperties>();
            mainDocumentPart1.Document.Body.PrependChild(new HeaderReference { Id = rId });
            foreach (var sectPr in sectPrs)
            {
                sectPr.RemoveAllChildren<HeaderReference>();
                sectPr.PrependChild(new HeaderReference { Id = rId });
            }

        }
        private static void GenerateHeaderPart1Content(HeaderPart headerPart1)
        {
            var header1 = new Header();
            var paragraph2 = new Paragraph();
            var run1 = new Run();
            var picture1 = new Picture();
            var shape1 = new V.Shape { Id = "WordPictureWatermark75517470", Style = "position:absolute;left:0;text-align:left;margin-left:0;margin-top:0;width:456.15pt;height:456.15pt;z-index:-251656192;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin", OptionalString = "_x0000_s2051", AllowInCell = false, Type = "#_x0000_t75" };
            var imageData1 = new V.ImageData { Gain = "19661f", BlackLevel = "22938f", Title = "水印", RelationshipId = "rId999" };
            shape1.Append(imageData1);
            picture1.Append(shape1);
            run1.Append(picture1);
            paragraph2.Append(run1);
            header1.Append(paragraph2);
            headerPart1.Header = header1;
            header1.Save();
        }
        private static void GenerateImagePart1Content(ImagePart imagePart1)
        {
            var data = GetBinaryDataStream(_imagePart1Data);
            imagePart1.FeedData(data);
            data.Close();
        }

        private static string _imagePart1Data = "";

        private static Stream GetBinaryDataStream(string base64String)
        {
            return new MemoryStream(Convert.FromBase64String(base64String));
        }

        private static void SetWaterMarkPicture(string file)
        {
            try
            {
                var inFile = new FileStream(file, FileMode.Open, FileAccess.Read);
                var byteArray = new byte[inFile.Length];
                long byteRead = inFile.Read(byteArray, 0, (int)inFile.Length);
                inFile.Close();
                _imagePart1Data = Convert.ToBase64String(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        public static void ApplyStyleTable(WordprocessingDocument document, string styleId, string styleName, Table t)
        {
            // If the paragraph has no ParagraphProperties object, create one.
            if (!t.Elements<TableProperties>().Any())
            {
                t.PrependChild(new TableProperties());
            }


            // Get the paragraph properties element of the paragraph.
            var tTr = t.Elements<TableProperties>().First();

            // Get the Styles part for this document.
            var part =
                document.MainDocumentPart.StyleDefinitionsPart;

            /*
            // If the Styles part does not exist, add it and then add the style.
            if (part != null)
            {
                // If the style is not in the document, add it.
                // If the style is not in the document, add it.
                if (IsStyleIdInDocument(document, styleId, StyleValues.Table) != true)
                {
                    // No match on styleId, so let's try style name.
                    var fromStyleName = GetStyleIdFromStyleName(document, styleName, StyleValues.Table);
                    if (fromStyleName != null)
                    {
                        tTr.TableStyle = new TableStyle { Val = fromStyleName };
                        return;

                    }
                }

            }
            */
            // Set the style of the paragraph.
            tTr.TableStyle = new TableStyle { Val = styleId };

        }
        
        public static void ApplyStyleToParagraph(WordprocessingDocument document, string styleId,
            string styleName, Paragraph p, JustificationValues justification = JustificationValues.Left)
        {
            // If the paragraph has no ParagraphProperties object, create one.
            if (!p.Elements<ParagraphProperties>().Any())
            {
                p.PrependChild(new ParagraphProperties
                {
                    Justification = new Justification { Val = justification }
                });
            }

            // Get the paragraph properties element of the paragraph.
            var pPr = p.Elements<ParagraphProperties>().First();

            // Get the Styles part for this document.
            var part =
                document.MainDocumentPart.StyleDefinitionsPart;

            /*
            // If the Styles part does not exist, add it and then add the style.
            if (part != null)
            {
                // If the style is not in the document, add it.
                if (IsStyleIdInDocument(PrintController.StylesPart, styleId) != true)
                {
                    // No match on styleId, so let's try style name.
                    var fromStyleName = GetStyleIdFromStyleName(PrintController.StylesPart, styleName);
                    if (fromStyleName != null)

                        styleId = fromStyleName;
                }
            }
*/
            // Set the style of the paragraph.
            pPr.ParagraphStyleId = new ParagraphStyleId() { Val = styleId };
        }

        //File access restriction for .net Core, skipping in this version
        private static bool IsStyleIdInDocument(WordprocessingDocument document, string styleId, StyleValues styleValues = StyleValues.Paragraph)
        {
            // Get access to the Styles element for this document.
            var s = document.MainDocumentPart.StyleDefinitionsPart.Styles;

            // Check that there are styles and how many.
            var n = s.Elements<Style>().Count();
            if (n == 0)
                return false;

            // Look for a match on styleId.
            var style = s
                .Elements<Style>()
                .FirstOrDefault(st => (st.StyleId == styleId) && (st.Type == styleValues));
            return style != null;
        }
        
        // Return styleId that matches the styleName, or null when there's no match.
        // //File access restriction for .net Core, skipping in this version
        private static string GetStyleIdFromStyleName(WordprocessingDocument document, string styleName, StyleValues styleValues = StyleValues.Paragraph)
        {
            var stylePart = document.MainDocumentPart.StyleDefinitionsPart;;
            string styleId = stylePart.Styles.Descendants<StyleName>()
                .Where(s => s.Val.Value.Equals(styleName) &&
                            ((Style)s.Parent).Type == styleValues)
                .Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();
            return styleId;
        }


        //https://docs.microsoft.com/en-us/office/open-xml/how-to-replace-the-styles-parts-in-a-word-processing-document
        public static void ReplaceStylesPart(WordprocessingDocument document, XDocument newStyles,
            bool setStylesWithEffectsPart = false)
        {

            // Get a reference to the main document part.
            var docPart = document.MainDocumentPart;

            // Assign a reference to the appropriate part to the
            // stylesPart variable.
            StylesPart stylesPart = null;
            if (setStylesWithEffectsPart)
                stylesPart = docPart.StylesWithEffectsPart;
            else
                stylesPart = docPart.StyleDefinitionsPart;

            // If the part exists, populate it with the new styles.
            if (stylesPart == null) return;
            newStyles.Save(new StreamWriter(stylesPart.GetStream(
                FileMode.Create, FileAccess.Write)));
            
            //newStyles.Save(new StreamWriter(PrintController.StylesPart.GetStream(
            //    FileMode.Create, FileAccess.Read)));

        }

        public static StyleDefinitionsPart AddStylesPartToPackage(WordprocessingDocument document)
        {
            var part = document.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            var root = new Styles();
            root.Save(part);
            return part;
        }

        // Extract the styles or stylesWithEffects part from a 
        // word processing document as an XDocument instance.
        public static XDocument ExtractStylesPart(
          string fileName,
          bool getStylesWithEffectsPart = false)
        {
            // Declare a variable to hold the XDocument.
            XDocument styles = null;

            // Open the document for read access and get a reference.
            using (var document =
                WordprocessingDocument.Open(fileName, false))
            {
                // Get a reference to the main document part.
                var docPart = document.MainDocumentPart;

                // Assign a reference to the appropriate part to the
                // stylesPart variable.
                StylesPart stylesPart = null;
                if (getStylesWithEffectsPart)
                    stylesPart = docPart.StylesWithEffectsPart;
                else
                    stylesPart = docPart.StyleDefinitionsPart;

                // If the part exists, read it into the XDocument.
                if (stylesPart != null)
                {
                    using (var reader = XmlReader.Create(
                      stylesPart.GetStream(FileMode.Open, FileAccess.Read)))
                    {
                        // Create the XDocument.
                        styles = XDocument.Load(reader);
                    }
                }
            }
            // Return the XDocument instance.
            return styles;
        }
        #endregion
        
        #region bullets and such

        
        public static void InsertSpacer(WordprocessingDocument document)
        {
            var body = document.MainDocumentPart.Document.Body;
            var bbDef = body.AppendChild(new Paragraph());
            var bbRun = bbDef.AppendChild(new Run());
            bbRun.AppendChild(new Text(""));
            ApplyStyleToParagraph(document, "Normal", "Normal", bbDef, JustificationValues.Center);
        }
        public static void AddParagraph(WordprocessingDocument document, string sentence)
        {
            var runList = ListOfStringToRunList(new List<string> {sentence});
            AddParagraph(document, runList);
        }

        public static void AddParagraph(WordprocessingDocument document, IEnumerable<string> sentences)
        {
            var runList = ListOfStringToRunList(sentences);
            AddParagraph(document, runList);
        }

        public static void AddParagraph(WordprocessingDocument document, IEnumerable<Run> runList)
        {
            var para = new Paragraph();
            foreach (var runItem in runList)
            {
                para.AppendChild(runItem);
            }

            var body = document.MainDocumentPart.Document.Body;
            body.AppendChild(para);
        }

        public static void AddBulletList(WordprocessingDocument document, IEnumerable<string> sentences)
        {
            var runList = ListOfStringToRunList(sentences);

            AddBulletList(document, runList);
        }


        public static void AddBulletList(WordprocessingDocument document, IEnumerable<Run> runList)
        {
            var numberingPart = document.MainDocumentPart.NumberingDefinitionsPart;
            if (numberingPart == null)
            {
                numberingPart =
                    document.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>(
                        "NumberingDefinitionsPart001");
                var element = new Numbering();
                element.Save(numberingPart);
            }

            // Insert an AbstractNum into the numbering part numbering list.  The order seems to matter or it will not pass the 
            // Open XML SDK Productivity Tools validation test.  AbstractNum comes first and then NumberingInstance and we want to
            // insert this AFTER the last AbstractNum and BEFORE the first NumberingInstance or we will get a validation error.
            var abstractNumberId = numberingPart.Numbering.Elements<AbstractNum>().Count() + 1;
            var abstractLevel =
                new Level(new NumberingFormat() {Val = NumberFormatValues.Bullet}, new LevelText() {Val = "·"})
                    {LevelIndex = 0};
            var abstractNum1 = new AbstractNum(abstractLevel) {AbstractNumberId = abstractNumberId};

            if (abstractNumberId == 1)
            {
                numberingPart.Numbering.Append(abstractNum1);
            }
            else
            {
                var lastAbstractNum = numberingPart.Numbering.Elements<AbstractNum>().Last();
                numberingPart.Numbering.InsertAfter(abstractNum1, lastAbstractNum);
            }

            // Insert an NumberingInstance into the numbering part numbering list.  The order seems to matter or it will not pass the 
            // Open XML SDK Productivity Tools validation test.  AbstractNum comes first and then NumberingInstance and we want to
            // insert this AFTER the last NumberingInstance and AFTER all the AbstractNum entries or we will get a validation error.
            var numberId = numberingPart.Numbering.Elements<NumberingInstance>().Count() + 1;
            var numberingInstance1 = new NumberingInstance() {NumberID = numberId};
            var abstractNumId1 = new AbstractNumId() {Val = abstractNumberId};
            numberingInstance1.Append(abstractNumId1);

            if (numberId == 1)
            {
                numberingPart.Numbering.Append(numberingInstance1);
            }
            else
            {
                var lastNumberingInstance = numberingPart.Numbering.Elements<NumberingInstance>().Last();
                numberingPart.Numbering.InsertAfter(numberingInstance1, lastNumberingInstance);
            }

            var body = document.MainDocumentPart.Document.Body;

            foreach (var runItem in runList)
            {
                // Create items for paragraph properties
                var numberingProperties = new NumberingProperties(new NumberingLevelReference() {Val = 0},
                    new NumberingId() {Val = numberId});
                var spacingBetweenLines1 = new SpacingBetweenLines() {After = "0"}; // Get rid of space between bullets
                var indentation = new Indentation() {Left = "720", Hanging = "360"}; // correct indentation 

                var paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
                var runFonts1 = new RunFonts() {Ascii = "Symbol", HighAnsi = "Symbol"};
                paragraphMarkRunProperties1.Append(runFonts1);

                // create paragraph properties
                var paragraphProperties = new ParagraphProperties(numberingProperties, spacingBetweenLines1,
                    indentation, paragraphMarkRunProperties1);

                // Create paragraph 
                var newPara = new Paragraph(paragraphProperties);

                // Add run to the paragraph
                newPara.AppendChild(runItem);

                // Add one bullet item to the body
                body.AppendChild(newPara);
            }
        }

        private static IEnumerable<Run> ListOfStringToRunList(IEnumerable<string> sentences)
        {
            var runList = new List<Run>();
            foreach (var item in sentences)
            {
                var newRun = new Run();
                newRun.AppendChild(new Text(item));
                runList.Add(newRun);
            }
            return runList;
        }

        #endregion
        public static string CalculateSha3Hash(string value)
        {
            var input = Encoding.UTF8.GetBytes(value);
            var output = CalculateSha3Hash(input);
            return output.ToHex();
        }

        private static byte[] CalculateSha3Hash(byte[] value)
        {
            var digest = new KeccakDigest(256);
            var output = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(value, 0, value.Length);
            digest.DoFinal(output, 0);
            return output;
        }

        private static string ToHex(this byte[] value, bool prefix = false)
        {
            var strPrex = prefix ? "0x" : "";
            return strPrex + string.Concat(value.Select(b => b.ToString("x2")));
        }
        
    }
}