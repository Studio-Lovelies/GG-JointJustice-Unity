INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Templates/Macros.ink

<- COURT_TMPH

<-SetupWitness

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

&FADE_OUT:0
&FADE_IN:1
&PLAY_SONG:fyiIWannaXYourExaminationAllegro

<- CrossExamination

-> Line1

=== SetupWitness ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SET_POSE:SweatyNoHelmet
&SPEAK:Ross

-> DONE

=== HoldIt ===
&HIDE_TEXTBOX
&HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SET_POSE:Normal
&SPEAK:Arin

-> DONE

=== Line1 ===
<- SetupWitness
<color=green>Yeah, so{ellipsis} After I heard the dinos went missing, I remembered that I saw Jory go back to the recording space.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press
    
=== Line2 ===
<- SetupWitness
<color=green>S-so I went back there to search his backpack for the dinos.
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press
    
=== Line3 ===
<- SetupWitness
<color=green>I first searched the front pocket, where I saw the coins in the bag.
+ [Continue]
    -> Line4
+ [Press]
    -> Line3Press
    
=== Line4 ===
<- SetupWitness
<color=green>But when I saw they weren't there, I, uh{ellipsis} I turned the backpack to check the side pocket! Yeah!
+ [Continue]
    -> Line5
+ [Press]
    -> Line4Press

=== Line5 ===
<- SetupWitness
<color=green>When I opened up the left side pocket, that's where I found the dinosaurs! Y-yes, that's exactly how it happened!#correct
+ [Continue]
    -> Line1
+ [Press]
    -> Line5Press
    
=== Line1Press ===
<- HoldIt
So you instantly remembered something like that?
&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
Seems rather convenient to me!

&HIDE_TEXTBOX
&SET_POSE:GlaringNoHelmet,Ross
&PAN_TO_POSITION:2,{panTime}
&SPEAK:Ross
It happened like an hour after I saw him!
Not exactly hard to remember that. Not that you would know, Arin{ellipsis}

&JUMP_TO_POSITION:1
&SPEAK:Arin
Hey, that's out of line, Ross! My memory is fine.

&SCENE:TMPHAssistant
&SPEAK:Dan
Dude, you literally complain about how bad your memory is every other day.

&SCENE:TMPHCourt
&SET_POSE:Annoyed
&SPEAK:Arin
Hey, just whose side are you on, anyway?!

&HIDE_TEXTBOX
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
If the defense is done bickering over trivial matters, I'd like to finish this case today.

&SCENE:TMPHCourt
&SET_POSE:Embarrassed
&SPEAK:Arin
Uh, ahaha, right you are, Your Honor!

&SCENE:TMPHJudge
&SET_POSE:Normal
&SPEAK:JudgeBrent
Good. The witness will continue his testimony.

-> Line2

=== Line2Press ===
<- HoldIt
Why search the backpack?

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
Ross says he saw Jory stuff the dinos into his backpack.
Of course that's the first thing he'd think of.
Try to pay attention to the story for once, Mr. Video Game Boy!
&HIDE_TEXTBOX
&SET_POSE:Embarrassed,Arin
&PAN_TO_POSITION:1,{doublePanTime}
&SPEAK:Arin
UH, yeah of course, I knew that. I was just making sure{ellipsis}
&SET_POSE:Point
That YOU knew it!

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
{ellipsis}
&HIDE_TEXTBOX
&WAIT:1
What did you do next, Ross?

-> Line3

=== Line3Press ===
<- HoldIt
You say you saw the coins in the front pocket?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Yes{ellipsis}?

&JUMP_TO_POSITION:1
&SET_POSE:Embarrassed
&SPEAK:Arin
Uh{ellipsis} Ok, good! Just{ellipsis} making sure I heard you correctly.

&JUMP_TO_POSITION:2
&SET_POSE:GlaringNoHelmet
&SPEAK:Ross
What, do you have biscuits in your ears or something?

