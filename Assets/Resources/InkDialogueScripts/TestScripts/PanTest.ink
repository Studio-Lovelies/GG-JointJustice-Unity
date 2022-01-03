// This script tests whether the PAN_TO_POSITION action functions correctly
// It should pan to the position and invoke the next line of dialogue when the
// pan has completed

&SCENE:TMPH_Court
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:3,Tutorial_Boy
&SPEAK:Arin
This script tests whether the PAN_TO_POSITION action functions correctly
&HIDE_TEXTBOX
&PAN_TO_POSITION:3,3
&SPEAK:Tutorial_Boy
Did it pan correctly?
&HIDE_TEXTBOX
&PAN_TO_POSITION:1,2
&SPEAK:Tutorial_Boy
&SPEAK:Arin
I'm not sure...

-> END
