INCLUDE ../Templates/Macros.ink

&SCENE:TMPHJudge
&ACTOR:JudgeBrent
&SET_POSE:Thinking
&SPEAK:JudgeBrent
...
You can't be serious with this, right?

&SCENE:TMPHCourt
&SET_ACTOR_POSITION:Defense,Arin
&JUMP_TO_POSITION:Defense
&SET_POSE:Sweaty
&SPEAK:Arin
Uhhhh{ellipsis}

&SET_POSE:Embarrassed
Maybe?

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
You need to take this more seriously Arin. Hopefully this Penalty will help you focus.

&ISSUE_PENALTY

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:Arin
Y-yes, Your Honor. My bad.

-> END
