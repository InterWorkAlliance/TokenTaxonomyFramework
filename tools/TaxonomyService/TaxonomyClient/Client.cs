using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using log4net;
using Microsoft.Extensions.Configuration;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy
{
	public static class Client
	{
		private static IConfigurationRoot _config;
		private static ILog _log;
		private static string _gRpcHost;
		private static int _gRpcPort;
		private static string _printHost;
		private static int _printPort;
		internal static Service.ServiceClient TaxonomyClient;
		private static PrinterService.PrinterServiceClient _printerClient;
		private static bool _saveArtifact;
		private static string _templateFormulaId;
		private static string _newArtifactName = "";
		private static string _artifactId = "";

		private static void Main(string[] args)
		{
			#region config

			_config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.AddEnvironmentVariables()
				.Build();

			#endregion

			#region logging

			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

			#endregion

			var symbol = "";
			var artifactType = ArtifactType.Base;
			var artifactSet = false;
			var create = false;
			
			var newSymbol = "";

			if (args.Length > 0 &&  args.Length < 7 )
			{
				_gRpcHost = _config["gRpcHost"];
				_gRpcPort = Convert.ToInt32(_config["gRpcPort"]);
				
				_printHost = _config["printHost"];
				_printPort = Convert.ToInt32(_config["printPort"]);
			
				_log.Info("Connection to TaxonomyService: " + _gRpcHost + " port: " + _gRpcPort);
				TaxonomyClient = new Service.ServiceClient(
					new Channel(_gRpcHost, _gRpcPort, ChannelCredentials.Insecure));
				
				_log.Info("Connection to TTF-Printer: " + _printHost + " port: " + _printPort);
				_printerClient = new PrinterService.PrinterServiceClient(
					new Channel(_printHost, _printPort, ChannelCredentials.Insecure));
				
				switch (args.Length)
				{
					case 1 when args[0] == "--f":
						GetFullTaxonomy(TaxonomyClient);
						return;
					case 1 when args[0] == "--a":
						_log.Info("Printing all artifacts individually.");
						_printerClient.PrintTTF(new PrintTTFOptions
						{
							Book = false,
							Draft = true
						});
						return;
					case 1 when args[0] == "--b":
						_log.Info("Printing all artifacts in a Book.");
						_printerClient.PrintTTF(new PrintTTFOptions
						{
							Book = true,
							Draft = true
						});
						return;
					case 1:
						OutputUsage();
						return;
					case 2:
						foreach (var t in args)
						{
							switch (t)
							{
								case "--spec":
									GetTokenSpec(args[1]);
									return;
								default:	
									OutputUsage();
									return;
							}
						}
						return;
					case 3:
						OutputUsage();
						return;
					case 4:
						var artifactFolder = "";
						for (var i = 0; i < args.Length; i++)
						{
							switch (args[i])
							{
								case "--u":
									i++;
									artifactFolder = args[i];
									continue;
								case "--t":
									i++;
									var t = Convert.ToInt32(args[i]);
									artifactType = (ArtifactType) t;
									artifactSet = true;
									continue;
								case "--d":
									i++;
									_templateFormulaId = args[i];
									continue;
								case "--n":
									i++;
									_newArtifactName = args[i];
									continue;
								case "--p":
									i++;
									_artifactId = args[i];
									continue;
								default:
									continue;
							}
						}

						if (!string.IsNullOrEmpty(_artifactId) && artifactSet)
						{
							var result = _printerClient.PrintTTFArtifact(new ArtifactToPrint
							{
								Id = _artifactId,
								Type = artifactType,
								Draft = true
							});
							OutputLib.OutputPrintout(result.OpenXmlDocument);
							return;
						}
						
						if (!string.IsNullOrEmpty(_templateFormulaId) && !string.IsNullOrEmpty(_newArtifactName))
						{
							var definition = CreateDefinition();
							OutputLib.OutputTemplateDefinition(definition.Artifact.ArtifactSymbol.Tooling,definition);
							return;
						}
						if(artifactSet && !string.IsNullOrEmpty(artifactFolder))
							UpdateArtifact(artifactType, artifactFolder);
						else
							GetArtifact(args, symbol, artifactType);
						return;
					case 5:
						foreach (var t in args)
						{
							switch (t)
							{
								case "--s":
									_saveArtifact = true;
									GetArtifact(args, symbol, artifactType);
									return;
								
								default:
									continue;
							}
						}
						OutputUsage();
						return;
					case 6:
					{
						for (var i = 0; i < args.Length; i++)
						{
							var arg = args[i];
							switch (arg)
							{
								case "--c":
									i++;
									create = true;
									newSymbol = args[1];
									continue;
								case "--t":
									i++;
									var t = Convert.ToInt32(args[i]);
									artifactType = (ArtifactType) t;
									artifactSet = true;
									continue;
								case "--n":
									i++;
									_newArtifactName = args[i];
									continue;
								default:
									continue;
							}
						}

						if (string.IsNullOrEmpty(_newArtifactName))
						{
							Console.WriteLine("Missing New Artifact Name, include --n [NEW_ARTIFACT_NAME]");
							return;
						}
						
						if (string.IsNullOrEmpty(newSymbol))
						{
							Console.WriteLine("Missing New Artifact Symbol, include --c [NEW_ARTIFACT_SYMBOL]");
							return;
						}

						if (!artifactSet)
						{
							Console.WriteLine(
								"Missing New Artifact Type, include --ts [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet, 4 - TemplateFormula or 5 - TemplateDefinition (use --d id)]");
							return;
						}
						
						if (!create)
						{
							Console.WriteLine(
								"Missing Create Switch, include --c to create a new artifact.");
							return;
						}

						if (artifactType == ArtifactType.Base)
						{
							Console.WriteLine("This tool does not support creating a new base token type or --t 0");
							return;
						}
						
						var newArtifactRequest = new NewArtifactRequest
						{
							Type = artifactType
						};
						
						Any newType;
						switch (artifactType)
						{
							case ArtifactType.Base:
								return;
							case ArtifactType.Behavior:
								var newBehavior = new Behavior
								{
									Artifact = new Artifact
									{
										Name = _newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											Id = Guid.NewGuid().ToString(),											
											Tooling = newSymbol.ToUpper(),
											Visual = "<i>" + newSymbol.ToUpper()+"</i>",
											Type = ArtifactType.Behavior,
											Version = "1.0"					}
									}
								};
								newType = Any.Pack(newBehavior);
								break;
							case ArtifactType.BehaviorGroup:
								var newBehaviorGroup = new BehaviorGroup
								{
									Artifact = new Artifact
									{
										Name = _newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											Id = Guid.NewGuid().ToString(),											
											Tooling = newSymbol.ToUpper(),
											Visual = "<i>" + newSymbol.ToUpper()+"</i>",
											Type = ArtifactType.BehaviorGroup,
											Version = "1.0",
											TemplateValidated = false
										}
									}
								};
								newType = Any.Pack(newBehaviorGroup);
								break;
							case ArtifactType.PropertySet:
								var newPropertySet = new PropertySet
								{
									Artifact = new Artifact
									{
										Name = _newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											Id = Guid.NewGuid().ToString(),											
											Tooling = newSymbol.ToUpper(),
											Visual = "&phi;" + newSymbol,
											Type = ArtifactType.PropertySet,
											Version = "1.0"
										}
									}
								};
								newType = Any.Pack(newPropertySet);
								break;
							case ArtifactType.TemplateFormula:
								var newTokenTemplate = new TemplateFormula()
								{
									Artifact = new Artifact
									{
										Name = _newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											Id = Guid.NewGuid().ToString(),											
											Tooling = newSymbol.ToUpper(),
											Visual = newSymbol,
											Version = "1.0,",
											Type = ArtifactType.TemplateFormula,
											TemplateValidated = false
										}
									}
								};
								newType = Any.Pack(newTokenTemplate);
								break;
							case ArtifactType.TemplateDefinition:
								var templateDefinition = new TemplateDefinition{
									Artifact = new Artifact
									{
										Name = _newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											Id = Guid.NewGuid().ToString(),											
											Tooling = newSymbol.ToUpper(),
											Visual = newSymbol,
											Version = "1.0,",
											Type = ArtifactType.TemplateDefinition,
											TemplateValidated = false
										}
									}
								};
								newType = Any.Pack(templateDefinition);
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						newArtifactRequest.Artifact = newType;
						
						_log.Error("Create New Artifact of Type: " + artifactType);
						if (artifactType == ArtifactType.TokenTemplate)
						{
							_log.Warn("	& Token Tooling Formula = " + symbol);
						}
						else
						{
							_log.Warn("	& Tooling Symbol" + symbol);
						}
						_log.Info("-----------------------------------------");

						var newArtifactResponse = TaxonomyClient.CreateArtifact(newArtifactRequest); 
						_log.Error("New Artifact Response");
						switch (newArtifactResponse.Type)
						{
							case ArtifactType.Base:
								break;
							case ArtifactType.Behavior:
								var newBehavior = newArtifactResponse.ArtifactTypeObject.Unpack<Behavior>();
								OutputLib.OutputBehavior(newBehavior.Artifact.ArtifactSymbol.Tooling, newBehavior);
								return;
							case ArtifactType.BehaviorGroup:
								var newBehaviorGroup = newArtifactResponse.ArtifactTypeObject.Unpack<BehaviorGroup>();
								OutputLib.OutputBehaviorGroup(newBehaviorGroup.Artifact.ArtifactSymbol.Tooling, newBehaviorGroup);
								return;
							case ArtifactType.PropertySet:
								var newPropertySet = newArtifactResponse.ArtifactTypeObject.Unpack<PropertySet>();
								OutputLib.OutputPropertySet(newPropertySet.Artifact.ArtifactSymbol.Tooling, newPropertySet);
								return;
							case ArtifactType.TokenTemplate:
								break;
							case ArtifactType.TemplateFormula:
								var newTokenTemplate = newArtifactResponse.ArtifactTypeObject.Unpack<TemplateFormula>();
								OutputLib.OutputTemplateFormula(newTokenTemplate.Artifact.ArtifactSymbol.Tooling, newTokenTemplate);
								return;
							case ArtifactType.TemplateDefinition:
								var templateDefinition = newArtifactResponse.ArtifactTypeObject.Unpack<TemplateDefinition>();
								OutputLib.OutputTemplateDefinition(templateDefinition.Artifact.ArtifactSymbol.Tooling, templateDefinition);
								return;							default:
								throw new ArgumentOutOfRangeException();
						}
						return;
					}
					default:
						OutputUsage();
						return;
				}
			}
			OutputUsage();
		}

		private static void GetTokenSpec(string s)
		{
			var tokenSpec = TaxonomyClient.GetTokenSpecification(new TokenTemplateId
			{
				DefinitionId = s
			});
			OutputLib.OutputSpecification(tokenSpec);
		}

		private static TemplateDefinition CreateDefinition()
		{
			return TaxonomyClient.CreateTemplateDefinition(new NewTemplateDefinition
			{
				TemplateFormulaId = _templateFormulaId, TokenName = _newArtifactName
			});
		}

		private static void UpdateArtifact(ArtifactType type, string updateFolder)
		{
			OutputLib.UpdateArtifact(type, updateFolder);
		}

		private static void OutputUsage()
		{
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --f");
			Console.WriteLine("	Retrieves the entire Taxonomy and writes it to the console.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --spec");
			Console.WriteLine("	Retrieves a generated specification with the required argument that is the template definition Id.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --ts [TOOLING_SYMBOL_OR_ID] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Retrieves the Taxonomy Artifact by Tooling Symbol or Id and writes it to the console.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --ts [TOOLING_SYMBOL_OR_Id] --s --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Retrieves the Taxonomy Artifact by Tooling Symbol or Id and SAVES it locally in a folder and writes it to the console.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --u [UPDATE_ARTIFACT_FOLDER] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Updates an artifact edited locally if updated from a saved query using --s.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --c [NEW_ARTIFACT_SYMBOL] --n [NEW_ARTIFACT_NAME] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Creates a new Taxonomy Artifact and returns it after creation.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --d [TEMPLATE_FORMULA_ID] --n [NEW_ARTIFACT_NAME]");
			Console.WriteLine("	 Creates a new Taxonomy Template Definition from a Template Formula, saves it in the taxonomy and returns it after creation.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --p [ARTIFACT_ID] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet, 4 - TemplateFormula, 5 - TemplateDefinition, 6 - TokenSpecification]");
			Console.WriteLine("	Prints an artifact individually with the output placed in the artifact's folder.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --a");
			Console.WriteLine("	Prints all artifacts individually with the output placed in the artifact's folder.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --b");
			Console.WriteLine("	Prints all artifacts in a Book with the output placed in root of the repo.");
		}

		private static void GetFullTaxonomy(Service.ServiceClient taxonomyClient)
		{
			_log.Warn("Fetching 1.0 version of the Taxonomy");
			Model.Taxonomy taxonomy;
			try
			{
				taxonomy = taxonomyClient.GetFullTaxonomy(new TaxonomyVersion {Version = "1.0"});
			}
			catch (Exception e)
			{
				_log.Error(e.Message);
				_log.Error("Cannot connect to Taxonomy Service: " + _gRpcHost + " on port: " +_gRpcPort);
				return;
			}

			_log.Warn("-----------------------------Taxonomy Start---------------------------------------");
			_log.Info("-Taxonomy Version: " + taxonomy.Version);
			_log.Info("-Taxonomy Base Token Types Count: " + taxonomy.BaseTokenTypes.Count);
			_log.Info("-Taxonomy Behaviors Count: " + taxonomy.Behaviors.Count);
			_log.Info("-Taxonomy BehaviorGroups Count: " + taxonomy.BehaviorGroups.Count);
			_log.Info("-Taxonomy PropertySet Count: " + taxonomy.PropertySets.Count);
			_log.Info("-Taxonomy TemplateFormula Count: " + taxonomy.TemplateFormulas.Count);
			_log.Info("-Taxonomy TemplateDefinition Count: " + taxonomy.TemplateDefinitions.Count);
			_log.Info("-Taxonomy Hierarchy Fractional Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.Fungibles.Fractional.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Whole Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.Fungibles.Whole.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Fractional Non-Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.NonFungibles.Fractional.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Whole Non-Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.NonFungibles.Whole.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Singleton Non-Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.NonFungibles.Singleton.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Hybrid-Fractional Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.Hybrids.Fungible.Fractional.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Hybrid-Whole Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.Hybrids.Fungible.Whole.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Hybrid-Fractional Non-Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.Hybrids.NonFungible.Fractional.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Hybrid-Whole Non-Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.Hybrids.NonFungible.Whole.Templates.Template.Count);
			_log.Info("-Taxonomy Hierarchy Hybrid-Singleton Non-Fungible TokenTemplates Count: " + taxonomy.TokenTemplateHierarchy.Hybrids.NonFungible.Singleton.Templates.Template.Count);

			_log.Error("-> Base Token Types <-");
			foreach (var (symbol, baseType) in taxonomy.BaseTokenTypes)
			{
				OutputLib.OutputBaseType(symbol, baseType);
			}

			_log.Error("-> Base Token Types End <-");
			_log.Warn("-> Behaviors <-");
			foreach (var (symbol, behavior) in taxonomy.Behaviors)
			{
				OutputLib.OutputBehavior(symbol, behavior);
			}

			_log.Warn("-> Behaviors End <-");
			_log.Error("-> BehaviorGroups <-");

			foreach (var (symbol, bg) in taxonomy.BehaviorGroups)
			{
				OutputLib.OutputBehaviorGroup(symbol, bg);
			}

			_log.Error("-> BehaviorGroups End <-");

			_log.Warn("-> PropertySets <-");
			foreach (var (symbol, propSet) in taxonomy.PropertySets)
			{
				OutputLib.OutputPropertySet(symbol, propSet);
			}

			_log.Warn("-> PropertySets End <-");
			_log.Error("-> TokenFormulas <-");
			foreach (var (symbol, value) in taxonomy.TemplateFormulas)
			{
				OutputLib.OutputTemplateFormula(symbol, value);
			}

			_log.Warn("-> TokenFormulas End <-");
			
			_log.Error("-> TemplateDefinitions <-");
			foreach (var (symbol, value) in taxonomy.TemplateDefinitions)
			{
				OutputLib.OutputTemplateDefinition(symbol, value);
			}

			_log.Warn("-> TemplateDefinitions End <-");

			_log.Warn("-> Hierarchy 			   <-");

			OutputLib.OutputHierarchy(taxonomy.TokenTemplateHierarchy);
			_log.Warn("-> END Hierarchy 			   <-");
			_log.Warn("-----------------------------Taxonomy   End----------------------------------------");
		}
		
		private static void GetArtifact(IReadOnlyList<string> args, string symbol, ArtifactType artifactType)
		{
			var artifactSet = false;
			for (var i = 0; i < args.Count; i++)
			{
				var arg = args[i];
				switch (arg)
				{
					case "--ts":
						i++;
						symbol = args[i];
						continue;
					case "--t":
						i++;
						var t = Convert.ToInt32(args[i]);
						artifactType = (ArtifactType) t;
						artifactSet = true;
						continue;
					default:
						continue;
				}
			}

			if (string.IsNullOrEmpty(symbol))
			{
				Console.WriteLine("Missing Symbol for Taxonomy Query, include --ts [TOOLING_SYMBOL]");
				return;
			}

			if (!artifactSet)
			{
				Console.WriteLine(
					"Missing Artifact Type, include --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
				return;
			}

			var symbolQuery = new ArtifactSymbol();
			if (Utils.IsValidGuid(symbol))
			{
				symbolQuery.Id = symbol;
			}
			else
			{
				symbolQuery.Tooling = symbol;
			}
			
			_log.Error("Taxonomy Artifact Symbol Query for Type: " + artifactType);
			if (artifactType == ArtifactType.TokenTemplate)
			{
				_log.Warn("	& Token Tooling Formula = " + symbol);
			}
			else
			{
				_log.Warn("	& Tooling Symbol " + symbol);
			}

			_log.Info("-----------------------------------------");
			
			var jsf = new JsonFormatter(new JsonFormatter.Settings(true));

			switch (artifactType)
			{
				case ArtifactType.Base:
					var baseType = TaxonomyClient.GetBaseArtifact(symbolQuery);
					OutputLib.OutputBaseType(symbol, baseType);
					if (!_saveArtifact) return;
					var typedJson = jsf.Format(baseType);
					OutputLib.SaveArtifact(artifactType, baseType.Artifact.Name, typedJson);
					return;
				case ArtifactType.Behavior:
					var behavior = TaxonomyClient.GetBehaviorArtifact(symbolQuery);
					OutputLib.OutputBehavior(symbol, behavior);
					if (!_saveArtifact) return;
					var typedBehaviorJson = jsf.Format(behavior);
					OutputLib.SaveArtifact(artifactType, behavior.Artifact.Name, typedBehaviorJson);
					return;
				case ArtifactType.BehaviorGroup:
					var behaviorGroup = TaxonomyClient.GetBehaviorGroupArtifact(symbolQuery);
					OutputLib.OutputBehaviorGroup(symbol, behaviorGroup);
					if (!_saveArtifact) return;
					var typedBehaviorGroupJson = jsf.Format(behaviorGroup);
					OutputLib.SaveArtifact(artifactType, behaviorGroup.Artifact.Name, typedBehaviorGroupJson);
					return;
				case ArtifactType.PropertySet:
					var propertySet = TaxonomyClient.GetPropertySetArtifact(symbolQuery);
					OutputLib.OutputPropertySet(symbol, propertySet);
					if (!_saveArtifact) return;
					var typedPropertySetJson = jsf.Format(propertySet);
					OutputLib.SaveArtifact(artifactType, propertySet.Artifact.Name, typedPropertySetJson);
					return;
				case ArtifactType.TemplateFormula:
					var tokenTemplate = TaxonomyClient.GetTemplateFormulaArtifact(symbolQuery);
					OutputLib.OutputTemplateFormula(symbol, tokenTemplate);
					if (!_saveArtifact) return;
					var typedTemplateJson = jsf.Format(tokenTemplate);
					OutputLib.SaveArtifact(artifactType, tokenTemplate.Artifact.Name, typedTemplateJson);
					return;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}