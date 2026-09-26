# CO2 Regulatory Passport  -  Field Reduction Addendum (Production Readiness)

**Status:** Analysis + proposal. No production file has been modified to produce this document. Reviewed once by a separate coding agent (2026-07-12); corrections from that review are folded in below.
**Supersedes-in-part:** the field-level status table in `VRE-PARFE-ContinuousCO2e-SPEC.md` Section 3, which predates the live `SCHEMAS_V2.3.js` lock referenced below.
**Schema authority checked against:** `C:\Users\Admin\Desktop\LIVE-WORKING-REPO-2026-07-03\SCHEMAS_V2.3.js` (`CO2_REGULATORY_PASSPORT`, lines 302-312).
**Sample analyzed:** live `CO2_REGULATORY_PASSPORT` payload, HCS topic `0.0.8585272`, sequences `403228..403232`, `co2ClaimId PC-MU-CO2-1783864267761`.
**Relationship to Phase 5:** Mission Control already shipped a key-renaming-only observe mode for this message type  -  `MC_CO2_PASSPORT_ABBREVIATION_MODE=off|observe|replace` in `MISSIONCONTROL_LIVE.js` (`docs/implementation/IMPLEMENTATION-RUNBOOK-MISSION-CONTROL-PHASE-5-CO2-PASSPORT-ABBREVIATION-2026-07-11.md`, completed 2026-07-12), using the same scoped alias map as `AbbreviationProfile.json`. Measured savings:

- canonical sample fixture: `2379`->`1860` bytes
- live passport: `4743`->`4224` bytes (`10.94%`)
- live confirmation: `3491`->`2980` bytes (`14.64%`)

Phase 5 only renames the five canonical envelope keys shorter; it does not touch nested field content. This addendum is the basis for **Phase 6**: content-level trimming (dedup/null/derivable-field removal) inside those envelopes, staged the same way Phase 5 was  -  default-off, observe mode first, no canonical HCS write changes until proven.

**Closure note, 2026-07-27:** The repo-doc mismatch identified below has been closed in the local submission package. `VRE-PARFE-ContinuousCO2e-SPEC.md` now documents the full 21-key passport object, `examples/canonical_passport_sample.json` now contains the complete top-level field set, `AbbreviationProfile.json` covers the longer live-shape keys, and `VariableTemplates.json` now includes the passport-level checklist. Phase 6 remains a post-launch optimization track, not a silent rewrite of the canonical builder output.

---

## 0. The governing constraint (read this first)

`SCHEMAS_V2.3.js` is imported by all four protocol layers (Edge Notary, Spine, Mission Control, Marketplace) and its `validateSchema()` throws on any missing `required` key or any present `forbidden` key. The locked contract for this message type is:

```js
CO2_REGULATORY_PASSPORT: {
    version:   '2.5',
    required:  ['type', 'assetType', 'co2ClaimId', 'lotId', 'scopeKey',
                'claim', 'accounting', 'parentRecEvidence', 'gefScope',
                'methodScope', 'standardEnvelope', 'externalMethodology',
                'continuousVerification', 'auditAssurance', 'issuedUnit',
                'allocations', 'netCo2eMg', 'tonneLotMgCO2e',
                'status', 'ts', 'meta'],
    forbidden: ['telemetry', 'rawTelemetry', 'mc_co2_offsets',
                'legacyCo2Offset', 'mutable', 'balance']
}
```

**Critical mechanical fact:** `validateSchema()` only checks `field in payload` at the **top level**. It never inspects what's inside `claim`, `standardEnvelope`, `issuedUnit`, `accounting`, `gefScope`, `methodScope`, `auditAssurance`, `parentRecEvidence`, or `allocations`. This means:

- Every top-level key in the `required` list above **must stay present**, or every layer that validates this message type breaks.
- The **contents** of each of those objects/arrays are unconstrained by the schema lock. Nested field trimming, merging, or pointer-izing costs nothing at the schema-validation level and needs no version bump.

That single fact reclassifies almost everything flagged in the earlier field-by-field review from "needs a schema change" to "safe to do today." Only one item in this passport actually requires a version bump  -  see Section 2.

**Caution  -  validation-safe is not the same as production-safe.** "No `SCHEMAS_V2.x` change needed" only describes what `validateSchema()` checks. Registry rendering, Marketplace display, public audit views, and future forensic replay may read nested fields that this addendum proposes dropping (e.g. `issuedUnit.assetType`, `auditAssurance.controlEvidence.governanceTopicId`) even though `validateSchema()` never required them. Passing schema validation is necessary, not sufficient, for a nested trim to be safe to ship. Treat every Section 3 change below as a **Phase 6 compact profile change**, not a silent edit to the existing canonical builder output  -  see Section 5.

