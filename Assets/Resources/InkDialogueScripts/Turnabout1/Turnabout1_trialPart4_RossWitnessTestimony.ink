INCLUDE ../Colors.ink
INCLUDE ../Options.ink

&FADE_OUT:0
&SCENE:TMPH_Assistant
&ACTOR:Dan
&SCENE:TMPH_Judge
&ACTOR:Brent_Judge
&SCENE:TMPH_Court
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:2,Ross
&SET_ACTOR_POSITION:3,Tutorial_Boy

&FADE_OUT:0
&SCENE:TMPH_Assistant
&ACTOR:Dan
&SCENE:TMPH_Judge
&ACTOR:Brent_Judge
&SCENE:TMPH_Court
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:2,Ross
&SET_ACTOR_POSITION:3,Tutorial_Boy

&JUMP_TO_POSITION:2
&FADE_IN:1
&WAIT:1

&PLAY_ANIMATION:WitnessTestimony

&SPEAK:None
&APPEAR_INSTANTLY
<align=center><color={orange}>-- Witness' Account --

&PLAY_SONG:fyiIWannaXYourExaminationModerato
&SPEAK:Ross
<color=green>I was animating by myself over in my room at the office
<color=green>But then... I saw someone taking the dinos!!
<color=green>It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
<color=green>Now that I know they were stolen, that means the culprit must be Jory

&HIDE_TEXTBOX
&FADE_OUT:1
&PLAY_SONG:None
&WAIT:1

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&FADE_IN:1

&PLAY_SONG:aBoyAndHisTrial
&SPEAK:Brent_Judge
Hm...
&SET_POSE:Normal
A remarkably solid testimony here. Great witness, Mr. Boy.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Of course, Your Honor. You can only expect the BEST from me.

&HIDE_TEXTBOX
&JUMP_TO_POSITION:1
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
Dude, that testimony was incredible! Stupendous! AMAZING!

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Alright, already! You can stop jerking him off now, I get it.

&SPEAK:Arin
<size=40>There's absolutely no way we can get Jory off now.

&SET_POSE:SideNormal
&SPEAK:Dan
Phrasing, Arin.
But we have to try don't we? I mean, no matter how bulletproof that testimony may seem, we have to take the shot, right?

&SPEAK:Arin
You're right, Dan. But what the heck should I do next?

&SPEAK:Dan
I don't know, dude. Just look for things in his testimony that don't add up.
He totally has to have messed up in there somewhere!
I'm sure if we keep <color={red}>asking questions</color>, we'll get some information out of him.

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
Alright, let's do it then.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Is the defense ready for the CROSS-EXAMINATION?

&SCENE:TMPH_Court
&SET_POSE:Normal
&SPEAK:Arin
Yes, Your Honor.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Good. Then you may begin.

&HIDE_TEXTBOX
&FADE_OUT:1
&PLAY_SONG:None
&WAIT:3