&SCENE:TMPHAssistant
&SET_POSE:Fist
&SPEAK:Dan
Hey, that's my phrase, you can't just steal it!

&SCENE:TMPHCourt
&SPEAK:Ross
What're you going to do about it? Charge me with a crime?
I'd like to see you try!

&SCENE:TMPHJudge
&SET_POSE:Angry
&SPEAK:JudgeBrent
AHEM!

&SCENE:TMPHCourt
&SET_POSE:SweatyNoHelmet
&SPEAK:Ross
Yes, anyways, as I was saying{ellipsis}

-> Line4

=== Line4Press ===
<- HoldIt
You said you opened up the LEFT side pocket? My left or your left?

&JUMP_TO_POSITION:2
&SPEAK:Ross
It was my left, without a doubt.

&JUMP_TO_POSITION:1
&SPEAK:Arin
So you say, but we all know you have trouble with left and right!

&SCENE:TMPHAssistant
&SPEAK:Dan
Actually, dude, that's me. I have the trouble with that.

&SCENE:TMPHCourt
&SET_POSE:Embarrassed
&SPEAK:Arin
Oh{ellipsis}Right. I knew that.

&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
Please refrain from silly mistakes like that.
The witness will continue with his testimony.

-> Line5

=== Line5Press ===
&MODE:Dialogue
<- HoldIt
Can you prove you found the dinos in the left side pocket?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Well{ellipsis} no, but I don't think you can prove that I didn't!

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
I'd like to remind the court that the burden of proof{ellipsis}
{ellipsis}rests in the hands of the defense! Ross is a witness, not the accused, after all.

&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
The prosecution is correct.
Unless you can prove otherwise, this court will be taking this testimony as fact.

&SCENE:TMPHAssistant
&SPEAK:Dan
This isn't good, man. Isn't there something we can do?

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
I don't know. If only there was some way to prove he's lying.

&SCENE:TMPHAssistant
&SET_POSE:SideNormalTurned
&SPEAK:Dan
If there is, it has to have something to do with that backpack.

&SCENE:TMPHCourt
&SET_POSE:Thinking
&SPEAK:Arin
You think so?

&SCENE:TMPHAssistant
&SPEAK:Dan
I don't know, man, I don't know what else we can do! What do you think?

-> FakeChoice

=== FakeChoice ===

&HIDE_TEXTBOX#0
+ [Check the backpack]
    -> CheckBackpack
+ [Give the fuck up this shit is too hard]
    -> GiveUp
    
=== GiveUp ===
&WAIT:1
&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:Arin
Fuck, man I dunno! Fucking fuck! I don't know!!! Shit!

&SCENE:TMPHAssistant
&SET_POSE:SideNormalTurned
&SPEAK:Dan
Damn, dude, no need to freak out! Let's just see if we can look at the backpack.

&SCENE:TMPHCourt
&SPEAK:Arin
Alright, I hope you're right about this!

-> FakeChoice

=== CheckBackpack ===
&WAIT:1
&SCENE:TMPHCourt
&SET_POSE:Normal
&SPEAK:Arin
Your Honor, I think there is something worth checking out about that backpack.
&HIDE_TEXTBOX
&PLAY_SFX:DeskSlam
&PLAY_EMOTION:DeskSlamAnimation
If the court will allow, I'd like to see if my hunch is correct!

&HIDE_TEXTBOX
&OBJECTION:TutorialBoy
&PAN_TO_POSITION:3,{doublePanTime}
&SPEAK:TutorialBoy
Absolutely not! Your Honor, they will tamper with the evidence!
&SET_POSE:Angry
These two care not for justice, only for their friend to AVOID JUSTICE!

&HIDE_TEXTBOX
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam

