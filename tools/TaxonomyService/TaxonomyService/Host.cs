using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using log4net;
using IWA.TTF.Taxonomy.Controllers;
using IWA.TTF.Taxonomy.Model;
using IWA.TTF.Taxonomy.Model.Artifact;
using IWA.TTF.Taxonomy.Model.Core;

namespace IWA.TTF.Taxonomy
{
    internal class Host: Service.ServiceBase
    {
        private static ILog _log;

        public Host()
        {
            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public override Task<Model.Taxonomy> GetFullTaxonomy(TaxonomyVersion version, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetFullTaxonomy");
            return Task.FromResult(ModelManager.GetFullTaxonomy(version));
        }

        public override Task<Model.Taxonomy> GetLiteTaxonomy(TaxonomyVersion request, ServerCallContext context)
        {
            _log.Info("gRpc request for: GetLiteTaxonomy");
            return Task.FromResult(ModelManager.GetLiteTaxonomy(request));
        }

        public override Task<Base> GetBaseArtifact(ArtifactSymbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetBaseArtifact");
            return Task.FromResult(ModelManager.GetBaseArtifact(symbol));
        }
        
        public override Task<Behavior> GetBehaviorArtifact(ArtifactSymbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetBehaviorArtifact");
            return Task.FromResult(ModelManager.GetBehaviorArtifact(symbol));
        }
        
        public override Task<BehaviorGroup> GetBehaviorGroupArtifact(ArtifactSymbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetBehaviorGroupArtifact");
            return Task.FromResult(ModelManager.GetBehaviorGroupArtifact(symbol));
        }
        
        public override Task<PropertySet> GetPropertySetArtifact(ArtifactSymbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetPropertySetArtifact");
            return Task.FromResult(ModelManager.GetPropertySetArtifact(symbol));
        }

        public override Task<TemplateFormula> GetTemplateFormulaArtifact(ArtifactSymbol formula, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetTemplateFormulaArtifact");
            return Task.FromResult(ModelManager.GetTemplateFormulaArtifact(formula));
        }
        
        public override Task<TemplateDefinition> GetTemplateDefinitionArtifact(ArtifactSymbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetTemplateDefinitionArtifact");
            return Task.FromResult(ModelManager.GetTemplateDefinitionArtifact(symbol));
        }

        public override Task<TokenTemplate> GetTokenTemplate(TokenTemplateId request, ServerCallContext context)
        {
            _log.Info("gRpc request for: GetTokenTemplate");
            return Task.FromResult(ModelManager.GetTokenTemplate(request)); 
        }

        public override Task<TokenSpecification> GetTokenSpecification(TokenTemplateId definitionId, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetTokenSpecification");
            return Task.FromResult(ModelManager.GetTokenSpecification(definitionId));
        }
        
        public override Task<NewArtifactResponse> CreateArtifact(NewArtifactRequest artifactRequest, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: CreateArtifact");
            return Task.FromResult(ModelManager.CreateArtifact(artifactRequest));
        }
        
        public override Task<UpdateArtifactResponse> UpdateArtifact(UpdateArtifactRequest artifactRequest, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: UpdateArtifact");
            return Task.FromResult(ModelManager.UpdateArtifact(artifactRequest));
        }
        
        public override Task<DeleteArtifactResponse> DeleteArtifact(DeleteArtifactRequest artifactRequest, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: DeleteArtifact");
            return Task.FromResult(ModelManager.DeleteArtifact(artifactRequest));
        }
        
        public override Task<InitializeNewArtifactResponse> InitializeNewArtifact(InitializeNewArtifactRequest request, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: InitializeNewArtifact");
            return Task.FromResult(ModelManager.InitializeNewArtifact(request));
        }
        
        public override Task<CommitUpdatesResponse> CommitLocalUpdates(CommitUpdatesRequest request, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: Commit Local Updates");
            return Task.FromResult(GitController.Commit(request.CommitMessage));
        }
        
        public override Task<IssuePullResponse> PullRequest(IssuePullRequest request, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: PullRequest");
            return Task.FromResult(GitController.Pull());
        }
        
        public override Task<ServiceConfiguration> GetConfig(ConfigurationRequest request, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetConfig");
            return Task.FromResult(GitController.GetConfig());
        }
        
        public override Task<QueryResult> GetArtifactsOfType(QueryOptions options, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetArtifactsOfType");
            return Task.FromResult(ModelManager.GetArtifactsOfType(options));
        }
        
        public override Task<TemplateDefinition> CreateTemplateDefinition(NewTemplateDefinition newTemplateDefinition, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: CreateTemplateDefinition");
            return Task.FromResult(ModelManager.CreateTemplateDefinition(newTemplateDefinition));
        }
    }
}
