## ACTOR

⏲ Instant

Sets the current shown actor on screen to the one provided. Starts it in the normal pose.

Examples: 
  - `&ACTOR:Arin`

## SHOW_ACTOR
Values: 
  - whether to show (`true`) or not show (`false`) an actor

⏲ Instant

Shows or hides the actor on the screen. Has to be re-done after switching a scene.

Examples: 
  - `&SHOW_ACTOR:true`
  - `&SHOW_ACTOR:false`

## SET_POSE
Values: 
  - [Poses defined per Actor](../constants.md#ActorPoseAssetName)

⏲ Instant

Makes the currently shown actor switch to target pose. Plays any animation associated with target pose / emotion, but doesn't wait until it is finished before continuing.

Examples: 
  - `&SET_POSE:Normal`

## PLAY_EMOTION

⏳ Waits for completion

Makes the currently shown actor perform target emotion (fancy word animation on an actor). Practically does the same as SET_POSE, but waits for the emotion to complete. Doesn't work on all poses, possible ones are flagged.

Examples: 
  - `&PLAY_EMOTION:Nodding`

## SET_ACTOR_POSITION

⏲ Instant

Sets the target sub-position of the current bg-scene to have the target actor.

Examples: 
  - `&SET_ACTOR_POSITION:1,Arin`