# IWA Submission Recap and Next Stage

## Current submission decision

The public-facing IWA extension set name is:

```text
VRE-PARFE-CO2e-Passport
```

The current local package and schema working name remains:

```text
VRE-PARFE-ContinuousCO2e
```

This preserves continuity with the files already assembled while giving IWA a clearer public title. The package may be renamed later if IWA prefers the public name to be used everywhere.

## Discussion recap

- The package is being prepared locally and files will be manually selected for upload into the GitHub pull request.
- Git repository status is not a readiness criterion for this local review.
- The earlier encoding corruption was cleaned. The package now uses ASCII-safe naming such as `Vre Parfe` rather than corrupted accent text.
- JSON files parse successfully.
- Embedded `dataExample` JSON strings parse successfully.
- The proto now defines the message types referenced by `dataSchemaOrType` entries.
- The trailing whitespace found in `HydroRE-fixes/vreParfeExtensionSet.proto` was removed.
- The CO2 extension set must not reintroduce a direct telemetry dependency.
- The GitHub/PR feedback identified a spec/sample mismatch: the docs over-emphasised the five-envelope core while the canonical live passport object has 21 top-level keys. This has now been closed in the spec, examples, abbreviation profile, and variable-template checklist.
- Raw telemetry remains reachable by traversing from CO2 into parent REC evidence, then from REC into the telemetry evidence layer.

## Canonical model retained for now

The canonical `CO2_REGULATORY_PASSPORT` structure is the 21-key top-level object:

```text
type, assetType, co2ClaimId, lotId, scopeKey, claim, accounting,
parentRecEvidence, gefScope, methodScope, standardEnvelope,
externalMethodology, continuousVerification, auditAssurance, issuedUnit,
allocations, netCo2eMg, tonneLotMgCO2e, status, ts, meta
```

Within that object, the five-envelope core remains:

```text
standardEnvelope
externalMethodology
continuousVerification
auditAssurance
issuedUnit
```

The binding dependency chain remains:

```text
Telemetry -> REC issuance -> REC allocation -> CO2 methodology -> CO2 passport / HTS unit
```

CO2 owns the governed methodology scope, emission-factor scope, parent REC validity check, allocation reconciliation, HCS replay coordinates for its own records, and issued-unit lifecycle state. The REC/telemetry layer owns sensor, gateway, signature, physics, fraud, sequence-integrity, and raw telemetry proof.

## Next stage: abbreviated forms

The next stage has begun: the canonical structure now has a scoped abbreviated transport/view profile. This reduces payload size and review noise without changing meaning. The canonical 21-key passport object and its five-envelope core remain the source of truth until IWA accepts aliases as schema-facing names.

This is abbreviation v1: key-level mapping only. Phase 6 nested-field and content-level optimization is roadmapped for post-launch.

Proposed abbreviation principles:

- Keep canonical names in the spec and proto definitions until IWA accepts the abbreviated surface.
- Use abbreviated aliases only as mapped views, not as independent semantics.
- Do not collapse CO2-owned evidence with REC/telemetry-owned evidence.
- Do not add `telemetryTopicId` or any direct telemetry coordinate to the CO2 envelope.
- Keep external methodology distinct from sovereign Vre Parfe methodology identity.
- Keep `REPLAYABLE_VIA_REC` semantics intact.

Root abbreviation map:

| Canonical name | Candidate abbreviation | Notes |
|---|---|---|
| `co2ClaimId` | `co2Claim` | Sovereign CO2 claim ID |
| `lotId` | `lot` | CO2 tonne-lot ID |
| `scopeKey` | `scope` | Governed CO2 scope key |
| `claim` | `clm` | Existing claim object |
| `accounting` | `acct` | Existing accounting object |
| `gefScope` | `gef` | GEF governance object |
| `methodScope` | `method` | Method-scope governance object |
| `standardEnvelope` | `std` | Standard, extension-set, and sovereign methodology identity |
| `externalMethodology` | `extMethod` | Reference-only external methodology citation |
| `continuousVerification` | `cv` | CO2-owned governance and reconciliation gates |
| `auditAssurance` | `audit` | CO2 replay coordinates and assurance model |
| `issuedUnit` | `unit` | HTS token, quantity, lifecycle, retirement status |
| `parentRecEvidence` | `parentRec` | Existing traversal seam into REC evidence |
| `allocations` | `alloc` | Allocation reconciliation rows |
| `netCo2eMg` | `netMg` | Net mgCO2e quantity |
| `tonneLotMgCO2e` | `lotMg` | Tonne-lot mgCO2e quantity |

