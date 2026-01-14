# Getting Started with the sample CCS+ Extension Set

## Introduction

This sample CCS+ Extension Set provides a partial framework for Carbon Capture and Storage (CCS) monitoring, reporting, and verification (MRV) using the Digital MRV Framework. The full Extension Set, still in development, contains 375 Variable Templates and 67 Formula Templates, so we chose a small subset for this sample for easier understanding. The main projects for the CCS+ Extension Set are:

- [DeploymentPackage](./DeploymentPackage/): This contains the Extension Set structure and templates (Entity Extensions, Variable Templates, Formula Templates, and Message Pairs) that define the data types and calculations used in the CCS+ methodology.
  - [protos](./DeploymentPackage/protos/): This contains the protocol buffer definitions for the custom data types used in the Extension Set templates.
- [InstancePackage](./InstancePackage/): This contains the instance configuration, including Impact Claim and Checkpoint templates, ClaimSources, and models/schemas for Verra's `MonitoringReport` and `VerificationReport`.

Depending on the implementation of the specification, additional development may be required to fully integrate any Extension Set for use.

## Understanding The Deployment Package and Templates

Each Extension Set Template type has a master JSON definition file, where each definition has a fixed identifier (GUID/UUID or composite Id & Version) so that definitions remain constant across implementations and projects.

