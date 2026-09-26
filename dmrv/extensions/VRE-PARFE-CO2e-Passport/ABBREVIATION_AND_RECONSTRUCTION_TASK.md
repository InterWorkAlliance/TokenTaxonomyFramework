# Abbreviation and Record Reconstruction Task

## Objective

Create an abbreviated representation of the `VRE-PARFE-CO2e-Passport` canonical 21-key passport object and five-envelope core while preserving a lossless path back to the canonical form.

The abbreviation task must not change semantics. It is a naming and transport compactness pass only.

## Canonical source of truth

The complete canonical `CO2_REGULATORY_PASSPORT` structure has 21 top-level keys:

```text
type, assetType, co2ClaimId, lotId, scopeKey, claim, accounting,
parentRecEvidence, gefScope, methodScope, standardEnvelope,
externalMethodology, continuousVerification, auditAssurance, issuedUnit,
allocations, netCo2eMg, tonneLotMgCO2e, status, ts, meta
```

The canonical five-envelope core remains:

```text
standardEnvelope
externalMethodology
continuousVerification
auditAssurance
issuedUnit
```

The canonical dependency chain remains:

```text
Telemetry -> REC issuance -> REC allocation -> CO2 methodology -> CO2 passport / HTS unit
```

CO2 verifies issued REC evidence, governed CO2 methodology scope, governed GEF/emission-factor scope, allocation reconciliation, HCS replayability for CO2-owned records, and issued-unit lifecycle state. It does not verify raw telemetry directly.

## Accepted scoped abbreviation map

This pass uses scoped aliases. Top-level aliases are global, but inner aliases
apply only inside their owning envelope. This prevents ambiguous rewrites such as
changing `parentRecEvidence[].tokenId` when only `issuedUnit.tokenId` is being
abbreviated.

### Top-level aliases

| Canonical name | Abbreviation | Scope |
|---|---|---|
| `co2ClaimId` | `co2Claim` | root |
| `lotId` | `lot` | root |
| `scopeKey` | `scope` | root |
| `claim` | `clm` | root existing claim object |
| `accounting` | `acct` | root accounting object |
| `gefScope` | `gef` | root GEF governance object |
| `methodScope` | `method` | root methodology governance object |
| `standardEnvelope` | `std` | root |
| `externalMethodology` | `extMethod` | root |
| `continuousVerification` | `cv` | root |
| `auditAssurance` | `audit` | root |
| `issuedUnit` | `unit` | root |
| `parentRecEvidence` | `parentRec` | root traversal field |
| `allocations` | `alloc` | root reconciliation field |
| `netCo2eMg` | `netMg` | root |
| `tonneLotMgCO2e` | `lotMg` | root |

The required keys `type`, `assetType`, `status`, `ts`, and `meta` remain unaliased in v1 because they are already short or review-critical.

### Envelope-scoped aliases

