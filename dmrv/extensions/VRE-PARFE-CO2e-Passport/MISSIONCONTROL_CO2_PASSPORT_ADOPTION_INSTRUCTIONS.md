# Mission Control Instruction: Adopt Vre Parfe CO2 Passport Format

## Current local decision log

- Public IWA extension set name: `VRE-PARFE-CO2e-Passport`.
- Current local package/schema ID: `VRE-PARFE-ContinuousCO2e`.
- Submitted text uses ASCII-safe `Vre Parfe`.
- Files will be manually selected for GitHub upload; local git status is not part of the package readiness check.
- The local package has passed JSON parse, embedded `dataExample` parse, non-ASCII scan, and trailing-whitespace scan.
- Next stage after this canonical adoption instruction: define abbreviated forms as mapped aliases of the canonical five-envelope model.

## Objective

Adopt the corrected `VRE-PARFE-ContinuousCO2e` extension set completely as the canonical **Vre Parfe CO2 Passport** format in `MISSIONCONTROL_LIVE.js`.

This is not a telemetry-first CO2 schema. The binding dependency chain is:

```text
Telemetry -> REC issuance -> REC allocation -> CO2 methodology -> CO2 passport / HTS unit
```

CO2 depends directly on **issued REC evidence**, governed CO2 methodology scope, governed GEF/emission-factor scope, allocation reconciliation, HCS replayability, and HTS lifecycle state. Raw telemetry remains reachable by traversing from CO2 into each parent REC passport, then from the REC passport into telemetry. Do not model raw telemetry as a direct CO2 issuance dependency.

## Canonical Extension Set Source

Use the corrected local package as the source of truth:

```text
VRE-PARFE-ContinuousCO2e/
```

If the target repo has an older or partial CO2 extension set, replace it with this corrected extension set rather than patching the old telemetry-first draft. The corrected set is internally consistent and should be adopted as the canonical CO2 Passport package.

Do not reintroduce any of the first-draft fields that made CO2 depend directly on telemetry.

## Primary Target Files

```text
C:\Users\Admin\Desktop\LIVE-WORKING-REPO-2026-07-03\MISSIONCONTROL_LIVE.js
C:\Users\Admin\Desktop\LIVE-WORKING-REPO-2026-07-03\SCHEMAS_V2.3.js
```

The new agent must update both files. `MISSIONCONTROL_LIVE.js` must emit the corrected five-envelope CO2 Passport shape, and `SCHEMAS_V2.3.js` / `validateSchema()` must accept and require the appropriate new envelopes without requiring CO2-level `telemetryTopicId`.

Main Mission Control functions to update:

- `validateGovernedCo2TonneLotForMint`
- `buildExternalMethodologySnapshot`
- `buildCo2OffsetLotConfirmationPayload`
- `buildCo2RegulatoryPassportPayload`
- schema probe helpers for `CO2_REGULATORY_PASSPORT` and `CO2_OFFSET_LOT_CONFIRMATION`

Main schema/validator file to update:

- `SCHEMAS_V2.3.js`, including any JSON schema/validator definitions used by `validateSchema`

## Required Payload Shape

The CO2 passport and confirmation payloads must retain existing live fields for backward compatibility, but must add the corrected extension-set envelopes.

Existing fields that should remain:

- `claim`
- `accounting`
- `parentRecEvidence`
- `gefScope`
- `methodScope`
- `allocations`
- `co2Token`
- `co2Passport`
- existing IDs, timestamps, and `meta`

New required envelopes:

```js
standardEnvelope
externalMethodology
continuousVerification
auditAssurance
issuedUnit
```

These canonical envelope names should be implemented first. The later abbreviation pass may introduce aliases such as `std`, `extMethod`, `cv`, `audit`, and `unit`, but those aliases must map back to the canonical names and must not alter the CO2 -> REC -> telemetry dependency model.

## Required Envelope Details

### `standardEnvelope`

Add to `CO2_REGULATORY_PASSPORT`; also add to confirmation if the schema accepts extension envelopes there.

