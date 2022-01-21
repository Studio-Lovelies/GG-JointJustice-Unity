&SCENE:TMPHAssistant
&ACTOR:Dan
&SPEAK:Dan
Arin...

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SPEAK:Arin
What? It's the right answer, right?

&SCENE:TMPHAssistant
&SPEAK:Dan
...
&SET_POSE:Angry
No arin, we're getting a penalty for that one.

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
<color=\#0084ff>(I need to be more thoughtful and pay more attention I guess.)
&ISSUE_PENALTY
    -> END
