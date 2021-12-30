INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&JUMP_TO_POSITION:2
&FADE_IN:1
&WAIT:1

&PLAY_SONG:fyiIWannaXYourExaminationModerato

&PLAY_ANIMATION:WitnessTestimony

&SPEAK:None
&APPEAR_INSTANTLY
<align=center><color={orange}>-- Witness' Account --

&SPEAK:Ross
I guess you got me. I did lie about what I was doing.
I was actually making a special Mario Maker level for you guys.
You've always been good sports about my troll levels, so I wanted to make you a nice one for a change!
I was working on it, thinking about what to make next, when I saw Jory walk by my office.
I felt it was a good time to take a break, so I went to see what he was up to.
That's when I saw it! He had taken the dinos and stuffed them into his backpack!

&HIDE_TEXTBOX
&PLAY_SONG:None
&FADE_OUT:3
&WAIT:1

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&FADE_IN:3

&SET_POSE:Normal
&SPEAK:Brent_Judge
Hmm. So you were in your office working on a Mario Maker level but needed a break.
That's when you saw Jory, followed him, and saw the dinosaurs being taken.
That seems reasonable to me.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:3
&SPEAK:Tutorial_Boy
Without a doubt, Your Honor. That is how it really happened.
We -- er, I mean, my witness, wanted to keep it a surprise.
&SET_POSE:Angry
I hope the defense is happy with themselves for ruining Ross' great gesture!

&HIDE_TEXTBOX
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&SPEAK:Arin
Ho boy...
&SET_POSE:Thinking
What do you think, Dan?

&SCENE:TMPH_Assistant
&SET_POSE:SideNormal
&SPEAK:Dan
Yeah... I can't really see any holes in his claim...

&SCENE:TMPH_Court
&SET_POSE:DeskSlam
&SPEAK:Arin
Damn...

&PLAY_SFX:lightbulb
&SET_POSE:Thinking
Wait! I've got an idea!

&SPEAK:Dan
What is it?

&HIDE_TEXTBOX
&PLAY_SFX:deskslam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:ARin
I'll just BS my way through by questioning everything he said!

&SPEAK:Dan
I don't know, man. Do you really think that will work?

&SET_POSE:Embarrassed
&SPEAK:Arin
Who knows? I mean, it's worked for everything else I've ever done...

&SCENE:TMPH_Assistant
&SPEAK:Dan
If you say so...
&SET_POSE:Angry
&AUTO_SKIP:true
Wait, what do you mean every--

&HIDE_TEXTBOX
&AUTO_SKIP:false
&SCENE:TMPH_Court
&PLAY_SFX:DeskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
LET'S DO IT!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
If the defense would like to cross examine now...?

&SCENE:TMPH_Court
&SET_POSE:Point
&SPEAK:Arin
You bet your sweet bippie I would, Your Honor! I've got some questions that need answering!

&SCENE:TMPH_Judge
&SPEAK:Brent_Judge
Very well. The defense may begin their cross-examination.

&HIDE_TEXTBOX
&PLAY_SONG:None
&FADE_OUT:3
&WAIT:1

-> END









