## PLAY_SFX
Values: 
  - [Filename of a sound effect](../constants.md#SfxAssetName)

⏲ Instant

Plays provided SFX.

Examples: 
  - `&PLAY_SFX:EvidenceShoop`

## PLAY_SONG
Values: 
  - [Filename of a song](../constants.md#StaticSongAssetName)
  - (Optional) The time taken to transition between songs

⏲ Instant

Plays the provided song. Stops the current one. Loops infinitely.

Examples: 
  - `&PLAY_SONG:TurnaboutGrumpsters`
  - `&PLAY_SONG:TurnaboutGrumpsters,2`

## PLAY_SONG_VARIANT
Values: 
  - [Filename of a dynamic song asset](../constants.md#DynamicSongAssetName)
  - [Name of the variant of the song](../constants.md#DynamicSongVariantAssetName)
  - (Optional) The time taken to transition between songs

⏲ Instant

When a static or different dynamic song is playing: Stops the current song and plays the base and variant of the provided dynamic song. When the dynamic song is already playing: Cross-fades the current variant into the provided one. Loops infinitely.

Examples: 
  - `&PLAY_SONG_VARIANT:YouBurgieBurgie,Dan`
  - `&PLAY_SONG_VARIANT:YouBurgieBurgie,EvilBurgie`
  - `&PLAY_SONG_VARIANT:YouBurgieBurgie,Burgie`
  - `&PLAY_SONG_VARIANT:YouBurgieBurgie,Burgie,2`
  - `&PLAY_SONG_VARIANT:YouBurgieBurgie,Burgie,2`

## STOP_SONG

⏲ Instant

If music is currently playing, stop it.

Examples: 
  - `&STOP_SONG`

## FADE_OUT_SONG
Values: 
  - The time taken to fade out

⏲ Instant


Fade out the currently playing song over a given time


Examples: 
  - `&FADE_OUT_SONG:2`