using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using log4net;
using Newtonsoft.Json.Linq;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy
{
	public static class OutputLib
	{
		private static readonly ILog Log;
		private static readonly string FolderSeparator = Os.IsWindows() ? "\\" : "/";
		private static readonly string ArtifactPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		static OutputLib()
		{
			Utils.InitLog();
			Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		internal static void OutputPropertySet(string symbol, PropertySet propSet)
		{
			Log.Error("	***PropertySet***");
			Log.Info("		-Symbol: " + symbol);
			OutputArtifact(propSet.Artifact);
			foreach (var prop in propSet.Properties)
			{
				Log.Warn("		***Non-Behavioral Property***");
				Log.Info("			-Name: " + prop.Name);
				Log.Info("			-ValueDescription: " + prop.ValueDescription);
				OutputInvocations(prop.PropertyInvocations);
				Log.Warn("		***Non-Behavioral Property End***");
			}

			Log.Error("	***PropertySet End***");
		}

		internal static void OutputTemplatePropertySet(string symbol, TemplatePropertySet propSet)
		{
			Log.Error("	***Template PropertySet***");
			Log.Info("		-Symbol: " + symbol);
			OutputSymbol(propSet.PropertySet);

			Log.Error("	***Template PropertySet End***");
		}

		private static void OutputInvocations(IEnumerable<Invocation> propPropertyInvocations)
		{
			foreach (var i in propPropertyInvocations)
			{
				Log.Error("				-Invocation");
				Log.Info("					-Name: " + i.Name);
				Log.Info("					-Description: " + i.Description);
				OutputInvocationRequest(i.Request);
				OutputInvocationResponse(i.Response);
				Log.Error("					-----------------------------------------------------");
			}
		}

		private static void OutputInvocationResponse(InvocationResponse iResponse)
		{
			Log.Warn("					[Response]");
			Log.Info("						-Name: " + iResponse.ControlMessageName);
			Log.Info("						-Description: " + iResponse.Description);
			Log.Info(" 						[Output] ");
			foreach (var p in iResponse.OutputParameters)
			{
				Log.Info("  						[Parameter] ");
				Log.Info("   							-Parameter Name: " + p.Name);
				Log.Info("   							-Parameter Value Description: " + p.ValueDescription);
			}
		}

		private static void OutputInvocationRequest(InvocationRequest iRequest)
		{
			Log.Warn("					[Request]");
			Log.Info("						-Name: " + iRequest.ControlMessageName);
			Log.Info("						-Description: " + iRequest.Description);
			Log.Info(" 						[Input] ");
			foreach (var p in iRequest.InputParameters)
			{
				Log.Info("  						[Parameter] ");
				Log.Info("   							-Parameter Name: " + p.Name);
				Log.Info("   							-Parameter Value Description: " + p.ValueDescription);
			}
		}

		internal static void OutputBehaviorGroup(string symbol, BehaviorGroup bg)
		{
			Log.Warn("	***BehaviorGroup***");
			Log.Info("			-Symbol: " + symbol);
			if (bg.Artifact != null)
			{
				OutputArtifact(bg.Artifact);
				OutputBehaviorSymbols(bg.Behaviors);
			}

			Log.Warn("	***BehaviorGroup End***");
		}

		internal static void OutputBehaviorGroupReference(BehaviorGroupReference bg)
		{
			Log.Warn("	***BehaviorGroup***");
			if (bg.Reference != null)
			{
				OutputArtifactReference(bg.Reference);
				OutputBehaviorReferences(bg.BehaviorArtifacts);
			}

			Log.Warn("	***BehaviorGroup End***");
		}


		internal static void OutputTemplateBehaviorGroup(string symbol, TemplateBehaviorGroup bg)
		{
			Log.Warn("	***BehaviorGroup***");
			Log.Info("			-Symbol: " + bg.BehaviorGroup.Tooling);
			OutputSymbol(bg.BehaviorGroup);

			Log.Warn("	***BehaviorGroup End***");
		}

		private static void OutputBehaviorSymbols(IEnumerable<BehaviorReference> bgBehaviorSymbols)
		{
			Log.Warn("		***Behavior Symbol ReferenceId***");
			foreach (var s in bgBehaviorSymbols)
			{
				Log.Error("		----------------------------------------------");
				Log.Info("			tooling: " + s.Reference.Id);
				Log.Error("		----------------------------------------------");
			}

			Log.Warn("		***Behavior Symbols End***");
		}

		private static void OutputBehaviorReferenceSymbols(IEnumerable<ArtifactReference> bgBehaviorSymbols)
		{
			Log.Warn("		***Behavior Reference***");
			foreach (var s in bgBehaviorSymbols)
			{
				Log.Error("		----------------------------------------------");
				Log.Info("			Id: " + s.Id);
				Log.Error("		----------------------------------------------");
			}

			Log.Warn("		***Behavior Reference End***");
		}

		internal static void OutputBehavior(string symbol, Behavior behavior)
		{
			Log.Warn("		***Behavior***");
			Log.Info(" 			-Symbol: " + symbol);
			if (behavior.Artifact != null)
			{
				OutputArtifact(behavior.Artifact);
				Log.Info(" 			-IsExternal: " + behavior.IsExternal);
				Log.Info(" 			-Constructor: " + behavior.ConstructorType);
				OutputInvocations(behavior.Invocations);
				OutputBehaviorProperties(behavior.Properties);
			}

			Log.Warn("		***Behavior End***");
		}

		private static void OutputBehaviorReference(BehaviorReference behavior)
		{
			Log.Warn("		***BehaviorReference***");

			if (behavior.Reference != null)
			{
				OutputArtifactReference(behavior.Reference);
				Log.Info(" 			-IsExternal: " + behavior.IsExternal);
				Log.Info(" 			-Constructor: " + behavior.ConstructorType);
				OutputInvocations(behavior.Invocations);
				OutputBehaviorProperties(behavior.Properties);
			}

			Log.Warn("		***Behavior End***");
		}

		private static void OutputBehaviorReferences(IEnumerable<BehaviorReference> behaviors)
		{
			foreach (var b in behaviors)
			{
				Log.Warn("		***BehaviorReference***");

				if (b.Reference != null)
				{
					OutputArtifactReference(b.Reference);
					Log.Info(" 			-IsExternal: " + b.IsExternal);
					Log.Info(" 			-Constructor: " + b.ConstructorType);
					OutputInvocations(b.Invocations);
					OutputBehaviorProperties(b.Properties);
				}

				Log.Warn("		***Behavior End***");
			}
		}

		private static void OutputTemplateBehavior(string symbol, TemplateBehavior behavior)
		{
			Log.Warn("		***Template Behavior***");
			Log.Info(" 			-Symbol: " + symbol);
			if (behavior.Behavior != null)
			{
				OutputSymbol(behavior.Behavior);
			}

			Log.Warn("		***Template Behavior End***");
		}


		private static void OutputBehaviorProperties(IEnumerable<Property> behaviorProperties)
		{
			foreach (var prop in behaviorProperties)
			{
				Log.Warn("		***Behavioral Property***");
				Log.Info(" 			-Name: " + prop.Name);
				Log.Info("			-ValueDescription: " + prop.ValueDescription);
				OutputInvocations(prop.PropertyInvocations);
				Log.Warn("		***Behavioral Property End***");
			}
		}

		internal static void OutputBaseType(string symbol, Base baseType)
		{
			Log.Warn("	***Base Token Type***");
			Log.Info("		-Symbol: " + symbol);
			Log.Info("		-Name: " + baseType.Name);
			Log.Info("		-Type: " + baseType.TokenType);

			OutputArtifact(baseType.Artifact);
			Log.Warn("		-Formula:");
			Log.Info("			" + baseType.Artifact.ArtifactSymbol.Tooling);
			Log.Info("		-Constructor: " + baseType.ConstructorName);
			Log.Info("		-TokenSymbol: " + baseType.Symbol);
			Log.Info("		-Owner: " + baseType.Owner);
			Log.Info("		-Decimals: " + baseType.Decimals);
			Log.Info("		-Quantity: " + baseType.Quantity);
			Log.Error("		***Properties***");
			foreach (var tp in baseType.TokenProperties)
			{
				Log.Info("  		[Properties]");
				Log.Info("   			" + tp);
			}

			Log.Info("		***PropertiesEnd***");
			Log.Warn("	***Base Token Type End***");
		}

		internal static void OutputTemplateBase(string symbol, TemplateBase baseType)
		{
			Log.Warn("	***Base Token Template***");
			Log.Info("		-Symbol: " + symbol);
			Log.Warn("		-Formula:");
			Log.Info("			" + baseType.Base.Tooling);
			Log.Error("		***Properties***");

			Log.Warn("	***Base Token Reference End***");
		}

		internal static void OutputBaseReference(BaseReference baseType)
		{
			Log.Warn("	***Base Token Reference***");
			OutputArtifactReference(baseType.Reference);
			Log.Warn("	***Base Token Reference End***");
		}

		private static void OutputArtifactReference(ArtifactReference reference)
		{
			Log.Warn("	***Artifact Reference***");
			Log.Info("		-ReferencedId: " + reference.Id);
			Log.Warn("		-Reference Type:");
			Log.Info("			" + reference.Type);
			Log.Error("		-Notes" + reference.ReferenceNotes);
			Log.Warn("		-Values:");
			if (reference.Values != null)
			{
				Log.Warn("			-ControlUri: " + reference.Values.ControlUri);
				Log.Warn("			-Maps:");
				OutputMaps(reference.Values.Maps);
				Log.Warn("			-Files:");
				OutputArtifactFiles("		", reference.Values.ArtifactFiles);
			}

			Log.Warn("	***Artifact Reference End***");
		}

		internal static void OutputTemplateFormula(string symbol, TemplateFormula template)
		{
			Log.Warn("	***TemplateFormula***");
			Log.Info("		-Formula: " + symbol);
			OutputArtifact(template.Artifact);
			OutputTemplateBase(template.TokenBase.Base.Tooling, template.TokenBase);
			Log.Error("		***Behaviors***");
			foreach (var b in template.Behaviors)
			{
				OutputTemplateBehavior(b.Behavior.Tooling, b);
			}

			Log.Error("		***Behaviors End***");
			Log.Warn("		***BehaviorGroups ***");
			foreach (var b in template.BehaviorGroups)
			{
				OutputTemplateBehaviorGroup(b.BehaviorGroup.Tooling, b);
			}

			Log.Warn("		***BehaviorGroups End***");
			Log.Error("		***PropertySets***");
			foreach (var b in template.PropertySets)
			{
				OutputTemplatePropertySet(b.PropertySet.Tooling, b);
			}

			Log.Error("		***PropertySets End***");
			Log.Warn("	***TemplateFormula End***");
		}

		internal static void OutputTemplateDefinition(string symbol, TemplateDefinition template)
		{
			Log.Warn("	***TemplateDefinition***");
			Log.Info("		-Formula: " + symbol);
			OutputArtifact(template.Artifact);
			OutputBaseReference(template.TokenBase);
			Log.Error("		***Behaviors***");
			foreach (var b in template.Behaviors)
			{
				OutputBehaviorReference(b);
			}

			Log.Error("		***Behaviors End***");
			Log.Warn("		***BehaviorGroups ***");
			foreach (var b in template.BehaviorGroups)
			{
				OutputBehaviorGroupReference(b);
			}

			Log.Warn("		***BehaviorGroups End***");
			Log.Error("		***PropertySets***");
			foreach (var b in template.PropertySets)
			{
				OutputPropertySetReference(b);
			}

			Log.Error("		***PropertySets End***");
			Log.Warn("	***TemplateDefinition End***");
		}

		private static void OutputPropertySetReference(PropertySetReference propertySetReference)
		{
			Log.Error("	***PropertySet Reference***");
			OutputArtifactReference(propertySetReference.Reference);
			foreach (var prop in propertySetReference.Properties)
			{
				Log.Warn("		***Non-Behavioral Property***");
				Log.Info("			-Name: " + prop.Name);
				Log.Info("			-ValueDescription: " + prop.ValueDescription);
				OutputInvocations(prop.PropertyInvocations);
				Log.Warn("		***Non-Behavioral Property End***");
			}

			Log.Error("	***PropertySet Reference End***");
		}


		private static void OutputArtifact(Artifact artifact)
		{
			if (artifact == null) return;
			Log.Error("		[Details]:");
			Log.Info("			-ArtifactName: " + artifact.Name);
			Log.Info("			-Type: " + artifact.ArtifactSymbol.Type);
			OutputSymbol(artifact.ArtifactSymbol);
			Log.Info("			-Aliases: " + artifact.Aliases);
			OutputArtifactDefinition(artifact.ArtifactDefinition);
			Log.Info("			-ControlUri: " + artifact.ControlUri);
			OutputDependencies(artifact.Dependencies);
			OutputInfluencedBy(artifact.InfluencedBySymbols);
			OutputIncompatible(artifact.IncompatibleWithSymbols);
			Log.Warn("			-ArtifactFiles:");
			OutputArtifactFiles("			", artifact.ArtifactFiles);
			OutputMaps(artifact.Maps);
		}


		private static void OutputDependencies(IEnumerable<SymbolDependency> artifactDependencies)
		{
			Log.Warn("			-Dependencies: ");
			foreach (var i in artifactDependencies)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Symbol: " + i.Symbol);
				Log.Info("				-Description: " + i.Description);
				Log.Error("				---------------------------------------------------");
			}
		}

		private static void OutputArtifactFiles(string buffer, IEnumerable<ArtifactFile> artifactFiles)
		{
			foreach (var f in artifactFiles)
			{
				Log.Info(buffer + "-FileContent: " + f.Content);
				Log.Info(buffer + "-FileName: " + f.FileName);
				Log.Info(buffer + "-FileContents:");
				Log.Error(buffer +
				          "---------------------------------------------------------------------------------------");

				if (f.Content != ArtifactContent.Other)
					Log.Info(f.FileData.ToStringUtf8());
				Log.Error(buffer +
				          "---------------------------------------------------------------------------------------");
			}
		}

		private static void OutputMaps(Maps artifactMaps)
		{
			if (artifactMaps != null)
			{
				Log.Error("			[Maps]:");
				Log.Warn("				-Implementation Maps:");
				foreach (var m in artifactMaps.ImplementationReferences)
				{
					Log.Error("				---------------------------------------------------");
					Log.Info("				-Name: " + m.Name);
					Log.Info("				-Type: " + m.MappingType);
					Log.Info("				-Supported Platform: " + m.Platform);
					Log.Info("				-Reference: " + m.ReferencePath);
					Log.Error("				---------------------------------------------------");
				}

				Log.Warn("				-Code Maps:");
				foreach (var m in artifactMaps.CodeReferences)
				{
					Log.Error("				---------------------------------------------------");
					Log.Info("				-Name: " + m.Name);
					Log.Info("				-Type: " + m.MappingType);
					Log.Info("				-Supported Platform: " + m.Platform);
					Log.Info("				-Reference: " + m.ReferencePath);
					Log.Error("				---------------------------------------------------");
				}

				Log.Warn("				-Resource Maps:");
				foreach (var m in artifactMaps.Resources)
				{
					Log.Error("				---------------------------------------------------");
					Log.Info("				-Name: " + m.Name);
					Log.Info("				-Type: " + m.MappingType);
					Log.Info("				-Description: " + m.Description);
					Log.Info("				-Reference: " + m.ResourcePath);
					Log.Error("				---------------------------------------------------");
				}
			}
		}

		private static void OutputIncompatible(IEnumerable<ArtifactSymbol> artifactIncompatibleWithSymbols)
		{
			Log.Warn("			-Incompatible With: ");
			foreach (var i in artifactIncompatibleWithSymbols)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Symbol: " + i.Tooling);
				Log.Error("				---------------------------------------------------");
			}
		}

		private static void OutputSymbol(ArtifactSymbol symbol)
		{
			Log.Warn("			-Symbol: ");
			Log.Error("				---------------------------------------------------");
			Log.Info("				-Id: " + symbol.Id);
			Log.Info("				-Type: " + symbol.Type);
			Log.Info("				-Tooling: " + symbol.Tooling);
			Log.Info("				-Visual: " + symbol.Visual);
			Log.Info("				-Version: " + symbol.Version);
			Log.Info("				-TemplateValidated: " + symbol.TemplateValidated);
			Log.Error("				---------------------------------------------------");
		}

		private static void OutputInfluencedBy(IEnumerable<SymbolInfluence> artifactInfluencedBySymbols)
		{
			Log.Warn("			-Influenced By: ");
			foreach (var i in artifactInfluencedBySymbols)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Symbol: " + i.Symbol);
				Log.Info("				-Description: " + i.Description);
				Log.Error("				---------------------------------------------------");
			}
		}

		private static void OutputArtifactDefinition(ArtifactDefinition artifactArtifactDefinition)
		{
			Log.Warn("			-Definition: ");
			Log.Info("				-Description: " + artifactArtifactDefinition.BusinessDescription);
			Log.Info("");
			Log.Info("				-Example: " + artifactArtifactDefinition.BusinessExample);
			Log.Info("");
			Log.Warn("				-Analogies: ");
			foreach (var a in artifactArtifactDefinition.Analogies)
			{
				Log.Info("				-Analogy: " + a.Name);
				Log.Info("				-Description: " + a.Description);
			}

			Log.Info("				-Comments: " + artifactArtifactDefinition.Comments);
		}

		internal static void SaveArtifact(ArtifactType type, string artifactName, string artifactJson)
		{
			Log.Info("Artifact Destination: " + ArtifactPath + FolderSeparator + type + " folder");
			var formattedJson = JToken.Parse(artifactJson).ToString();

			//test to make sure formattedJson will Deserialize.
			try
			{
				switch (type)
				{
					case ArtifactType.Base:
						var testBase = JsonParser.Default.Parse<Base>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						         testBase.Artifact.Name);
						break;
					case ArtifactType.Behavior:
						var testBehavior = JsonParser.Default.Parse<Behavior>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						         testBehavior.Artifact.Name);
						break;
					case ArtifactType.BehaviorGroup:
						var testBehaviorGroup = JsonParser.Default.Parse<BehaviorGroup>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						         testBehaviorGroup.Artifact.Name);
						break;
					case ArtifactType.PropertySet:
						var testPropertySet = JsonParser.Default.Parse<PropertySet>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						         testPropertySet.Artifact.Name);
						break;
					case ArtifactType.TemplateFormula:
						var testTemplate = JsonParser.Default.Parse<TemplateFormula>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						         testTemplate.Artifact.Name);
						break;
					case ArtifactType.TemplateDefinition:
						var templateDefinition = JsonParser.Default.Parse<TemplateDefinition>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						         templateDefinition.Artifact.Name);
						break;
					case ArtifactType.TokenTemplate:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			catch (Exception e)
			{
				Log.Error("Json failed to deserialize back into: " + type);
				Log.Error(e);
				return;
			}

			Log.Info("Saving Artifact: " + artifactName);
			if (ArtifactPath != null)
			{
				var savedTo = Directory.CreateDirectory(ArtifactPath + FolderSeparator + artifactName);
				var artifactStream = File.CreateText(ArtifactPath + FolderSeparator + artifactName
				                                     + FolderSeparator + artifactName + ".json");
				artifactStream.Write(formattedJson);
				artifactStream.Close();
				Log.Info("Saved to folder: " + savedTo.Name);
			}

			Log.Info("Local Artifact Save Complete");
		}

		internal static void UpdateArtifact(ArtifactType type, string folderName)
		{
			Log.Error("Updating: " + type + " name = " + folderName);
			var path = ArtifactPath + FolderSeparator + folderName;
			if (Directory.Exists(path))
			{
				var artifactFolder = new DirectoryInfo(path);
				Log.Info("Loading Updated Artifact From: " + artifactFolder.Name);
				var bJson = artifactFolder.GetFiles("*.json");
				//eventually load the proto and md as well.
				var updateArtifactRequest = new UpdateArtifactRequest
				{
					Type = type
				};

				switch (type)
				{
					case ArtifactType.Base:
						try
						{
							var baseType = GetArtifact<Base>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(baseType);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							Log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load base token type: " + artifactFolder.Name);
							Log.Error(e);
						}

						break;
					case ArtifactType.Behavior:
						try
						{
							var typeFromJson = GetArtifact<Behavior>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							Log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							Log.Error(e);
						}

						break;
					case ArtifactType.BehaviorGroup:
						try
						{
							var typeFromJson = GetArtifact<BehaviorGroup>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							Log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							Log.Error(e);
						}

						break;
					case ArtifactType.PropertySet:
						try
						{
							var typeFromJson = GetArtifact<PropertySet>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							Log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							Log.Error(e);
						}

						break;
					case ArtifactType.TokenTemplate:
						try
						{
							var typeFromJson = GetArtifact<TokenTemplate>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							Log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							Log.Error(e);
						}

						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(type), type, null);
				}
			}
			else
			{
				Log.Warn("Local Artifact Folder: " + path + " NOT FOUND.");
			}
		}

		private static T GetArtifact<T>(FileInfo artifact) where T : IMessage, new()
		{
			var typeFile = artifact.OpenText();
			var json = typeFile.ReadToEnd();
			var formattedJson = JToken.Parse(json).ToString();
			return JsonParser.Default.Parse<T>(formattedJson);
		}

		public static void OutputHierarchy(Hierarchy taxonomyTokenTemplateHierarchy)
		{
			Log.Error("[Hierarchy]:");
			Log.Warn("	-Fungibles:");
			Log.Warn("		-Fractional:");
			foreach (var f in taxonomyTokenTemplateHierarchy.Fungibles.Fractional.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			Log.Warn("		-Whole:");
			foreach (var f in taxonomyTokenTemplateHierarchy.Fungibles.Whole.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}

			Log.Warn("	-Non-Fungibles:");
			Log.Warn("		-Fractional:");
			foreach (var f in taxonomyTokenTemplateHierarchy.NonFungibles.Fractional.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			Log.Warn("		-Whole:");
			foreach (var f in taxonomyTokenTemplateHierarchy.NonFungibles.Whole.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			Log.Warn("		-Singleton:");
			foreach (var f in taxonomyTokenTemplateHierarchy.NonFungibles.Singleton.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			Log.Warn("	-Hybrid Fungible Parent:");
			Log.Warn("		-Singleton:");
			foreach (var f in taxonomyTokenTemplateHierarchy.Hybrids.Fungible.Fractional.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			Log.Warn("		-Whole:");
			foreach (var f in taxonomyTokenTemplateHierarchy.Hybrids.Fungible.Whole.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			
			Log.Warn("	-Hybrid Non-Fungible Parent:");
			Log.Warn("		-Fractional:");
			foreach (var f in taxonomyTokenTemplateHierarchy.Hybrids.NonFungible.Fractional.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			Log.Warn("		-Whole:");
			foreach (var f in taxonomyTokenTemplateHierarchy.Hybrids.NonFungible.Whole.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
			Log.Warn("		-Singleton:");
			foreach (var f in taxonomyTokenTemplateHierarchy.Hybrids.NonFungible.Singleton.Templates.Template)
			{
				Log.Error("				---------------------------------------------------");
				Log.Info("				-Name: " + f.Value.Definition.Artifact.Name);
				Log.Info("				-Aliases: " + f.Value.Definition.Artifact.Aliases);
				Log.Info("				-Formula: " + f.Value.Definition.Artifact.ArtifactSymbol.Tooling);
				Log.Error("				---------------------------------------------------");
			}
		}

		public static void OutputSpecification(TokenSpecification tokenSpec)
		{
			Log.Error("---------------------------------------------------");
			Log.Error("Token Specification:");
			Log.Error("---------------------------------------------------");

			var tDef = tokenSpec.ToString();

			var fDef = JToken.Parse(tDef).ToString();
			
			Log.Info(fDef);
		}

		public static void OutputPrintout(string resultOpenXmlDocument)
		{
			Log.Error("---------------------------------------------------");
            			Log.Error("TTF Printer Returned:");
            			Log.Error("---------------------------------------------------");
                        
            			Log.Info(resultOpenXmlDocument);
		}
	}
}