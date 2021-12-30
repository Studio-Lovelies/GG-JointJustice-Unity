INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&JUMP_TO_POSITION:2
&FADE_IN:1
&WAIT:1

&PLAY_SONG:fyiIWannaXYourExaminationModerato

&PLAY_ANIMATION:CrossExamination

&SPEAK:None
&APPEAR_INSTANTLY
<align=center><color={orange}>-- Witness' Account --

-> Line1

=== Line1 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>So, I saw Jory walk by my office.
+ [Continue]
    -> Line2
+ [Press]
    -> Line1Press

=== Line2 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>But I suddenly had to use the bathroom right away!
+ [Continue]
    -> Line3
+ [Press]
    -> Line2Press

=== Line3 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>I ran to use the nearby bathroom. On my way back, I saw Jory stashing the dinos away.
+ [Continue]
    -> Line4
+ [Press]
    -> Line3Press
+ [Plumber_Invoice]
    -> Present

=== Line4 ===
&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
<color=green>I didn't think anything of it, that's why I forgot to mention it earlier!
+ [Continue]
    -> Line1
+ [Press]
    -> Line4Press

=== Line1Press ===
# &HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
So you saw Jory while you were busy animating?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Well, I saw someone move past my office door out of the corner of my eye.
It was only after I got up and left my office that I realized it was Jory.

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Please tell the court the reason you had to get up and leave your office.
&JUMP_TO_POSITION:2

-> Line2

=== Line2Press ===
# &HOLD_IT:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
You 'suddenly' had to poop? Seems a little convenient, don't you think?

&JUMP_TO_POSITION:2
&SET_POSE:Glaring
&SPEAK:Ross
You're one to talk! YOU have to suddenly poop all the time!

&SCENE:TMPH_Assistant
&SPEAK:Dan
He's got you there, bro.

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
You have quite a reputation with 'making bears', Arin. I think you should let this one go.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&SPEAK:Arin
Uh... right...

-> Line3

=== Line3Press ===
# &HOLD_IT
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
What were you doing by the office toilet, Ross?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Well obviously I'd just finished taking a massive dump. Because that's the reason the toilet is there.

&JUMP_TO_POSITION:1
&SET_POSE:Embarrassed
&SPEAK:Arin
Hahah... of course. Stupid question, I guess.

&SPEAK:Dan
Hold on, Arin.

&SCENE:TMPH_Court
&SET_POSE:Thinking
&SPEAK:Arin
What's up, Dan?

&SCENE:TMPH_Assistant
&SPEAK:Dan
Something about that doesn't seem right, but... I dunno...
Maybe we should look at the <color={red}>evidence</color>, see if there's something about this?

-> Line4

=== Line4Press ===
# &HOLD_IT
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
POSE Arin Point
&SPEAK:Arin
You didn't think pooping was important enough to mention?

&JUMP_TO_POSITION:2
&SET_POSE:Glaring
&SPEAK:Ross
No, of course not! Not everyone has memorable bathroom trips like you, Arin.

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&SPEAK:Arin
Yeah, my shits are pretty legendary. Point taken.

-> Line1

=== Present ===

# &OBJECTION:Arin
&STOP_SONG
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
Not so fast, Jafar! I-I mean, Ross!
Are you absolutely certain that you had just used the office toilet?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Of course I am.

&JUMP_TO_POSITION:1
&SET_POSE:PaperSlap
&SHOW_ITEM:Plumber_Invoice,Right
&SPEAK:Arin
Then why does this plumbing invoice state very clearly...
&PLAY_SONG:dragonObjection
..that the toilets were undergoing maintenance at that time?
&HIDE_ITEM

&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&HIDE_TEXTBOX
&PLAY_EMOTION:Damage
&SET_POSE:Sweaty
&SPEAK:Ross
W-What's that now?

&JUMP_TO_POSITION:1
&SET_POSE:Point
&SPEAK:Arin
I have an invoice from the plumbers who were working on that bathroom earlier today!
&SET_POSE:PaperSlap
They had just finished working on that very bathroom when we found out about Jory being accused.

&HIDE_TEXTBOX
&PLAY_SFX:deskslam
&PLAY_EMOTION:DeskSlamAnimation
That means you couldn't have possibly been using that bathroom. And you couldn't have seen Jory!

&HIDE_TEXTBOX
&SCENE:TMPH_WideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&SCENE:TMPH_Judge
&SET_POSE:Warning
&SPEAK:Brent_Judge
Order in the court!
Witness, what do you have to say about this?

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SET_POSE:Sweaty
&SPEAK:Ross
Uh, I.. er...
Well, you see... the thing is...

# &OBJECTION:Tutorial_Boy

&HIDE_TEXTBOX
&STOP_SONG
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
&SPEAK:Tutorial_Boy
I think it's time to reveal the truth, Ross.

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&SPEAK:Brent_Judge
What's that you say?

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Oh, I do not like where this is going.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SPEAK:Ross
The... truth?

&PLAY_SONG:GGJJROSS

&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Remember? You told me all about it, how you wanted to keep it a surprise?
The Mario Maker level you were working on? I think you need to come clean!

&JUMP_TO_POSITION:2
&SPEAK:Ross
...
&PLAY_SFX:realization
!
&SET_POSE:Sweaty
I... guess you're right! Guess I can't hide it any longer.

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
What the hey are you even talking about?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Well... the truth is, I wasn't animating anything at all.
I was actually working on a secret level for you guys.

&JUMP_TO_POSITION:1
&PLAY_SFX:stab
&SET_POSE:Sweaty
&SPEAK:Arin
What...?

&JUMP_TO_POSITION:2
&SET_POSE:Sad
&SPEAK:Ross
I know, I shouldn't have lied about it, but I wanted it to be a surprise!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
This changes things quite a bit, you know.
&SET_POSE:Normal
Tutorial Boy, I think we need to redo your witness' testimony again in light of this new information.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Yes, of course, Your Honor.

&HIDE_TEXTBOX
&JUMP_TO_POSITION:1
&PLAY_SFX:stab
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
What? Again?

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
I know it's unprofessional, but Ross tends to have good intentions.
Therefore, I'm willing to give him another chance. Besides, how could I say no to that face?

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SET_POSE:Sad
&SPEAK:Ross
\*whimper\*.

&JUMP_TO_POSITION:1
&THINK:Arin
(Lord spare me this crap.)

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
This time, Ross, I need the honest truth from you. You may begin your testimony.

&HIDE_TEXTBOX
&FADE_OUT:1
&PLAY_SONG:None
&WAIT:3

-> END