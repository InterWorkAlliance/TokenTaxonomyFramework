#Retirement of CET against Scorecard Emissions


@startuml
actor ReportingOrganization as org #blue
entity ESGScorecard as scorecard #lightblue
collections ReportingPeriods as periods #orange
entity CarbonCredit as credit #green

org -> scorecard: create ESG Scorecard
org <-> periods: add ReportingPeriod
org <-> credit: retire Credit supplying Period Id
credit <-> periods: net down Effective Emmisions

@enduml


@startuml
actor ReportingOrganization as org #blue
entity CET as cet #lightblue
control Transfer as transfer #orange
Actor ReceivingOrganization as receiver #green

org -> cet: issue(quantityOfScope1)
org <-> transfer: transfer(1000, receivingOrganization)
transfer -> cet: changeScope(2)
transfer -> receiver: receive CET

@enduml

@startuml
actor ReportingOrganization as org #blue
actor EmissionsAuditor as auditor #yellow
entity ESGScorecard as scorecard #lightblue
collections ReportingPeriods as periods #orange
entity ReportingPeriod as period #green

org -> scorecard: setEmsisionsAuditor(emissionsAuditor)
auditor -> scorecard: validatePeriodReporting(reportingPeriod)
scorecard <-> periods: update ReportingPeriod
periods <-> period: updateReport


@enduml