INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&SCENE:TMPH_Assistant
&SPEAK:Dan
This isn't good, man. Isn't there something we can do?

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
I don't know. If only there was some way to prove he's lying.

&SCENE:TMPH_Assistant
&SET_POSE:SideNormalTurned
&SPEAK:Dan
If there is, it has to have something to do with that backpack.

&SCENE:TMPH_Court
&SET_POSE:Thinking
&SPEAK:Arin
You think so?

&SCENE:TMPH_Assistant
&SPEAK:Dan
I don't know, man, I don't know what else we can do! What do you think?

-> FakeChoice

=== FakeChoice ===

&HIDE_TEXTBOX
+ [Check the backpack]
    -> CheckBackpack
+ [Give the fuck up this shit is too hard]
    -> GiveUp
    
=== GiveUp ===
&WAIT:1
&SCENE:TMPH_Court
&SET_POSE:Sweaty
&SPEAK:Arin
Fuck, man I dunno! Fucking fuck! I don't know!!! Shit!

&SCENE:TMPH_Assistant
&SET_POSE:SideNormalTurned
&SPEAK:Dan
Damn, dude, no need to freak out! Let's just see if we can look at the backpack.

&SCENE:TMPH_Court
&SPEAK:Arin
Alright, I hope you're right about this!

-> FakeChoice

=== CheckBackpack ===
&WAIT:1
&SCENE:TMPH_Court
&SET_POSE:Normal
&SPEAK:Arin
Your Honor, I think there is something worth checking out about that backpack.
&HIDE_TEXTBOX
&PLAY_SFX:deskslam
&PLAY_EMOTION:DeskSlamAnimation
If the court will allow, I'd like to see if my hunch is correct!

&HIDE_TEXTBOX
//&OBJECTION:Tutorial_Boy
&PAN_TO_POSITION:3,{doublePanTime}
&SPEAK:Tutorial_Boy
Absolutely not! Your Honor, they will tamper with the evidence!
&SET_POSE:Angry
These two care not for justice, only for their friend to AVOID JUSTICE!

&HIDE_TEXTBOX
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam

&SCENE:TMPH_Judge
&SET_POSE:Warning
&SPEAK:Brent_Judge
I've known Arin for years now, and he is one of the most honest people I've ever met.
&SET_POSE:Normal
Certainly in a normal court, like in Attitude City, such things are not allowed.
&SET_POSE:Warning
But these are hardly normal circumstances. Thus, I will allow the defense to examine the backpack.
Do I make myself clear?

&SCENE:TMPH_Court
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
Y-Yes! Absolutely crystal, Your Honor.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Good.
&SET_POSE:Normal
The defense may examine the evidence.

&SCENE:TMPH_Court
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
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
That's it! I got it! I know how he's lying!

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Well, don't just stand there, present the proof so we can get on with our lives!

&HIDE_TEXTBOX
&PRESENT_EVIDENCE // Jorys_Backpack
&HIDE_ITEM
// &TAKE_THAT:Arin
&SCENE:TMPH_Court
&SET_POSE:Point
&SPEAK:Arin
Your Honor, I'd like you to inspect the pocket Ross claims to have found our dinos in!

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
Inspect the backpack...?
&PLAY_SFX:realization
&SET_POSE:Normal
Oh yes, of course. #We can check the left side pocket to see if our witness is worth his bacon.

&SCENE:TMPH_Court
&SET_POSE:DeskSlam
&SPEAK:Arin
PRECISELY! And judging by the sweat on his face, I believe all we have here is soggy bacon, Your Honor!

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
...what...?

&HIDE_TEXTBOX
&SCENE:TMPH_Judge
&PLAY_EMOTION:Nodding
&SET_POSE:Normal
&SPEAK:Brent_Judge
Very well. Let's not delay any further, open 'er up!

&SHOW_ITEM:Jorys_Backpack,Right
&PLAY_SFX:shoomp
&STOP_SONG
&PLAY_SFX:potatoes2

&SCENE:TMPH_Court
&SET_POSE:Sweaty
&SPEAK:Arin
...

&PLAY_SFX:potatoes
&PLAY_SFX:potatoes2

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
...

&PLAY_SFX:potatoes
&PLAY_SFX:potatoes2

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
...

&WAIT:1
&PLAY_SFX:thud4
&PLAY_SONG:ninjaSexPursuit

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&SPEAK:Ross
P... POTATOES???

&HIDE_TEXTBOX
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
That's right! A nearly endless supply of potatoes...
&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
...and his backpack is filled to the brim with them!

