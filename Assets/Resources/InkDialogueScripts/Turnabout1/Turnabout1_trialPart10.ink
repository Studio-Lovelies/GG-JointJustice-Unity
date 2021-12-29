INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit
&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Order in this courtroom!
&SET_POSE:Thinking
Hmmm. That's a bold accusation you've raised, Arin. As well as a terrible pun on Ross' name.
&SET_POSE:Surprised
But how could this be true? Didn't Ross find them in the first place?
&SET_POSE:Normal
We saw him find them in Jory's backpack. Why would Ross find them himself if he was the one who stole them?

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SET_POSE:Confident
&SPEAK:Tutorial_Boy
The answer to that is simple, Your Honor! It's because he DIDN'T steal them!
The defense is simply grasping at straws!
They know Jory is guilty, so they're throwing out accusations to confuse the jury!
&SET_POSE:Angry
How dare you sully the name of JUSTICE by proposing this preposterous position!

&HIDE_TEXTBOX
// OBJECTION:Arin
&PAN_TO_POSITION:1,{doublePanTime}
&PLAY_EMOTION:ShakingHead
&SET_POSE:Normal
&SPEAK:Arin
We have no decisive evidence that it was either Jory or Ross, Your Honor!
All we have is eyewitness accounts from someone who himself could have been the culprit!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Regardless of that fact, I need proof or evidence that suggests that Ross was the culprit.

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
&SPEAK:Tutorial_Boy
HAH! Good luck proving that, "Video Game Boy". I'd like to see you try!

&STOP_SONG
&SET_POSE:Confident,Arin
&PAN_TO_POSITION:1,{doublePanTime}
&SPEAK:Arin
I'll do better than that...
&PLAY_SFX:objectionclean
&PLAY_SONG:dragonObjection
&SET_POSE:Objection
...because I'm the one who wins!

&JUMP_TO_POSITION:3
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:stab
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
Not that joke again...

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
Hell yea dude, check this shit out!

-> PresentEvidence

=== PresentEvidence ===

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:Confident
&HIDE_TEXTBOX
&PRESENT_EVIDENCE
+ [Wrong]
    -> PresentEvidence
+ [Ross]
    -> Continue

=== Continue ===

// &TAKE_THAT:Arin
&SPEAK:Arin
These are the Goodboy Coins found inside the backpack!
Notice how they're bent and scuffed now? Don't you realize what that means?

&STOP_SONG
&PLAY_SFX:recordScratch
&SCENE:TMPH_Assistant
&SET_POSE:SideNormal
That we're cheap bastards making coins out of cardboard?

&SCENE:TMPH_Court
&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&SPEAK:Arin
Well--

&JUMP_TO_POSITION:3
&SET_POSE:Normal
&SPEAK:Tutorial_Boy
I second that notion.

&JUMP_TO_POSITION:1
&PLAY_SFX:stab
&SET_POSE:DeskSlam
&SPEAK:Arin
Now, that was uncalled f--

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
I mean, just use some quarters and paint them up. Clearly we don't care about being fancy.

&PLAY_SFX:damage2
&SCENE:TMPH_Court
&SET_POSE:Sweaty
&SPEAK:Arin
OK! I get it, I'll get REAL coins after this.
&SET_POSE:Normal
Regardless, Jory would never let his coins be ruined like this!
&PLAY_SONG:dragonObjection
&SET_POSE:Point
There is no way he would have stuffed the dinosaurs so carelessly with those coins there!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
GUH!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:3
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
&SPEAK:Tutorial_Boy
RIDICULOUS! I don't believe a word of that! There's no way someone would be so careful around such pointless coins.

&HIDE_TEXTBOX
&JUMP_TO_POSITION:1
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
That might be true for most people, but not our Jory!
&SET_POSE:Point
We've already heard evidence that proves Jory cared about those coins, and I've got it right here!

-> PresentLivestreamRecording

=== PresentLivestreamRecording ===

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:Point
&HIDE_TEXTBOX
&PRESENT_EVIDENCE
+ [Wrong]
    -> PresentLivestreamRecording
