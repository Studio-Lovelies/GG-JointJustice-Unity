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

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
Something{ellipsis} off?

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&SPEAK:TutorialBoy
It's covered in JIZZ, right? Hah hah, very funny. We've heard that joke a million times!
That's no reason to interrupt the verdict.

&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&SPEAK:Ross
Y-Yeah..! What does that have to do with anything!?

&SCENE:TMPHAssistant
&SET_POSE:SideObjection
&SPEAK:Dan
It has EVERYTHING to do with this!

&SPEAK:Arin
What are you talking about, Dan?

&SET_POSE:SideNormalTurned
&SPEAK:Dan
Roll with me, Arin, I got this one.

&HIDE_TEXTBOX
&SET_POSE:SideObjection
What I mean is this!

&PLAY_SONG:ninjaSexPursuit
&SCENE:Anime
&ACTOR:Dan
&SET_POSE:CloseUp
&PLAY_SFX:objectionClean
I don't think that's jizz AT ALL!

&HIDE_TEXTBOX
&PLAY_SFX:stab
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
Huh?!

&PLAY_SFX:stab
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Not{ellipsis} jizz?

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
If it's not jizz, then what is it? And what does that have to do with this case?

&SCENE:TMPHAssistant
&SET_POSE:SideLean
&SPEAK:Dan
I think I know what it is, and if I can have those dinosaurs{ellipsis}

