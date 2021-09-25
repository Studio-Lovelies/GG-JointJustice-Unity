//Do these before fading in, sets up the scene

&SCENE:TMPH_Judge
&ACTOR:Brent_Judge

&SCENE:TMPH_Assistant
&ACTOR:Dan

&SCENE:TMPH_Court
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:2,Ross
&SET_ACTOR_POSITION:3,Tutorial_Boy

-> Line1

=== Line1 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">I was animating by myself over in my room at the office.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">But then... I saw someone taking the dinos!!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
+ [Continue]
    -> Line4
+ [Press]
    -> Finale

=== Line4 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">Now that I know they were stolen, that means the culprit must be Jory!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press


=== Line1Press ===
&PLAYSFX:holdItArin
&WAIT:0.2
&PAN_TO_POSITION:1,0.5
&WAIT:0.5
&SPEAK:Arin
What were you animating?

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Your Honor, this is clearly irrelevant to the case.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
I agree. Arin, try being serious about this.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Ross, continue your testimony.

-> Line2


=== Line2Press ===
&PLAYSFX:holdItArin
&WAIT:0.2
&PAN_TO_POSITION:1,0.5
&WAIT:0.5
&SPEAK:Arin
Who did you see?

&JUMP_TO_POSITION:2
&SPEAK:Ross
I'm getting to it, just be patient. I'm trying to build suspense for the viewers!

&JUMP_TO_POSITION:1
&SPEAK:Arin
But this isn't being broadcasted...

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Quick! Back to the testimony before we break the fourth wall again!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Witness, carry on.
-> Line3

=== Line4Press ===
&PLAYSFX:holdItArin
&WAIT:0.2
&PAN_TO_POSITION:1,0.5
&WAIT:0.5
&SPEAK:Arin
What makes you so sure that the dinos were stolen, anyways!?

&JUMP_TO_POSITION:2
&SPEAK:Ross
...

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
...

&SCENE:TMPH_Assistant
&SPEAK:Dan
Arin, that's literally the reason we're all here.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SPEAK:Arin
...

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
I'll just pretend that didn't happen.
-> Line1



=== Finale ===
&PLAYSFX:objectionArin
&PAN_TO_POSITION:1,0.5
&PLAY_EMOTION:Objection
&SPEAK:Arin
You said you saw Jory in the 10 Minute Power Hour room, correct?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Yes, that's correct!

&JUMP_TO_POSITION:1
&SET_POSE:PaperSlap
&SPEAK:Arin
Yet you also say you were in your office animating
&SET_POSE:Confident
Seems very odd to me! How could you see anyone while you were focused on your work!

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Are you saying that my witness is a liar?
I'm sure Ross has a very reasonable explanation for all this.

&SCENE:TMPH_Assistant
&SPEAK:Dan
He oughta have a real good reason for this.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
T-That's right! The reason I was able to see Jory was... because I needed to poop!
Yeah!

&JUMP_TO_POSITION:1
&SET_POSE:Annoyed
&SPEAK:Arin
Um... excuse me?

&SCENE:TMPH_Assistant
&SPEAK:Dan
Hah hah hah hah!!!

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&THINK:Arin
(Goddamnit, Ross.)

&SPEAK:Arin
What does you needing to poop have to do with seeing Jory?

&JUMP_TO_POSITION:2
&SPEAK:Ross
W-Well, you see, I had to go out to use the bathroom, which is how I saw Jory!

&JUMP_TO_POSITION:1
&SPEAK:Arin
Uh-huh...
&SET_POSE:Normal
Your Honor, I believe this needs to be added to the witness's testimony.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Agreed. Witness, add your poop story to your testimony.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
Uh... Yes, why of course, Your Honor. Let me go over it again.

&SCENE:TMPH_Assistant
&SPEAK:Dan
Way to go, Big Cat! Let's see how this changes things.
->END