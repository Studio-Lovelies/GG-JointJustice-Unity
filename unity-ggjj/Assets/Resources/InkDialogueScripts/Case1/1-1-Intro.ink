INCLUDE ../Options.ink
INCLUDE StartingEvidence.ink

<- Part1StartingEvidence

&FADE_OUT:0
&PLAY_SONG:prologueInPMinor,{songFadeTime}

&SPEAK:Ross
After all the work I put into those levels...

&SCENE:TMPH_Ross
&CAMERA_SET:0,-204 //x, y
&HIDE_TEXTBOX
&FADE_IN:1

&CAMERA_PAN:2,0,0 //Speed, x, y
&WAIT:1
...We'll see who the real good boy is now, won't we Jory?

&HIDE_TEXTBOX
&FADE_OUT:1
&SCENE:TMPH_Ross_With_Dinos
&CAMERA_SET:0,0 //x, y
&FADE_IN:1
Just as planned...

&CAMERA_PAN:3,270,0 //Speed, x, y
&HIDE_TEXTBOX
Soon, I'll have all the coins!

&HIDE_TEXTBOX

&SCENE:WhiteScreen
&WAIT:0.1
&SCENE:TMPHRossCloseUp1
&WAIT:0.5
&SCENE:WhiteScreen
&WAIT:0.1
&SCENE:TMPHRossCloseUp2
&WAIT:0.5
&SCENE:WhiteScreen
&WAIT:0.1
&SCENE:TMPHGalaxyBrain
&WAIT:0.5
Now this is a galaxy-brain move right here!

&HIDE_TEXTBOX
&WAIT:1
&FADE_OUT:0
&PLAY_SFX:RossEvilLaugh
&WAIT:2

&LOAD_SCRIPT:Case1/1-2-PreTrial

-> END

