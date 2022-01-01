//Do these before fading in, sets up the scene

&SCENE:TMPH_Judge
&ACTOR:Brent_Judge

&SCENE:TMPH_Assistant
&ACTOR:Dan

&SCENE:TMPH_Court
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:2,Ross
&SET_ACTOR_POSITION:3,TutorialBoy

&SET_POSE:Normal
Normal pose.

&JUMP_TO_POSITION:2
Arin.big = true

&SET_POSE:CloseUp,Arin
I'm big now, switch to Scene view to see

&SET_POSE:Angry,TutorialBoy
This makes Tutorial Boy angry.

ANGERY!
He gonna throw.

&PLAY_EMOTION:HelmetHit,Arin

He threw

-> END