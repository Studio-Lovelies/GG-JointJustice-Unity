//Do these before fading in, sets up the scene
&FADE_OUT:0

&SCENE:TMPHWitness
&ACTOR:Ross

&SCENE:TMPHDefense
&ACTOR:Arin

&SCENE:TMPHProsecution
&ACTOR:TutorialBoy

&SCENE:TMPHJudge
&ACTOR:JudgeBrent

&SCENE:TMPHAssistant
&ACTOR:Dan

&SCENE:TMPHWitness
&PLAYSONG:FyiIWannaXYourExaminationAllegro
//Set up done

&FADE_IN:1

-> Line1

=== Line1 ===
&SCENE:TMPHWitness
&SPEAK:Ross
<color="green">I was animating by myself over in my room at the office.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SCENE:TMPHWitness
&SPEAK:Ross
<color="green">But then... I saw someone taking the dinos!!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SCENE:TMPHWitness
&SPEAK:Ross
<color="green">It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
+ [Continue]
    -> Line4
+ [Press]
    -> Finale

=== Line4 ===
&SCENE:TMPHWitness
&SPEAK:Ross
<color="green">Now that I know they were stolen, that means the culprit must be Jory!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press
+ [Attorneys_Badge] //Shouldn't be here, just for testing purposes
    &SCENE:TMPHWitness
    &SPEAK:Ross
    Oi m8, that's a noice bit of stuff roit thare!
    
    &SCENE:TMPHDefense
    &SPEAK:Arin
    Thanks bud!
    -> Line1


=== Line1Press ===
&SCENE:TMPHDefense
&SPEAK:Arin
What were you animating?

&SCENE:TMPHProsecution
&SPEAK:TutorialBoy
Your Honor, this is clearly irrelevant to the case.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I agree. Arin, try being serious about this.

&SCENE:TMPHProsecution
&SPEAK:TutorialBoy
Ross, continue your testimony.

-> Line2


=== Line2Press ===
&SCENE:TMPHDefense
&SPEAK:Arin
Who did you see?

&SCENE:TMPHWitness
&SPEAK:Ross
I'm getting to it, just be patient. I'm trying to build suspense for the viewers!

&SCENE:TMPHDefense
&SPEAK:Arin
But this isn't being broadcasted...

&SCENE:TMPHProsecution
&SPEAK:TutorialBoy
Quick! Back to the testimony before we break the fourth wall again!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Witness, carry on.
-> Line3

=== Line4Press ===
&SCENE:TMPHDefense
&SPEAK:Arin
What makes you so sure that the dinos were stolen, anyways!?

&SCENE:TMPHWitness
&SPEAK:Ross
...

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
...

&SCENE:TMPHAssistant
&SPEAK:Dan
Arin, that's literally the reason we're all here.

&SCENE:TMPHDefense
&SPEAK:Arin
...

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
I'll just pretend that didn't happen.
-> Line1



=== Finale ===
&SCENE:TMPHDefense
&SPEAK:Arin
You said you saw Jory in the 10 Minute Power Hour room, correct?

&SCENE:TMPHWitness
&SPEAK:Ross
Yes, that's correct!

&SCENE:TMPHDefense
&SPEAK:Arin
Yet you also say you were in your office animating

&SCENE:TMPHDefense
&SPEAK:Arin
Seems very odd to me! How could you see anyone while you were focused on your work!

&SCENE:TMPHProsecution
&SPEAK:TutorialBoy
Are you saying that my witness is a liar?
I'm sure Ross has a very reasonable explanation for all this.

&SCENE:TMPHAssistant
&SPEAK:Dan
He oughta have a real good reason for this.

&SCENE:TMPHWitness
&SPEAK:Ross
T-That's right! The reason I was able to see Jory was... because I needed to poop!
Yeah!

&SCENE:TMPHDefense
&SPEAK:Arin
Um... excuse me?

&SCENE:TMPHAssistant
&SPEAK:Dan
Hah hah hah hah!!!

&SCENE:TMPHDefense
&THINK:Arin
(Goddamnit, Ross.)

&SCENE:TMPHDefense
&SPEAK:Arin
What does you needing to poop have to do with seeing Jory?

&SCENE:TMPHWitness
&SPEAK:Ross
W-Well, you see, I had to go out to use the bathroom, which is how I saw Jory!

&SCENE:TMPHDefense
&SPEAK:Arin
Uh-huh...
Your Honor, I believe this needs to be added to the witness's testimony.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Agreed. Witness, add your poop story to your testimony.

&SCENE:TMPHWitness
&SPEAK:Ross
Uh... Yes, why of course, Your Honor. Let me go over it again.

&SCENE:TMPHAssistant
&SPEAK:Dan
Way to go, Big Cat! Let's see how this changes things.
&HIDE_TEXTBOX
&FADE_OUT:1
->END