All template definitions are wrappers for custom data types that can vary based on the implementation. Similar to an envelop and a letter, where the envelop is the template and the letter is the custom data type. Custom data types can be defined as simple JSON schemas, primitive data types like int or boolean, or [protocol buffers](https://protobuf.dev), the CCS+ Extension Set uses protocol buffers to support native cross platform serialization and object models. Regardless of the implementation of the custom data types, the template's DataSchemaOrType property is where the information about the custom type, i.e., a namespace to a class or path to a schema, and the DataExample property is a sample of the type serialized to a string/text. An instance of a template has a corresponding DataSchemaOrType -> DataType and DataExample -> Data.

### Entity Extension Templates

[EntityExtensionTemplates.json](./DeploymentPackage/EntityExtensionTemplates.json): these are the most generic extension that can be defined, it allows for any ECS entity to have a data extension and they have fixed identifiers. When installing the templates, Entity Extensions should be installed first.

```json
{
    "id": "1a00284e-6cd1-486f-8630-91b58efae4c4",
    "name": "VM0049 Options",
    "version": "1.0.0",
    "description": "This extension records the VM0049 options that will apply to all formula and variables for the module that are specific to each option.",
    "documentation": "",
    "extensionContext": "ExtensionSet",
    "dataSchemaOrType": "ccs.extensionSet.verra.VM0049Options",
    "dataExample": "{ \"cO2MeasurementOption\": \"MassFlow\"}",
    "modules": [
    "VM0049"
    ]
}
```

The `dataSchemaOrType` property is defined as:

```proto
message VM0049Options{
  CO2MeasurementOption co2_measurement_option = 1; //Option for measuring CO2 in the captured gas stream
}

//Verra options start with 1, so the default value of 0 can be used to indicate "not set".
enum CO2MeasurementOption{
    NOT_APPLICABLE = 0; //CO2 measurement does not apply.
    MASS_FLOW = 1; //Measurement of total mass flow rate of the captured gas stream and CO2 concentration.
    VOLUMETRIC_FLOW = 2; //Measurement of total volumetric flow rate of the captured gas stream and CO2 concentration.
    BOTH_MASS_AND_VOLUMETRIC_FLOW = 3; //If set for a variable, it is used in both mass flow and volumetric flow measurement methods. e.g., Xk, XCO2
}
```

### Variable Templates

[Variable Templates](./DeplomentPackage/VariableTemplates.json): Variable templates are used to define the methodology module parameters that are required for reporting and formula calculations. These have fixed identifiers and should be installed second. VariableTemplates represent the variables or parameters that are required for monitoring and reporting in a methodology module and are the most frequently used template type. They are use in defining ClaimSources and AIM Fixed Variables, thus a naming convention that is easily understood by project proponents is recommended.

The Id is a composite key that includes the `Id` and the `Version` so that multiple versions of a variable template can be defined if the parameter definition changes over time. It is recommended that the `Id` be a string, minus special characters like commas or spaces, that represents the parameter name used in the methodology documentation and that the `Version` include the `ModuleName` followed by a `:` and the version number, e.g., `VM0049:1.0`. If a variable template is updated, a new version should be created with the same `Id` but a different `Version`. When a ClaimSource, CheckpointTemplate or FormulaTemplate references a VariableTemplate, it should reference the specific version required. Below is an example of a VariableTemplate definition for the `FRmass,x` parameter used in the `VM0049` module.

```json
    {
      "id": "Frmassx",
      "name": "FRmass,x",
      "description": "Total mass flow measured by mass flow meter x during the monitoring period. This maps to the Verra TotalMassFlow parameter.",
      "documentation": "inherited",
      "alias": "",
      "version": "VM0049:1.0",
      "dataSchemaOrType": "ccs.extensionSet.DoubleValue",
      "dataExample": "{ \"value\": 0.0, \"uOM\": \"tonnes\" }",
      "suggestedUnit": "tonnes",
      "requiredAtValidation": false,
      "requiredAtMonitoring": true,
      "entityExtensions": [
        {
          "templateId": "6001624e-64de-4d7f-88d8-ecbe61613618",
          "dataType": "ccs.extensionSet.verra.VariableTemplateOptions",
          "data": "{ \"appliesForCo2MeasurementType\": \"MASS_FLOW\", \"appliesForFossilFuelUse\": false, \"transportDirectMonitoring\": false, \"storageIntentionalDischargeMeasurementApproach\": \"MEASUREMENT_OF_VENTING\", \"storageSurfaceIntentionalDischargeMeasurement\": \"VOLUMETRIC_FLOW\", \"storageSubsurfaceIntentionalDischargeMeasurement\": \"MASS_FLOW\", \"storageUnintentionalSurfaceDischargeEmissionSource\": \"DOESNT_APPLY\", \"beccsBiomassCultivationLeakageCalculation\": \"NOT_USED\", \"vt0013AllocateProjectAndLeakageEmissions\": \"DOES_NOT_APPLY\" }",
          "extensionContext": "VariableTemplate"
        }
      ]
    }
```

### Formula Templates

[Formula Templates](./DeplomentPackage/FormulaTemplates.json): Formula templates are used to define the methodology module equations that have references to the variable templates that are required for calculation. These are installed third.

```json
{
      "id": "7c90069b-911a-4604-9b80-0a907c8d8c84",
      "name": "VM0049 Measurement based on Total Mass Flow. Equation 1, option 1 - Equation 2",
      "description": "To determine the quantity of CO2 using total mass flow measurements, the project proponent must multiply the total mass flow by the concentration of CO2 in that flow.",
      "documentation": "inherited",
      "version": "1.0",
      "dataSchemaOrType": "ccs.extensionSet.QuantityOfCo2InjectedFormula",
      "dataExample": "{}",
      "formula": "QCO2,x = FRmass,x × %CO2mass,x",
      "variableTemplates": [
        {"id": "Frmassx", "version": "VM0049:1.0"},
        {"id": "pCO2massx", "version": "VM0049:1.0"}
      ],
      "modules": [
        "VM0049", "VMD0058"
      ]
    }
```

### Message Pairs

[Message Pairs](./DeplomentPackage/MessagePairs.json): Message pairs allow for any request/response message pair to be defined to customize business workflow between parties. These are installed last.

```json
{
      "id": "a47e91c3-9435-485d-b5af-0bf544d17e59",
      "name": "Calibrate Instrument Message Pair",
      "version": "1.0.0",
      "description": "This message pair provides the request/response messages for calibrating an instrument used in monitoring. Because there are different monitoring types, the CalibrateInstrument response includes a CalibrationResult object that can have different calibration data for different types of instruments. Receiving an CalibrationResult will require inspecting via reflection the data type in the CalibrationData object to properly deserialize.",
      "documentation": "",
      "requestMessageDefinition": {
        "dataSchemaOrType": "ccs.extensionSet.CalibrateInstrumentRequest",
        "definition": "{\n          \"@type\": \"type.googleapis.com/ccs.extensionSet.CalibrateInstrumentRequest\",\n          \"claimId\": \"\",\n          \"checkpointId\": \"\",\n          \"claimSourceId\": \"\"\n        }"
      },
      "responseMessageDefinition": {
        "dataSchemaOrType": "ccs.extensionSet.CalibrateInstrumentResponse",
        "definition": "{\n          \"@type\": \"type.googleapis.com/ccs.extensionSet.CalibrateInstrumentResponse\",\n          \"claimId\": \"\",\n          \"checkpointId\": \"\",\n          \"claimSourceId\": \"\"\n        }"
      },
      "modules": [
        "VM0049"
      ]
    }
```

### Understanding Extension Set and Extension Set Modules

[Extension sets](./DeploymentPackage/ExtensionSet.json) are used to organize the templates that are used in the origination process. Templates that are used across all modules should be placed in the `root` Extension Set, where templates that are only valid for a specific module should be placed in the Extension Set Module.

ExtensionSets *do not* have fixed identifiers a multiple versions of the extension set can be installed for different supply chains. The ExtensionSet and Modules are assigned an identifier when installed.

The CCS+ Extension Set Structure:

- VM0049 - root
- VMD0056 - VMD0056 Module
- VMD0057 - VMD0057 Module
- VMD0058 - VMD0058 Module
- VMD0059 - VMD0059 Module
- VT0013 - VT0013 Tool

```json
{
  "name": "VM0049",
  "description": "Extension set for carbon capture and storage (CCS) projects",
  "version": "1.0",
  "documentation": "https://verra.org/methodologies/vm0049-carbon-capture-and-storage/",
  "entityExtensions": [
    {
      "templateId": "1a00284e-6cd1-486f-8630-91b58efae4c4",
      "dataType": "ccs.extensionSet.verra.VM0049Options",
      "data": "{\"co2MeasurementOption\": \"MassFlow\"}",
      "extensionContext": "ExtensionSet"
    }
  ],
  "modules": [
    {
      "name": "VMD0056",
      "description": "Direct Air Capture (DAC) module",
      "version": "1.0",
      "documentation": "https://verra.org/methodologies/vmd0056-co2-capture-from-air-direct-air-capture-v1-0/",
      "entityExtensions": [
        {
          "templateId": "08e67d02-f35d-4361-b39f-f69d189f41a2",
          "dataType": "ccs.extensionSet.verra.VMD0056Options",
          "data": "{\"usesFossilFuels\": \"true\"}",
          "extensionContext": "ExtensionSetModule"
        }
      ]
    },
    {
      "name": "VMD0057",
      "description": "CO2 Transport for CCS Projects module",
      "version": "1.0",
      "documentation": "https://verra.org/methodologies/vmd0057-co2-transport-for-ccs-projects-v1-0/",
      "entityExtensions": [
        {
          "templateId": "0f5e31e8-15a6-4b9b-9554-c2ce65500495",
          "dataType": "ccs.extensionSet.verra.VMD0057Options",
          "data": "{\"usesFossilFuels\": \"true\", \"DirectMonitoring\": \"true\"}",
          "extensionContext": "ExtensionSetModule"
        }
      ]
    },
    {
      "name": "VMD0058",
      "description": "VMD0058 CO2 Storage in Saline Aquifers and Depleted Hydrocarbon Reservoirs",
      "version": "1.0",
      "documentation": "https://verra.org/methodologies/vmd0058-co2-storage-in-saline-aquifers-and-depleted-hydrocarbon-reservoirs-v1-0/",
      "entityExtensions": [
        {
          "templateId": "6d9a2629-b66b-4338-a210-3d176f4d9209",
          "dataType": "ccs.extensionSet.verra.VMD0058Options",
          "data": "{\"surfaceDischargeMeasurementOption\": \"MeasurementOfVenting\", \"intentionalDischargeMeasurementApproach\": \"MassFlow\", \"intentionalSubsurfaceDischargeMeasurementOption\": \"MassFlow\", \"unintentionalSurfaceDischargeEmissionSource\": \"FugitiveEmissions\"}",
          "extensionContext": "ExtensionSetModule"
        }
      ]
    },
    {
      "name": "VMD0059",
      "description": "VMD0059 CO2 Capture FROM Bioenergy (BECCS)",
      "version": "1.0",
      "documentation": "https://verra.org/wp-content/uploads/2025/04/VMD0059-CO2-Capture-from-Bioenergy-final-publication.pdf",
      "entityExtensions": [
        {
          "templateId": "5523bc7c-0eb6-48c2-84a4-c032b8175fb9",
          "dataType": "ccs.extensionSet.verra.VMD0059Options",
          "data": "{\"calculatingLeakageFromBiomassCultivationOptions\": \"CalculateLeakageEmissions\"}",
          "extensionContext": "ExtensionSetModule"
        }
      ]
    },
    {
      "name": "VT0013",
      "description": "VT0013 Differentiating Reductions and Removals in CCS Projects, v1.0",
      "version": "1.0",
      "documentation": "https://verra.org/documents/vt0013-differentiating-reductions-and-removals-in-css-projects/",
      "entityExtensions": [
        {
          "templateId": "f3e2e2b1-1c7e-4f0c-8a4e-1d3f3e5b6c7d",
          "dataType": "ccs.extensionSet.verra.VT0013Options",
          "data": "{\"allocateProjectAndLeakageEmissionOptions\": \"MassBalanceMethod\"}",
          "extensionContext": "ExtensionSetModule"
        }
      ]
    }
  ]
}
```

The deployment package should deploy the Templates in the following order:

1. Entity Extension Templates
2. Variable Templates
3. Formula Templates
4. Message Pairs

Then deploy the Extension Set and its Modules. Followed by adding each template to their corresponding Extension Set or Module, as defined in each template's `modules` property or 'version' property for VariableTemplates. Once this is done, the Extension Set is ready for use in creating ClaimSources, Impact Claims and Checkpoints and assigned to one or more Origination Process Agreements.

## Understanding the Instance Package: ClaimSources and AimFixedVariables

The Instance Package contains the configuration required to create ClaimSources, Impact Claim and Checkpoint templates, and AIM Fixed Variables for use in a deployment.

### Understanding Claim Sources

A `ClaimSource` is a registration of an information or evidence source like a sensor, reference library, application or document. Each Claim Source will have one or more VariableTemplates for each Variable it is capable of reporting/submitting a value for. So there is a dependency between a ClaimSource and VariableTemplates, that essentially creates a variable route for reporting.

It is a best practice to only allow for Checkpoint.Variables to be reported from registered Claim Sources, this provides traceability for the source of the variable value and allows for easier validation of the data being reported.

Claim Sources are `ActivityImpactModule` specific and are configured in the [ClaimSources.json](./ImplementationPackage/ClaimSources.json) file. Each claim source has one or more VariableTemplates associated with it where each VariableTemplate as a `VariableTemplateId`, `VariableVersion` and `SourceIdentifier`.

For common libraries like the Project Design Document (PDD) or Emission Factor library, a single Claim Source can be used to report multiple VariableTemplates across multiple modules.

The `SourceIdentifier` property can be used to identify the specific source of the variable value in an external system known to the supplier, i.e., sensor id, etc. Naming conventions for the `SourceIdentifier` should be coordinated with the external system that is providing the evidence data, in the example we use a prefix of `PDD:` for the Project Design Document followed by an identifier that could be the parameter name.

```json
{

  "name": "Project Design Document",
  "description": "This claim source represents the project design document (PDD) that contains the project parameters required for monitoring and reporting.",
  "documentation": "",
  "variableTemplates": [
    {
      "variableTemplateId": "MCO2",
      "variableVersion": "VM0049:1.0",
      "sourceIdentifier": "PDD:MCO2"
    },
    {
      "variableTemplateId": "XCO2",
      "variableVersion": "VM0049:1.0",
      "sourceIdentifier": "PDD:tXCO2"
    }
  ]
}
```

### Understanding AIM Fixed Variables

Some fixed variables make sense to define AIM wide, like a GHG value. These can be added to the AIM instead of submitted in the claims process. The report generator that creates the appropriate report for the issuing registry can pull the value for the variable from the AIM instead of looking for it in a checkpoint.

```json
{
  "aimFixedVariables": [
    {
      "variableTemplateId": "GWPCH4",
      "variableTemplateVersion": "VM0049:1.0",
      "value": "{ \"value\": 28, \"uOM\": \"tCO2e/tCH4\" }",
      "dataType": "ccs.extensionSet.DoubleValue"
    }
  ]
}
```

## References

For additional support and examples, refer to:

- [Digital MRV Framework Specification](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html)
- [Verra CCS+ Methodology](https://verra.org/methodologies/methodology-for-carbon-capture-and-storage/)