Required fields:

```js
{
  standardFamily: "GBBC_IWA_DMRV",
  standardVersion: "3.0",
  extensionSetId: "VRE-PARFE-ContinuousCO2e",
  extensionSetVersion: "1.0.0",
  methodologySystem: "VRE_PARFE_SOVEREIGN_DMRV",
  methodologyAuthority: "VRE_PARFE",
  methodologyId,
  methodologyVersion,
  qualityStandardRole: "SOVEREIGN_DIGITAL_DMRV_METHOD"
}
```

Use `methodScope` values where present; only use defaults where Mission Control already defaults them.

### `externalMethodology`

Mission Control emits:

- `authority`
- `methodologyId`
- `methodologyName`
- `validationRefCodes`

These values must come from governed `CO2_METHOD_SCOPE_GRANT` evidence. They must not be silently defaulted by Mission Control or Spine.

Binding rule:

- `externalMethodology.methodologyId` is separate from the sovereign Vre Parfe `methodologyId`.
- A governed method scope must explicitly declare `externalMethodology.methodologyId`.
- A governed method scope must explicitly declare non-empty `externalMethodology.validationRefCodes`.
- If these fields are absent, the method scope is incomplete and CO2 minting must hold/fail closed. Do not substitute `ACM0002`, `AMS-I.D`, `TOOL01`, or any other citation/tool by runtime default.

Extend `buildExternalMethodologySnapshot()` to also emit:

```js
documentVersion
documentRef
documentHash
purpose: "METHODOLOGY_AND_ADDITIONALITY_REFERENCE_ONLY",
dependencyStatus: "REFERENCE_ONLY_NOT_ISSUANCE_AUTHORITY"
```

Populate document fields from `methodScope.externalMethodology` or method-scope equivalents if available; otherwise use `null`.

Live proof to preserve in later implementations:

- Governance topic `0.0.8479702`, sequences `76-77`, explicitly declared `externalMethodology.methodologyId: "AMS-I.D"` and `validationRefCodes: ["ARTICLE_6_4_PRINCIPLES"]`.
- Governance sequence `78` recorded the `CO2_METHOD_SCOPE_GRANTED` audit event.
- Passport topic `0.0.8585272`, sequences `374200-374204`, and confirmation sequences `374205-374208`, then carried `AMS-I.D` and `methodScopeHcsSequence: "77"`.

### `continuousVerification`

Add a CO2-level governance/verification envelope. Do not include telemetry-layer gates.

Required fields:

```js
{
  mode: "CONTINUOUS_MONITORING_AND_VERIFICATION_VIA_REC_EVIDENCE",
  sourceOfTruth: "HCS",
  verificationEngine: "VRE_PARFE_EDGE_NOTARY_AND_HCS_REPLAY",
  recEvidenceValidityStatus,
  methodologyGovernanceStatus,
  gefScopeGovernanceStatus,
  allocationReconciliationStatus,
  auditReplayStatus
}
```

Computation requirements:

- `recEvidenceValidityStatus`: `PASSED` only if every `parentRecEvidence` entry has resolvable REC coordinates: parent claim ID, token/serial where available, `finalHcsTopicId`, and `finalHcsSequence`. If Mission Control can check non-revoked/minted state, include that check. Otherwise use `NOT_CHECKED` or `FAILED`; do not hard-code `PASSED`.
- `methodologyGovernanceStatus`: `PASSED` only if the selected method scope came from the direct HCS governance topic and is active/current.
- `gefScopeGovernanceStatus`: `PASSED` only if the GEF scope/grant used by the lot resolves from governed HCS scope and matches the lot allocation scope.
- `allocationReconciliationStatus`: `PASSED` only if `sum(allocations[].consumedMgCO2e || consumedMg) === CO2_TONNE_LOT_MG`.
- `auditReplayStatus`: `REPLAYABLE_VIA_REC` only if CO2 control evidence coordinates are present/resolvable and `recEvidenceValidityStatus === "PASSED"`. Otherwise `NOT_REPLAYABLE` or `NOT_CHECKED`.

