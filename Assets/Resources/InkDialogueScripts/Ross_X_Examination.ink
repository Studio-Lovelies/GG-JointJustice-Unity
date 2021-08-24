-> Line1

=== Line1 ===
&SPEAK:Ross
I was animating by myself over in my room at the office. #Textbox accepts markup. Do coloring via action or by adding markup to the spoken line?
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SPEAK:Ross
But then... I saw someone taking the dinos!!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SPEAK:Ross
It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
+ [Continue]
    -> Line4
+ [Press]
    -> Finale

=== Line4 ===
&SPEAK:Ross
Now that I know they were stolen, that means the culprit must be Jory!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press
+ [UnoReverse] #evidence example, not actually used
    &SPEAK:Ross
    ... Wait, you can't do that here! We're playing bridge!
    
    &SPEAK:Arin
    Oh yeah, my b.
    -> Line1


=== Line1Press ===
&SPEAK:Arin
What were you animating?

&SPEAK:TB
Your Honor, this is clearly irrelevant to the case.

&SPEAK:Brent_Judge
I agree. Arin, try being serious about this.

&SPEAK:TB
Ross, continue your testimony.

-> Line2


=== Line2Press ===
&SPEAK:Arin
Who did you see?

&SPEAK:Ross
I'm getting to it, just be patient. I'm trying to build suspense for the viewers!

&SPEAK:Arin
But this isn't being broadcasted...

&SPEAK:TB
Quick! Back to the testimony before we break the fourth wall again!

&SPEAK:Brent_Judge
Witness, carry on.
-> Line3

=== Line4Press ===
&SPEAK:Arin
What makes you so sure that the dinos were stolen, anyways!?

&SPEAK:Ross
...

&SPEAK:Brent_Judge
...

&SPEAK:Dan
Arin, that's literally the reason we're all here.

&SPEAK:Arin
...

&SPEAK:Brent_Judge
I'll just pretend that didn't happen.
-> Line1



=== Finale ===
&SPEAK:Arin
You said you saw Jory in the 10 Minute Power Hour room, correct?

&SPEAK:Ross
Yes, that's correct!

&SPEAK:Arin
Yet you also say you were in your office animating

&SPEAK:Arin
Seems very odd to me! How could you see anyone while you were focused on your work!

&SPEAK:TB
Are you saying that my witness is a liar?
I'm sure Ross has a very reasonable explanation for all this.

&SPEAK:Dan
He oughta have a real good reason for this.

&SPEAK:Ross
T-That's right! The reason I was able to see Jory was... because I needed to poop!
Yeah!

&SPEAK:Arin
Um... excuse me?

&SPEAK:Dan
Hah hah hah hah!!!

&THINK:Arin
(Goddamnit, Ross.)

&SPEAK:Arin
What does you needing to poop have to do with seeing Jory?

&SPEAK:Ross
W-Well, you see, I had to go out to use the bathroom, which is how I saw Jory!

&SPEAK:Arin
Uh-huh...
Your Honor, I believe this needs to be added to the witness's testimony.

&SPEAK:Brent_Judge
Agreed. Witness, add your poop story to your testimony.

&SPEAK:Ross
Uh... Yes, why of course, Your Honor. Let me go over it again.

&SPEAK:Dan
Way to go, Big Cat! Let's see how this changes things.
->END