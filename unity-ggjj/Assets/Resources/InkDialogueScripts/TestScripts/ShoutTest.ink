&SCENE:TMPH_Judge
&ACTOR:JudgeBrent

&SCENE:TMPH_Assistant
&ACTOR:Dan

&SCENE:TMPH_Court
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:2,Ross
&SET_ACTOR_POSITION:3,TutorialBoy

&JUMP_TO_POSITION:1
&SPEAK:Arin
I'm gonna shout!

&OBJECTION:Arin
I'm gonna shout again!

&HOLD_IT:Arin
...and again!

&TAKE_THAT:Arin
This next one sounds like a regular objection, but is actually a called using the SHOUT action.

&SHOUT:Arin,Objection
Ok, I'm done.

&OBJECTION:TutorialBoy
&PAN_TO_POSITION:3,1
&SPEAK:TutorialBoy
I would also like to say a few words.

&OBJECTION:Dan
&SCENE:TMPHAssistant
&SPEAK:Dan
No! We're done!

-> END