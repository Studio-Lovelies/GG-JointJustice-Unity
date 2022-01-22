INCLUDE ../Colors.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&JUMP_TO_POSITION:2
&FADE_OUT:0
&PLAY_SONG:aBoyAndHisTrial
&FADE_IN:1
&WAIT:1

&JUMP_TO_POSITION:1
&PLAY_SFX:dramaPound
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
Ross?! They roped you into this as well?

&JUMP_TO_POSITION:2
&SPEAK:Ross
Yeah... It looked important, you know? Plus, I could use the extra money.

&SCENE:TMPH_Judge
&SET_POSE:Surprised
&PLAY_SFX:lightbulb
&SPEAK:Brent_Judge
Mr. O'Donovan, being a witness isn't a paying job...

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Not that anyone here is getting paid anyway...

&JUMP_TO_POSITION:2
&SET_POSE:Sweaty
&SPEAK:Ross
I-I see...

&JUMP_TO_POSITION:3
&SET_POSE:Confident
&SPEAK:Tutorial_Boy
Except if you count being paid in JUSTICE!
&PLAY_SFX:damage1
&HIDE_TEXTBOX
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
Ahem... Witness, please state your name and occupation for the court.

&JUMP_TO_POSITION:2
&SET_POSE:Normal
&DIALOGUE_SPEED:0.06
&SPEAK:Ross
<size=40>...Kangaroo court if I ever saw one...

&SCENE:TMPH_Judge
&DIALOGUE_SPEED:0.04
&SPEAK:Brent_Judge
What was that?

&SCENE:TMPH_Court
&SET_POSE:Sweaty
&SPEAK:Ross
Nothing, Your Honor!
&SET_POSE:Normal
I am Ross O'Donovan: local animator, Mario Maker enthusiast, apparent sadist, and <color={red}>friend to all here!</color>

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Some friend... testifying against Jory, treating him like a criminal...

&SPEAK:Arin
What do you mean? Even WE don't know if Jory is innocent!

&SET_POSE:Normal
&SPEAK:Dan
Arin, have you learned nothing from the Penix Wright<sup>(tm)</sup> playthrough?

&SPEAK:Arin
That a gavel, lubed properly, has many uses?

&SHAKE_SCREEN:0.25,0.2
&SET_POSE:Fist
&PLAY_SFX:smack
&DIALOGUE_SPEED:0.02
&SPEAK:Dan
No!
&SET_POSE:SideLaughing
&DIALOGUE_SPEED:0.04
Well, I mean yeah... The “gay-liff” in that game sure was creative.
&SET_POSE:Normal
But more importantly, you should go into every case with confidence that your client is innocent.
We don't know if he's guilty, but if his own attorney doesn't believe him, why should the rest of the court?

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&JUMP_TO_POSITION:1
&PLAY_SFX:deskSlam
&SHAKE_SCREEN:0.25,0.2
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
You're right! We're here to defend our friend, so we should at least assume we're making the right call.

&SCENE:TMPH_Assistant
&SPEAK:Dan
Also, knowing how these things go, the defendant is always innocent...

&SET_POSE:SideLean
&DIALOGUE_SPEED:0.06
...Usually, anyway.

&SCENE:TMPH_Judge
&SET_POSE:Warning
&DIALOGUE_SPEED:0.04
&SPEAK:Brent_Judge
If the defense is done sucking each other's toes, may we begin with Mr. O'Donovan's testimony?

&SCENE:TMPH_Court
&SET_POSE:Sweaty
&THINK:Arin
<color=lightblue>(Why does he act like he doesn't know anyone here?)

&HIDE_TEXTBOX
&PLAY_EMOTION:Nodding
&SET_POSE:Normal
&SPEAK:Arin
We're ready, Your Honor.

&SCENE:TMPH_Judge
&SET_POSE:Normal
&SPEAK:Brent_Judge
Alright. The witness may begin his testimony.

&HIDE_TEXTBOX
&FADE_OUT:2
&PLAY_SONG:None
&WAIT:3

-> END