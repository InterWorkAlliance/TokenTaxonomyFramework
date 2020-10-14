# Example sequence diagram for the TTF

Using standard, TBD, diagraming, complete token sequence diagrams can be composed from the TTF.

Assumptions:

- A role is defined in the token. i.e. Minters
- A behavior(s) is scoped to be allowed by members of the role. i.e. Mintable

## GetRoleMembers

```plantuml
@startuml
    actor owner #blue
    entity token #red
    collections roleMembers

    owner ->    token: GetRoleMembersRequest
    token -> roleMembers: getMembers
    roleMembers -> token: members
    token -> owner: GetRoleMembersResponse

@enduml
```

## AddRoleMembers

```plantuml
@startuml
    actor owner #blue
    entity token #red
    collections roleMembers

    owner ->    token: AddRoleMemberRequest(accountId)
    token -> roleMembers: addMember(accountId)
    roleMembers -> token: confirmation
    token -> owner: AddRoleMemberResponse

@enduml
```

## RemoveRoleMembers

```plantuml
@startuml
    actor owner #blue
    entity token #red
    collections roleMembers

    owner ->    token: RemoveRoleMemberRequest(accountId)
    token -> roleMembers: removeMember(accountId)
    roleMembers -> token: confirmation
    token -> owner: RemoveRoleMemberResponse

@enduml
```

## Role used during role scoped behavior

For example, here is how this would behave for a token that implements Mintable, has a role defined called Minters and has Bob as a member of the Minters role.  Bob is going to Mint 100 tokens to his account.

```plantuml
@startuml
    actor Bob #blue
    entity token #red
    collections roleMembers
    control Mintable

    Bob ->    token: MintToRequest(100, bobAccountId)
    token -> roleMembers: isInRole(Bob)
    roleMembers -> token: true
    token -> Mintable: MintToRequest(100, bobAccountId)
    Mintable -> token: confirmation
    token -> Bob: MintToResponse(true)

@enduml
```

Alice is not in the role and  going to Mint 100 tokens to her account.

```plantuml
@startuml
    actor Alice #pink
    entity token #red
    collections roleMembers
    control Mintable

    Alice ->    token: MintToRequest(100, aliceAccountId)
    token -> roleMembers: isInRole(Alice)
    roleMembers -> token: false
        token -> Bob: MintToResponse(false)

@enduml
```