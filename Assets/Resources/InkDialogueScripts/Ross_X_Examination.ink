-> Line1

=== Line1 ===
&ACTOR:Ross
&SPEAK:Ross
<color="green">I was animating by myself over in my room at the office.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&ACTOR:Ross
&SPEAK:Ross
<color="green">But then... I saw someone taking the dinos!!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&ACTOR:Ross
&SPEAK:Ross
<color="green">It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
+ [Continue]
    -> Line4
+ [Press]
    -> Finale

=== Line4 ===
&ACTOR:Ross
&SPEAK:Ross
<color="green">Now that I know they were stolen, that means the culprit must be Jory!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press
+ [Attorneys_Badge] 
    &ACTOR:Ross //This shouldn't be read
    &SPEAK:Ross
    Oi m8, that's a noice bit of stuff roit thare!
    
    &ACTOR:Arin
    &SPEAK:Arin
    Thanks bud!
    -> Line1


=== Line1Press ===
&ACTOR:Arin
&SPEAK:Arin
What were you animating?

&ACTOR:Tutorial_Boy
&SPEAK:Tutorial_Boy
Your Honor, this is clearly irrelevant to the case.

&ACTOR:Brent_Judge
&SPEAK:Brent_Judge
I agree. Arin, try being serious about this.

&ACTOR:Tutorial_Boy
&SPEAK:Tutorial_Boy
Ross, continue your testimony.

-> Line2


=== Line2Press ===
&ACTOR:Arin
&SPEAK:Arin
Who did you see?

&ACTOR:Ross
&SPEAK:Ross
I'm getting to it, just be patient. I'm trying to build suspense for the viewers!

&ACTOR:Arin
&SPEAK:Arin
But this isn't being broadcasted...

&ACTOR:Tutorial_Boy
&SPEAK:Tutorial_Boy
Quick! Back to the testimony before we break the fourth wall again!

&ACTOR:Brent_Judge
&SPEAK:Brent_Judge
Witness, carry on.
-> Line3

=== Line4Press ===
&ACTOR:Arin
&SPEAK:Arin
What makes you so sure that the dinos were stolen, anyways!?

&ACTOR:Ross
&SPEAK:Ross
...

&ACTOR:Brent_Judge
&SPEAK:Brent_Judge
...

&ACTOR:Dan
&SPEAK:Dan
Arin, that's literally the reason we're all here.

&ACTOR:Arin
&SPEAK:Arin
...

&ACTOR:Brent_Judge
&SPEAK:Brent_Judge
I'll just pretend that didn't happen.
-> Line1



=== Finale ===
&ACTOR:Arin
&SPEAK:Arin
You said you saw Jory in the 10 Minute Power Hour room, correct?

&ACTOR:Ross
&SPEAK:Ross
Yes, that's correct!

&ACTOR:Arin
&SPEAK:Arin
Yet you also say you were in your office animating

&ACTOR:Arin
&SPEAK:Arin
Seems very odd to me! How could you see anyone while you were focused on your work!

&ACTOR:Tutorial_Boy
&SPEAK:Tutorial_Boy
Are you saying that my witness is a liar?
I'm sure Ross has a very reasonable explanation for all this.

&ACTOR:Dan
&SPEAK:Dan
He oughta have a real good reason for this.

&ACTOR:Ross
&SPEAK:Ross
T-That's right! The reason I was able to see Jory was... because I needed to poop!
Yeah!

&ACTOR:Arin
&SPEAK:Arin
Um... excuse me?

&ACTOR:Dan
&SPEAK:Dan
Hah hah hah hah!!!

&ACTOR:Arin
&THINK:Arin
(Goddamnit, Ross.)

&ACTOR:Arin
&SPEAK:Arin
What does you needing to poop have to do with seeing Jory?

&ACTOR:Ross
&SPEAK:Ross
W-Well, you see, I had to go out to use the bathroom, which is how I saw Jory!

&ACTOR:Arin
&SPEAK:Arin
Uh-huh...
Your Honor, I believe this needs to be added to the witness's testimony.

&ACTOR:Brent_Judge
&SPEAK:Brent_Judge
Agreed. Witness, add your poop story to your testimony.

&ACTOR:Ross
&SPEAK:Ross
Uh... Yes, why of course, Your Honor. Let me go over it again.

&ACTOR:Dan
&SPEAK:Dan
Way to go, Big Cat! Let's see how this changes things.
->END