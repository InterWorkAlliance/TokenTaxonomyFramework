# What are MRV Extension Sets?

Version 3.0 introduces MRV Extension Sets, a grouping of templates which encompass entities, messages, formulae, and variables that are specific to the Quality Standard or Methodology that the Activity Impact Module is bound to. They are typically composed of multiple MRV Extensions for modules or activities and can be defined by the supplier, verifier, and issuer based on a set of methodology modules. To support Quality Standards that have multiple modules or phases of a process (e.g., Carbon Capture, Transport and Storage or projects which are making claims related to both soil and water), Extension Sets can be organized into Extension Set Modules that act as a shared library of MRV Extensions available to participants in the dMRV process. Through this process, we aim to create reusable libraries that dMRV participants can use as a starting place for conversations with registries and other relevant stakeholders while allowing for the contribution of new Extension Sets from third parties as methodologies are further digitized.

## How to Build and Use Extension Sets

Issuing Registries govern a set of methodologies under their issuing programs for specific types of projects. These methodologies are large documents that describe the process a project must follow in order to have credits issued based on their activities. For example, methodologies use formulae and parameters, or variables, to determine the quantity of credits that will be issued that all the parties involved use to document and verify activity outcomes.

An Extension Set is rooted in these methodology documents and puts it into a digital, machine readable format so that most of the activities in the process can be automated. Extension Sets should be created collaboratively by the supplier, VVB, and Issuer, usually as an iterative process.

Each methodology, or modules of a methodology, is used to extract Extension Templates:

- Entity Extension Templates - are data that are needed for an entity, like an Activity Impact Module, that does not exist in the dMRV schema. These extensions allow for the customization of the pre-defined entities in the specification that the methodology requires
- Formula Templates - are pulled from the documentation and used to record the formula syntax and additional documentation that is agreed upon by all parties in the dMRV process.
- Variable Templates - individual parameters/variables that are used in formulae that represent a data point used in the calculation of the formulae.
- Message Pairs - a set of request and response messages that allow for custom workflows to be introduced and recorded in the dMRV process.

Each template is given a globally “static” identifier, meaning it will never change, and is then added to the Extension Set or Extension Set Module. Once added, it becomes a registered “type” that all parties in the dMRV process will be able to understand and agree upon. Each template usually contains a custom defined user type where the actual extension data is stored. Templates are essential for establishing these custom data types so that they are understood by all parties in the dMRV process.

Parties in the dMRV process use the templates to create instances that reference their parent template, and record those instances in the dMRV data. When another party wants to read the data, they will use the parent template to understand the instance.

Once an Extension Set is created, it can be contributed back to the IWA where it can exist in a publicly-available repository of other Extension Sets. This contribution must include data, which can be sample data, to allow for testability and show that the input data is properly handled within the Extension Set. This allows for others using the same methodology to more easily re-use the Extension Set as a way to begin conversations with their chosen registry. The IWA will make available a sample implementation of an MRV Extension Set and a mechanism for public contribution to a shared library of MRV Extension Sets.

## How to Contribute an Extension Set

Once an Extension Set has been developed alongside the relevant stakeholders, it can be contributed to the IWA's repository of Extension Sets by issuing a pull request to this folder in the [IWA's GitHub repository.](https://github.com/InterWorkAlliance/TokenTaxonomyFramework/tree/main/dmrv/extensions)

The IWA will review the Extension Set for completeness and to ensure that sample data has been provided. If approved, the pull request will be accepted and the Extension Set will be added to the library. If not approved, the IWA will reach out with feedback on what changes need to be made for the submission to be approved.

*NOTE: The IWA will not verify the accuracy of the formulae, variables, entities, or messages contained in the Extension Sets as they are often the product of lengthy conversations between multiple parties. Due to the complexity and differentiation between methodologies and registries, organizations looking to use the Extension Sets to accelerate their development cycles should perform due diligence when using the Extension Sets to ensure alignment with existing registry requirements.*

## Sample MRV Extension Set Library

This is a partial implementation of [Verra's VM0049 suite of CCS+ modules](https://verra.org/methodologies/vm0049-carbon-capture-and-storage/) in an Extension Set. It is intended to provide a starting point for those looking to create other dMRV Extension Sets for different methodologies. At some point in the future a full implementation of this methodology will be made available, but for now this is a sample to demonstrate how to create an Extension Set for dMRV.

### Overview

This library contains the MRV Extensions that are derived from the CCS+ set of modules being governed and credits issued by [Verra](https://verra.org/methodologies/methodology-for-carbon-capture-and-storage/). The library is intended to be used with the [Digital MRV Framework](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html) and implementations of the specification.

The Extension Set is a library of data definitions that extends the functionality of an implementation of the dMRV Framework to work specifically with the CCS+ VM0049 methodology governed by Verra. This sample provides examples of both the set of templates that comprise the extension set and the instance configuration to use the templates once it is deployed.

The library contains:

- Deployment Package: the templates organized into an Extension Set for deployment to an dMRV implementation.
  - An Extension Set that is a collection of templates for Entity Extensions, Formulas, Variables and Message Pairs. These templates register the known data types that are used between the parties, organized in the Extension Set into Modules (VM0049, VMD0056 - VMD0059), that are deployed to the specification implementation.
- Instance Package: the instance configuration to use the deployed templates.
  - Example ClaimSources for deployment.
  - Example AIM Fixed Variables for deployment.

Once deployed, the Extension Set is assigned to a `Supply Chain` of a Project/Supplier, VVB and Issuer via the Origination Process Agreement (OPA). Each party then uses the templates defined in the Extension Set to create and exchange data in a standardized way.

### Contents

- Deployment Package: `dmrv/extensions/sample/DeploymentPackage`
- Instance Package: `dmrv/extensions/sample/InstancePackage`

### Documentation

- **[Getting Started Guide](./sample/GettingStarted.md)** - Quick start guide and basic usage.

### CCS+ Modules

- **VM0049 Carbon Capture Module** - CO2 capture monitoring
- **VMD0056 Direct Air Capture (DAC) Module** - DAC-specific measurements
- **VMD0057 Transport Module** - CO2 transport tracking
- **VMD0058 Storage Modules** - Geological storage monitoring
- **VMD0059 Bioenergy with CCS (BECCS) Module** - BECCS operations

### Contributing

Please read our [contributing guidelines](CONTRIBUTING.md) before submitting pull requests.

### License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

### Related Projects

- [Microsoft Environmental Credit Service](https://www.microsoft.com/en-us/sustainability/environmental-credit-service)
- [Digital MRV Framework](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html)
- [Verra CCS+ Methodology](https://verra.org/methodologies/methodology-for-carbon-capture-and-storage/)