| Scope | Canonical name | Abbreviation |
|---|---|---|
| `claim` | `claimType` | `type` |
| `claim` | `methodologySystem` | `methSys` |
| `claim` | `methodologyAuthority` | `methAuth` |
| `claim` | `ttfAlignment` | `ttf` |
| `claim` | `marketAlignment` | `market` |
| `claim` | `article64Applicability` | `art64` |
| `claim` | `validationRefCodes` | `valRefs` |
| `claim` | `claimUse` | `use` |
| `claim` | `methodologyId` | `methId` |
| `claim` | `methodologyVersion` | `methVer` |
| `claim` | `factorVintageYear` | `factorYear` |
| `claim` | `factorDocumentRef` | `factorRef` |
| `claim` | `validationStatus` | `valStatus` |
| `claim` | `verificationStatus` | `verStatus` |
| `accounting` | `accountingBoundary` | `boundary` |
| `accounting` | `calculationMethod` | `calc` |
| `accounting` | `baselineScenario` | `baseline` |
| `accounting` | `baselineEmissionsTreatment` | `baseEmis` |
| `accounting` | `projectEmissionsTreatment` | `projEmis` |
| `accounting` | `leakageTreatment` | `leakage` |
| `accounting` | `deductionsTreatment` | `deduct` |
| `accounting` | `uncertaintyTreatment` | `uncert` |
| `accounting` | `nettingRule` | `netRule` |
| `accounting` | `monitoringPlanRef` | `monPlan` |
| `gefScope` | `grantId` | `grant` |
| `gefScope` | `methodologyId` | `methId` |
| `gefScope` | `methodologyVersion` | `methVer` |
| `gefScope` | `gefValueKgPerKwh` | `kgPerKwh` |
| `gefScope` | `sourceRef` | `srcRef` |
| `gefScope` | `sourceType` | `srcType` |
| `gefScope` | `regionCode` | `region` |
| `gefScope` | `jurisdiction` | `jur` |
| `methodScope` | `methodScopeId` | `methodId` |
| `methodScope` | `hcsTopicId` | `topic` |
| `methodScope` | `hcsSequence` | `seq` |
| `methodScope` | `projectRef` | `project` |
| `methodScope` | `technologyType` | `tech` |
| `methodScope` | `status` | `state` |
| `standardEnvelope` | `standardFamily` | `sf` |
| `standardEnvelope` | `standardVersion` | `sv` |
| `standardEnvelope` | `extensionSetId` | `esId` |
| `standardEnvelope` | `extensionSetVersion` | `esVer` |
| `standardEnvelope` | `methodologySystem` | `methSys` |
| `standardEnvelope` | `methodologyAuthority` | `methAuth` |
| `standardEnvelope` | `methodologyId` | `methId` |
| `standardEnvelope` | `methodologyVersion` | `methVer` |
| `standardEnvelope` | `qualityStandardRole` | `qsRole` |
| `externalMethodology` | `authority` / `externalMethodologyAuthority` | `extAuth` |
| `externalMethodology` | `methodologyId` / `externalMethodologyId` | `extId` |
| `externalMethodology` | `methodologyName` / `externalMethodologyName` | `extName` |
| `externalMethodology` | `documentVersion` | `docVer` |
| `externalMethodology` | `documentRef` | `docRef` |
| `externalMethodology` | `documentHash` | `docHash` |
| `externalMethodology` | `validationRefCodes` | `valRefs` |
| `externalMethodology` | `purpose` | `purp` |
| `externalMethodology` | `dependencyStatus` | `depStatus` |
| `continuousVerification` | `sourceOfTruth` | `srcTruth` |
| `continuousVerification` | `verificationEngine` | `verEng` |
| `continuousVerification` | `recEvidenceValidityStatus` | `recValid` |
| `continuousVerification` | `methodologyGovernanceStatus` | `methodGov` |
| `continuousVerification` | `gefScopeGovernanceStatus` | `gefGov` |
| `continuousVerification` | `allocationReconciliationStatus` | `allocRecon` |
| `continuousVerification` | `auditReplayStatus` | `replay` |
| `auditAssurance` | `assuranceModel` | `assrModel` |
| `auditAssurance` | `auditorRole` | `audRole` |
| `auditAssurance` | `vvbDependency` | `vvbDep` |
| `auditAssurance` | `vvbUseWhereRequired` | `vvbUse` |
| `auditAssurance` | `controlEvidence` | `ctrl` |
| `controlEvidence` | `schemaVersion` | `schemaVer` |
| `controlEvidence` | `governanceTopicId` | `govTopic` |
| `controlEvidence` | `mintEventsTopicId` | `mintTopic` |
| `controlEvidence` | `methodScopeHcsSequence` | `methodSeq` |
| `controlEvidence` | `passportHcsSequence` | `passportSeq` |
| `controlEvidence` | `confirmationHcsSequence` | `confirmSeq` |
| `issuedUnit` | `assetType` | `asset` |
| `issuedUnit` | `claimType` | `claim` |
| `issuedUnit` | `quantityMgCO2e` | `qtyMg` |
| `issuedUnit` | `quantityKgCO2e` | `qtyKg` |
| `issuedUnit` | `unit` | `u` |
| `issuedUnit` | `tokenId` | `tok` |
| `issuedUnit` | `serial` | `ser` |
| `issuedUnit` | `htsTxId` | `htsTx` |
| `issuedUnit` | `lifecycleStatus` | `life` |
| `issuedUnit` | `retirementStatus` | `retire` |
| `parentRecEvidence` | `claimId` | `claim` |
| `parentRecEvidence` | `tokenId` | `tok` |
| `parentRecEvidence` | `serial` | `ser` |
| `parentRecEvidence` | `finalHcsTopicId` | `topic` |
| `parentRecEvidence` | `finalHcsSequence` | `seq` |
| `allocations` | `entitlementId` | `ent` |
| `allocations` | `parentRecClaimId` | `recClaim` |
| `allocations` | `consumedMgCO2e` | `consMg` |
| `allocations` | `sourceKwh` | `kwh` |
| `allocations` | `parentRecTokenId` | `recTok` |
| `allocations` | `parentRecSerial` | `recSer` |
| `allocations` | `parentRecFinalHcsTopicId` | `recTopic` |
| `allocations` | `parentRecFinalHcsSequence` | `recSeq` |

