INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
Something... off?

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&SPEAK:Tutorial_Boy
It's covered in JIZZ, right? Hah hah, very funny. We've heard that joke a million times!
That's no reason to interrupt the verdict.

&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&SPEAK:Ross
Y-Yeah..! What does that have to do with anything!?

&SCENE:TMPH_Assistant
&SET_POSE:SideObjection
&SPEAK:Dan
It has EVERYTHING to do with this!

&SPEAK:Arin
What are you talking about, Dan?

&SET_POSE:SideNormalTurned
&SPEAK:Dan
Roll with me, Arin, I got this one.

&HIDE_TEXTBOX
// &PLAY_EMOTION:SideObjection
What I mean is this!

&PLAY_SONG:ninjaSexPursuit
&SCENE:Anime
&ACTOR:Dan
&SET_POSE:CloseUp
&PLAY_SFX:objectionClean
I don't think that's jizz AT ALL!

&HIDE_TEXTBOX
&PLAY_SFX:stab
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
Huh?!

&PLAY_SFX:stab
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
Not... jizz?

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
If it's not jizz, then what is it? And what does that have to do with this case?

&SCENE:TMPH_Assistant
&SET_POSE:SideLean
&SPEAK:Dan
I think I know what it is, and if I can have those dinosaurs...

