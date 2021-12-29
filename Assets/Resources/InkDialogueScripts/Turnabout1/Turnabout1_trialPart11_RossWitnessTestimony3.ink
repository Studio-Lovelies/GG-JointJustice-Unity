INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../SceneInitialization.ink

<- COURT.TMPH

&SCENE:TMPH_Court
&JUMP_TO_POSITION:2
&SET_POSE:SweatyNoHelm
&FADE_IN:1
&PLAY_SONG:fyiIWannaXYourExaminationAllegro
&PLAY_ANIMATION:WitnessTestimony

&SPEAK:None
&APPEAR_INSTANTLY
<color={orange}><align=center>-- Witness's Account --

&SPEAK:Ross
Yeah, so... After I heard the dinos went missing, I remembered that I saw Jory go back to the recording space.
S-so I went back there to search his backpack for the dinos.
I first searched the front pocket, where I saw the coins in the bag.
But when I saw they weren't there, I, uh... I turned the backpack to check the side pocket! Yeah!
When I opened up the left side pocket, that's where I found the dinosaurs! Y-yes, that's exactly how it happened!

&HIDE_TEXTBOX
&PLAY_SONG:None
&FADE_OUT:1
&SCENE:TMPH_Judge
&SET_POSE:Thinking
&FADE_IN:1
&SPEAK:Brent_Judge
So you searched the backpack in more than one place and found the dinos.
&SET_POSE:Normal
Alright, I'll accept this.
Arin, you may cross examine the witness now.

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Man! What do we do now?

&SPEAK:Arin
I'm not sure, but there's something fishy about this whole thing, and I'm gonna change his tun-a!

&SET_POSE:SideNormalTurned
&SPEAK:Dan
Glad you're bringing your A-game, Arin.

&HIDE_TEXTBOX
&FADE_OUT:1

-> END