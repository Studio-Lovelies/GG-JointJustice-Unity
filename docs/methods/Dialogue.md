## DIALOGUE_SPEED
Values: 
  - Time in seconds, use `.` (not `,`) for decimal places.

⏲ Instant

Makes regular letters take the given amount of seconds before showing the next letter in dialogue.

Examples: 
  - `&DIALOGUE_SPEED:1.05`
  - `&DIALOGUE_SPEED:0.2`
  - `&DIALOGUE_SPEED:0.05`

## PUNCTUATION_SPEED
Values: 
  - Time in seconds, use `.` (not `,`) for decimal places.

⏲ Instant

Makes punctuation take the given amount of seconds before showing the next letter in dialogue.

Examples: 
  - `&PUNCTUATION_SPEED:1.05`
  - `&PUNCTUATION_SPEED:0.2`
  - `&PUNCTUATION_SPEED:0.05`

## AUTO_SKIP
Values: 
  - Set to either `true` or `false` to enable or disable automatic dialogue skipping respectively.

⏲ Instant

Starts or stops autoskipping of dialogue, where it automatically continues after it is done.

Examples: 
  - `&AUTO_SKIP:true`
  - `&AUTO_SKIP:false`

## DISABLE_SKIPPING
Values: 
  - Set to either `true` or `false` to not speedup or speedup text respectively.

⏲ Instant

Disables or enables text speedup. Enabled by default.

Examples: 
  - `&DISABLE_SKIPPING:true`
  - `&DISABLE_SKIPPING:false`

## CONTINUE_DIALOGUE

⏲ Instant

Makes the next dialogue add to the current one instead of replacing it.

Examples: 
  - `&CONTINUE_DIALOGUE`

## APPEAR_INSTANTLY

⏲ Instant

Makes the next line of dialogue appear all at once, instead of character by character.

Examples: 
  - `&APPEAR_INSTANTLY`

## HIDE_TEXTBOX

⏲ Instant

Hides the dialogue textbox until the next line of dialogue.

Examples: 
  - `&HIDE_TEXTBOX`

## OBJECTION

⏳ Waits for completion

Plays an "Objection!" animation and soundeffect for the specified actor.

Examples: 
  - `&OBJECTION:Arin`

## TAKE_THAT

⏳ Waits for completion

Plays a "Take that!" animation and soundeffect for the specified actor.

Examples: 
  - `&TAKE_THAT:Arin`

## HOLD_IT

⏳ Waits for completion

Plays a "Hold it!" animation and soundeffect for the specified actor.

Examples: 
  - `&HOLD_IT:Arin`

## SHOUT

⏳ Waits for completion

Uses the specified actor to play the specified shout.

Examples: 
  - `&SHOUT:Arin,OBJECTION,false`

## SPEAK

⏲ Instant

Makes the next non-action line spoken by the provided actor. If the speaking actor matches the actor on screen, it makes their mouth move when speaking.

Examples: 
  - `&SPEAK:Arin`

## THINK

⏲ Instant

Makes the next non-action line spoken by the provided actor. Doesn't make the actor's mouth.

Examples: 
  - `&THINK:Arin`

## SPEAK_UNKNOWN

⏲ Instant

Makes the next non-action line spoken by the provided actor but hides the name.

Examples: 
  - `&SPEAK_UNKNOWN:Arin`

## NARRATE

⏲ Instant

Makes the next non-action line spoken by a "narrator" actor.

Examples: 
  - `&NARRATE:Arin`