+ [Ross]
    -> Continue2

=== Continue2 ===

&TAKE_THAT:Arin
&SET_POSE:PaperSlap
&SPEAK:Arin
Recall Jory's testimony, how he was noted to be taking care of those coins carefully!
&SET_POSE:Thinking
Now I ask the court to consider this: Why would Jory take such care of his coins only to trash them in his backpack minutes later?
&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SCENE:Anime
&ACTOR:Arin
&PLAY_SFX:objectionClean
&SET_POSE:CloseUp
The answer is simple: HE DIDN'T PUT THE DINOS IN THERE AT ALL!

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&HIDE_TEXTBOX
&PLAY_SFX:stab2
&PLAY_EMOTION:DamageNoHelmet
&SPEAK:Ross
OOF!
&HIDE_TEXTBOX
&PLAY_SFX:stab2
// &PLAY_EMOTION:DamageNoHelmet
ARGH!

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
Hmm... that makes a lot of sense. I, personally, have seen Jory care for those coins like they were his children.
&SET_POSE:Normal
I'm not sure how you polish cardboard... but regardless, Jory's care of those coins cannot be denied.
Does the prosecution have anything to say about this?

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
Well... uh...

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&THINK:Arin
(Yeah, let's see you sneak your way out of this one, dawg!)

&HIDE_TEXTBOX
&STOP_SONG
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
&SPEAK:Tutorial_Boy
YES! There is an explanation!

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&THINK:Arin
(Come ON dude!)

&PLAY_SONG:investigationUniCore
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Clearly, my witness made an error! The dinos were simply found in a different part of the backpack.
When he found them, Mr. O'Donovan simply was so shocked that Jory would do such a thing...
...that he mistook which pocket they were found in.

&JUMP_TO_POSITION:2
&PLAY_SFX:realization
&SPEAK:Ross
...!
&SET_POSE:Sweaty
Oh yeah, I remember now! I looked in the front pocket first and saw the coins.
I realized that there was no room, so I checked the side pocket and that's where I found the dinos!

&HIDE_TEXTBOX
//&OBJECTION:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
That's fucking bullshit, Your Honor! That's not what he said in his testimony.

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
Hmm... That does seem like a reach.
&SET_POSE:Normal
There were a lot of people in the area at the time. Someone must have seen where the dinos were found in the backpack.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Well, why don't we ask them?
Esteemed court members who were present when Ross found the dinos...
Did ANY of you see which part of the backpack they were found in?

&SCENE:TMPH_WideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:gavel
&PLAY_ANIMATION:TripleGavelHit

&SCENE:TMPH_Judge
&SET_POSE:Angry
&SPEAK:Brent_Judge
ORDER! ORDER I SAY!

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&THINK:Arin
(I think Brent just likes hitting that gavel and yelling...)

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
It is as I said, Your Honor. Ross simply mistook which pocket he found them in.
The coins were damaged at some point between Jory placing them inside the backpack and the dinos being found.
Most likely, Ross bent them while searching for the dinos Jory had put into his backpack!

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:Brent_Judge
Hmm... I could see that happening, certainly.
&SET_POSE:Normal
And since there is nobody here that can refute this point, I have to side with the prosecution.

&STOP_SONG
&HIDE_TEXTBOX
&SHAKE_SCREEN:0.25,0.25
&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&PLAY_SFX:stab2
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
Are you freaking kidding me?!?

&SCENE:TMPH_Judge
&SET_POSE:Angry
&SPEAK:Brent_Judge
HOWEVER.
I would like the witness to revise his testimony to reflect this change.
Let me be clear: This is the LAST time I will allow this.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Such a wise judgement, Your Honor! Your years of judicial experience shine brightly today!

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&THINK:Arin
(This is it, then. I have to get him here. For Jory!)

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
You got this, dude. We can't give up now!

&HIDE_TEXTBOX
&FADE_OUT:3
&WAIT:2

-> END