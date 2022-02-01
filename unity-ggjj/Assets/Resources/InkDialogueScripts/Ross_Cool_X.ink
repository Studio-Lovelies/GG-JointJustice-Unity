&MODE:CrossExamination
//Do these before fading in, sets up the scene

&ADD_EVIDENCE:BentCoins
&SCENE:TMPHJudge
&ACTOR:JudgeBrent

&SCENE:TMPHAssistant
&ACTOR:Dan

&SCENE:TMPHCourt
&SET_ACTOR_POSITION:1,Arin
&SET_ACTOR_POSITION:2,Ross
&SET_ACTOR_POSITION:3,TutorialBoy

-> Line1

=== Line1 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">I was animating by myself over in my room at the office.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">But then... I saw someone taking the dinos!!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
+ [Continue]
    -> Line4
+ [Press]
    -> Finale

=== Line4 ===
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color="green">Now that I know they were stolen, that means the culprit must be Jory!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press


=== Line1Press ===
&PLAY_SFX:HoldItArin
&WAIT:0.2
&PAN_TO_POSITION:1,0.5
&WAIT:0.5
&SPEAK:Arin
What were you animating?

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
Your Honor, this is clearly irrelevant to the case.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I agree. Arin, try being serious about this.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
Ross, continue your testimony.

-> Line2


=== Line2Press ===
&PLAY_SFX:HoldItArin
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
&SPEAK:TutorialBoy
Quick! Back to the testimony before we break the fourth wall again!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Witness, carry on.
-> Line3

=== Line4Press ===
&PLAY_SFX:HoldItArin
&WAIT:0.2
&PAN_TO_POSITION:1,0.5
&WAIT:0.5
&SPEAK:Arin
What makes you so sure that the dinos were stolen, anyways!?

&JUMP_TO_POSITION:2
&SPEAK:Ross
...

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
...

&SCENE:TMPHAssistant
&SPEAK:Dan
Arin, that's literally the reason we're all here.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SPEAK:Arin
...

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I'll just pretend that didn't happen.
-> Line1



=== Finale ===
&PLAY_SFX:ObjectionArin
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
&SPEAK:TutorialBoy
Are you saying that my witness is a liar?
I'm sure Ross has a very reasonable explanation for all this.

&SCENE:TMPHAssistant
&SPEAK:Dan
He oughta have a real good reason for this.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
T-That's right! The reason I was able to see Jory was... because I needed to poop!
Yeah!

&JUMP_TO_POSITION:1
&SET_POSE:Annoyed
&SPEAK:Arin
Um... excuse me?

&SCENE:TMPHAssistant
&SPEAK:Dan
Hah hah hah hah!!!

&SCENE:TMPHCourt
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

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Agreed. Witness, add your poop story to your testimony.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SPEAK:Ross
Uh... Yes, why of course, Your Honor. Let me go over it again.

&SCENE:TMPHAssistant
&SPEAK:Dan
Way to go, Big Cat! Let's see how this changes things.
->END