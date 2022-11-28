INCLUDE ../Templates/Macros.ink

&SCENE:TMPHJudge
&ACTOR:JudgeBrent
&SET_POSE:Surprised
&SPEAK:JudgeBrent
WHAM BAM BAZAAAM, THAT'S THE WRONG ANSWER MA'AM!

&ISSUE_PENALTY
&HIDE_TEXTBOX
&PLAY_ANIMATION:GavelHit
&PLAY_SFX:Gavel

&SCENE:TMPHAssistant
&ACTOR:Dan
&SET_POSE:SideNormalTurned
&SPEAK:Dan
{ellipsis}I think you should try a different answer Arin.

&SPEAK:Arin
Gee ya THINK SO, DAN?

&SET_POSE:SideNormal
&SPEAK:Dan
Yes. Yes I do Arin. I do.

&SPEAK:Arin
...
Yeah I guess so{ellipsis}

-> END
