using System;
using System.Linq;
using System.Reflection;
using log4net;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy.Model
{
	public static class ModelManager
	{
		private static ILog _log;
		internal static Taxonomy Taxonomy { get; set; }

		static ModelManager()
		{
			TypePrinters.Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
		
		public static Base GetBaseArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBaseArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<Base>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.BaseTokenTypes
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new Base();
		}

		public static Behavior GetBehaviorArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<Behavior>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.Behaviors
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new Behavior();
		}

		public static BehaviorGroup GetBehaviorGroupArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorGroupArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<BehaviorGroup>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.BehaviorGroups
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new BehaviorGroup();
		}

		public static PropertySet GetPropertySetArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetPropertySetArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<PropertySet>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.PropertySets
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new PropertySet();
		}

		public static TemplateFormula GetTemplateFormulaArtifact(ArtifactSymbol formula)
		{
			_log.Info("GetTemplateFormulaArtifact: " + formula);
			if (!string.IsNullOrEmpty(formula.Id))
			{
				return GetArtifactById<TemplateFormula>(formula.Id);
			}

			if (!string.IsNullOrEmpty(formula.Tooling))
			{
				return Taxonomy.TemplateFormulas
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == formula.Tooling).Value;
			}

			return new TemplateFormula();
		}
		
		public static TemplateDefinition GetTemplateDefinitionArtifact(ArtifactSymbol formula)
		{
			_log.Info("GetTemplateDefinitionArtifact: " + formula);
			if (!string.IsNullOrEmpty(formula.Id))
			{
				return GetArtifactById<TemplateDefinition>(formula.Id);
			}

			if (!string.IsNullOrEmpty(formula.Tooling))
			{
				return Taxonomy.TemplateDefinitions
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == formula.Tooling).Value;
			}

			return new TemplateDefinition();
		}

		public static TokenTemplate GetTokenTemplate(TokenTemplateId request)
		{
			var definition = Taxonomy.TemplateDefinitions.SingleOrDefault(e => e.Key == request.DefinitionId)
				.Value;
			if (definition == null) return null;
			var formula = Taxonomy.TemplateFormulas
				.SingleOrDefault(e => e.Key == definition.FormulaReference.Id).Value;
			var t = new TokenTemplate
			{
				Definition = definition,
				Formula = formula
			};
			return t;
		}

		public static T GetArtifactById<T>(string id)
		{
			if (typeof(T) == typeof(Base))
			{
				Taxonomy.BaseTokenTypes.TryGetValue(id, out var tokenBase);
				return (T) Convert.ChangeType(tokenBase, typeof(T));
			}

			if (typeof(T) == typeof(Behavior))
			{
				Taxonomy.Behaviors.TryGetValue(id, out var behavior);
				return (T) Convert.ChangeType(behavior, typeof(T));
			}

			if (typeof(T) == typeof(BehaviorGroup))
			{
				Taxonomy.BehaviorGroups.TryGetValue(id, out var behaviorGroup);
				return (T) Convert.ChangeType(behaviorGroup, typeof(T));
			}

			if (typeof(T) == typeof(PropertySet))
			{
				Taxonomy.PropertySets.TryGetValue(id, out var propSet);
				return (T) Convert.ChangeType(propSet, typeof(T));
			}

			if (typeof(T) == typeof(TemplateFormula))
			{
				Taxonomy.TemplateFormulas.TryGetValue(id, out var formula);
				return (T) Convert.ChangeType(formula, typeof(T));
			}

			if (typeof(T) != typeof(TemplateDefinition)) return (T) Convert.ChangeType(new object(), typeof(T));
			Taxonomy.TemplateDefinitions.TryGetValue(id, out var definition);
			return (T) Convert.ChangeType(definition, typeof(T));
		}
	}
}