# DMRV Diagrams to support the specification

These are data type *highlights* and do not include all type properties, just the main ones.

Example assets, CRU and REC, are included in the diagrams to show how they are linked to their origination lineage.

```mermaid
erDiagram
    %%entities
    AccountableImpactOrganization {
        guid id PK "unique id"
        string name "name of the project"
        string description "description of the project"
        Addresses addresses "addresses of the project, legal, physical, etc."
        Owners owners "owners of the project"
        Country country "country of the project"
        Region region "region of the project"
    }
    ActivityImpactModule {
        guid id PK "unique id"
        guid aioId FK "ref AccountableImpactOrganization"
        string name "name of the project"
        ClassificationCategory classificationCategory "category of the project"
        BenefitCategory benefitCategory "category of the project"
        ProjectScope projectScope "scope of the project"
        ProjectType projectType "type of the project"
        Valildations validations "validation reports"
        ClaimSources claimSources "sources of data for claims"
        GeographicLocation geographicLocation "location of the project"
        Developers developers "developers of the project"
        Sponsors sponsors "sponsors of the project"
    }
    ImpactClaim {
        guid id PK "unique id"
        guid aimId FK "ref ActivityImpactModule"
        Unit unit "unit of measure for quantity"
        decimal quantity "estimated cumulative benefit amount"
        Cobenefits cobenefits "attached Cobenefits-UN-SDGs"
    }
    ImpactClaimCheckpoint {
        guid id PK "unique id"
        guid impactClaimId FK "ref ImpactClaim"
        guid developerId FK "ref ProjectDeveloper"
        guid verifiedLinkId FK "ref VerifiedLink"
        ClaimSources claimSources "sources of data for this checkpoint"
        string spanDataPackageManifest "manifest of span data package"
    }

    AccountableImpactOrganization ||--|{ ActivityImpactModule : has
    ActivityImpactModule ||--o{ ImpactClaim : creates
    ImpactClaim ||--o{ ImpactClaimCheckpoint : contains-multiple
    ActivityImpactModule ||--|| Verifier-VVB : validates

    ProcessedClaim {
        guid id PK "unique id"
        guid impactClaimId FK "ref ImpactClaim"
        guid vpaId FK "ref VerificationProcessAgreement"
        Unit unit "unit of measure for quantity"
        decimal quantity "estimated cumulative benefit amount"
        Cobenefits cobenefits "attached Cobenefits-UN-SDGs"
        SuggestedAsset asset "suggested asset for the claim based on verification"
    }
    CheckpointResult {
        guid id PK "unique id"
        guid checkpointId FK "ref ImpactClaimCheckpoint"
        guid verifiedLinkId FK "ref VerifiedLink to findings"
    }

    ProcessedClaim ||--o{ CheckpointResult : contains-multiple
    ProcessedClaim ||--|| ImpactClaim : paired-with
    ProcessedClaim ||--|| Verifier-VVB : verifies

    VerifiedLink {
        guid id PK "unique id"
        string uri "uniform resource identifier for the span data package file"
        string description ""
        string hashProof "hash of the data file"
        int hashAlgorithmId "SHA256"
    }
    HashAlgorithm {
        int id PK
        string Name "SHA256, SHA3, etc."
    }

    VerifiedLink ||--|| HashAlgorithm : uses

    VerificationProcessAgreement {
        guid id PK "unique id"
        guid mbpId FK "ref ActivityImpactModule"
        string name ""
        string description "description of the project and parties involved in origination process"
        QualityStandard qualityStandard "methodology, version, etc."
        MRVRequirements mrvRequirements "measurement specification, precision, etc."
        ClaimPeriod claimPeriod "time duration between claims for verification"
        Audits audits "scheduled audit reports"
    }

    VerificationProcessAgreementParty {
        guid id "unique id"
    }

    VerificationProcessAgreement ||--|| ActivityImpactModule : contracted
    VerificationProcessAgreement ||--|{ VerificationProcessAgreementParty : signatories

    ImpactClaimCheckpoint ||--|| VerifiedLink : has
    CheckpointResult ||--|| VerifiedLink : has

    CRU { 
        guid id PK "unique id"
        string ProcessedClaimId "ref ProcessedClaim"
        guid issuerId "ref Issuing Registry"
        decimal quantity "tonnes CO2e"
        Durability durability "sequestration length, risk, etc."
        CoreCarbonPrinciples ccps "ccp properties"
        ClimateLabels labels "optional labels"
        Status status "active, retired, revoked, etc."
        guid ownerId  "ref Account of Owner"
        guid listingAgentId "ref to distribution agent-marketplace-xchg locking asset"
    }

    REC {
        guid id PK "unique id"
        string ProcessedClaimId "ref ProcessedClaim"
        guid issuerId "ref Issuing Registry"
        decimal quantity "MWh"
        ClimateLabels labels "optional labels"
        Status status "active, retired, revoked, etc."
        guid ownerId  "ref Account of Owner"
        guid listingAgentId "ref to distribution agent-marketplace-xchg locking asset"
    }

    REC ||--|| ProcessedClaim : linked-to
    CRU ||--|| ProcessedClaim : linked-to
    ProcessedClaim ||--|| VerificationProcessAgreement : linked-to
    ProcessedClaim ||--|| IssuingRegistry : approved-by
    CRU ||--|| IssuingRegistry : issued-by
    REC ||--|| IssuingRegistry : issued-by
    ImpactClaimCheckpoint ||--|| ProjectDeveloper : submits

```
