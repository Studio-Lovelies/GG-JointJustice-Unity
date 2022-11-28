INCLUDE ../Templates/Macros.ink

&MODE:Dialogue
&HIDE_TEXTBOX
&FADE_OUT:3
&WAIT:1

&SCENE:TMPHJudge
&ACTOR:JudgeBrent
&FADE_IN:3

&SPEAK:JudgeBrent
I see no further reason to continue this trial. I declare the defendant, Mr Jory Griffis{ellipsis}

&HIDE_TEXTBOX
&PLAY_ANIMATION:BadBoy
Take the defendant away and strip him of all his GoodBoy coins!

&HIDE_TEXTBOX
&PLAY_ANIMATION:GavelHit
&PLAY_SFX:Gavel
&FADE_OUT:3

&RELOAD_SCENE
-> END