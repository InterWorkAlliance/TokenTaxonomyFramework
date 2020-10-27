using System;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;
using IWA.TTF.Taxonomy.TypePrinters;

namespace IWA.TTF.Taxonomy
{
    public static class PrintController
    {
        private static WordprocessingDocument _document;
        private static readonly ILog _log;
        private static string _outputFolder = "";
        private static string _filePath = "";

        static PrintController()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            #endregion
        }

        internal static PrintResult PrintArtifact(ArtifactToPrint printArtifact)
        {
            var id = printArtifact.Id;
            var draft = printArtifact.Draft;
            switch (printArtifact.Type)
            {
                case ArtifactType.Base:
                    var b = ModelManager.GetBaseArtifact(new ArtifactSymbol
                    {
                        Id = id
                    });

                    if (b != null)
                    {
                        PrintBase(b, draft);
                    }

                    break;
                case ArtifactType.Behavior:
                    var behavior = ModelManager.GetBehaviorArtifact(new ArtifactSymbol
                    {
                        Id = id
                    });

                    if (behavior != null)
                    {
                        PrintBehavior(behavior, draft);
                    }

                    break;
                case ArtifactType.BehaviorGroup:
                    var behaviorGroup = ModelManager.GetBehaviorGroupArtifact(new ArtifactSymbol
                    {
                        Id = id
                    });

                    if (behaviorGroup != null)
                    {
                        PrintBehaviorGroup(behaviorGroup, draft);
                    }

                    break;
                case ArtifactType.PropertySet:
                    var propertySet = ModelManager.GetPropertySetArtifact(new ArtifactSymbol
                    {
                        Id = id
                    });

                    if (propertySet != null)
                    {
                        PrintPropertySet(propertySet, draft);
                    }

                    break;
                case ArtifactType.TemplateFormula:
                    var formula = ModelManager.GetTemplateFormulaArtifact(new ArtifactSymbol
                    {
                        Id = id
                    });

                    if (formula != null)
                    {
                        PrintFormula(formula, draft);
                    }

                    break;
                case ArtifactType.TemplateDefinition:
                    var definition = ModelManager.GetTemplateDefinitionArtifact(new ArtifactSymbol
                    {
                        Id = id
                    });

                    if (definition != null)
                    {
                        PrintDefinition(definition, draft);
                    }

                    break;
                case ArtifactType.TokenTemplate:
                    var spec = Printer.TaxonomyClient.GetTokenSpecification(new TokenTemplateId
                    {
                        DefinitionId = id
                    });

                    if (spec != null)
                    {
                        if (spec.Artifact.Name==null || spec.Artifact.Name.Contains("Error:"))
                        {
                            _log.Error(spec.Artifact.Name);
                            break;
                        }

                        PrintSpec(spec, draft);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return GetPrintResult();
        }

        private static void InitWorkingDocument(string styleSource)
        {
            _document =
                WordprocessingDocument.Create(_filePath, WordprocessingDocumentType.Document);
            _log.Info("Artifact file: " + _filePath + " created");
            // Add a main document part.     
            var mainPart = _document.AddMainDocumentPart();

            // Create the document structure 
            mainPart.Document = new Document();
            mainPart.Document.AppendChild(new Body());
            
            // Get the Styles part for this document.
            Utils.AddStylesPartToPackage(_document);
            var styles = Utils.ExtractStylesPart(styleSource);
            Utils.ReplaceStylesPart(_document, styles);
            Save();
        }

        private static void PrintBase(Base baseToPrint, bool draft)
        {
            _log.Info("Print Controller printing Base: " + baseToPrint.Artifact.Name);

            var trimName = ModelMap.GetBaseFolderName(baseToPrint.TokenType, baseToPrint.TokenUnit,
                baseToPrint.RepresentationType);
            _outputFolder = ModelMap.FilePath +  ModelMap.BaseFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(ModelMap.StyleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            BasePrinter.PrintTokenBase(_document, baseToPrint, false);
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, baseToPrint.Artifact.Name);
            Save();
        }

        private static void PrintBehavior(Model.Core.Behavior behaviorToPrint, bool draft)
        {
            _log.Info("Print Controller printing Behavior: " + behaviorToPrint.Artifact.Name);

            var trimName = behaviorToPrint.Artifact.Name.Replace(" ", "-").ToLower();
            _outputFolder = ModelMap.FilePath +  ModelMap.BehaviorFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(ModelMap.StyleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }

            BehaviorPrinter.PrintBehavior(_document, behaviorToPrint, false);
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, behaviorToPrint.Artifact.Name);
            Save();

        }

        private static void PrintBehaviorGroup(BehaviorGroup behaviorGroupToPrint, bool draft)
        {
            _log.Info("Print Controller printing Behavior Group: " + behaviorGroupToPrint.Artifact.Name);

            var trimName = behaviorGroupToPrint.Artifact.Name.Replace(" ", "-").ToLower();
            _outputFolder = ModelMap.FilePath +  ModelMap.BehaviorGroupFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(ModelMap.StyleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            // Add a main document part. 
            BehaviorGroupPrinter.PrintBehaviorGroup(_document, behaviorGroupToPrint, false);
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, behaviorGroupToPrint.Artifact.Name);
            Save();

        }

        private static void PrintPropertySet(PropertySet psToPrint, bool draft)
        {
            _log.Info("Print Controller printing Property Set: " + psToPrint.Artifact.Name);

            var trimName = psToPrint.Artifact.Name.Replace(" ", "-").ToLower();
            _outputFolder = ModelMap.FilePath +  ModelMap.PropertySetFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(ModelMap.StyleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }

            PropertySetPrinter.AddPropertySetProperties(_document, psToPrint, false);
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, psToPrint.Artifact.Name);
            Save();
        }