## Reconstruction requirement

Any abbreviated record must be reconstructable into canonical form by deterministic key mapping alone. The reconstruction step must:

- Preserve unknown fields.
- Preserve arrays and nested objects.
- Expand top-level envelope aliases back to canonical names.
- Expand nested field aliases back to canonical names within the owning scope.
- Warn on forbidden CO2-level telemetry coordinates such as `telemetryTopicId` or `telemetry_topic_id`.
- Avoid creating or defaulting evidence values.

## Local reconstruction tool

The local helper is:

```text
tools/record_reconstruction_tool.js
```

Usage:

```text
tools\record_reconstruction_tool.cmd --to canonical input.json output.canonical.json
tools\record_reconstruction_tool.cmd --to abbreviated input.json output.abbrev.json
tools\record_reconstruction_tool.cmd --map
```

If no output path is supplied, the transformed JSON is written to stdout.

## Deployment package implementation

The abbreviation profile is now represented in the deployment package as a
registry-layer implementation artifact:

```text
VRE-PARFE-ContinuousCO2e/DeploymentPackage/AbbreviationProfile.json
```

It is wired into the package through:

- `REGISTRY-RECONSTRUCTION-MODULE` in `DeploymentPackage/ExtensionSet.json`.
- `Abbreviated Passport Transport Profile Options` in `DeploymentPackage/EntityExtensionTemplates.json`.
- `AbbreviatedPassportProfileOptions`, `AliasMapping`, `PassportReconstructionRequest`, and `PassportReconstructionResponse` in `DeploymentPackage/protos/vreParfeContinuousCO2e.proto`.
- `Registry Passport Reconstruction Message Pair` in `DeploymentPackage/MessagePairs.json`.

This keeps reconstruction in the registry layer and keeps the canonical passport
messages unchanged.

## Completed implementation step

The examples now cover the full 21-key passport object:

1. A canonical sample passport using the 21 top-level keys and five-envelope core: `examples/canonical_passport_sample.json`.
2. The abbreviated equivalent: `examples/abbreviated_passport_sample.json`.

The local tool confirms that `abbreviated -> canonical` reproduces the original
sample exactly for mapped fields. The abbreviated profile remains a v1 key-level
transport/view profile; Phase 6 nested-field and content-level optimization is
roadmapped for post-launch. The canonical spec and proto definitions remain the
source of truth until IWA accepts aliases as schema-facing names.
