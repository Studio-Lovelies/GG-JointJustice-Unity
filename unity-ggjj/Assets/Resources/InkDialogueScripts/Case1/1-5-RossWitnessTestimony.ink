INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Templates/Macros.ink
INCLUDE ../Options.ink

<- COURT_TMPH

&ADD_RECORD:Arin
&ADD_RECORD:Dan
&ADD_RECORD:Jory
&ADD_RECORD:JudgeBrent
&ADD_RECORD:TutorialBoy
&ADD_RECORD:Ross

&ADD_EVIDENCE:AttorneysBadge
&ADD_EVIDENCE:PlumberInvoice
&ADD_EVIDENCE:Switch
&ADD_EVIDENCE:Jory_Srs_Letter
&ADD_EVIDENCE:LivestreamRecording
&ADD_EVIDENCE:JorysBackpack
&ADD_EVIDENCE:StolenDinos
&ADD_EVIDENCE:BentCoins

&JUMP_TO_POSITION:Witness
&FADE_OUT:0
&PLAY_SONG:aBoyAndHisTrial,{songFadeTime}
&FADE_IN:1
&WAIT:1

&JUMP_TO_POSITION:Defense
&PLAY_SFX:dramaPound
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
Ross?! They roped you into this as well?

&JUMP_TO_POSITION:Witness
&SPEAK:Ross
Yeah{ellipsis} It looked important, you know? Plus, I could use the extra money.

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&PLAY_SFX:lightbulb
&SPEAK:JudgeBrent
Mr. O'Donovan, being a witness isn't a paying job{ellipsis}

&SCENE:TMPH_Court
&JUMP_TO_POSITION:Prosecution
&SPEAK:Tutorial_Boy
Not that anyone here is getting paid anyway{ellipsis}

&JUMP_TO_POSITION:Witness
&SET_POSE:Sweaty
&SPEAK:Ross
I-I see{ellipsis}

&JUMP_TO_POSITION:Prosecution
&SET_POSE:Confident
&SPEAK:Tutorial_Boy
Except if you count being paid in JUSTICE!
&PLAY_SFX:damage1
&HIDE_TEXTBOX
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
Ahem{ellipsis} Witness, please state your name and occupation for the court.

&JUMP_TO_POSITION:Witness
&SET_POSE:Normal
&DIALOGUE_SPEED:0.06
&SPEAK:Ross
<size=40>{ellipsis}Kangaroo court if I ever saw one{ellipsis}

&SCENE:TMPH_Judge
&DIALOGUE_SPEED:0.04
&SPEAK:JudgeBrent
What was that?

&SCENE:TMPH_Court
&SET_POSE:Sweaty
&SPEAK:Ross
Nothing, Your Honor!
&SET_POSE:Normal
I am Ross O'Donovan: local animator, Mario Maker enthusiast, apparent sadist, and <color={red}>friend to all here!</color>

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Some friend{ellipsis} testifying against Jory, treating him like a criminal{ellipsis}

&SPEAK:Arin
What do you mean? Even WE don't know if Jory is innocent!

&SET_POSE:Normal
&SPEAK:Dan
Arin, have you learned nothing from the Penix Wright<sup>(tm)</sup> playthrough?

&SPEAK:Arin
That a gavel, lubed properly, has many uses?

&SHAKE_SCREEN:0.25,0.2
&SET_POSE:Fist
&PLAY_SFX:smack
&DIALOGUE_SPEED:0.02
&SPEAK:Dan
No!
&SET_POSE:SideLaughing
&DIALOGUE_SPEED:0.04
Well, I mean yeah{ellipsis} The “gay-liff” in that game sure was creative.
&SET_POSE:Normal
But more importantly, you should go into every case with confidence that your client is innocent.
We don't know if he's guilty, but if his own attorney doesn't believe him, why should the rest of the court?

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&JUMP_TO_POSITION:Defense
&PLAY_SFX:DeskSlam
&SHAKE_SCREEN:0.25,0.25
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
You're right! We're here to defend our friend, so we should at least assume we're making the right call.

&SCENE:TMPH_Assistant
&SPEAK:Dan
Also, knowing how these things go, the defendant is always innocent{ellipsis}

&SET_POSE:SideLean
&DIALOGUE_SPEED:0.06
{ellipsis}Usually, anyway.

&SCENE:TMPH_Judge
&SET_POSE:Warning
&DIALOGUE_SPEED:0.04
&SPEAK:JudgeBrent
If the defense is done sucking each other's toes, may we begin with Mr. O'Donovan's testimony?

&SCENE:TMPH_Court
&SET_POSE:Sweaty
&THINK:Arin
<color=lightblue>(Why does he act like he doesn't know anyone here?)

&HIDE_TEXTBOX
&PLAY_EMOTION:Nodding
&SET_POSE:Normal
&SPEAK:Arin
We're ready, Your Honor.

&SCENE:TMPH_Judge
&SET_POSE:Normal
&SPEAK:JudgeBrent
Alright. The witness may begin his testimony.

&HIDE_TEXTBOX
&FADE_OUT:2
&FADE_OUT_SONG:2
&WAIT:2
&SCENE:TMPHCourt
&JUMP_TO_POSITION:Witness
&PLAY_SONG:fyiIWannaXYourExaminationModerato,{songFadeTime}
&FADE_IN:2

<- WitnessTestimony

&NARRATE
&APPEAR_INSTANTLY
<align=center><color={orange}>-- Witness' Account --

&SPEAK:Ross
<color=green>I was animating by myself over in my room at the office
<color=green>But then{ellipsis} I saw someone taking the dinos!!
<color=green>It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
<color=green>Now that I know they were stolen, that means the culprit must be Jory

&END_WITNESS_TESTIMONY
&HIDE_TEXTBOX
&FADE_OUT:1
&FADE_OUT_SONG:{songFadeTime}
&WAIT:1

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&FADE_IN:1

&PLAY_SONG:aBoyAndHisTrial,{songFadeTime}
&SPEAK:JudgeBrent
Hm{ellipsis}
&SET_POSE:Normal
A remarkably solid testimony here. Great witness, Mr. Boy.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:Prosecution
&SPEAK:Tutorial_Boy
Of course, Your Honor. You can only expect the BEST from me.

&HIDE_TEXTBOX
&JUMP_TO_POSITION:Defense
&PLAY_SFX:DeskSlam
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
&PLAY_SFX:DeskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
Alright, let's do it then.

&SCENE:TMPH_Judge
&SPEAK:JudgeBrent
Is the defense ready for the CROSS-EXAMINATION?

&SCENE:TMPH_Court
&SET_POSE:Normal
&SPEAK:Arin
Yes, Your Honor.

&SCENE:TMPH_Judge
&SPEAK:JudgeBrent
Good. Then you may begin.

&HIDE_TEXTBOX
&FADE_OUT:1
&FADE_OUT_SONG:{songFadeTime}
&WAIT:2

&LOAD_SCRIPT:Case1/1-6-RossCrossExamination

-> END