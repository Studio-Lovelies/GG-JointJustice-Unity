&SCENE:TMPH_Court
&SET_ACTOR_POSITION:Defense,Arin
&SET_ACTOR_POSITION:Witness,Ross

&JUMP_TO_POSITION:Defense
&SPEAK:Arin
I'm visible!

&JUMP_TO_POSITION:Witness
&SPEAK:Ross
So am I!

&HIDE_ACTOR
&SPEAK:Ross
Oh no! I'm invisible!

&JUMP_TO_POSITION:Defense
&SPEAK:Arin
I'm not!

&JUMP_TO_POSITION:Witness
&SPEAK:Ross
I'm still invisible!

&HIDE_ACTOR:Arin
&SPEAK:Arin
Oh no!

&JUMP_TO_POSITION:Defense
&SPEAK:Arin
Now I'm invisible too!

&SHOW_ACTOR:Ross
&JUMP_TO_POSITION:Witness
&SPEAK:Ross
But I'm back now!

&SHOW_ACTOR:Arin
&JUMP_TO_POSITION:Defense
&SPEAK:Arin
So am I!

-> END