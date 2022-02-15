## ADD_EVIDENCE
Values: 
  - [Name of evidence to add](../constants.md#EvidenceAssetName)

⏲ Instant

Adds the provided evidence to the court record.

Examples: 
  - `&ADD_EVIDENCE:Bent_Coins`

## REMOVE_EVIDENCE

⏲ Instant

Removes the provided evidence from the court record.

Examples: 
  - `&REMOVE_EVIDENCE:Bent_Coins`

## ADD_RECORD
Values: 
  - [Name of the actor to add to the court record](../constants.md#ActorAssetName)

⏲ Instant

Adds the provided actor to the court record.

Examples: 
  - `&ADD_RECORD:Jory`

## PRESENT_EVIDENCE

⏳ Waits for completion

Forces the evidence menu open and doesn't continue the story until the player presents evidence.

Examples: 
  - `&PRESENT_EVIDENCE`

## SUBSTITUTE_EVIDENCE

⏲ Instant

Substitutes the provided evidence for their substitute.

Examples: 
  - `&SUBSTITUTE_EVIDENCE:Plumber_Invoice,Bent_Coins`