## LOAD_SCRIPT
Values: 
  - [The name of the narrative script to load](../constants.md#NarrativeScriptAssetName)

⏲ Instant


Loads a narrative script, ending the current narrative script
and continuing the beginning of the loaded script


Examples: 
  - `&LOAD_SCRIPT:Case_1_Part_1`

## SET_GAME_OVER_SCRIPT
Values: 
  - [The name of the game over script](../constants.md#GameOverScriptAssetName)

⏲ Instant


Sets the game over narrative script for the currently playing narrative script


Examples: 
  - `&SET_GAME_OVER_SCRIPT:TMPH_GameOver`

## ADD_FAILURE_SCRIPT
Values: 
  - [The name of the failure script to add](../constants.md#FailureScriptAssetName)

⏲ Instant


Adds a failure script for the currently playing narrative script


Examples: 
  - `&ADD_FAILURE_SCRIPT:TMPH_FAIL_1`

## LOAD_SCENE
Values: 
  - [The name of the scene to load](../constants.md#UnitySceneAssetName)

⏳ Waits for completion


Loads a Unity scene


Examples: 
  - `&LOAD_SCENE:Credits`