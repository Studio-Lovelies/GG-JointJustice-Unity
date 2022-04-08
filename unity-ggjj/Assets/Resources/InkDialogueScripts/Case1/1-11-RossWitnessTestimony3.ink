INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Templates/Macros.ink
INCLUDE StartingEvidence.ink

<- Part5StartingEvidence
<- COURT_TMPH

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SET_POSE:SweatyNoHelmet
&FADE_IN:1
&PLAY_SONG:FyiIWannaXYourExaminationAllegro

<- WitnessTestimony

&SPEAK:Ross
Yeah, so... After I heard the dinos went missing, I remembered that I saw Jory go back to the recording space.
S-so I went back there to search his backpack for the dinos.
I first searched the front pocket, where I saw the coins in the bag.
But when I saw they weren't there, I, uh... I turned the backpack to check the side pocket! Yeah!
When I opened up the left side pocket, that's where I found the dinosaurs! Y-yes, that's exactly how it happened!

&END_WITNESS_TESTIMONY
&HIDE_TEXTBOX
&FADE_OUT_SONG:2
&FADE_OUT:2
&SCENE:TMPHJudge
&SET_POSE:Thinking
&FADE_IN:2
&SPEAK:JudgeBrent
So you searched the backpack in more than one place and found the dinos.
&SET_POSE:Normal
Alright, I'll accept this.
Arin, you may cross examine the witness now.

&SCENE:TMPHAssistant
&SET_POSE:Fist
&SPEAK:Dan
Man! What do we do now?

&SPEAK:Arin
I'm not sure, but there's something fishy about this whole thing, and I'm gonna change his tun—a!

&SET_POSE:SideNormalTurned
&SPEAK:Dan
Glad you're bringing your A-game, Arin.

&HIDE_TEXTBOX
&FADE_OUT:2

&LOAD_SCRIPT:Case1/1-12-RossCrossExamination4

-> END