&JUMP_TO_POSITION:3
&HIDE_TEXTBOX
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SPEAK:Tutorial_Boy
RIDICULOUS!
&HIDE_TEXTBOX
&PLAY_SFX:damage1
// &PLAY_EMOTION:HeadSlam
PREPOSTEROUS!
&HIDE_TEXTBOX
&PLAY_SFX:damage1
// &PLAY_EMOTION:HeadSlam
LUDICROUS!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:1
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
I assure you it's not!
Your Honor, if you would: Please confirm for the court that, other than the front pocket here...
&PLAY_SFX:objectionClean
&SET_POSE:Objection
...there is positively no other place that the dinos AND coins could have been!

&HIDE_TEXTBOX
&SCENE:TMPH_Judge
&SET_POSE:Thinking
&WAIT:1
&PLAY_SFX:lightbulb
&SET_POSE:Surprised
&SPEAK:Brent_Judge
It is as the defense says. This backpack is COMPLETELY filled with what feels like a million potatoes!

&HIDE_ITEM
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&SPEAK:Ross
But, but... why?!?
&PLAY_SFX:stab
&HIDE_TEXTBOX
&PLAY_EMOTION:DamageNoHelmet
It doesn't make any sense at all! Why would Jory have such an endless supply potatoes in his backpack?

&JUMP_TO_POSITION:1
&SET_POSE:Thinking
&THINK:Arin
(That's a good question. Why DOES he seem to have infinite potatoes all inside one backpack?)

&SCENE:TMPH_Assistant
&SET_POSE:NormalSideTurned
&SPEAK:Dan
I think he just really likes potatoes, dude.

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
Well, you might have a serious problem here, Jory. I think we can get you the help that you need.

&SCENE:TMPH_Court
&SET_POSE:Confident
&SPEAK:Arin
The only help my client needs right now is help being cleared of these charges!

&SCENE:TMPH_Judge
&SET_POSE:Normal
&SPEAK:Brent_Judge
Well, you have made a strong case that Jory would not have done this...

&HIDE_TEXTBOX
// &OBJECTION:Tutorial_Boy
&STOP_SONG
&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&PLAY_EMOTION:Objection
&SPEAK:Tutorial_Boy
He has no case at all!
Sure, it SEEMS like Jory is innocent, but that's simply what the defense would have you believe!

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&THINK:Arin
(It's my job to do that, though...)

&PLAY_SONG:investigationUniCore
&JUMP_TO_POSITION:3
&SET_POSE:Angry
&SPEAK:Tutorial_Boy
But all he has stated is baseless conjecture! He has no REAL proof that Ross is the real culprit!
The fact is that the dinos were found in JORY'S backpack!
We also have a motive of avoiding today's 10 Minute Power Hour!
I don't care how many potatoes were stuffed in his backpack-
-or how much the defense CLAIMS he loves those coins!
All of those are lies made up by the defense to distract you from the story the hard evidence tells: Jory is guilty!

&HIDE_TEXTBOX
&SCENE:TMPH_WideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:gavel
&PLAY_ANIMATION:TripleGavelHit

&SCENE:TMPH_Judge
&SET_POSE:Angry
&SPEAK:Brent_Judge
Order in this court!
&SET_POSE:Surprised
That was quite an impassioned speech from the prosecution!
&SET_POSE:Normal
And he does bring up good points.

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&PLAY_SFX:shock2
&SPEAK:Dan
No way!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
I've not heard of this potato obsession before today, so it strikes me as maybe too convenient.
I know that in the past Jory has been nothing but a good boy...
...but the evidence seems stacked in the prosecution's favor.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
You can't be serious! After all that?

&JUMP_TO_POSITION:3
&SET_POSE:Confident
&SPEAK:Tutorial_Boy
You are very wise, Judge Brent!
I know that justice will be measured for Jory according to the devious and sinister nature of his crimes!

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
...
&SET_POSE:Normal
Yes, I think I have to agree with the prosecution on this matter.
&STOP_SONG

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&PLAY_SFX:stab2
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
WHAT?!? We lost?!

&HIDE_TEXTBOX
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
I'm sorry, Jory. I like you a lot but given the circumstances it seems I have no choice.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SET_POSE:Confident
&SPEAK:Tutorial_Boy
Looks like you lose this time, 'Video Game Boy'!

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&THINK:Arin
(I'm sorry, Jory... I failed.)

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
This court finds the defendant, Jory Griffis...

&HIDE_TEXTBOX
&WAIT:1

// &OBJECTION:Dan

&PLAY_SFX:shock2
&SCENE:TMPH_Court
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

&SCENE:TMPH_Assistant
&SET_POSE:SideObjection
&SPEAK:Dan
Wait, Your Honor! There's something off about those dinosaurs, and I think I know what it is!

-> END