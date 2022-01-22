INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SET_POSE:SweatyNoHelmet
&FADE_IN:1
&PLAY_SONG:fyiIWannaXYourExaminationAllegro
&PLAY_ANIMATION:CrossExamination

&SPEAK:None
&APPEAR_INSTANTLY
<color={orange}><align=center>-- Witness's Account --

-> Line1

=== SetupWitness ===
&SCENE:TMPH_Court
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
<color=green>Yeah, so... After I heard the dinos went missing, I remembered that I saw Jory go back to the recording space.
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
<color=green>But when I saw they weren't there, I, uh... I turned the backpack to check the side pocket! Yeah!
+ [Continue]
    -> Line5
+ [Press]
    -> Line4Press

=== Line5 ===
<- SetupWitness
<color=green>When I opened up the left side pocket, that's where I found the dinosaurs! Y-yes, that's exactly how it happened!
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
Not exactly hard to remember that. Not that you would know, Arin...

&JUMP_TO_POSITION:1
&SPEAK:Arin
Hey, that's out of line, Ross! My memory is fine.

&SCENE:TMPH_Assistant
&SPEAK:Dan
Dude, you literally complain about how bad your memory is every other day.

&SCENE:TMPH_Court
&SET_POSE:Annoyed
&SPEAK:Arin
Hey, just whose side are you on, anyway?!

&HIDE_TEXTBOX
&PLAY_ANIMATION:GavelHit
&PLAY_SFX:gavel

&SCENE:TMPH_Judge
&SET_POSE:Warning
&SPEAK:Brent_Judge
If the defense is done bickering over trivial matters, I'd like to finish this case today.

&SCENE:TMPH_Court
&SET_POSE:Embarrassed
&SPEAK:Arin
Uh, ahaha, right you are, Your Honor!

&SCENE:TMPH_Judge
&SET_POSE:Normal
&SPEAK:Brent_Judge
Good. The witness will continue his testimony.

-> Line2

=== Line2Press ===
<- HoldIt
Why search the backpack?

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Ross says he saw Jory stuff the dinos into his backpack.
Of course that's the first thing he'd think of.
Try to pay attention to the story for once, Mr. Video Game Boy!
&HIDE_TEXTBOX
&SET_POSE:Embarrassed,Arin
&PAN_TO_POSITION:1,{doublePanTime}
&SPEAK:Arin
UH, yeah of course, I knew that. I was just making sure...
&SET_POSE:Point
That YOU knew it!

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
...
&HIDE_TEXTBOX
&WAIT:1
What did you do next, Ross?

-> Line3

=== Line3Press ===
<- HoldIt
You say you saw the coins in the front pocket?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Yes...?

&JUMP_TO_POSITION:1
&SET_POSE:Embarrassed
&SPEAK:Arin
Uh... Ok, good! Just... making sure I heard you correctly.

&JUMP_TO_POSITION:2
&SET_POSE:GlaringNoHelmet
&SPEAK:Ross
What, do you have biscuits in your ears or something?

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Hey, that's my phrase, you can't just steal it!

&SCENE:TMPH_Court
&SPEAK:Ross
What're you going to do about it? Charge me with a crime?
I'd like to see you try!

&SCENE:TMPH_Judge
&SET_POSE:Angry
&SPEAK:Brent_Judge
AHEM!

&SCENE:TMPH_Court
&SET_POSE:SweatyNoHelmet
&SPEAK:Ross
Yes, anyways, as I was saying...

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

&SCENE:TMPH_Assistant
&SPEAK:Dan
Actually, dude, that's me. I have the trouble with that.

&SCENE:TMPH_Court
&SET_POSE:Embarrassed
&SPEAK:Arin
Oh...Right. I knew that.

&SCENE:TMPH_Judge
&SET_POSE:Warning
&SPEAK:Brent_Judge
Please refrain from silly mistakes like that.
The witness will continue with his testimony.

-> Line5

=== Line5Press ===
<- HoldIt
Can you prove you found the dinos in the left side pocket?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Well... no, but I don't think you can prove that I didn't!

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
I'd like to remind the court that the burden of proof...
...rests in the hands of the defense! Ross is a witness, not the accused, after all.

&SCENE:TMPH_Judge
&SET_POSE:Warning
&SPEAK:Brent_Judge
The prosecution is correct.
Unless you can prove otherwise, this court will be taking this testimony as fact.

-> END