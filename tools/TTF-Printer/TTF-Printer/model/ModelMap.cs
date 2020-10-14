using System;
using System.Collections.Specialized;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;
using IWA.TTF.Taxonomy.TypePrinters;
using ValueType = IWA.TTF.Taxonomy.Model.Artifact.ValueType;

namespace IWA.TTF.Taxonomy.Model
{
    public static class ModelMap
    {
        public static readonly string BaseFolder = "base";
        public static readonly string BehaviorFolder = "behaviors";
        public static readonly string BehaviorGroupFolder = "behavior-groups";
        public static readonly string PropertySetFolder = "property-sets";
        private static readonly string TokenTemplatesFolder = "token-templates";
        public static readonly string TemplateFormulasFolder;
        public static readonly string TemplateDefinitionsFolder;
        public static readonly string SpecificationsFolder;
        internal static string Latest { get; }
        public static string FolderSeparator { get; }
        
        internal static string FilePath { get; set; }
        internal static string WaterMark { get; set; }
        internal static string StyleSource { get; set; }
        public static string DraftWaterMark { get; set; }

        static ModelMap()
        {
            FolderSeparator = Os.IsWindows() ? "\\" : "/";
            TemplateFormulasFolder = TokenTemplatesFolder + FolderSeparator + "formulas";
            TemplateDefinitionsFolder = TokenTemplatesFolder + FolderSeparator + "definitions";
            SpecificationsFolder = TokenTemplatesFolder + FolderSeparator + "specifications";
            Latest =  "latest";
            Latest =  "latest";
        }

        public static ListDictionary GetClassificationDescription(Classification classification)
        {
            var retVal = new ListDictionary
            {
                {
                    classification.TemplateType.ToString(), classification.TemplateType == TemplateType.SingleToken
                        ? "This token has no sub or child tokens."
                        : "This token has sub or child tokens defined in the Child Tokens section below."
                },
                {
                    classification.TokenType.ToString(), classification.TokenType == TokenType.Fungible
                        ? "Tokens have interchangeable value with one another, where any quantity of them has the same value as another equal quantity if they are in the same class or series."
                        : "This token is not interchangeable with other tokens of the same type as they have different values."
                }
            };
            
            switch (classification.TokenUnit)
            {
                case TokenUnit.Fractional:
                    retVal.Add(classification.TokenUnit.ToString(), "This token can be sub-divided or split into smaller units or parts based on a certain number of decimal places.");
                    break;
                case TokenUnit.Singleton:
                    retVal.Add(classification.TokenUnit.ToString(), "There is only one instance of this token and it cannot be divided.");
                    break;
                case TokenUnit.Whole:
                    retVal.Add(classification.TokenUnit.ToString(), "There can be many instances of this token, but they cannot be divided.");
                    break;
                default:
                    return retVal;
            }
            
            retVal.Add(classification.ValueType.ToString(),
                classification.ValueType == ValueType.Intrinsic
                    ? "This token is purely a digital token represents value directly, it represents no external physical form and cannot be a receipt or title for a material item or property."
                    : "This token is a receipt or title to a material item, property or right. The token represents a reference to the value, can be owned or used digitally via its token. Sometimes referred to as a digital twin.");
            
            retVal.Add(classification.RepresentationType.ToString(),
                classification.RepresentationType == RepresentationType.Common
                    ? "This token is simply represented as a balance or quantity attributed to an owner address where all the balances are recorded on the same balance sheet, like a bank account. All instances can easily share common properties and locating them is simple."
                    : "Token instances are unique having their own identities and can be individually traced. Each unique token can carry unique properties that cannot be changed in one place and their balances must be summed. These are like bank notes, paper bills and metal coins, they are interchangeable but have unique properties like a serial number.");
            
            switch (classification.Supply)
            {
                case Supply.Fixed:
                    retVal.Add(classification.Supply.ToString(), "This token may issue an initial quantity upon creation, tokens cannot be removed or added to the supply.");
                    break;
                case Supply.CappedVariable:
                    retVal.Add(classification.Supply.ToString(), "There is a maximum number of tokens that may exist at any given time, with quantities added and removed within the quantity cap.");
                    break;
                case Supply.Gated:
                    retVal.Add(classification.Supply.ToString(), "Token quantities are added in tranches at certain points in time or specified events. Quantities in each tranche and when the tranche is issued are pre-defined that will represent the total quantity for the class, like a cap.");
                    break;
                case Supply.Infinite:
                    retVal.Add(classification.Supply.ToString(), "Infinite supply indicates that tokens in the class can be created and removed with no cap and also potentially reflect negative supply for certain business cases.");
                    break;
                default:
                    return retVal;
            }
            return retVal;
        }
        
        public static ListDictionary GetPropertySetRepresentationType(RepresentationType representationType)
        {
            var retVal = new ListDictionary
            {
                {
                    representationType.ToString(), representationType == RepresentationType.Common
                        ? "This property set's value is common or shared for all token instances in the class. Meaning all tokens in the class will share the same value of the property set."
                        : "Token instances can have unique or different values for this property set in the class."
                }
            };

            return retVal;
        }
        
        public static Classification GetClassification(TokenSpecification spec)
        {
            var template = ModelManager.GetTokenTemplate(new TokenTemplateId
            {
                DefinitionId = spec.DefinitionReference.Id
            });

            return new Classification
            {
                RepresentationType = spec.TokenBase.RepresentationType,
                TemplateType = template.Formula.TemplateType,
                ValueType = spec.TokenBase.ValueType,
                TokenType = spec.TokenBase.TokenType,
                TokenUnit = spec.TokenBase.TokenUnit,
                Supply = spec.TokenBase.Supply
            };
        }
        
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
