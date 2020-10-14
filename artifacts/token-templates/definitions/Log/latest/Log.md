# Log Token Example Sequence Diagram

Using standard, TBD, diagraming, complete token sequence diagrams can be composed from the TTF.

Assumptions:

- A role is defined in the token. i.e. Viewers
- A behavior(s) is scoped to be allowed by members of the role. i.e. Viewer

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