&HIDE_TEXTBOX
&SET_POSE:SideObjection
I think I can prove Jory's innocence!

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Thinking
&THINK:Arin
(Wait{ellipsis} I think I know what Dan's getting at.)
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:deskSlam
&SET_POSE:Point
Your Honor! I request for Dan{ellipsis}

-> Choice

=== Choice ===
&HIDE_TEXTBOX#0
+ [Taste them.]
    -> Taste
+ [Smell them.]
    -> Smell
+ [Rub them on his nipples.]
    -> Rub

=== Rub ===
{ellipsis}to be allowed to rub the dinos all over his beautiful nude body!

&SCENE:TMPHJudge
&SET_POSE:Angry
&SPEAK:JudgeBrent
{ellipsis}

&SCENE:TMPHAssistant
&SET_POSE:Angry
&SPEAK:Dan
Arin! That's not at all what I want with those things. Why did you say that?

&SCENE:TMPHCourt
&SET_POSE:Embarrassed
Oh, oops. Wishful thinking, I guess.
&SET_POSE:Point
&SPEAK:Arin
What I mean to say is, Your Honor{ellipsis}

-> Choice

=== Taste ===
be allowed to taste those dinosaurs{ellipsis}
&SET_POSE:Confident
{ellipsis}so we can find out what's really covering them!

&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&JUMP_TO_POSITION:2
&SET_POSE:GlaringNoHelmet
&SPEAK:Ross
EW, SICK!

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
You can't be serious.

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
I'm deadly serious!

-> EndChoice

=== Smell ===
be allowed to smell those stain-covered dinosaurs{ellipsis}
&SET_POSE:Confident
{ellipsis}so we can find out what's really covering them!

&HIDE_TEXTBOX
&PLAY_SFX:stab
&JUMP_TO_POSITION:2
&PLAY_EMOTION:DamageNoHelmet
&SPEAK:Ross
Ack!

&HIDE_TEXTBOX
&OBJECTION:TutorialBoy
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&SPEAK:TutorialBoy
That is an absurd request! I object to this!
No trivial stain is going to change the outcome of this trial!
You fools probably got it covered in some paint or something and you don't remember.

&JUMP_TO_POSITION:1
&SET_POSE:Point
&SPEAK:Arin
Then let's find out for sure!

-> EndChoice

=== EndChoice ===
&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
I'm not sure what to think of this{ellipsis} request.

&SCENE:TMPHCourt
&SET_POSE:DeskSlam
&THINK:Arin
(Come on, Brent{ellipsis})

&HIDE_TEXTBOX
&OBJECTION:TutorialBoy
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Your Honor, please think about this. This is a trivial matter!
These{ellipsis} 'stains' have no relation to anything relevant. The defense is stalling!

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
{ellipsis}
&SET_POSE:Normal
The prosecution has a point.
Defense, can you prove the importance of these stains?

&SCENE:TMPHAssistant
&SET_POSE:SideLean
&SPEAK:Dan
Arin, I think we can.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&SPEAK:Arin
Uh, yeah!
&SET_POSE:Normal
Yeah, we can prove it. With{ellipsis}

-> Present("JorysBackpack") ->
    
&TAKE_THAT:Arin
&SHOW_ITEM:Jorys_Backpack,Right
&STOP_SONG
&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
The{ellipsis} backpack?

&SCENE:TMPHCourt
&SET_POSE:Normal
&SPEAK:Arin
Your Honor, take a closer look at it.

&SCENE:TMPHJudge
&SET_POSE:Normal
&SPEAK:JudgeBrent
Okay, but this better be relevant.
&SET_POSE:Thinking
{ellipsis}
&HIDE_TEXTBOX
&WAIT:2
&PLAY_SONG:starlightObjection
&PLAY_SFX:lightbulb
&SET_POSE:Surprised
There's a stain here too!
&HIDE_ITEM

&SCENE:TMPHAssistant
&SPEAK:Dan
Exactly. On one item, a stain may be trivial{ellipsis}

&SCENE:TMPHCourt
&SET_POSE:Confident
&SPEAK:Arin
But similar stains on two pieces of evidence? That seems relevant to the case.

&HIDE_TEXTBOX
&OBJECTION:TutorialBoy
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
No! I don't{ellipsis} I{ellipsis} it's{ellipsis}

&HIDE_TEXTBOX
&SCENE:TMPHJudge
&PLAY_EMOTION:HeadShake
&SET_POSE:Angry
&SPEAK:JudgeBrent
The prosecution will refrain from objecting without purpose.

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
I{ellipsis} apologize, Your Honor.

&SCENE:TMPHJudge
&SET_POSE:Normal
&SPEAK:JudgeBrent
The defense has proven its point. Mr. Avidan will be permitted to conduct his, erm, 'test'.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&THINK:Arin
(Okay, now we're getting somewhere. It's all on you Dan{ellipsis})

&PLAY_SFX:slurp
&SCENE:TMPHAssistant
&SET_POSE:SideLean
&SPEAK:Dan
{ellipsis}
&HIDE_TEXTBOX
&WAIT:2
&PLAY_SFX:lightbulb
&PLAY_SFX:stab
&SET_POSE:Fist
I KNEW IT!

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&PLAY_SFX:shock2
&PLAY_EMOTION:ShockAnimation
&WAIT:2

&SCENE:TMPHJudge
&SET_POSE:Surprised
&PLAY_SFX:shock2
&WAIT:2

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&PLAY_SFX:shock2
&WAIT:2

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&PLAY_SFX:shock2
&WAIT:1

&SCENE:TMPHAssistant
&SET_POSE:SideObjection
&SPEAK:Dan
The white stain on those dinos{ellipsis} It's still fresh, and it's not what you think it is!

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
Then, what exactly is covering the dinosaurs if it's not dick milk?

&SCENE:TMPHAssistant
&SPEAK:Dan
Gross
But funny you should say that!
It's actually just regular milk! Whole milk, to be exact!

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
OH NO!

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
You can't be serious! How does that prove anything?

&JUMP_TO_POSITION:1
&SET_POSE:Thinking
&THINK:Arin
(Yeah, how does that prove anything?)
&HIDE_TEXTBOX
&WAIT:1
&PLAY_SFX:realization
&PLAY_EMOTION:ShockAnimation
&WAIT:1
&PLAY_SFX:deskSlam
&SHAKE_SCREEN:0.25,0.25
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
It proves everything, actually! Not only does this clear our client's name{ellipsis}
&SET_POSE:Point
It also proves that Ross was the real culprit!

&SHAKE_SCREEN:0.25,0.25
&JUMP_TO_POSITION:3
&PLAY_SFX:stab
&SPEAK:TutorialBoy
No way!

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
Yes way, and I can back it up!
&SET_POSE:Normal
See{ellipsis} what you don't know, Mister Tutorial dude--

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
It's Tutorial BOY, thank you very much!

&JUMP_TO_POSITION:1
&SET_POSE:Annoyed
&SPEAK:Arin
{ellipsis}
&HIDE_TEXTBOX
&WAIT:2
&SET_POSE:Normal
&SPEAK:Arin
What you don't know is that Jory is deathly allergic to milk and dairy products!
He wouldn't have gotten near these dinos if they had milk stains on them when he left the stream.
&SET_POSE:Confident
There is one person, however, who is obsessed with dairy products, here in this courtroom!
&SCENE:Anime
&ACTOR:Arin
&SET_POSE:CloseUp
ROSS!!!

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&PLAY_SFX:stab2
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
OOF!

&HIDE_TEXTBOX
&SCENE:TMPHWideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:Triplegavel
&PLAY_ANIMATION:TripleGavelHit

&SCENE:TMPHJudge
&SET_POSE:Angry
&SPEAK:JudgeBrent
Order Order ORDER, I say! ORDER!!!
&SET_POSE:Thinking
{ellipsis} ahem{ellipsis}
&SET_POSE:Normal
I sense that you have some kind of idea of what really happened, Arin?

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
You bet I do, Your Honor!
&SET_POSE:Normal
When Jory left to put his backpack in the 10 Minute Power Hour studio, he passed by Ross' office.
Ross, who noticed Jory walk by, decided he would take him down a peg, and set him up to be framed.
Right after Jory left, Ross snuck into the room and stashed the dinos in Jory's backpack.
He was obviously jealous of all the positive recognition Jory had gotten.
In his rush, he clearly spilt milk he always seems to have over the dinos as he was hiding them.
It's clear from the evidence, Your Honor, that Jory could not have possibly stolen the dinos!

&HIDE_TEXTBOX
&OBJECTION:TutorialBoy
&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
N-No! That can't be true!
&HIDE_TEXTBOX
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Sweaty
You must be lying! It's way too convenient for Jory to be allergic to milk!

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
As if! I have proof of his condition RIGHT HERE!

-> Present("JorySrsLetter") ->

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
A{ellipsis} letter?

&SCENE:TMPHCourt
&SPEAK:Arin
Absolutely!
&SET_POSE:Point
It's a letter from Jory's father, and take a look at the very end!

&HIDE_TEXTBOX
&SCENE:TMPHJudge
&PLAY_EMOTION:Nodding
&SET_POSE:Thinking
&WAIT:1
&PLAY_SFX:realization
&SET_POSE:Surprised
&SPEAK:JudgeBrent
It's as the defense says! <color={red}>Deathly allergic to milk!

&SCENE:TMPHCourt
&SET_POSE:Normal
&SPEAK:Arin
And since Dan has proven that the stains are in fact milk, that makes it impossible for Jory to go near them!
&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
No one else likes milk like Ross does, and that makes him our real culprit!

&HIDE_TEXTBOX
&STOP_SONG
&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&PLAY_EMOTION:Breakdown
&PLAY_SFX:wham
&SHAKE_SCREEN:0.25,0.25
&WAIT:1
&FADE_OUT:3
&WAIT:1

&SCENE:TMPHJudge
&SET_POSE:Normal
&FADE_IN:1
&SPEAK:JudgeBrent
Well, that certainly was quite the turnaround there.
I've presided over quite a few cases in my day, but I've never seen a case resolved quite like that!

&SCENE:TMPHAssistant
&SET_POSE:SideLean
&SPEAK:Dan
All in a day's work for Danny Sexbang!
&HIDE_TEXTBOX
&PLAY_SFX:airGuitar
//&PLAY_EMOTION:AirGuitar
&SET_POSE:SideNormal

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Yes{ellipsis} Well done, Mr. Avidan.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
Hey, I did a lot too!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
But with that, I think we can safely say justice is served today.
&SET_POSE:Warning
Therefore, I find the defendant, Jory Griffis{ellipsis}
&HIDE_TEXTBOX
&PLAY_ANIMATION:GoodBoy

&PLAY_SFX:galleryCheer
&SCENE:TMPHWideShot
&WAIT:2

&FADE_OUT:3

&LOAD_SCRIPT:Case1/1-14-PostTrial
-> END