**Housekeeping note (not blocking):** the file's own docblock header still says "SCHEMAS V2.1" (line 4) while `PROTOCOL_VERSION` (line 46) and the filename say `V2.3`, and the version history in the same docblock runs to `V2.6`. Worth a one-line fix in a separate commit so the header stops lying about the file's own version.

---

## 1. Repo docs are stale against the live contract  -  fix first

`VRE-PARFE-ContinuousCO2e-SPEC.md` Section 3 and `examples/canonical_passport_sample.json` describe a **five-envelope model**: `standardEnvelope`, `externalMethodology`, `continuousVerification`, `auditAssurance`, `issuedUnit` (+ `parentRecEvidence`, `allocations`)  -  7 of the schema's 21 required top-level keys. Neither document mentions the other 14: `type`, `assetType`, `co2ClaimId`, `lotId`, `scopeKey`, `claim`, `accounting`, `gefScope`, `methodScope`, `netCo2eMg`, `tonneLotMgCO2e`, `status`, `ts`, `meta`.

Of those 14, 5 are short scalar names that don't need shortening (`type`, `assetType`, `status`, `ts`, `meta`) but still need documenting so the spec shape is accurate. The remaining 9 are structured objects/values genuinely worth an abbreviation alias: `claim`, `accounting`, `gefScope`, `methodScope`, `co2ClaimId`, `lotId`, `scopeKey`, `netCo2eMg`, `tonneLotMgCO2e`.

Consequence: `AbbreviationProfile.json` has no aliases for any of those 9 structured keys, so the reconstruction tool currently passes them through unabbreviated  -  silently correct today (the reconstruction requirement explicitly says "preserve unknown fields"), but it means the abbreviation coverage is incomplete relative to what's actually on the wire, and anyone reading the SPEC doc to understand "the passport shape" gets a shape that doesn't match what `validateSchema()` will accept.

**Closed before IWA review:** SPEC.md Section 3 and `canonical_passport_sample.json` have been updated to the real 21-key top-level shape, and `AbbreviationProfile.json` now has scoped aliases for the 9 structured keys above. (Table 3 below gives the field lists used for this closure.)

Note the SPEC doc already flags the root cause at line 44: `methodologySystem`/`methodologyAuthority`/`methodologyId`/`methodologyVersion` are "Present, scattered across `claim`/`methodScope`"  -  the spec authors already identified `standardEnvelope` as the intended single owner. Section 3 below acts on that.

**Resolved (was an open question):** `meta` is present on the wire; the earlier markdown rendering just omitted it. Confirmed contents:

```json
"meta": {
  "authorityRef": "0.0.8411690",
  "issuer": "Three T's (Mauritius) Limited",
  "registrationNo": "C19166743",
  "network": "testnet"
}
```

This resolves the open item  -  see Section 3.8 for the reduction treatment of these specific fields.

---

## 2. Tier B  -  the one item that needs an actual schema version bump

`netCo2eMg` and `tonneLotMgCO2e` are **both independently required top-level keys**, and in the live sample both carry the identical value `1000000000`. This is the only true duplicate the schema itself mandates, so dropping either one requires a `required` array edit  -  i.e. a new schema version.

Two options, not mutually exclusive:

1. **Keep both, reframe as a cheap integrity check.** Two independently-set copies of the same number, written by the same builder call, catch a real class of bug (partial-write, mismatched unit conversion) for the cost of one int64 field. Not worth a version bump just to save 8-16 bytes. Recommended default.
2. **If you do want to consolidate**, propose it additively per the file's own versioning discipline (each version in the history adds fields, never silently removes/renames  -  see V2.0's note "preserves all V1.0 field names exactly"): add `CO2_REGULATORY_PASSPORT` `version: '2.7'` with `tonneLotMgCO2e` moved out of `required` and kept as an optional deprecated mirror for one release, then dropped from `required` entirely in `2.8`. Coordinate the `2.7` required-array edit across Edge Notary / Spine / Mission Control / Marketplace before any builder stops emitting the field, since removing it from the payload before the schema stops requiring it will throw at `validateSchema()`.

Everything else below is Tier A.

---

## 3. Tier A  -  validation-safe; ship as Phase 6 compact profile, not a canonical rewrite

