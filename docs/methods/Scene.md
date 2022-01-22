## FADE_OUT
Values: 
  - number of seconds for the fade out to take. Decimal numbers allowed

⏳ Waits for completion

Fades the screen to black, only works if not faded out.

Examples: 
  - `&FADE_OUT:1`

## FADE_IN
Values: 
  - number of seconds for the fade in to take. Decimal numbers allowed

⏳ Waits for completion

Fades the screen in from black, only works if faded out.

Examples: 
  - `&FADE_IN:1`

## CAMERA_PAN
Values: 
  - number of seconds for the fade in to take. Decimal numbers allowed
  - x axis position to pan to (0 is the default position)
  - y axis position to pan to (0 is the default position)

⏲ Instant

Pans the camera over a given amount of time to a given position in a straight line. Continues story after starting. Use WAIT to add waiting for completion.

Examples: 
  - `&CAMERA_PAN:2,0,-204`

## CAMERA_SET
Values: 
  - x axis position to pan to (0 is the default position)
  - y axis position to pan to (0 is the default position)

⏲ Instant

Sets the camera to a given position.

Examples: 
  - `&CAMERA_SET:0,-204`

## SHAKE_SCREEN
Values: 
  - Decimal number representing the intensity of the screen shake
  - Decimal number representing the duration of the shake in seconds
  - (Optional, `false` by default) `true` or `false` for whether the narrative script should continue immediately (`false`) or wait for the shake to finish (`true`)

⏳ Waits for completion

Shakes the screen.

Examples: 
  - `&SHAKE_SCREEN:1,0.5,true`

## SCENE
Values: 
  - [Name of a scene](../constants.md#SceneAssetName)

⏲ Instant

Sets the scene. If an actor was already attached to target scene, it will show up as well.

Examples: 
  - `&SCENE:TMPH_Court`

## SHOW_ITEM

⏲ Instant

Shows the given evidence on the screen in the given position.

Examples: 
  - `&SHOW_ITEM:Switch,Left`

## HIDE_ITEM

⏲ Instant

Hides the item shown when using SHOW_ITEM.

Examples: 
  - `&HIDE_ITEM`

## PLAY_ANIMATION
Values: 
  - [Name of a fullscreen animation to play](../constants.md#FullscreenAnimationAssetName)

⏳ Waits for completion

Plays a fullscreen animation.

Examples: 
  - `&PLAY_ANIMATION:GavelHit`

## JUMP_TO_POSITION
Values: 
  - Whole number representing the target sub-position of the currently active scene

⏲ Instant

Makes the camera jump to focus on the target sub-position of the currently active scene.

Examples: 
  - `&JUMP_TO_POSITION:1`

## PAN_TO_POSITION
Values: 
  - Whole number representing the target sub-position of the currently active scene
  - Decimal number representing the amount of time the pan should take in seconds

⏳ Waits for completion

Makes the camera pan to focus on the target sub-position of the currently active scene. Takes the provided amount of time to complete. If you want the system to wait for completion, call WAIT with the appropriate amount of seconds afterwards.

Examples: 
  - `&PAN_TO_POSITION:1,1`

## RELOAD_SCENE

⏳ Waits for completion

Restarts the currently playing script from the beginning.

Examples: 
  - `&RELOAD_SCENE`