INCLUDE ../Templates/Macros.ink

&SCENE:TMPHCourt
&SET_ACTOR_POSITION:Prosecution,TutorialBoy
&SET_ACTOR_POSITION:Defense,Arin
&JUMP_TO_POSITION:Prosecution
&SPEAK:TutorialBoy
Wow, you're terrible at this.

&JUMP_TO_POSITION:Defense

&PLAY_EMOTION:ShockAnimation
&WAIT:0.5
&SET_POSE:Sweaty
&THINK:Arin
<color=\#0084ff>(God I hate this guy{char(".")})
&ISSUE_PENALTY
    -> END