Grouped by the top-level key that stays present; only the **contents** shrink. "No schema-file change needed" below means `validateSchema()` won't reject it  -  it does not mean skip observe mode. Every item here is a candidate for a **compact passport profile**, derived alongside the existing canonical payload and reconstructed back to canonical for comparison, exactly as Phase 5 did for key renaming. Do not implement any of Section 3.1-3.6 as a direct edit to the current canonical builder output.

### 3.1 `claim`
| Field | Action | Why |
|---|---|---|
| `methodologySystem`, `methodologyAuthority`, `methodologyId`, `methodologyVersion` | **Drop** | Exact duplicates of `standardEnvelope.*`; SPEC.md already names `standardEnvelope` as the intended owner |
| `validationRefCodes` | **Drop** | Exact duplicate of `externalMethodology.validationRefCodes` |
| `factorVintageYear`, `factorDocumentRef`, `validationStatus`, `verificationStatus` | **Omit key when null** | Currently sent as explicit `null` on every message; add only once populated |
| `claimType`, `ttfAlignment`, `marketAlignment`, `article64Applicability`, `claimUse` | Keep | Genuine instance data |

### 3.2 `standardEnvelope` (becomes the single owner of methodology identity)
No fields removed here  -  this object already correctly holds the canonical `methodologySystem/Authority/Id/Version`. It's the *target* that 3.1 and 3.3 stop duplicating.

### 3.3 `externalMethodology`
| Field | Action | Why |
|---|---|---|
| `documentVersion`, `documentRef`, `documentHash` | **Omit key when null** | Not yet populated; add once a source document is hashed |
| `authority`, `methodologyId`, `methodologyName`, `purpose`, `dependencyStatus` | Keep as-is now; candidate for methodology-spec pointer (Section 3.7) once that lands | Static per `(methodologyId, methodologyVersion)` but genuinely belongs to this object today |

### 3.4 `issuedUnit`
| Field | Action | Why |
|---|---|---|
| `assetType` | **Drop** | Duplicate of top-level `assetType` (schema-required key) |
| `claimType` | **Drop** | Duplicate of `claim.claimType` |
| `quantityKgCO2e` | **Drop** | `= quantityMgCO2e / 1e6`; a formatting choice, not data  -  derive at read time |
| `unit` | **Drop** | Fully implied by top-level `assetType: CO2_TONNE_LOT` |
| `tokenId`, `serial`, `htsTxId` | **Omit key when null** | Chicken-and-egg with mint timing; add once `CO2_OFFSET_LOT_CONFIRMATION`/mint has happened rather than pre-declaring empty |
| `quantityMgCO2e`, `lifecycleStatus`, `retirementStatus` | Keep | Genuine instance state |

