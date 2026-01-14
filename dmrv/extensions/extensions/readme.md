# Sample dMRV Extension Set Library

This is a partial implementation of [Verra's VM0049 suite of CCS+ modules](https://verra.org/methodologies/vm0049-carbon-capture-and-storage/) in an Extension Set. It is intended to provide a starting point for those looking to create other dMRV Extension Sets for different methodologies. At some point in the future a full implementation of this methodology will be made available, but for now this is a sample to demonstrate how to create an Extension Set for dMRV.

## Overview

This library contains the MRV Extensions that are derived from the CCS+ set of modules being governed and credits issued by [Verra](https://verra.org/methodologies/methodology-for-carbon-capture-and-storage/). The library is intended to be used with the [Digital MRV Framework](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html) and implementations of the specification.

The Extension Set is a library of data definitions that extends the functionality of an implementation of the dMRV Framework to work specifically with the CCS+ VM0049 methodology governed by Verra. This sample provides examples of both the set of templates that comprise the extension set and the instance configuration to use the templates once it is deployed.

The library contains:

- Deployment Package: the templates organized into an Extension Set for deployment to an dMRV implementation.
  - An Extension Set that is a collection of templates for Entity Extensions, Formulas, Variables and Message Pairs. These templates register the known data types that are used between the parties, organized in the Extension Set into Modules (VM0049, VMD0056 - VMD0059), that are deployed to the specification implementation.
- Instance Package: the instance configuration to use the deployed templates.
  - Example ClaimSources for deployment.
  - Example AIM Fixed Variables for deployment.

Once deployed, the Extension Set is assigned to a `Supply Chain` of a Project/Supplier, VVB and Issuer via the Origination Process Agreement (OPA). Each party then uses the templates defined in the Extension Set to create and exchange data in a standardized way.

## Contents

- Deployment Package: `dmrv/extensions/sample/DeploymentPackage`
- Instance Package: `dmrv/extensions/sample/InstancePackage`

## Documentation

- **[Getting Started Guide](./sample/GettingStarted.md)** - Quick start guide and basic usage.

### CCS+ Modules

- **VM0049 Carbon Capture Module** - CO2 capture monitoring
- **VMD0056 Direct Air Capture (DAC) Module** - DAC-specific measurements
- **VMD0057 Transport Module** - CO2 transport tracking
- **VMD0058 Storage Modules** - Geological storage monitoring
- **VMD0059 Bioenergy with CCS (BECCS) Module** - BECCS operations

## Contributing

Please read our [contributing guidelines](CONTRIBUTING.md) before submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.

## Related Projects

- [Microsoft Environmental Credit Service](https://www.microsoft.com/en-us/sustainability/environmental-credit-service)
- [Digital MRV Framework](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html)
- [Verra CCS+ Methodology](https://verra.org/methodologies/methodology-for-carbon-capture-and-storage/)