### `auditAssurance`

Add:

```js
{
  assuranceModel: "SYSTEM_CONTROLS_AND_EVIDENCE_REPLAY",
  auditorRole: "INDEPENDENT_REPLAY_AND_CONTROL_ASSURANCE",
  vvbDependency: "NOT_REQUIRED_FOR_PREPRODUCTION_OR_AUTOMATED_ISSUANCE",
  vvbUseWhereRequired: [
    "METHODOLOGY_RECOGNITION",
    "SOVEREIGN_APPROVAL",
    "EXTERNAL_ASSURANCE",
    "ARTICLE_6_OR_COMPLIANCE_USE_IF_REQUIRED"
  ],
  controlEvidence: {
    schemaVersion: "SCHEMAS_V2_OR_LATER",
    governanceTopicId,
    mintEventsTopicId,
    methodScopeHcsSequence,
    passportHcsSequence,
    confirmationHcsSequence
  }
}
```

Important:

- Do **not** add `telemetryTopicId` to `controlEvidence`.
- Do **not** cache raw telemetry coordinates in the CO2 audit-assurance envelope.
- Traversal from CO2 to REC must use `parentRecEvidence.finalHcsTopicId` and `parentRecEvidence.finalHcsSequence`.
- Traversal from REC to telemetry belongs to the REC/HydroRE passport layer.

For the passport payload, `passportHcsSequence` may not be known until after submit. Use `null` at build time if necessary, then include it in confirmation once known.

### `issuedUnit`

For confirmation payloads, normalize the minted unit into:

```js
{
  assetType: "CO2_TONNE_LOT",
  claimType: "SOVEREIGN_CO2E_DISPLACEMENT_CLAIM",
  quantityMgCO2e,
  quantityKgCO2e,
  unit: "tCO2e",
  tokenId,
  serial,
  htsTxId,
  lifecycleStatus: "MINTED",
  retirementStatus: "NOT_RETIRED"
}
```

For passport-first payloads before HTS mint, emit `issuedUnit` with known quantity fields and `null` token fields, or omit only if the validator requires minted-unit evidence later. Preferred format is to include the envelope with pending/null token coordinates.

## Fields To Demote Or Remove As CO2 Requirements

Mission Control currently emits and guards on telemetry fields at the CO2 level. These are not normative CO2 Passport requirements:

- top-level `telemetryTopicId`
- top-level `telemetrySeqFrom`
- top-level `telemetrySeqTo`
- top-level `telemetryTopics`
- top-level `telemetryEvidence`
- `parentRecEvidence[].telemetryTopicId`
- `parentRecEvidence[].telemetrySeqFrom`
- `parentRecEvidence[].telemetrySeqTo`
- `allocations[].telemetryTopicId`
- `allocations[].telemetrySeqFrom`
- `allocations[].telemetrySeqTo`

Do not use those as CO2 issuance dependencies. If kept temporarily for legacy compatibility, mark them as non-normative legacy traversal hints and do not include them in the new extension-set envelopes.

## Required Guard Fix

Update `validateGovernedCo2TonneLotForMint()`.

Current mismatch: it treats missing telemetry topic as mint-blocking.

Correct behavior:

- Require lot identity/status/quantity/scope.
- Require allocations.
- Require allocation sum equals `CO2_TONNE_LOT_MG`.
- Require all allocations share the lot scope.
- Require parent REC evidence coordinates:
  - `entitlementId`
  - `parentRecClaimId`
  - `parentRecFinalHcsTopicId`
  - `parentRecFinalHcsSequence`
  - `parentRecTokenId` and/or `parentRecSerial` where available
- Do **not** fail CO2 mint solely because telemetry coordinates are absent.

## Required Schema Probe Fixes

Update the probe payloads used by:

- `isCo2RegulatoryPassportSchemaApproved()`
- `isCo2OffsetLotConfirmationSchemaApproved()`