The short required keys `type`, `assetType`, `status`, `ts`, and `meta` remain unaliased in v1.

Representative scoped aliases:

| Scope | Canonical name | Abbreviation |
|---|---|---|
| `claim` | claim identity and methodology fields | `type`, `methSys`, `methAuth`, `ttf`, `market`, `art64`, `valRefs`, `use`, `methId`, `methVer`, `factorYear`, `factorRef`, `valStatus`, `verStatus` |
| `accounting` | accounting boundary and calculation fields | `boundary`, `calc`, `baseline`, `baseEmis`, `projEmis`, `leakage`, `deduct`, `uncert`, `netRule`, `monPlan` |
| `gefScope` | GEF governance fields | `grant`, `methId`, `methVer`, `kgPerKwh`, `srcRef`, `srcType`, `region`, `jur` |
| `methodScope` | method-scope governance fields | `methodId`, `topic`, `seq`, `project`, `tech`, `state` |
| `standardEnvelope` | standard and methodology identity fields | `sf`, `sv`, `esId`, `esVer`, `methSys`, `methAuth`, `methId`, `methVer`, `qsRole` |
| `externalMethodology` | external citation fields | `extAuth`, `extId`, `extName`, `docVer`, `docRef`, `docHash`, `valRefs`, `purp`, `depStatus` |
| `continuousVerification` | CO2 governance and replay gates | `srcTruth`, `verEng`, `recValid`, `methodGov`, `gefGov`, `allocRecon`, `replay` |
| `auditAssurance` / `controlEvidence` | assurance and HCS replay fields | `assrModel`, `audRole`, `vvbDep`, `vvbUse`, `ctrl`, `schemaVer`, `govTopic`, `mintTopic`, `methodSeq`, `passportSeq`, `confirmSeq` |
| `issuedUnit` | issued HTS unit fields | `asset`, `claim`, `qtyMg`, `qtyKg`, `u`, `tok`, `ser`, `htsTx`, `life`, `retire` |
| `parentRecEvidence` | parent REC traversal fields | `claim`, `tok`, `ser`, `topic`, `seq` |
| `allocations` | reconciliation row fields | `ent`, `recClaim`, `consMg`, `kwh`, `recTok`, `recSer`, `recTopic`, `recSeq` |

The abbreviation pass should generate an explicit canonical-to-abbreviated mapping before any schema files are modified.

## Record reconstruction tool

A local reconstruction helper has been added:

```text
tools/record_reconstruction_tool.js
```

It converts known canonical keys to abbreviated aliases, or expands abbreviated aliases back to canonical names. Inner aliases are scoped to their owning envelope so that common field names such as `tokenId`, `serial`, or `unit` are not rewritten in unrelated evidence blocks. It preserves unknown fields and warns if a forbidden CO2-level telemetry coordinate appears.

Usage:

```text
tools\record_reconstruction_tool.cmd --to canonical input.json output.canonical.json
tools\record_reconstruction_tool.cmd --to abbreviated input.json output.abbrev.json
tools\record_reconstruction_tool.cmd --map
```

Sample records for the next-stage round trip are provided in:

```text
examples/canonical_passport_sample.json
examples/abbreviated_passport_sample.json
```

The abbreviated sample reconstructs back to the canonical sample exactly.

## Implementation now added

The abbreviation/reconstruction layer is now represented inside the deployment
package:

- `DeploymentPackage/AbbreviationProfile.json` contains the machine-readable scoped alias map.
- `REGISTRY-RECONSTRUCTION-MODULE` is listed in `DeploymentPackage/ExtensionSet.json`.
- `Abbreviated Passport Transport Profile Options` is listed in `DeploymentPackage/EntityExtensionTemplates.json`.
- `AbbreviatedPassportProfileOptions`, `AliasMapping`, `PassportReconstructionRequest`, and `PassportReconstructionResponse` are defined in the proto.
- `Registry Passport Reconstruction Message Pair` is listed in `DeploymentPackage/MessagePairs.json`.

This is intentionally a registry-layer view/reconstruction profile, not a change
to the canonical 21-key passport model.

## IWA submission notes now carried forward

- Integration with Three T's Mission Control platform is documented separately in `MISSIONCONTROL_CO2_PASSPORT_ADOPTION_INSTRUCTIONS.md`. The TTF extension set is platform-agnostic; integrators may implement it using any dMRV platform compatible with the GBBC/IWA TTF schema.
- Runtime validation MUST reject any `CO2_REGULATORY_PASSPORT` with a missing, empty, or defaulted `externalMethodology.methodologyId`. Use of CDM, Gold Standard, or other external methodology codes must be explicit in governance records.
