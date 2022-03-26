INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Templates/Macros.ink

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

&JUMP_TO_POSITION:2
&FADE_IN:1
&WAIT:1

&PLAY_SONG:fyiIWannaXYourExaminationModerato,{songFadeTime}

<- WitnessTestimony

&SPEAK:Ross
I guess you got me. I did lie about what I was doing.
I was actually making a special Mario Maker level for you guys.
You've always been good sports about my troll levels, so I wanted to make you a nice one for a change!
I was working on it, thinking about what to make next, when I saw Jory walk by my office.
I felt it was a good time to take a break, so I went to see what he was up to.
That's when I saw it! He had taken the dinos and stuffed them into his backpack!

&HIDE_TEXTBOX
&END_WITNESS_TESTIMONY
&FADE_OUT_SONG:{songFadeTime}
&FADE_OUT:2
&WAIT:1

&SCENE:TMPHJudge
&SET_POSE:Thinking
&FADE_IN:2

&SET_POSE:Normal
&SPEAK:JudgeBrent
Hmm. So you were in your office working on a Mario Maker level but needed a break.
That's when you saw Jory, followed him, and saw the dinosaurs being taken.
That seems reasonable to me.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
Without a doubt, Your Honor. That is how it really happened.
We -- er, I mean, my witness, wanted to keep it a surprise.
&SET_POSE:Angry
I hope the defense is happy with themselves for ruining Ross' great gesture!

&HIDE_TEXTBOX
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&SPEAK:Arin
Ho boy...
&SET_POSE:Thinking
What do you think, Dan?

&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
Yeah... I can't really see any holes in his claim...

&SCENE:TMPHCourt
&SET_POSE:DeskSlam
&SPEAK:Arin
Damn...

&PLAY_SFX:lightbulb
&SET_POSE:Thinking
Wait! I've got an idea!

&SPEAK:Dan
What is it?

&HIDE_TEXTBOX
&PLAY_SFX:DeskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
I'll just BS my way through by questioning everything he said!

&SPEAK:Dan
I don't know, man. Do you really think that will work?

&SET_POSE:Embarrassed
&SPEAK:Arin
Who knows? I mean, it's worked for everything else I've ever done...

&SCENE:TMPHAssistant
&SPEAK:Dan
If you say so...
&SET_POSE:Angry
&AUTO_SKIP:true
Wait, what do you mean every--

&HIDE_TEXTBOX
&AUTO_SKIP:false
&SCENE:TMPHCourt
&PLAY_SFX:DeskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
LET'S DO IT!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
If the defense would like to cross examine now...?

&SCENE:TMPHCourt
&SET_POSE:Point
&SPEAK:Arin
You bet your sweet bippie I would, Your Honor! I've got some questions that need answering!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Very well. The defense may begin their cross-examination.

&HIDE_TEXTBOX
&FADE_OUT_SONG:{songFadeTime}
&FADE_OUT:2
&WAIT:1

&LOAD_SCRIPT:Case1/1-9-RossCrossExamination2

-> END