They must include the new envelopes so schema approval proves the final passport shape, not only the old payload.

## Required Validator Fix

Update `SCHEMAS_V2.3.js` and the schema used by `validateSchema()` so these new envelopes are accepted and, where appropriate, required:

- `standardEnvelope`
- `externalMethodology` with extended fields
- `continuousVerification`
- `auditAssurance`
- `issuedUnit`

The validator must not require CO2-level `telemetryTopicId`.

## Recommended Helper Functions

Add small helpers rather than duplicating envelope construction:

```js
buildCo2StandardEnvelope(methodScope)
buildCo2ContinuousVerification(lot, methodScope, options)
buildCo2AuditAssurance(lot, methodScope, evidence)
buildCo2IssuedUnit(lot, tokenEvidence)
sumCo2AllocationMg(allocations)
getCo2AllocationReconciliationStatus(lot)
getCo2RecEvidenceValidityStatus(allocations)
getCo2MethodologyGovernanceStatus(methodScope)
getCo2GefScopeGovernanceStatus(lot, methodScope)
```

Keep these additive and close to the existing CO2 payload builder functions.

## Acceptance Criteria

1. `CO2_REGULATORY_PASSPORT` emits all five extension-set envelopes.
2. `CO2_OFFSET_LOT_CONFIRMATION` emits `issuedUnit`, `auditAssurance`, `continuousVerification`, and uses the same standard/external methodology model where schema permits.
3. `standardEnvelope.extensionSetId === "VRE-PARFE-ContinuousCO2e"`.
4. `continuousVerification.auditReplayStatus` is `REPLAYABLE_VIA_REC`, not `REPLAYABLE_FROM_HCS`.
5. No new CO2 envelope contains `telemetryTopicId`.
6. `validateGovernedCo2TonneLotForMint()` no longer blocks minting solely because telemetry coordinates are absent.
7. Allocation reconciliation is computed from the actual lot allocations.
8. Schema probes include the new envelopes and pass.
9. `SCHEMAS_V2.3.js` / `validateSchema()` accepts and requires the appropriate new envelopes without requiring CO2-level `telemetryTopicId`.
10. Existing live fields remain available for backward compatibility unless the project owner explicitly approves a breaking payload change.
11. `externalMethodology.methodologyId` and non-empty `externalMethodology.validationRefCodes` are required from governed method-scope evidence; missing values must not be defaulted to `ACM0002`, `AMS-I.D`, `TOOL01`, or any other citation/tool.
12. Unit tests or local probes prove:
    - a valid REC-backed lot passes,
    - a lot with missing parent REC coordinates fails,
    - a lot with bad allocation total fails,
    - a lot with missing telemetry but valid parent REC evidence does not fail for telemetry alone.
    - a method scope missing explicit external methodology fields is held as incomplete.

## Do Not Do

- Do not edit the extension set back into a telemetry-first model.
- Do not add `controlEvidence.telemetryTopicId`.
- Do not add sensor/gateway/signature/physics/fraud/sequence-integrity gates to the CO2 Passport.
- Do not hard-code `PASSED` statuses unless the corresponding guard/check actually ran.
- Do not remove `parentRecEvidence`; it is the CO2 Passport's canonical downward traversal path.

## Summary For The Implementing Agent

Critical implementation note: the new agent must update both `MISSIONCONTROL_LIVE.js` and `SCHEMAS_V2.3.js`. Mission Control must emit the corrected five-envelope CO2 Passport shape, and `SCHEMAS_V2.3.js` / `validateSchema()` must accept and require the appropriate new envelopes without requiring CO2-level `telemetryTopicId`.

Adopt `VRE-PARFE-ContinuousCO2e` as the canonical Vre Parfe CO2 Passport. Keep Mission Control's existing payload fields for compatibility, but add the corrected five-envelope extension-set shape. Fix the mismatch where Mission Control still treats telemetry as a CO2-level dependency. CO2 must depend on valid parent REC evidence, not raw telemetry.