&HIDE_TEXTBOX
&SET_POSE:SideObjection
I think I can prove Jory's innocence!

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:Thinking
&THINK:Arin
(Wait... I think I know what Dan's getting at.)
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:deskSlam
&SET_POSE:Point
Your Honor! I request for Dan...

-> Choice

=== Choice ===
&HIDE_TEXTBOX
+ [Taste them.]
    -> Taste
+ [Smell them.]
    -> Smell
+ [Rub them on his nipples."]
    -> Rub

=== Rub ===
...to be allowed to rub the dinos all over his beautiful nude body!

&SCENE:TMPH_Judge
&SET_POSE:Angry
&SPEAK:Brent_Judge
...

&SCENE:TMPH_Assistant
&SET_POSE:Angry
&SPEAK:Dan
Arin! That's not at all what I want with those things. Why did you say that?

&SCENE:TMPH_Court
&SET_POSE:Embarrassed
Oh, oops. Wishful thinking, I guess.
&SET_POSE:Point
&SPEAK:Arin
What I mean to say is, Your Honor...

-> END

=== Taste ===
be allowed to taste those dinosaurs...
&SET_POSE:Confident
...so we can find out what's really covering them!

&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&JUMP_TO_POSITION:2
&SET_POSE:GlaringNoHelmet
&SPEAK:Ross
EW, SICK!

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
You can't be serious.

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
I'm deadly serious!

-> EndChoice

=== Smell ===
be allowed to smell those stain-covered dinosaurs...
&SET_POSE:Confident
...so we can find out what's really covering them!

&HIDE_TEXTBOX
&PLAY_SFX:stab
&JUMP_TO_POSITION:2
&PLAY_EMOTION:DamageNoHelmet
&SPEAK:Ross
Ack!

&HIDE_TEXTBOX
// &OBJECTION:Tutorial_Bot
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&SPEAK:Tutorial_Boy
That is an absurd request! I object to this!
No trivial stain is going to change the outcome of this trial!
You fools probably got it covered in some paint or something and you don't remember.

&JUMP_TO_POSITION:1
&SET_POSE:Point
&SPEAK:Arin
Then let's find out for sure!

-> EndChoice

=== EndChoice ===
&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
I'm not sure what to think of this... request.

&SCENE:TMPH_Court
&SET_POSE:DeskSlam
&THINK:Arin
(Come on, Brent...)

&HIDE_TEXTBOX
// &OBJECTION:Tutorial_Boy
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
Your Honor, please think about this. This is a trivial matter!
These... 'stains' have no relation to anything relevant. The defense is stalling!

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
...
&SET_POSE:Normal
The prosecution has a point.
Defense, can you prove the importance of these stains?

&SCENE:TMPH_Assistant
&SET_POSE:SideLean
&SPEAK:Dan
Arin, I think we can.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&SPEAK:Arin
Uh, yeah!
&SET_POSE:Normal
Yeah, we can prove it. With...

-> PresentEvidence

=== PresentEvidence ===
&HIDE_TEXTBOX
&PRESENT_EVIDENCE
+ [Wrong]
    -> PresentEvidence
+ [Jorys_Backpack]
    -> PresentJorysBackpack
    
=== PresentJorysBackpack ===
// &TAKE_THAT:Arin
&SHOW_ITEM:Jorys_Backpack,Right
&STOP_SONG
&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
The... backpack?

&SCENE:TMPH_Court
&SET_POSE:Normal
&SPEAK:Arin
Your Honor, take a closer look at it.

&SCENE:TMPH_Judge
&SET_POSE:Normal
&SPEAK:Brent_Judge
Okay, but this better be relevant.
&SET_POSE:Thinking
...
&HIDE_TEXTBOX
&WAIT:2
&PLAY_SONG:starlightObjection
&PLAY_SFX:lightbulb
&SET_POSE:Surprised
There's a stain here too!
&HIDE_ITEM

&SCENE:TMPH_Assistant
&SPEAK:Dan
Exactly. On one item, a stain may be trivial...

&SCENE:TMPH_Court
&SET_POSE:Confident
&SPEAK:Arin
But similar stains on two pieces of evidence? That seems relevant to the case.

&HIDE_TEXTBOX
// &OBJECTION:Tutorial_Boy
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
No! I don't... I... it's...

&HIDE_TEXTBOX
&SCENE:TMPH_Judge
&PLAY_EMOTION:HeadShake
&SET_POSE:Angry
&SPEAK:Brent_Judge
The prosecution will refrain from objecting without purpose.

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
I... apologize, Your Honor.

&SCENE:TMPH_Judge
&SET_POSE:Normal
&SPEAK:Brent_Judge
The defense has proven its point. Mr. Avidan will be permitted to conduct his, erm, 'test'.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&THINK:Arin
(Okay, now we're getting somewhere. It's all on you Dan...)

&PLAY_SFX:slurp
&SCENE:TMPH_Assistant
&SET_POSE:SideLean
&SPEAK:Dan
...
&HIDE_TEXTBOX
&WAIT:2
&PLAY_SFX:lightbulb
&PLAY_SFX:stab
&SET_POSE:Fist
I KNEW IT!

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&PLAY_SFX:shock2
&PLAY_EMOTION:ShockAnimation
&WAIT:2

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&PLAY_SFX:shock2
&WAIT:2

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&PLAY_SFX:shock2
&WAIT:2

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&PLAY_SFX:shock2
&WAIT:1

&SCENE:TMPH_Assistant
&SET_POSE:SideObjection
&SPEAK:Dan
The white stain on those dinos... It's still fresh, and it's not what you think it is!

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
Then, what exactly is covering the dinosaurs if it's not dick milk?

&SCENE:TMPH_Assistant
&SPEAK:Dan
Gross
But funny you should say that!
It's actually just regular milk! Whole milk, to be exact!

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
OH NO!

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
You can't be serious! How does that prove anything?

&JUMP_TO_POSTION:1
&SET_POSE:Thinking
&THINK:Arin
(Yeah, how does that prove anything?)
&HIDE_TEXTBOX:
&WAIT:1
&PLAY_SFX:realization
&PLAY_ANIMATION:ShockAnimation
&WAIT:1
&PLAY_SFX:deskSlam
&SHAKE_SCREEN:0.25,0.25
&PLAY_ANIMATION:DeskSlam
&SPEAK:Arin
It proves everything, actually! Not only does this clear our client's name...
&SET_POSE:Point
It also proves that Ross was the real culprit!

&SHAKE_SCREEN:0.25,0.25
&JUMP_TO_POSITION:3
&PLAY_SFX:stab
&SPEAK:Tutorial_Boy
No way!

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
Yes way, and I can back it up!
&SET_POSE:Normal
See... what you don't know, Mister Tutorial dude--

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
It's Tutorial BOY, thank you very much!

&JUMP_TO_POSITION:1
&SET_POSE:Annoyed
&SPEAK:Arin
...
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
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&PLAY_SFX:stab2
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
OOF!

&SCENE:TMPH_WideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:triplegavel
&PLAY_ANIMATION:TripleGavelHit

&SCENE:TMPH_Judge
&SET_POSE:Angry
&SPEAK:Brent_Judge
Order Order ORDER, I say! ORDER!!!
&SET_POSE:Thinking
... ahem...
&SET_POSE:Normal
I sense that you have some kind of idea of what really happened, Arin?

&SCENE:TMPH_Court
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
// &OBJECTION:Tutorial_Boy
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
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

-> PresentEvidence2

=== PresentEvidence2 ===
&PRESENT_EVIDENCE
+ [Wrong]
    -> PresentEvidence2
+ [Jory_Srs_Letter]
    -> PresentJorySrsLetter

=== PresentJorySrsLetter ===
&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
A... letter?

&SCENE:TMPH_Court
&SPEAK:Arin
Absolutely!
&SET_POSE:Point
It's a letter from Jory's father, and take a look at the very end!

&HIDE_TEXTBOX
&SCENE:TMPH_Judge
&PLAY_EMOTION:Nodding
&SET_POSE:Thinking
&WAIT:1
&PLAY_SFX:realization
&SET_POSE:Surprised
&SPEAK:Brent_Judge
It's as the defense says! <color={red}>Deathly allergic to milk!

&SCENE:TMPH_Court
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

&SCENE:TMPH_Judge
&SET_POSE:Normal
&FADE_IN:1
&SPEAK:Brent_Judge
Well, that certainly was quite the turnaround there.
I've presided over quite a few cases in my day, but I've never seen a case resolved quite like that!

&SCENE:TMPH_Assistant
&SET_POSE:SideLean
&SPEAK:Dan
All in a day's work for Danny Sexbang!
&HIDE_TEXTBOX
&PLAY_SFX:airGuitar
&PLAY_EMOTION:AirGuitar
&SET_POSE:SideNormal

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Yes... Well done, Mr. Avidan.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
Hey, I did a lot too!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
But with that, I think we can safely say justice is served today.
&SET_POSE:Warning
Therefore, I find the defendant, Jory Griffis...
&HIDE_TEXTBOX
&PLAY_ANIMATION:GoodBoy

&PLAY_SFX:galleryCheer
&SCENE:TMPH_WideShot
&WAIT:2

&FADE_OUT:3
-> END