&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
I've known Arin for years now, and he is one of the most honest people I've ever met.
&SET_POSE:Normal
Certainly in a normal court, like in Attitude City, such things are not allowed.
&SET_POSE:Warning
But these are hardly normal circumstances. Thus, I will allow the defense to examine the backpack.
Do I make myself clear?

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Y-Yes! Absolutely crystal, Your Honor.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Good.
&SET_POSE:Normal
The defense may examine the evidence.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
Thank you, Your Honor! Now, let's take a look at this backpack!
&SET_POSE:Normal
&SHOW_ITEM:Jorys_Backpack,Right
&HIDE_TEXTBOX
&WAIT:1
&PLAY_SFX:dramaPound
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
Holy jeeez, man! What's in this thing? It's so heavy.
&HIDE_TEXTBOX
&WAIT:1
&PLAY_EMOTION:ShockAnimation
&WAIT:1
&PLAY_SFX:realization
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
That's it! I got it! I know how he's lying!

&SCENE:TMPHAssistant
&SET_POSE:Fist
&SPEAK:Dan
Well, don't just stand there, present the proof so we can get on with our lives!

&HIDE_TEXTBOX

-> Present("JorysBackpack") ->

&HIDE_ITEM
&TAKE_THAT:Arin
&SCENE:TMPHCourt
&SET_POSE:Point
&SPEAK:Arin
Your Honor, I'd like you to inspect the pocket Ross claims to have found our dinos in!

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
Inspect the backpack{ellipsis}?
&PLAY_SFX:realization
&SET_POSE:Normal
Oh yes, of course. We can check the left side pocket to see if our witness is worth his bacon.

&SCENE:TMPHCourt
&SET_POSE:DeskSlam
&SPEAK:Arin
PRECISELY! And judging by the sweat on his face, I believe all we have here is soggy bacon, Your Honor!

&SCENE:TMPHAssistant
&SET_POSE:Fist
&SPEAK:Dan
{ellipsis}what{ellipsis}?

&HIDE_TEXTBOX
&SCENE:TMPHJudge
&PLAY_EMOTION:Nodding
&SET_POSE:Normal
&SPEAK:JudgeBrent
Very well. Let's not delay any further, open 'er up!

&SHOW_ITEM:Jorys_Backpack,Right
&PLAY_SFX:shoomp
&STOP_SONG
&PLAY_SFX:potatoes2

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:Arin
{ellipsis}

&PLAY_SFX:potatoes
&PLAY_SFX:potatoes2

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
{ellipsis}

&PLAY_SFX:potatoes
&PLAY_SFX:potatoes2

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
{ellipsis}

&WAIT:1
&PLAY_SFX:thud4
&PLAY_SONG:ninjaSexPursuit

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&SPEAK:Ross
P{ellipsis} POTATOES???

&HIDE_TEXTBOX
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
That's right! A nearly endless supply of potatoes{ellipsis}
&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
{ellipsis}and his backpack is filled to the brim with them!

&JUMP_TO_POSITION:3
&HIDE_TEXTBOX
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SPEAK:TutorialBoy
RIDICULOUS!
&HIDE_TEXTBOX
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
PREPOSTEROUS!
&HIDE_TEXTBOX
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
LUDICROUS!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:1
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
I assure you it's not!
Your Honor, if you would: Please confirm for the court that, other than the front pocket here{ellipsis}
&PLAY_SFX:objectionClean
&SET_POSE:Objection
{ellipsis}there is positively no other place that the dinos AND coins could have been!

&HIDE_TEXTBOX
&SCENE:TMPHJudge
&SET_POSE:Thinking
&WAIT:1
&PLAY_SFX:lightbulb
&SET_POSE:Surprised
&SPEAK:JudgeBrent
It is as the defense says. This backpack is COMPLETELY filled with what feels like a million potatoes!

&HIDE_ITEM
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&SPEAK:Ross
But, but{ellipsis} why?!?
&PLAY_SFX:stab
&HIDE_TEXTBOX
&PLAY_EMOTION:DamageNoHelmet
It doesn't make any sense at all! Why would Jory have such an endless supply potatoes in his backpack?

