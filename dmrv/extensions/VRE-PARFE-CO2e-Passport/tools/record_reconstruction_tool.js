#!/usr/bin/env node
"use strict";

const fs = require("fs");

const rootCanonicalToAbbreviated = {
  co2ClaimId: "co2Claim",
  lotId: "lot",
  scopeKey: "scope",
  claim: "clm",
  accounting: "acct",
  gefScope: "gef",
  methodScope: "method",
  standardEnvelope: "std",
  externalMethodology: "extMethod",
  continuousVerification: "cv",
  auditAssurance: "audit",
  issuedUnit: "unit",
  parentRecEvidence: "parentRec",
  allocations: "alloc",
  netCo2eMg: "netMg",
  tonneLotMgCO2e: "lotMg"
};

const scopedCanonicalToAbbreviated = {
  claim: {
    claimType: "type",
    methodologySystem: "methSys",
    methodologyAuthority: "methAuth",
    ttfAlignment: "ttf",
    marketAlignment: "market",
    article64Applicability: "art64",
    validationRefCodes: "valRefs",
    claimUse: "use",
    methodologyId: "methId",
    methodologyVersion: "methVer",
    factorVintageYear: "factorYear",
    factorDocumentRef: "factorRef",
    validationStatus: "valStatus",
    verificationStatus: "verStatus"
  },
  accounting: {
    accountingBoundary: "boundary",
    calculationMethod: "calc",
    baselineScenario: "baseline",
    baselineEmissionsTreatment: "baseEmis",
    projectEmissionsTreatment: "projEmis",
    leakageTreatment: "leakage",
    deductionsTreatment: "deduct",
    uncertaintyTreatment: "uncert",
    nettingRule: "netRule",
    monitoringPlanRef: "monPlan"
  },
  gefScope: {
    grantId: "grant",
    methodologyId: "methId",
    methodologyVersion: "methVer",
    gefValueKgPerKwh: "kgPerKwh",
    sourceRef: "srcRef",
    sourceType: "srcType",
    regionCode: "region",
    jurisdiction: "jur"
  },
  methodScope: {
    methodScopeId: "methodId",
    hcsTopicId: "topic",
    hcsSequence: "seq",
    projectRef: "project",
    technologyType: "tech",
    status: "state"
  },
  standardEnvelope: {
    standardFamily: "sf",
    standardVersion: "sv",
    extensionSetId: "esId",
    extensionSetVersion: "esVer",
    methodologySystem: "methSys",
    methodologyAuthority: "methAuth",
    methodologyId: "methId",
    methodologyVersion: "methVer",
    qualityStandardRole: "qsRole"
  },
  externalMethodology: {
    authority: "extAuth",
    externalMethodologyAuthority: "extAuth",
    methodologyId: "extId",
    externalMethodologyId: "extId",
    methodologyName: "extName",
    externalMethodologyName: "extName",
    documentVersion: "docVer",
    documentRef: "docRef",
    documentHash: "docHash",
    validationRefCodes: "valRefs",
    purpose: "purp",
    dependencyStatus: "depStatus"
  },
  continuousVerification: {
    sourceOfTruth: "srcTruth",
    verificationEngine: "verEng",
    recEvidenceValidityStatus: "recValid",
    methodologyGovernanceStatus: "methodGov",
    gefScopeGovernanceStatus: "gefGov",
    allocationReconciliationStatus: "allocRecon",
    auditReplayStatus: "replay"
  },
  auditAssurance: {
    assuranceModel: "assrModel",
    auditorRole: "audRole",
    vvbDependency: "vvbDep",
    vvbUseWhereRequired: "vvbUse",
    controlEvidence: "ctrl"
  },
  controlEvidence: {
    schemaVersion: "schemaVer",
    governanceTopicId: "govTopic",
    mintEventsTopicId: "mintTopic",
    methodScopeHcsSequence: "methodSeq",
    passportHcsSequence: "passportSeq",
    confirmationHcsSequence: "confirmSeq"
  },
  issuedUnit: {
    assetType: "asset",
    claimType: "claim",
    quantityMgCO2e: "qtyMg",
    quantityKgCO2e: "qtyKg",
    unit: "u",
    tokenId: "tok",
    serial: "ser",
    htsTxId: "htsTx",
    lifecycleStatus: "life",
    retirementStatus: "retire"
  },
  parentRecEvidence: {
    claimId: "claim",
    tokenId: "tok",
    serial: "ser",
    finalHcsTopicId: "topic",
    finalHcsSequence: "seq"
  },
  allocations: {
    entitlementId: "ent",
    parentRecClaimId: "recClaim",
    consumedMgCO2e: "consMg",
    sourceKwh: "kwh",
    parentRecTokenId: "recTok",
    parentRecSerial: "recSer",
    parentRecFinalHcsTopicId: "recTopic",
    parentRecFinalHcsSequence: "recSeq"
  }
};

function invertFirstWins(map) {
  const output = {};
  for (const [canonical, abbreviated] of Object.entries(map)) {
    if (!Object.prototype.hasOwnProperty.call(output, abbreviated)) {
      output[abbreviated] = canonical;
    }
  }
  return output;
}

