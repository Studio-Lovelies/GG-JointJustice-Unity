INCLUDE ../Templates/SceneInitialization.ink

<- COURT_TMPH

&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&ACTOR:Baby
&FADE_OUT:0
&SPEAK_UNKNOWN:Baby
<i>Knock knock knock</i> Anybody home?
&THINK:Arin
<color=green>I didn't recognize that voice but it sounded oddly familiar...
<color=green>I caved into my curiosity and opened the door.
&HIDE_TEXTBOX
&FADE_IN:3

&THINK:Baby
Hello. Father.

&HIDE_TEXTBOX
&JUMP_TO_POSITION:Defense
&PLAY_EMOTION:ShockAnimation
&THINK:Arin
<color=green>I couldn't believe my eyes. It was our baby!
<color=green>My eyes welled up as I immediately embraced my long-lost child.
&SPEAK:Arin
&SET_POSE:Normal
Is it, is it really you?
I thought... I thought you...

&JUMP_TO_POSITION:Witness
&THINK:Baby
Yes, father, it is me.
Although my mother died before giving birth, Doctor Doodle was able to rescue me before it was too late.