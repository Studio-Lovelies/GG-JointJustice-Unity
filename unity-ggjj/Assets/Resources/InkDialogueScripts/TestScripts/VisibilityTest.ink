&SCENE:TMPH_Court
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:2,Ross

&JUMP_TO_POSITION:1
&SPEAK:Arin
I'm visible!

&JUMP_TO_POSITION:2
&SPEAK:Ross
So am I!

&SHOW_ACTOR:Ross,False
&SPEAK:Ross
Oh no! I'm invisible!

&JUMP_TO_POSITION:1
&SPEAK:Arin
I'm not!

&JUMP_TO_POSITION:2
&SPEAK:Ross
I'm still invisible!

&SHOW_ACTOR:Arin,False
&SPEAK:Arin
Oh no!

&JUMP_TO_POSITION:1
&SPEAK:Arin
Now I'm invisible too!

&SHOW_ACTOR:Ross,true
&JUMP_TO_POSITION:2
&SPEAK:Ross
But I'm back now!

&SHOW_ACTOR:Arin,true
&JUMP_TO_POSITION:1
&SPEAK:Arin
So am I!

-> END