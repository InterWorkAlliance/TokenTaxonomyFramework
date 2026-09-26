# Getting Started  -  VRE-PARFE-CO2e-Passport (Vre Parfe CO2 Passport)

**GBBC/IWA dMRV v3.0 Extension Set | Version 1.0.0**
**Adopted title:** Vre Parfe CO2 Passport
**Public IWA extension set name:** `VRE-PARFE-CO2e-Passport`
**Current local package/schema ID:** `VRE-PARFE-ContinuousCO2e`
**Sovereign Methodology Authority:** VRE_PARFE (VP-CO2E-RE-GRID-001, V1)
**Issuing Authority:** Three T's (Mauritius) Limited | Registration No: C19166743

---

## Submission status

This package is being prepared locally for manual file selection into the IWA GitHub pull request. The local review decisions are:

- `VRE-PARFE-CO2e-Passport` is the public IWA-facing extension set name.
- `VRE-PARFE-ContinuousCO2e` remains the current local package/schema ID for continuity.
- `Vre Parfe` is intentionally ASCII-safe in these files.
- The package passed local hygiene checks after cleanup: JSON parse, embedded `dataExample` JSON parse, non-ASCII scan, and trailing-whitespace scan.
- The next stage abbreviation profile has been defined as a scoped, lossless transport/view mapping. The canonical 21-key passport object and its five-envelope core remain the source of truth.

## 1. What this is

VRE-PARFE-ContinuousCO2e is the sovereign CO2e passport extension set for Vre Parfe-issued CO2 tonne-lots. CO2 issuance is derived from issued REC evidence, governed CO2 methodology scope, emission-factor scope, allocation reconciliation, HCS replayability, and HTS lifecycle state.

**CO2 issuance is not a direct consumer of raw telemetry.** The governed dependency chain is:

```
Telemetry -> REC issuance -> REC allocation -> CO2 methodology -> CO2 passport / HTS unit
```

Raw readings remain available for audit through the REC evidence chain -- reached by traversing this passport's `parentRecEvidence` references into each REC's own passport record. CO2 applies its own governed methodology and GEF/emission-factor scope on top of that REC evidence; it does not re-derive or re-verify telemetry directly. In short: this is a **CO2 Passport from governed REC evidence**, not a second, parallel telemetry consumer.

Every issued passport must still answer five questions -- but the second and third now resolve one hop earlier than a naive "telemetry-first" reading would suggest:

