## PLAY_SFX
Values: 
  - [Filename of a sound effect](../constants.md#SfxAssetName)

⏲ Instant

Plays provided SFX.

Examples: 
  - `&PLAY_SFX:EvidenceShoop`

## PLAY_SONG
Values: 
  - [Filename of a song](../constants.md#SongAssetName)
  - (Optional) The time taken to transition between songs

⏲ Instant

Plays the provided song. Stops the current one. Loops infinitely.

Examples: 
  - `&PLAY_SONG:TurnaboutGrumpsters`

## STOP_SONG

⏲ Instant

If music is currently playing, stop it.

Examples: 
  - `&STOP_SONG`

## FADE_SONG
Values: 
  - The time taken to fade out

⏲ Instant


Fade out the currently playing song over a given time


Examples: 
  - `&FADE_SONG:2`