        private static void PrintFormula(TemplateFormula formulaToPrint, bool draft)
        {
            _log.Info("Print Controller printing Property Set: " + formulaToPrint.Artifact.Name);

            var trimName = formulaToPrint.Artifact.Name.Replace(" ", "");
            _outputFolder = ModelMap.FilePath +  ModelMap.TemplateFormulasFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(ModelMap.StyleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
           
            FormulaPrinter.PrintFormula(_document, formulaToPrint, false);
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, formulaToPrint.Artifact.Name);
            Save();
        }
        
        private static void PrintDefinition(TemplateDefinition definitionToPrint, bool draft)
        {
            _log.Info("Print Controller printing Property Set: " + definitionToPrint.Artifact.Name);

            var trimName = definitionToPrint.Artifact.Name.Replace(" ", "-");
            _outputFolder = ModelMap.FilePath +  ModelMap.TemplateDefinitionsFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(ModelMap.StyleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
           
            DefinitionPrinter.PrintDefinition(_document, definitionToPrint, false);
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, definitionToPrint.Artifact.Name);
            Save();
        }
        
        private static void PrintSpec(TokenSpecification specification, bool draft)
        {
            _log.Info("Print Controller printing Token Specification: " + specification.Artifact.Name);

            var trimName = specification.Artifact.Name.Replace(" ", "-");
            _outputFolder = ModelMap.FilePath +  ModelMap.SpecificationsFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + "-spec.docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(ModelMap.StyleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            
            SpecificationPrinter.PrintSpecification(_document, specification, true);
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, specification.Artifact.Name + " - " + specification.SpecificationHash);
            Save();
        }
        
        internal static PrintResult PrintAllArtifacts(bool draft)
        {
            _log.Info("Taxonomy Printer: Printing All Artifacts");
     
            //bases
            foreach (var baseToken in ModelManager.Taxonomy.BaseTokenTypes)
            {
                PrintBase(baseToken.Value, draft);
            }

            foreach (var behavior in ModelManager.Taxonomy.Behaviors)
            {
                PrintBehavior(behavior.Value, draft);
            }

            foreach (var bg in ModelManager.Taxonomy.BehaviorGroups)
            {
                PrintBehaviorGroup(bg.Value, draft);
            }

            foreach (var ps in ModelManager.Taxonomy.PropertySets)
            {
                PrintPropertySet(ps.Value, draft);
            }

            foreach (var f in ModelManager.Taxonomy.TemplateFormulas)
            {
                PrintFormula(f.Value, draft);
            }

            foreach (var d in ModelManager.Taxonomy.TemplateDefinitions)
            {
                PrintDefinition(d.Value, draft);
                var spec = Printer.TaxonomyClient.GetTokenSpecification(new TokenTemplateId
                {
                    DefinitionId = d.Key
                });

                if (spec == null) continue;
                if (spec.Artifact.Name.Contains("Error:"))
                {
                    _log.Error(spec.Artifact.Name);
                    return new PrintResult();
                }
                PrintSpec(spec, draft);
            }

            return GetPrintResult();
        }
        
        /// <summary>
        /// This will create a single OpenXML document.  After it is created, it should be opened and a new Table of Contents before printing to PDF.
        /// </summary>
        internal static PrintResult BuildTtfBook(bool draft)
		{
            var styleSource = ModelMap.StyleSource;
            _filePath = 
                ModelMap.FilePath + 
                ModelMap.FolderSeparator + 
                ".." + 
                ModelMap.FolderSeparator + 
                "TTF-Book" + 
                (draft ? "-draft" : "") + 
                ".docx";
			try
			{
				InitWorkingDocument(styleSource);
                _document.MainDocumentPart.Document.AppendChild(new Body());
			}
			catch (Exception ex)
			{
				_log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
				_log.Error(ex);
                return new PrintResult
                {
                    OpenXmlDocument = ""
                };
			}

			CommonPrinter.AddTaxonomyInfo(_document, ModelManager.Taxonomy.Version);
			
			_log.Info("Adding Bases");
            CommonPrinter.AddSectionPage(_document, "Base Tokens");
			foreach (var b in ModelManager.Taxonomy.BaseTokenTypes.Values)
			{
				BasePrinter.PrintTokenBase(_document, b, true);
			}
			Save();
            
			_log.Info("Adding Behaviors");
            CommonPrinter.AddSectionPage(_document, "Behaviors");
			foreach (var b in ModelManager.Taxonomy.Behaviors.Values)
			{
                BehaviorPrinter.PrintBehavior(_document, b, true);
			}
			Save();
            
			_log.Info("Adding Behavior-Groups");
            CommonPrinter.AddSectionPage(_document, "Behavior Groups");
			foreach (var b in ModelManager.Taxonomy.BehaviorGroups.Values)
			{
				BehaviorGroupPrinter.PrintBehaviorGroup(_document, b, true);
			}
			Save();
            
            _log.Info("Adding Property-Sets");
            CommonPrinter.AddSectionPage(_document, "Property Sets");
            foreach (var ps in ModelManager.Taxonomy.PropertySets.Values)
			{
				PropertySetPrinter.AddPropertySetProperties(_document, ps, true);
			}
			Save();
            
			_log.Info("Adding Template Formulas");
            CommonPrinter.AddSectionPage(_document, "Template Formulas");
            foreach (var tf in ModelManager.Taxonomy.TemplateFormulas.Values)
			{
				FormulaPrinter.PrintFormula(_document, tf, true);
			}
			Save();
            
			_log.Info("Adding Template Definitions");
            CommonPrinter.AddSectionPage(_document, "Template Definitions");
            foreach (var td in ModelManager.Taxonomy.TemplateDefinitions.Values)
			{
				DefinitionPrinter.PrintDefinition(_document, td, true);
			}
			Save();
            
			_log.Info("Adding Specifications");
            CommonPrinter.AddSectionPage(_document, "Token Specifications");
            foreach (var tf in ModelManager.Taxonomy.TemplateDefinitions.Keys)
			{
				var spec = Printer.TaxonomyClient.GetTokenSpecification(new TokenTemplateId
				{
					DefinitionId = tf
				});
				if (spec == null) continue;
				SpecificationPrinter.PrintSpecification(_document, spec);
			}
			Save();
            
            Utils.InsertCustomWatermark(_document, draft ? ModelMap.DraftWaterMark : ModelMap.WaterMark);
            Utils.AddFooter(_document, "Token Taxonomy Framework" + " - " + ModelManager.Taxonomy.Version.Version + ": " + ModelManager.Taxonomy.Version.StateHash);
			Save();
            return GetPrintResult();
        }
        
        internal static void Save(bool validate = false)
        {
            _document.MainDocumentPart.Document.Save();
            _log.Info("Document saved: " + _filePath);
            if (!validate) return;

            _document = WordprocessingDocument.Open(_filePath,false);
                
            if (!Utils.ValidateWordDocument(_document))
                Utils.ValidateCorruptedWordDocument(_document);

            _document = WordprocessingDocument.Open(_filePath, true);
        }

        private static PrintResult GetPrintResult()
        {
            _document.Close();
            var retVal = new PrintResult();
            try
            {
                retVal.OpenXmlDocument = Convert.ToBase64String(File.ReadAllBytes(_filePath));
            }
            catch(Exception e)
            {
                _log.Error("Error getting return OpenXml return string: " + e);
                retVal.OpenXmlDocument = "";
            }
            return retVal;
        }
    }
}