1. What was measured? *(answered by the REC evidence layer, not repeated here)*
2. Who or what measured it? *(answered by the REC evidence layer, not repeated here)*
3. Was the source authorised? *(answered by the REC evidence layer; CO2 checks that the REC itself is validly issued, via `recEvidenceValidityStatus`)*
4. What calculation converted REC evidence into the issued CO2 unit? *(CO2's own responsibility: governed GEF/methodology scope + allocation reconciliation)*
5. Can the entire chain be replayed independently from public records? *(CO2 replays its own record and traverses into REC; REC's own replay into telemetry is that layer's responsibility)*

## 2. The 21-key passport object and five-envelope core

The complete canonical `CO2_REGULATORY_PASSPORT` record is a 21-key top-level object:

```text
type, assetType, co2ClaimId, lotId, scopeKey, claim, accounting,
parentRecEvidence, gefScope, methodScope, standardEnvelope,
externalMethodology, continuousVerification, auditAssurance, issuedUnit,
allocations, netCo2eMg, tonneLotMgCO2e, status, ts, meta
```

The five envelopes below are the IWA extension-set core inside that larger passport object.

| Envelope | extensionContext | What it covers now |
|---|---|---|
| `standardEnvelope` | ExtensionSet | Standard/extension-set/methodology identity  -  new |
| `externalMethodology` | ExtensionSet | UNFCCC/CDM reference-only link  -  already emitted (`buildExternalMethodologySnapshot`), this set adds `documentVersion`/`documentRef`/`documentHash`/`purpose`/`dependencyStatus` |
| `continuousVerification` | ExtensionSetModule (CONTINUOUS-VERIFICATION-MODULE) | **CO2's own governance surface only**: `methodologyGovernanceStatus`, `gefScopeGovernanceStatus`, `recEvidenceValidityStatus`, `allocationReconciliationStatus`, `auditReplayStatus`. Deliberately excludes sensor/gateway/signature/physics/fraud/sequence-integrity gates  -  those live at the REC/telemetry layer |
| `auditAssurance` | ExtensionSetModule (AUDIT-ASSURANCE-MODULE) | `controlEvidence` coordinates for CO2's own governance/passport/confirmation records. Traversal into REC uses the existing `parentRecEvidence` array; no `telemetryTopicId` is cached here |
| `issuedUnit` | ExtensionSetModule (CO2-LOT-MODULE) | Already emitted as `co2Token`/`totalMgCO2e`/`totalKgCO2e`; this set adds `unit`, `lifecycleStatus`, `retirementStatus` |

## 3. External methodology is governed, not defaulted

The external UNFCCC/CDM-style citation is separate from Vre Parfe's own sovereign methodology identity.

- `claim.methodologyId` / `standardEnvelope.methodologyId` identify the sovereign Vre Parfe methodology, e.g. `VP-CO2E-RE-GRID-001`.
- `externalMethodology.methodologyId` identifies the reference-only external methodology citation, e.g. `AMS-I.D`.

This extension set requires the external citation to be explicit in governed method-scope HCS evidence. A runtime must not silently substitute `ACM0002`, `AMS-I.D`, `TOOL01`, or any other methodology/tool when `CO2_METHOD_SCOPE_GRANT.externalMethodology` is missing. Runtime validation MUST reject any `CO2_REGULATORY_PASSPORT` with a missing, empty, or defaulted `externalMethodology.methodologyId`. Missing `externalMethodology.validationRefCodes` also means the method scope is incomplete.

Latest live proof, supplied 2026-07-06:

- Governance topic `0.0.8479702`, sequences `76-77`: `CO2_METHOD_SCOPE_GRANT` explicitly declares `externalMethodology.methodologyId = "AMS-I.D"` and `validationRefCodes = ["ARTICLE_6_4_PRINCIPLES"]`.
- Governance topic `0.0.8479702`, sequence `78`: `CO2_METHOD_SCOPE_GRANTED` audit event.
- Mint-events topic `0.0.8585272`, sequences `374200-374204`: `CO2_REGULATORY_PASSPORT` carries `AMS-I.D` and `methodScopeHcsSequence = "77"`.
- Mint-events topic `0.0.8585272`, sequences `374205-374208`: `CO2_OFFSET_LOT_CONFIRMATION` carries the same `AMS-I.D` and mints HTS serial `31371`.

## 4. Why `telemetryTopicId` isn't a CO2-level field

An earlier draft of this Extension Set proposed a `telemetryTopicId` field directly on CO2's `auditAssurance.controlEvidence`, on the reasoning that an auditor should be able to jump straight from a CO2 passport to raw signed telemetry. On review, that framing was wrong: it implied CO2 depends on telemetry directly, when the actual governed dependency is CO2 -> REC -> telemetry.

The correct model:

- CO2's `parentRecEvidence` array (already present in the live payload -- `claimId`, `tokenId`, `serial`, `finalHcsTopicId`, `finalHcsSequence` per parent REC) is the one hop CO2 owns.
- Resolving a `parentRecEvidence` entry means fetching that REC's own passport record. That REC record is responsible for referencing its originating signed-telemetry topic (the CO2e-layer analogue of `VRE-PARFE-HydroRE` topic `0.0.8480236`) -- a second hop that belongs to the REC passport format, not to this extension set.
- CO2's own `recEvidenceValidityStatus` gate checks that every `parentRecEvidence` entry resolves to a valid, minted, non-revoked REC token. It does not re-check the telemetry underneath that REC; it trusts the REC issuance process to have already done so, and lets an auditor traverse further if they want to confirm it themselves.

`auditReplayStatus` therefore reads `REPLAYABLE_VIA_REC`, not `REPLAYABLE_FROM_HCS` -- naming the actual one-hop guarantee CO2 provides, rather than implying a direct telemetry guarantee it does not make.

## 5. Relationship to VRE-PARFE-HydroRE

This set does not replace `VRE-PARFE-HydroRE`. `VRE-PARFE-HydroRE` proves the physical measurement is real (physics gate, Ed25519 signature, HCS anchoring) and issues REC evidence from it. `VRE-PARFE-ContinuousCO2e` -- the Vre Parfe CO2 Passport -- takes that issued REC evidence, applies governed CO2 methodology and GEF/emission-factor scope, reconciles the allocation, and issues a serialised, lifecycle-tracked CO2e tonne-lot. The two compose along exactly one seam: `parentRecEvidence`.

Integration with Three T's Mission Control platform is documented separately in `MISSIONCONTROL_CO2_PASSPORT_ADOPTION_INSTRUCTIONS.md`. The TTF extension set is platform-agnostic; integrators may implement it using any dMRV platform compatible with the GBBC/IWA TTF schema.

## 6. Known defects inherited and fixed from VRE-PARFE-HydroRE

While assembling this set, two files in the merged `VRE-PARFE-HydroRE` package (PR #16, `main` branch) were found to be structurally invalid:

- `DeploymentPackage/VariableTemplates.json`  -  truncated mid-object at line 101; fails `json.load`.
- `DeploymentPackage/protos/vreParfeExtensionSet.proto`  -  truncated inside `PhysicsGateOptions`; never defines `message LedgerAttestationOptions`, despite being referenced by `ExtensionSet.json` and `EntityExtensionTemplates.json`.

Both defects were confirmed against the actual repository content and corrected copies are provided separately in `HydroRE-fixes/`. Neither defect is copied forward into this Extension Set's own files, all of which pass `json.load` / `protoc` checks.

## 7. Next stage: abbreviated forms

The current package keeps the canonical 21-key passport object and envelope names as the source of truth:

```text
standardEnvelope
externalMethodology
continuousVerification
auditAssurance
issuedUnit
```

The first abbreviation pass defines compact aliases for review or runtime payloads. These aliases are a mapped view of the canonical passport, not a replacement schema. Root aliases apply globally; inner aliases apply only inside their owning envelope.

This is abbreviation v1: key-level mapping only. Phase 6 nested-field and content-level optimization is roadmapped for post-launch, after the canonical model has been accepted and proven in production.

Root aliases:

| Canonical | Candidate abbreviation |
|---|---|
| `co2ClaimId` | `co2Claim` |
| `lotId` | `lot` |
| `scopeKey` | `scope` |
| `claim` | `clm` |
| `accounting` | `acct` |
| `gefScope` | `gef` |
| `methodScope` | `method` |
| `standardEnvelope` | `std` |
| `externalMethodology` | `extMethod` |
| `continuousVerification` | `cv` |
| `auditAssurance` | `audit` |
| `issuedUnit` | `unit` |
| `parentRecEvidence` | `parentRec` |
| `allocations` | `alloc` |
| `netCo2eMg` | `netMg` |
| `tonneLotMgCO2e` | `lotMg` |

The short required keys `type`, `assetType`, `status`, `ts`, and `meta` remain unaliased in v1.

Representative scoped aliases:

| Scope | Canonical | Abbreviation |
|---|---|---|
| `claim` | claim identity and methodology fields | `type`, `methSys`, `methAuth`, `ttf`, `market`, `art64`, `valRefs`, `use`, `methId`, `methVer`, `factorYear`, `factorRef`, `valStatus`, `verStatus` |
| `accounting` | accounting boundary and calculation fields | `boundary`, `calc`, `baseline`, `baseEmis`, `projEmis`, `leakage`, `deduct`, `uncert`, `netRule`, `monPlan` |
| `gefScope` | GEF governance fields | `grant`, `methId`, `methVer`, `kgPerKwh`, `srcRef`, `srcType`, `region`, `jur` |
| `methodScope` | method-scope governance fields | `methodId`, `topic`, `seq`, `project`, `tech`, `state` |
| `standardEnvelope` | `standardFamily`, `standardVersion`, `extensionSetId`, `extensionSetVersion` | `sf`, `sv`, `esId`, `esVer` |
| `standardEnvelope` | `methodologySystem`, `methodologyAuthority`, `methodologyId`, `methodologyVersion`, `qualityStandardRole` | `methSys`, `methAuth`, `methId`, `methVer`, `qsRole` |
| `externalMethodology` | `authority`, `methodologyId`, `methodologyName`, `validationRefCodes`, `dependencyStatus` | `extAuth`, `extId`, `extName`, `valRefs`, `depStatus` |
| `continuousVerification` | `sourceOfTruth`, `verificationEngine`, `recEvidenceValidityStatus`, `methodologyGovernanceStatus`, `gefScopeGovernanceStatus`, `allocationReconciliationStatus`, `auditReplayStatus` | `srcTruth`, `verEng`, `recValid`, `methodGov`, `gefGov`, `allocRecon`, `replay` |
| `auditAssurance.controlEvidence` | `schemaVersion`, `governanceTopicId`, `mintEventsTopicId`, `methodScopeHcsSequence`, `passportHcsSequence`, `confirmationHcsSequence` | `schemaVer`, `govTopic`, `mintTopic`, `methodSeq`, `passportSeq`, `confirmSeq` |
| `issuedUnit` | `assetType`, `claimType`, `quantityMgCO2e`, `quantityKgCO2e`, `unit`, `tokenId`, `serial`, `htsTxId`, `lifecycleStatus`, `retirementStatus` | `asset`, `claim`, `qtyMg`, `qtyKg`, `u`, `tok`, `ser`, `htsTx`, `life`, `retire` |
| `parentRecEvidence` | `claimId`, `tokenId`, `serial`, `finalHcsTopicId`, `finalHcsSequence` | `claim`, `tok`, `ser`, `topic`, `seq` |
| `allocations` | allocation and parent REC row fields | `ent`, `recClaim`, `consMg`, `kwh`, `recTok`, `recSer`, `recTopic`, `recSeq` |

Abbreviations must not change the dependency chain, collapse CO2 evidence into telemetry evidence, or reintroduce a direct CO2-level `telemetryTopicId`.

The local reconstruction helper is `tools/record_reconstruction_tool.js`. It converts records between canonical and abbreviated key forms:

```text
tools\record_reconstruction_tool.cmd --to canonical input.json output.canonical.json
tools\record_reconstruction_tool.cmd --to abbreviated input.json output.abbrev.json
tools\record_reconstruction_tool.cmd --map
```

The helper maps names only. It does not invent evidence values.

Sample records are provided in:

```text
examples/canonical_passport_sample.json
examples/abbreviated_passport_sample.json
```

The abbreviated sample reconstructs back to the canonical sample exactly.

## 8. Deployment package implementation

The abbreviated passport profile is included as a registry-layer module:

```text
REGISTRY-RECONSTRUCTION-MODULE
```

The machine-readable map is:

```text
DeploymentPackage/AbbreviationProfile.json
```

The module defines the `AbbreviatedPassportProfileOptions` template and the
`Registry Passport Reconstruction Message Pair`. It is for lossless
canonicalization/abbreviation and quasi-verification only; it does not mint,
re-sign, default evidence values, or introduce direct CO2-level telemetry fields.
