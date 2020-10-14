# Logical Interaction Model

This is a generic interaction model when using the concepts from the taxonomy to demonstrate how tokens are used.

## Token Create Class from Template sequence

```plantuml
@startuml
    actor owner #blue
    entity template #red
    control constructable

    owner ->    template: ConstructorRequest
    template -> constructable: constructor
    constructable -> template: confirmation
    template -> owner: ConstructorResponse(newTokenClassId)

@enduml
```

## Token Class interaction

```plantuml
@startuml
    actor user #blue
    entity token #red
    control getBalance

    user ->    token: GetBalanceRequest
    token -> getBalance: getBalance(user)
    token -> user: GetBalanceResponse(balance)

@enduml
```
