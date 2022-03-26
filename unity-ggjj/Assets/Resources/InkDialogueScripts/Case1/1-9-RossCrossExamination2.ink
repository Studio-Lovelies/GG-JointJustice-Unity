INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Templates/Macros.ink
INCLUDE ../Templates/FailureStates.ink

<- COURT_TMPH
<- Failures.TMPH

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

<- CrossExamination

-> Line1

=== Line1 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>I guess you got me. I did lie about what I was doing.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>I was actually making a special Mario Maker level for you guys.
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>You've always been good sports about my troll levels, so I wanted to make you a nice one for a change!
+ [Continue]
    -> Line4
+ [Press]
    -> Line3Press

=== Line4 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>I was working on it, thinking about what to make next, when I saw Jory walk by my office.#correct
+ [Continue]
    -> Line5
+ [Press]
    -> Line4Press
    
=== Line5 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>I felt it was a good time to take a break, so I went to see what he was up to.
+ [Continue]
    -> Line6
+ [Press]
    -> Line5Press
    
=== Line6 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>That's when I saw it! He had taken the dinos and stuffed them into his backpack!
+ [Continue]
    -> Line1
+ [Press]
    -> Line6Press

=== Line1Press ===
&HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
Why did you lie to us in the first place?

&JUMP_TO_POSITION:2
&SET_POSE:Sweaty
&SPEAK:Ross
Well{ellipsis} I didn't want to seem suspicious{ellipsis} That would incriminate me!

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&PLAY_SFX:smack
&SPEAK:Arin
You made yourself look suspicious when you lied!

&JUMP_TO_POSITION:2
&SPEAK:Ross
I'm really sorry for that. Seriously--!

&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
Yeah{ellipsis} I dunno. I'm not sure if I'm buying all this.

&OBJECTION:TutorialBoy
&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SET_POSE:Angry
&SPEAK:TutorialBoy
My witness already apologized, stop hectoring him and let him continue his testimony!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Objection sustained. The witness will continue.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
Yes, yes of course! Ehehe{ellipsis}
&SET_POSE:Normal

-> Line2

=== Line2Press ===
&HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
I don't like the way you define 'special', Ross.

&JUMP_TO_POSITION:2
&SET_POSE:Normal
&SPEAK:Ross
They were some fun and joyful levels I had planned! Honest!

&SCENE:TMPHAssistant
&SET_POSE:Fist
&SPEAK:Dan
Fun and joyful? With YOUR levels?! I have serious doubts about that.

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:Ross
I swear! They were going to be fun based on the concepts I drew out!

&HIDE_TEXTBOX
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
The witness will stop lying about the level of fun of his Mario Maker stages and continue with his testimony.

&SCENE:TMPHCourt
&PLAY_SFX:stab
&PLAY_EMOTION:Damage
&SET_POSE:Sweaty
&SPEAK:Ross
Er, yes, Your Honor. My sincerest apologies!
&SET_POSE:Normal

-> Line3

=== Line3Press ===
&HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
Good{ellipsis} sports? What do you mean by that?

&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
I think he means your tolerance of his style of levels.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
Yeah totally! You all haven't fired me yet, so I take that as you guys being good sports!
I just wanted to show you my appreciation for your patience with nicer level or two!

&JUMP_TO_POSITION:1
&SET_POSE:Thinking
&THINK:Arin
(That is a very Ross thing to do{ellipsis})
(Still{ellipsis})
&SPEAK:Arin
I can't see how a couple of nicer levels would make up for all that pain.

&JUMP_TO_POSITION:2
&SET_POSE:Glaring
&SPEAK:Ross
Well, I'd be finishing them right now, if not for Jory!
&SET_POSE:Normal
As a matter of fact{ellipsis}

-> Line4

=== Line4Press ===
&HOLD_IT:Arin
&STOP_SONG
&SET_POSE:Normal,Arin
&PAN_TO_POSITION:1,{panTime}
&SET_POSE:Normal
&SPEAK:Arin
So you were making a level when you saw Jory. Exactly how were you making it?

&LOAD_SCRIPT:Case1/1-10-RossCrossExamination2Contradiction
-> END

=== Line5Press ===
&HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SET_POSE:Normal
&SPEAK:Arin
Ross, when have you ever gotten tired of making Mario Maker stages?

&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
Yeah, I feel like you have a serious hard-on for making us suffer. I don't think that's something you tire of easily.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
Well, I happened to have a glass of warm milk before making them. A fault on my part, indeed. Makes me sleepy, you see.

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&SPEAK:Arin
Ross{ellipsis} It was the middle of the day. Why are you drinking warm milk at noon?

&JUMP_TO_POSITION:2
&SET_POSE:Glaring
&SPEAK:Ross
Well{ellipsis} I just felt like having a glass of warm milk!

&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
Can we get back to the trial, please? I don't want more irrelevant arguing in my courthouse.

&SCENE:TMPHCourt
&SET_POSE:Normal
&SPEAK:Ross
Y-Yes, of course.

-> Line6

=== Line6Press ===
&HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SET_POSE:Normal
&SPEAK:Arin
Are you SURE you saw the dinos?

&JUMP_TO_POSITION:2
&SPEAK:Ross
I'm pretty sure, it would be pretty difficult to mistake them for anything else.

&SCENE:TMPHJudge
&SET_POSE:Normal
&SPEAK:JudgeBrent
We've known Ross for quite a while now, and I personally have never been made aware--
-- of any defects related to his ophthalmologic health.
Since I am acquainted with his eye doctor and also observant, I would know if such a problem existed.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&PLAY_SFX:DramaPound
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
Uhh{ellipsis}
{ellipsis}what?

&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
{ellipsis}
Eyesight good. Point void. Moving on.

&SCENE:TMPHCourt
&SPEAK:Arin
Right{ellipsis} My bad.

-> Line1