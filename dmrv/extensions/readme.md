# dMRV Extensions

Digital Measurement, Reporting and Verification extensions are the way that Quality Standards, i.e., methodology, programs, tools, create the specific data model used for the collection of evidence
and processing it. These extensions are contextual in nature, meaning the data define is attached to it relavant data entity in the process. For example:

- An extension can be defined that applies specifically to the Activity Impact Module, it would have data properties that are contextually relavant to it, meaning project wide data elements that are important 
for the quality standard being followed.
- An extension can be deinfined that applies to a Impact Claim or Impact Claim Checkpoint, that would have data properties that are meaninful for the evidence being packaged for submission.

These extensions are defined using Json Schemas and bundled in a **set** of extensions for a specific methodology. 

## Structure of a dMRV Extension Set

MRV Extensions are organized under a (Quality Standard)[] with an extension defined for each contextual entity where it is needed. For example, for Renewable Energy Credts (RECs), [I-REC](https://www.irecstandard.org/vision-for-standard-development/) can have a set of extensions defined in a folder named: [irec](./rec/irec/)

Then have an extension file for each relavant context. Contexts, or data types, that can have an extension added are defined in the specification under [MRVExtensionContext](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html#enumdef-mrvextensioncontext).

For example:

- `IrecAimMrvExtension.json` - the schema for the data extensions that apply the the Acitivity Impact Module data type.
- `IrecAimClaimSourceMrvExtension.json` - the schema for the data extensions that apply to registered Claim Source for an IRec project.
- `IRecCheckpointMrvExtension.json` - the schema for the data extension that applies to an Impact Claim Checkpoint.

The name of the files should include the QualityStandard indicator as well as the full or abriviated context indicator as they are stored in the same parent folder/directory.

## Using dMRV Extensions

Using a dMRV Extensions is done by creating a `json` instance of data that can be validated by the context schema where it is applied. Each extension is added to the `MRVExtenions` collection that each
context can contain. [MRV Extensions](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html#mrvextension) in the specification have a context indicator and can be either a simple
`untyped` extensions, a simple collection of name/value pairs that does not require a schema, or a `typed` extension that would be an instance as indicated in this documentation.

A [Typed Extension](https://interworkalliance.github.io/TokenTaxonomyFramework/dmrv/spec/index.html#typedextension), has a URL/URI to the valid contextual schema file, a link to the extension's documentation (optional) and a `data` property where the serialized `json` string for the extension should be placed.