# Ecological Project Verification Process

@startuml
actor ProjectOwner as owner #green
participant Verifier as verifier #yellow
participant Registry as registry #orange
entity EcologicalProject as project #green
entity EcologicalClaim as claim #lightgreen
entity ProcessedClaim as processed #grey
entity IssuedAsset as asset  #gold
owner -> project: create Ecological Project
owner <-> registry: establish Verification Contract
registry <-> verifier: establish VVB for verification
verifier <-> owner: finalize Verification Contract
owner -> claim: create Ecological Claim
verifier -> claim: process Ecological Claim
verifier -> processed: create Processed Claim
processed -> claim: retire Ecological Claim
registry -> processed: validate Processed Claim
registry -> asset: issue Credit Asset
asset -> processed: retire Processed Claim
@enduml