const scopedAbbreviatedToCanonical = Object.fromEntries(
  Object.entries(scopedCanonicalToAbbreviated).map(([scope, map]) => [
    scope,
    invertFirstWins(map)
  ])
);

const rootAbbreviatedToCanonical = Object.fromEntries(
  Object.entries(rootCanonicalToAbbreviated).map(([canonical, abbreviated]) => [
    abbreviated,
    canonical
  ])
);

const rootContextByKey = new Map();
for (const [canonical, abbreviated] of Object.entries(rootCanonicalToAbbreviated)) {
  rootContextByKey.set(canonical, canonical);
  rootContextByKey.set(abbreviated, canonical);
}

const childContextByKey = new Map([
  ["controlEvidence", "controlEvidence"],
  ["ctrl", "controlEvidence"]
]);

const forbiddenTelemetryKeys = new Set([
  "telemetryTopicId",
  "telemetry_topic_id",
  "telemetryTopic",
  "telemetry_topic"
]);

function usage(exitCode = 0) {
  const text = [
    "Usage:",
    "  tools\\record_reconstruction_tool.cmd --to canonical input.json [output.json]",
    "  tools\\record_reconstruction_tool.cmd --to abbreviated input.json [output.json]",
    "  tools\\record_reconstruction_tool.cmd --map",
    "",
    "The tool maps known canonical/abbreviated keys, preserves unknown fields,",
    "and applies inner aliases only inside their owning envelope scope."
  ].join("\n");
  (exitCode === 0 ? console.log : console.error)(text);
  process.exit(exitCode);
}

function printMap() {
  const rows = [];
  for (const [canonical, abbreviated] of Object.entries(rootCanonicalToAbbreviated)) {
    rows.push({ scope: "$", canonical, abbreviated });
  }
  for (const [scope, map] of Object.entries(scopedCanonicalToAbbreviated)) {
    for (const [canonical, abbreviated] of Object.entries(map)) {
      rows.push({ scope, canonical, abbreviated });
    }
  }
  console.log(JSON.stringify(rows, null, 2));
}

function nextContextForKey(path, key, mappedKey, currentContext) {
  if (path === "$") {
    return rootContextByKey.get(key) || rootContextByKey.get(mappedKey) || currentContext;
  }
  return childContextByKey.get(key) || childContextByKey.get(mappedKey) || currentContext;
}

function transformKeys(value, rootKeyMap, scopedKeyMaps, warnings, path = "$", context = null) {
  if (Array.isArray(value)) {
    return value.map((item, index) =>
      transformKeys(item, rootKeyMap, scopedKeyMaps, warnings, `${path}[${index}]`, context)
    );
  }

  if (!value || typeof value !== "object") {
    return value;
  }

  const output = {};
  const scopedMap = context ? scopedKeyMaps[context] || {} : {};

  for (const [key, child] of Object.entries(value)) {
    if (forbiddenTelemetryKeys.has(key)) {
      warnings.push(
        `${path}.${key}: CO2-level telemetry coordinates are forbidden; traverse parent REC evidence instead.`
      );
    }

    const mappedKey = path === "$" ? rootKeyMap[key] || key : scopedMap[key] || key;
    const nextContext = nextContextForKey(path, key, mappedKey, context);

    if (Object.prototype.hasOwnProperty.call(output, mappedKey)) {
      warnings.push(
        `${path}.${key}: mapped key collides with another field named ${mappedKey}; preserve the canonical source.`
      );
    }

    output[mappedKey] = transformKeys(
      child,
      rootKeyMap,
      scopedKeyMaps,
      warnings,
      `${path}.${mappedKey}`,
      nextContext
    );
  }
  return output;
}

function readJson(path) {
  try {
    return JSON.parse(fs.readFileSync(path, "utf8"));
  } catch (error) {
    throw new Error(`Failed to read JSON from ${path}: ${error.message}`);
  }
}

function writeJson(path, data) {
  const json = `${JSON.stringify(data, null, 2)}\n`;
  if (path) {
    fs.writeFileSync(path, json, "utf8");
  } else {
    process.stdout.write(json);
  }
}

function main(argv) {
  if (argv.includes("--help") || argv.includes("-h")) {
    usage(0);
  }

  if (argv.includes("--map")) {
    printMap();
    return;
  }

  const toIndex = argv.indexOf("--to");
  if (toIndex === -1 || !argv[toIndex + 1]) {
    usage(1);
  }

  const target = argv[toIndex + 1];
  const inputPath = argv[toIndex + 2];
  const outputPath = argv[toIndex + 3];

  if (!inputPath) {
    usage(1);
  }

  const maps =
    target === "canonical"
      ? [rootAbbreviatedToCanonical, scopedAbbreviatedToCanonical]
      : target === "abbreviated"
        ? [rootCanonicalToAbbreviated, scopedCanonicalToAbbreviated]
        : null;

  if (!maps) {
    throw new Error(`Unsupported target "${target}". Use "canonical" or "abbreviated".`);
  }

  const warnings = [];
  const input = readJson(inputPath);
  const output = transformKeys(input, maps[0], maps[1], warnings);

  for (const warning of warnings) {
    console.error(`WARNING: ${warning}`);
  }

  writeJson(outputPath, output);
}

try {
  main(process.argv.slice(2));
} catch (error) {
  console.error(error.message);
  process.exit(1);
}
