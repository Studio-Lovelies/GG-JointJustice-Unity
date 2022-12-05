INCLUDE ../Templates/Macros.ink

&SCENE:TMPHJudge
&ACTOR:JudgeBrent
&SET_POSE:Normal
&SPEAK:JudgeBrent
I don't see how this could be the right answer...
But I'm in a good mood, so I think I won't penalize you this time.

&SCENE:TMPHCourt
&SET_ACTOR_POSITION:Defense,Arin
&JUMP_TO_POSITION:Defense
&SET_POSE:Embarrassed
&SPEAK:Arin
Dang, thanks Brent. This is a lot harder than it looks!

&SET_ACTOR_POSITION:Prosecution,TutorialBoy
&JUMP_TO_POSITION:Prosecution
&SET_POSE:Confident
&SPEAK:TutorialBoy
Yes, accept his freebie. It won't help you in the long run, Mr{char(".")} {char("'")}Video game BABY!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:Defense
&PLAY_SFX:Damage1
&SHAKE_SCREEN:0.2,0.2
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
You shut your goddamn pie hole{ellipsis}

&SCENE:Anime
&ACTOR:Arin
&SET_POSE:CloseUp
{ellipsis}you FUCKING CLOD{char("!")}{char("!")}{char("!")}

&SCENE:TMPHCourt
&JUMP_TO_POSITION:Prosecution
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
...

&JUMP_TO_POSITION:Defense
&SET_POSE:Embarrassed
&SPEAK:Arin
&AUTO_SKIP:true
Er... What I meant to say was-

&AUTO_SKIP:false
&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
&PLAY_SFX:Stab2
&SHAKE_SCREEN:0.2,0.2
Changed my mind. Penalty given.

&ISSUE_PENALTY

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:Arin
Aw man...

-> END
