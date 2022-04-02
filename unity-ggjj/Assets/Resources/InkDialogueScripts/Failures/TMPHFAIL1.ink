INCLUDE ../Templates/Macros.ink

&SCENE:TMPHAssistant
&ACTOR:Dan
&SPEAK:Dan
Arin{ellipsis}

&SCENE:TMPHCourt
&SET_ACTOR_POSITION:Defense,Arin
&JUMP_TO_POSITION:Defense
&SPEAK:Arin
What? It's the right answer, right?

&SCENE:TMPHAssistant
&SPEAK:Dan
{ellipsis}
&SET_POSE:Angry
No Arin, we're getting a penalty for that one.

&SCENE:TMPHCourt
&SPEAK:Arin
Wait, really?

&SCENE:TMPHJudge
&ACTOR:JudgeBrent
&SPEAK:JudgeBrent
Yes!

&SCENE:TMPHCourt
&ACTOR:Arin
&PLAY_EMOTION:ShockAnimation
&WAIT:0.5
&SET_POSE:Sweaty
&SPEAK:Arin
OOF.
&THINK:Arin
<color=\#0084ff>(I need to be more thoughtful and pay more attention I guess{char(".")})
&ISSUE_PENALTY
    -> END
