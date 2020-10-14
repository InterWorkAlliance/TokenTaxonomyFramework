using System;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;
using ValueType = IWA.TTF.Taxonomy.Model.Artifact.ValueType;

namespace IWA.TTF.Taxonomy.Controllers
{
    public static class ModelMap
    {
        public static readonly string BaseFolder = "base";
        public static readonly string BehaviorFolder = "behaviors";
        public static readonly string BehaviorGroupFolder = "behavior-groups";
        public static readonly string PropertySetFolder = "property-sets";
        private static readonly string TokenTemplatesFolder = "token-templates";
        public static readonly string TemplateFormulasFolder = TokenTemplatesFolder + TxService.FolderSeparator + "formulas";
        public static readonly string TemplateDefinitionsFolder = TokenTemplatesFolder + TxService.FolderSeparator + "definitions";

        public static string GetBaseFolderName(TokenType tokenType, TokenUnit tokenUnit, RepresentationType representation)
        {
            switch (tokenUnit)
            {
                case TokenUnit.Fractional:
                    switch (tokenType)
                    {
                        case TokenType.Fungible:
                            switch (representation)
                            {
                                case RepresentationType.Common:
                                    return "fractional-fungible";
                                case RepresentationType.Unique:
                                    return "unique-fractional-fungible";
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(representation), representation, null);
                            }
                            
                        case TokenType.NonFungible:
                            return "fractional-non-fungible";
                    }
                    break;
                case TokenUnit.Whole:
                    switch (tokenType)
                    {
                        case TokenType.Fungible:
                            switch (representation)
                            {
                                case RepresentationType.Common:
                                    return "whole-fungible";
                                case RepresentationType.Unique:
                                    return "unique-whole-fungible";
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(representation), representation, null);
                            }
      
                        case TokenType.NonFungible:
                            return "whole-non-fungible";
                    }
                    break;
                case TokenUnit.Singleton:
                    return "singleton";
            }
            return null;
        }
    }
}