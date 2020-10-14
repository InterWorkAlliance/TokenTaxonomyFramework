# TTF Services

The [TaxonomyService](../tools/TaxonomyService) provides access to the TOM, allowing Create, Read, Update and Delete (CRUD) functionality to the artifacts persisted in the [artifacts](../artifacts) folder. Artifacts are serialized and stored as json text files in the artifacts repo so that the GitHub infrastructure can provide change management services for local and global TTF maintenance.

The [TTF-Printer](../tools/TTF-Printer) prints artifacts to OpenXML documents. You can print a single artifact, which will generate a *artifactName*.docx file in the artifact's folder. If a file with the same name exists, it will be overwritten. You can also print a TokenSpecification and the entire TTF as a single book file.

## Taxonomy Services Documentation

### Table of Contents

- [TaxonomyService](#service.proto)
    - [Service](#taxonomy.Service)

- [TTF-Printer](#printersvc.proto)
    - [ArtifactToPrint](#taxonomy.ttfprinter.ArtifactToPrint)
    - [PrintResult](#taxonomy.ttfprinter.PrintResult)
    - [PrintTTFOptions](#taxonomy.ttfprinter.PrintTTFOptions)
    - [PrinterService](#taxonomy.ttfprinter.PrinterService)

<a name="TaxonomyService"></a>
<p align="right"><a href="#top">Top</a></p>

## TaxonomyService

<a name="taxonomy.Service"></a>

### Service

Taxonomy Service - Create, Read, Update, Delete for the Taxonomy Object Model.

| Method Name | Request Type | Response Type | Description |
| ----------- | ------------ | ------------- | ------------|
| GetFullTaxonomy | [model.TaxonomyVersion](#taxonomy.model.TaxonomyVersion) | [model.Taxonomy](#taxonomy.model.Taxonomy) | Get the a complete TOM in a single request. Preferred method for applications when the TOM is local. |
| GetLiteTaxonomy | [model.TaxonomyVersion](#taxonomy.model.TaxonomyVersion) | [model.Taxonomy](#taxonomy.model.Taxonomy) | Get a partial TOM with references only to artifacts. |
| GetBaseArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.Base](#taxonomy.model.core.Base) | Get a Token Base artifact by Id. |
| GetBehaviorArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.Behavior](#taxonomy.model.core.Behavior) | Get a Behavior by Id. |
| GetBehaviorGroupArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.BehaviorGroup](#taxonomy.model.core.BehaviorGroup) | Get a BehaviorGroup by Id. |
| GetPropertySetArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.PropertySet](#taxonomy.model.core.PropertySet) | Get a PropertySet by Id. |
| GetTemplateFormulaArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.TemplateFormula](#taxonomy.model.core.TemplateFormula) | Get a TemplateFormula by Id. |
| GetTemplateDefinitionArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.TemplateDefinition](#taxonomy.model.core.TemplateDefinition) | Get a TemplateDefinition by Id. |
| GetTokenTemplate | [model.artifact.TokenTemplateId](#taxonomy.model.artifact.TokenTemplateId) | [model.core.TokenTemplate](#taxonomy.model.core.TokenTemplate) | Get a Token Template by TokenDefinition.Id. |
| GetTokenSpecification | [model.artifact.TokenTemplateId](#taxonomy.model.artifact.TokenTemplateId) | [model.core.TokenSpecification](#taxonomy.model.core.TokenSpecification) | Get a Token Specification by TokenDefinition.Id. |
| GetArtifactsOfType | [model.artifact.QueryOptions](#taxonomy.model.artifact.QueryOptions) | [model.artifact.QueryResult](#taxonomy.model.artifact.QueryResult) | Get artifacts by type using query options. |
| InitializeNewArtifact | [model.artifact.InitializeNewArtifactRequest](#taxonomy.model.artifact.InitializeNewArtifactRequest) | [model.artifact.InitializeNewArtifactResponse](#taxonomy.model.artifact.InitializeNewArtifactResponse) | Initialize a new artifact object and return it for updating. Assigns a unique identifier for the type. |
| CreateArtifact | [model.artifact.NewArtifactRequest](#taxonomy.model.artifact.NewArtifactRequest) | [model.artifact.NewArtifactResponse](#taxonomy.model.artifact.NewArtifactResponse) | Submit a newly created artifact object to be saved. |
| UpdateArtifact | [model.artifact.UpdateArtifactRequest](#taxonomy.model.artifact.UpdateArtifactRequest) | [model.artifact.UpdateArtifactResponse](#taxonomy.model.artifact.UpdateArtifactResponse) | Submit an updated artifact object to be saved. |
| DeleteArtifact | [model.artifact.DeleteArtifactRequest](#taxonomy.model.artifact.DeleteArtifactRequest) | [model.artifact.DeleteArtifactResponse](#taxonomy.model.artifact.DeleteArtifactResponse) | Delete an artifact by Id. |
| CreateTemplateDefinition | [model.artifact.NewTemplateDefinition](#taxonomy.model.artifact.NewTemplateDefinition) | [model.core.TemplateDefinition](#taxonomy.model.core.TemplateDefinition) | Create a TemplateDefinition from a TemplateFormula. |
| CommitLocalUpdates* | [model.artifact.CommitUpdatesRequest](#taxonomy.model.artifact.CommitUpdatesRequest) | [model.artifact.CommitUpdatesResponse](#taxonomy.model.artifact.CommitUpdatesResponse) | Issue a commit for updates made to the local git. |
| PullRequest* | [model.artifact.IssuePullRequest](#taxonomy.model.artifact.IssuePullRequest) | [model.artifact.IssuePullResponse](#taxonomy.model.artifact.IssuePullResponse) | Issue a pull request from the local clone to the global source. |
| GetConfig | [model.artifact.ConfigurationRequest](#taxonomy.model.artifact.ConfigurationRequest) | [model.artifact.ServiceConfiguration](#taxonomy.model.artifact.ServiceConfiguration) | Retrieve service configuration. |

 *Local git commands are currently disabled.

<a name="TTF-Printer"></a>
<p align="right"><a href="#top">Top</a></p>

## printersvc.proto

<a name="taxonomy.ttfprinter.ArtifactToPrint"></a>

### ArtifactToPrint

| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| type | [taxonomy.model.artifact.ArtifactType](#taxonomy.model.artifact.ArtifactType) |  | ArtifactType to print. |
| id | [string](#string) |  | Id of the artifact to print. |
| draft | [bool](#bool) |  | Should it include the Draft watermark? |


<a name="taxonomy.ttfprinter.PrintResult"></a>

### PrintResult
Expected Output from Print Request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| open_xml_document | [string](#string) |  | May include a string containing openXML content. |


<a name="taxonomy.ttfprinter.PrintTTFOptions"></a>

### PrintTTFOptions
If Book, the all artifacts will print to a single file or book.  If not, each artifact will print into their respective folder.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| book | [bool](#bool) |  | If true, print a single book file. |
| draft | [bool](#bool) |  | Should it include the Draft watermark? |
 

<a name="taxonomy.ttfprinter.PrinterService"></a>

### PrinterService
Service to Print Artifacts to OpenXML format.

| Method Name | Request Type | Response Type | Description |
| ----------- | ------------ | ------------- | ------------|
| PrintTTFArtifact | [ArtifactToPrint](#taxonomy.ttfprinter.ArtifactToPrint) | [PrintResult](#taxonomy.ttfprinter.PrintResult) | Print an artifact by Type and Id. |
| PrintTTF | [PrintTTFOptions](#taxonomy.ttfprinter.PrintTTFOptions) | [PrintResult](#taxonomy.ttfprinter.PrintResult) | Print all artifacts in either multiple artifact files in their repective artifact folders or a single TTF Book file. |