### 3.5 `auditAssurance.controlEvidence`
| Field | Action | Why |
|---|---|---|
| `mintEventsTopicId` | **Drop** | Self-evident: it is the topic this very message is submitted to. Never needs to travel inside the message body |
| `governanceTopicId` | **Drop** | Duplicate of `methodScope.hcsTopicId` |
| `methodScopeHcsSequence` | **Drop** | Duplicate of `methodScope.hcsSequence` |
| `passportHcsSequence` | **Drop** | Unknowable at write time (this message doesn't know its own future sequence number)  -  always null by construction, not just "not yet populated". Let `CO2_OFFSET_LOT_CONFIRMATION` carry the back-reference instead, as it already does per `ClaimSources.json` |
| `confirmationHcsSequence` | **Omit key when null** | Genuinely unknown until confirmation is written; same treatment as other not-yet-known fields |
| `schemaVersion` | Keep | Real, load-bearing provenance |

### 3.6 `parentRecEvidence` + `allocations`
Both top-level keys must stay present (schema-required), but `allocations` already carries `parentRecClaimId`, and every row currently repeats the full `parentRecEvidence` row (token, serial, final topic, final sequence) a second time inside `allocations`. Consolidate the direction of duplication:

- `parentRecEvidence`: keep as identity-only  -  `claimId`, `tokenId` (hoist to a single shared field if, as in the sample, one token covers all rows).
- `allocations`: carries the replay coordinates once  -  `parentRecClaimId`, `consumedMgCO2e`, `sourceKwh`, `parentRecSerial`, `parentRecFinalHcsTopicId`, `parentRecFinalHcsSequence`.

Net: same two required keys present, same information, one fewer full copy of the REC pointer per row.

### 3.7 `standardEnvelope` / `externalMethodology` (static part) / `continuousVerification` (static part) / `auditAssurance` (all)  -  the big lever
For a fixed `(methodologyId, methodologyVersion)` pair, these fields are constant across **every** passport issued under that methodology  -  confirmed by your own `AimFixedVariables.json`, which already labels most of them `"Fixed for this instance"` or `"Proposed; not present in live data"`:

- `standardEnvelope`: `standardFamily`, `standardVersion`, `extensionSetId`, `extensionSetVersion`, `qualityStandardRole` (5 of 9  -  `methodologySystem/Authority/Id/Version` stay, they're the lookup key)
- `externalMethodology`: `authority`, `methodologyId`, `methodologyName`, `purpose`, `dependencyStatus` (5 of 9)
- `continuousVerification`: `mode`, `sourceOfTruth`, `verificationEngine` (3 of 8  -  the 5 PASS/FAIL gates stay, they're genuine per-instance results)
- `auditAssurance`: `assuranceModel`, `auditorRole`, `vvbDependency`, `vvbUseWhereRequired` (all 4)

Proposal: publish these ~17 fields once as a governed methodology-spec record on the governance topic (same pattern already used for `CO2_METHOD_SCOPE_GRANT`), and shrink each passport's copy of `standardEnvelope`/`externalMethodology`/`auditAssurance` to just the lookup key plus a spec pointer, e.g.:

```json
"standardEnvelope": {
  "methodologySystem": "VRE_PARFE_SOVEREIGN_DMRV",
  "methodologyAuthority": "VRE_PARFE",
  "methodologyId": "VP-CO2E-RE-GRID-001",
  "methodologyVersion": "V1",
  "specRef": "hedera:hcs:1:0.0.8479702#<methodology-spec-seq>"
}
```

The passport side needs **no `SCHEMAS_V2.x` edit**  -  `standardEnvelope`, `externalMethodology`, and `auditAssurance` remain present as required top-level keys on `CO2_REGULATORY_PASSPORT`; only their internal shape shrinks. This is the single largest byte-count reduction available and is primarily a Mission Control payload-builder change (`buildCo2RegulatoryPassportPayload` per `ClaimSources.json`).

**Caution:** the *governance* side of this is not schema-free. Publishing the methodology-spec record itself is a new message type on the governance topic. If Spine validates messages before HCS submit  -  which it does for every other governance record, per `SCHEMAS_V2.3.js`'s own rule "No message type is defined more than once" and the existing `CO2_METHOD_SCOPE_GRANT`/`MINTING_POLICY_GRANT` precedents  -  then this spec record needs its own additive entry in `SCHEMAS_V2.3.js` (e.g. `CO2_METHODOLOGY_SPEC_GRANT`) with its own `required`/`forbidden` arrays, the same way `CO2_METHOD_SCOPE_GRANT` and `MINTING_POLICY_GRANT` were each added additively in V2.4/V2.6. Non-breaking, additive, consistent with existing practice  -  but real schema work, not "zero schema change." Scope this as its own line item, separate from the passport-side trims in Section 3.1-3.6.

**Outcome (2026-07-13):** superseded by Mission Control Phase 7 (`docs/implementation/IMPLEMENTATION-RUNBOOK-MISSION-CONTROL-PHASE-7-CO2-PASSPORT-GOVERNANCE-REFERENCE-V3-2026-07-13.md`, `MC_CO2_COMPACT_PASSPORT_PROFILE=v3_observe|v3_replace`), which went further than this section's `specRef`-pointer proposal: instead of publishing a new governed methodology-spec record, it drops these fields from the wire packet (`CO2P3`/`CO2C3`) entirely and reconstructs them at audit time. Verified against the actual implementation (`registry-live.js`, `buildCo2StaticProtocolProfile`):

- `standardEnvelope.methodologySystem/Authority/Id/Version` are resolved live from the governed `CO2_METHOD_SCOPE_GRANT` record via HCS traversal (`resolveHcsMessage`)  -  genuinely HCS-anchored, no gap.
- `standardFamily`, `standardVersion`, `extensionSetId`, `extensionSetVersion`, `qualityStandardRole`, `assuranceModel`, `auditorRole`, `vvbDependency`, `vvbUseWhereRequired` remain hardcoded protocol constants with no HCS anchor of their own (confirmed: absent from `CO2_METHOD_SCOPE_GRANT`'s required fields too)  -  the same gap this section originally flagged. Phase 7 closes it not by anchoring these fields, but by disclosing them: the registry UI labels this block's provenance explicitly as `"Static protocol approvals bound by Spine governance onboarding..."` rather than presenting it as per-issuance HCS evidence. The optional `specRef`-style anchor proposed above is left in the Phase 7 doc as a documented future option, not implemented now.

This section's `SCHEMAS_V2.x` caution still applies if that optional anchor is ever built.

### 3.8 `meta`
| Field | Action | Why |
|---|---|---|
| `issuer`, `registrationNo` | Candidate for pointer treatment (same pattern as Section 3.7) | Static attributes of the entity behind `authorityRef`; a registry lookup on `0.0.8411690` recovers them without restating full legal name/reg number on every passport |
| `network` | Candidate to drop | Implicit in whichever Hedera network/topic the reader is already querying; not meaningful as in-message data |
| `authorityRef` | Keep | The actual pointer; everything else here is resolvable from it |

Lower priority than Section 3.7  -  smaller byte count, and `issuer`/`registrationNo` are the kind of field a regulator or auditor may expect to see inline without a lookup hop, so confirm with compliance before treating this as a pure trim.

---

## 4. Net effect

| | Count |
|---|---|
| Top-level required keys (schema-locked, all must stay present) | 21 |
| Of those, needing an actual schema version bump to change | 1 (`netCo2eMg`/`tonneLotMgCO2e`, and only if you choose to consolidate  -  see Section 2) |
| Validation-safe candidates with zero passport schema change | ~35-40 nested fields, concentrated in `claim`, `issuedUnit`, `auditAssurance.controlEvidence`, `parentRecEvidence`, and the static portions of `standardEnvelope`/`externalMethodology`/`auditAssurance`  -  still requires Phase 6 observe-mode proof before shipping, per the caution in Section 0 |

Realistic outcome: the wire payload shrinks by roughly the same 40-50% estimated in the prior pass, mostly from Section 3.1-3.6 (Mission Control builder change, no schema edit) plus Section 3.7 (one additive governance schema entry for the methodology-spec record). These are **projected observe-mode measurements**, not committed production changes  -  Phase 5's own numbers (`2379`->`1860` bytes, key-renaming only) came from exactly this kind of instrumented proof before any HCS write behavior changed, and Phase 6 should produce the equivalent measurement for content-level trimming before anything is decided.

---

## 5. Suggested sequencing  -  Phase 6, observe mode first

Frame this entire addendum as the design input for a **Phase 6** that follows the Phase 5 pattern exactly: default-off, observe-mode proof against real payloads, no canonical HCS write changes until the proof is reviewed and a separate decision is made to go further.

1. Reconcile `VRE-PARFE-ContinuousCO2e-SPEC.md` Section 3 and `canonical_passport_sample.json` against the real 21-key shape; extend `AbbreviationProfile.json` and `VariableTemplates.json` to cover `claim`, `accounting`, `gefScope`, `methodScope`, `co2ClaimId`, `lotId`, `scopeKey`, `netCo2eMg`, `tonneLotMgCO2e`. (`meta` is now confirmed present  -  no action needed there beyond what Section 3.8 proposes.)
2. Add a new flag alongside the existing `MC_CO2_PASSPORT_ABBREVIATION_MODE`, matching Mission Control's naming: `MC_CO2_COMPACT_PASSPORT_PROFILE=off|observe|replace`, defaulting to `off`. Keep it distinct from the Phase 5 flag so key-renaming and content-trimming can be toggled independently.
3. In `observe` mode: build the canonical payload and validate it as today (unchanged submit path); separately derive a **compact-v2 view** applying Section 3.1-3.6; reconstruct it back to canonical; assert the round-trip; record byte-size savings and round-trip status the same way Phase 5's `co2Regime.co2PassportAbbreviation` health block does (e.g. `co2Regime.co2CompactPassport.lastCanonicalBytes/lastCompactBytes/lastSavingsPct/lastRoundTripOk`). HCS writes remain canonical throughout.
4. Run observe mode against the same live passport and confirmation payloads Phase 5 used for its proof, so the two measurements are comparable.
5. Design the methodology-spec governance record for Section 3.7 separately (it has its own additive-schema line item now  -  see the caution in Section 3.7) and prove it independently before folding it into the same compact-v2 view.
6. Only after compact-v2 round-trip is proven from real emitted payloads  -  same bar Phase 5 set for itself  -  decide whether `replace` mode belongs in the current pre-production workspace or should wait for the clean testnet-production fresh start, consistent with Phase 5's own stated preference.
7. Decide on Section 2 (`netCo2eMg`/`tonneLotMgCO2e`) last and separately, since it's optional, low-value, and the only item touching the frozen `required` array.