&JUMP_TO_POSITION:1
&SET_POSE:Thinking
&THINK:Arin
(That's a good question. Why DOES he seem to have infinite potatoes all inside one backpack?)

&SCENE:TMPHAssistant
&SET_POSE:SideNormalTurned
&SPEAK:Dan
I think he just really likes potatoes, dude.

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
Well, you might have a serious problem here, Jory. I think we can get you the help that you need.

&SCENE:TMPHCourt
&SET_POSE:Confident
&SPEAK:Arin
The only help my client needs right now is help being cleared of these charges!

&SCENE:TMPHJudge
&SET_POSE:Normal
&SPEAK:JudgeBrent
Well, you have made a strong case that Jory would not have done this{ellipsis}

&HIDE_TEXTBOX
&OBJECTION:TutorialBoy
&STOP_SONG
&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&PLAY_EMOTION:Objection
&SPEAK:TutorialBoy
He has no case at all!
Sure, it SEEMS like Jory is innocent, but that's simply what the defense would have you believe!

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&THINK:Arin
(It's my job to do that, though{ellipsis})

&PLAY_SONG:investigationUniCore
&JUMP_TO_POSITION:3
&SET_POSE:Angry
&SPEAK:TutorialBoy
But all he has stated is baseless conjecture! He has no REAL proof that Ross is the real culprit!
The fact is that the dinos were found in JORY'S backpack!
We also have a motive of avoiding today's 10 Minute Power Hour!
I don't care how many potatoes were stuffed in his backpack-
-or how much the defense CLAIMS he loves those coins!
All of those are lies made up by the defense to distract you from the story the hard evidence tells: Jory is guilty!

&HIDE_TEXTBOX
&SCENE:TMPHWideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:Triplegavel
&PLAY_ANIMATION:TripleGavelHit

&SCENE:TMPHJudge
&SET_POSE:Angry
&SPEAK:JudgeBrent
Order in this court!
&SET_POSE:Surprised
That was quite an impassioned speech from the prosecution!
&SET_POSE:Normal
And he does bring up good points.

&SCENE:TMPHAssistant
&SET_POSE:Fist
&PLAY_SFX:shock2
&SPEAK:Dan
No way!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I've not heard of this potato obsession before today, so it strikes me as maybe too convenient.
I know that in the past Jory has been nothing but a good boy{ellipsis}
{ellipsis}but the evidence seems stacked in the prosecution's favor.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
You can't be serious! After all that?

&JUMP_TO_POSITION:3
&SET_POSE:Confident
&SPEAK:TutorialBoy
You are very wise, Judge Brent!
I know that justice will be measured for Jory according to the devious and sinister nature of his crimes!

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
{ellipsis}
&SET_POSE:Normal
Yes, I think I have to agree with the prosecution on this matter.
&STOP_SONG

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&PLAY_SFX:stab2
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
WHAT?!? We lost?!

&HIDE_TEXTBOX
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I'm sorry, Jory. I like you a lot but given the circumstances it seems I have no choice.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SET_POSE:Confident
&SPEAK:TutorialBoy
Looks like you lose this time, 'Video Game Boy'!

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&THINK:Arin
(I'm sorry, Jory{ellipsis} I failed.)

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
This court finds the defendant, Jory Griffis{ellipsis}

&HIDE_TEXTBOX
&WAIT:1

&OBJECTION:Dan

&PLAY_SFX:shock2
&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&WAIT:1

&PLAY_SFX:shock2
&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&WAIT:1

&PLAY_SFX:shock2
&JUMP_TO_POSITION:1
&SET_POSE:SweatyBlinking
&SPEAK:Arin
Dan?

&SCENE:TMPHAssistant
&SET_POSE:SideObjection
&SPEAK:Dan
Wait, Your Honor! There's something off about those dinosaurs, and I think I know what it is!

&LOAD_SCRIPT:Case1/1-13-TrialEnd

-> END