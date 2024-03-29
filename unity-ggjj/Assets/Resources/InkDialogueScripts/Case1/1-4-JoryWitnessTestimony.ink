INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Templates/Macros.ink
INCLUDE StartingEvidence.ink

<- COURT_TMPH
<- Part4StartingEvidence

&SET_ACTOR_POSITION:Witness,Jory
&JUMP_TO_POSITION:Witness
&SET_POSE:Nervous

&PLAY_SONG:logicAndTrains,{songFadeTime}
&FADE_IN:2

&PAN_TO_POSITION:Prosecution,{panTime}
&DIALOGUE_SPEED:0.06
&SPEAK:Tutorial_Boy
&AUTO_SKIP:True
Witness, state y-
&AUTO_SKIP:False

&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:supershock
&JUMP_TO_POSITION:Witness
&SET_POSE:Sweaty
&DIALOGUE_SPEED:0.02
&SPEAK:Jory
I'm sorry! Please forgive me!!!

&JUMP_TO_POSITION:Defense
&SET_POSE:Sweaty
&PLAY_SFX:stab
&SPEAK:Arin
Jory, WHAT THE HECK!

&JUMP_TO_POSITION:Witness
&SET_POSE:Nervous
&DIALOGUE_SPEED:0.06
&SPEAK:Jory
Ah, jeez{ellipsis}

&JUMP_TO_POSITION:Defense
&SPEAK:Arin
Just take a deep breath and answer the questions.

&JUMP_TO_POSITION:Witness
&SPEAK:Jory
O-Okay{ellipsis}

&JUMP_TO_POSITION:Defense
&DIALOGUE_SPEED:0.04
&THINK:Arin
<color=lightblue>(This is gonna be rough, I can already tell{ellipsis})

&HIDE_TEXTBOX
&PAN_TO_POSITION:Prosecution,{doublePanTime}
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&PLAY_SFX:stab2
&DIALOGUE_SPEED:0.02
&SPEAK:Tutorial_Boy
WITNESS!

&JUMP_TO_POSITION:Witness
&SET_POSE:Sweaty
&SPEAK:Jory
AHHH!

&JUMP_TO_POSITION:Prosecution
&SET_POSE:Normal
&DIALOGUE_SPEED:0.04
&SPEAK:Tutorial_Boy
I have questions about the time before the incident, and you will answer!

&JUMP_TO_POSITION:Witness
&SET_POSE:Nervous
&DIALOGUE_SPEED:0.06
&SPEAK:Jory
Ah{ellipsis} Jeez{ellipsis} Okay.

&JUMP_TO_POSITION:Prosecution
&DIALOGUE_SPEED:0.04
&SPEAK:Tutorial_Boy
When the console broke, you yourself were the first to volunteer to go get a backup console, were you not?

&JUMP_TO_POSITION:Witness
&SET_POSE:Thinking
&SPEAK:Jory
Well, it's sort of my job to help with technical issues, but-

&HIDE_TEXTBOX
&PAN_TO_POSITION:Prosecution,{panTime}
&SPEAK:Tutorial_Boy
So you ADMIT that you jumped on the opportunity to be the one to do the grunt work for the Grumps?
How suspicious! Surely there were other, less busy people who could have gotten it, but you didn't hesitate!

&JUMP_TO_POSITION:Witness
&SET_POSE:Nervous
&SPEAK:Jory
&AUTO_SKIP:True
Yeah{ellipsis} I guess I didn't{ellipsis} but again it's part of my jo-
&AUTO_SKIP:False

&HIDE_TEXTBOX
&PAN_TO_POSITION:Prosecution,{panTime}
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&DIALOGUE_SPEED:0.02
&SPEAK:Tutorial_Boy
AH-HAH!

&JUMP_TO_POSITION:Witness
&SET_POSE:Sweaty
&PLAY_SFX:stab
&SPEAK:Jory
YIKES!!!

&JUMP_TO_POSITION:Defense
&DIALOGUE_SPEED:0.04
&THINK:Arin
<color=lightblue>(Dang! This guy is intense!)

&JUMP_TO_POSITION:Prosecution
&SPEAK:Tutorial_Boy
Suspicious behavior indeed!
Especially since,  according to the livestream recording here, you were busy cleaning your Good Boy Coins.
Is this correct?

&SCENE:TMPH_Assistant
&SET_POSE:Fist
&SPEAK:Dan
Dude, what do his coins have to do with this? And what's with this guilt tripping over being helpful?
&SET_POSE:Angry
You should say something, Arin.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:Defense
&SET_POSE:Thinking
&THINK:Arin
Hm{ellipsis}
&HIDE_TEXTBOX

+ [Object!]
    -> playerObjects
+ [Nah, we gucci]
    -> playerDoesntObject

=== playerObjects ===

&OBJECTION:Arin
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
My client's behavior during the livestream isn't suspicious in the least!
Jory is a great employee who works hard for the team! The prosecution is twisting the truth here, Your Honor!

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
Hm{ellipsis}
&SET_POSE:Normal
Objection sustained. Mr. Boy, you will refrain from “flowery language” when questioning your witness.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:Prosecution
&SET_POSE:Sweaty
&SPEAK:Tutorial_Boy
Er{ellipsis} Y-Yes, Your Honor. The spirit of justice took hold of me and I got carried away.

&JUMP_TO_POSITION:Defense
&SPEAK:Arin
Also{ellipsis}
-> Continue

=== playerDoesntObject ===

&SPEAK:Arin
Nah, it's fine. Polishing some coins doesn't prove he's guilty.
&DIALOGUE_SPEED:0.06
However{ellipsis}
-> Continue

=== Continue ===

&SET_POSE:Annoyed
&DIALOGUE_SPEED:0.06
Please don't slam your head into the desk again.
It's making everyone nervous.

&HIDE_TEXTBOX
&OBJECTION:Tutorial_Boy
&PAN_TO_POSITION:Prosecution,{doublePanTime}
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&DIALOGUE_SPEED:0.04
&SPEAK:Tutorial_Boy
NEVER! It's my only way of objecting!

&JUMP_TO_POSITION:Defense
&SET_POSE:Annoyed
&SPEAK:Arin
You can't be serious.

&SCENE:TMPH_Judge
&SET_POSE:Normal
&SPEAK:JudgeBrent
Objection sustained. I'll allow it.

&SCENE:TMPH_Court
&JUMP_TO_POSITION:Defense
&SET_POSE:DeskSlam
&SPEAK:Arin
Your Honor! That kind of behavior isn't appropriate in this courtroom!

&SCENE:TMPH_Judge
&SPEAK:JudgeBrent
Perhaps, but I get to make the call, and it's amusing to watch him do it.

&HIDE_TEXTBOX
&SCENE:TMPH_Court
&JUMP_TO_POSITION:Prosecution
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam

&SCENE:TMPH_Judge
&SPEAK:JudgeBrent
{ellipsis}Very amusing.
The prosecution may continue with the witness' testimony.

&SCENE:TMPH_Court
&SPEAK:Tutorial_Boy
Thank you, Your Honor.
So, you were cleaning your coins during the livestream, which we have on record.
When you were asked to go get a replacement Switch, what did you do with those coins?

&HIDE_TEXTBOX
&SET_POSE:Thinking,Jory
&PAN_TO_POSITION:Witness,{panTime}
&SPEAK:Jory
Well, since I had just finished polishing them, I decided to put them away in my backpack{ellipsis}
and leave it in the 10 Minute Power Hour recording room since I knew I had to do setup for that right after.

&JUMP_TO_POSITION:Prosecution
&SPEAK:Tutorial_Boy
Would those coins happen to be{ellipsis}
&SHAKE_SCREEN:0.25,0.2
&SET_POSE:Angry
&PLAY_SFX:shock2
&DIALOGUE_SPEED:0.02
THESE!?!?

<- AddEvidence("BentCoins")

&PLAY_SFX:stab
&JUMP_TO_POSITION:Witness
&SET_POSE:Sweaty
&SPEAK:Jory
Uh{ellipsis} Uh{ellipsis}

&JUMP_TO_POSITION:Prosecution
&SET_POSE:Normal
&DIALOGUE_SPEED:0.04
&SPEAK:Tutorial_Boy
As stated before, these coins were found in the same pocket as the dinosaurs{ellipsis}
{ellipsis}and they fit the description of the coins the witness just gave! What more proof do you need!?

&SHAKE_SCREEN:0.25,0.2
&SET_POSE:Angry
&PLAY_SFX:supershock
&DIALOGUE_SPEED:0.02
CONFESS TO YOUR CRIMES, JORY!

&JUMP_TO_POSITION:Witness
&DIALOGUE_SPEED:0.06
&SPEAK:Jory
I{ellipsis} ER{ellipsis} UH{ellipsis} Oh man, yeah, those are{ellipsis} My coins{ellipsis} B-but that-

&JUMP_TO_POSITION:Prosecution
&SET_POSE:Confident
&DIALOGUE_SPEED:0.04
&SPEAK:Tutorial_Boy
Is that a confession I hear? You admit your guilt!?

&HIDE_TEXTBOX
&OBJECTION:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:Defense,{doublePanTime}
&SPEAK:Arin
The prosecution is hectoring the witness!
The coins being in the pocket with the dinos doesn't directly prove he put them both there!

&HIDE_TEXTBOX
&SET_POSE:Angry,Tutorial_Boy
&PAN_TO_POSITION:Prosecution,{doublePanTime}
&SPEAK:Tutorial_Boy
What? Just look at the witness! His sweatiness and nervous behavior is that of a criminal

&SCENE:TMPH_Judge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
What the defense says holds up{ellipsis}
&SET_POSE:Normal
{ellipsis}so I will sustain their objection.

&SCENE:TMPH_Court
&HIDE_TEXTBOX
&PLAY_EMOTION:Yeeta
&SET_POSE:Normal
&SPEAK:Tutorial_Boy
Very well. But don't think you're out of hot water yet.
You said you were doing setup for something right after the livestream.
Please tell the court what that <color={red}>something</color> was going to be.

&JUMP_TO_POSITION:Witness
&SET_POSE:Thinking
&SPEAK:Jory
Well{ellipsis} You see, we were planning on doing the 10 Minute Power Hour right after the livestream.
And I had to help set up since we were planning on doing something{ellipsis}
&SET_POSE:Nervous
{ellipsis}different.

&JUMP_TO_POSITION:Prosecution
&SPEAK:Tutorial_Boy
And what was this something that was so different?

&JUMP_TO_POSITION:Witness
&SET_POSE:Normal
&SPEAK:Jory
Well, to put it simply, we were going to do a dunk tank contest--
--but instead of water it was going to be Strawberries and Cream.
&SET_POSE:Thinking
Something about Ninja Party School{ellipsis}? I don't know, it was Dan and Arin's idea{ellipsis}
&SET_POSE:Nervous
But I wasn't really looking forward to being dunked in{ellipsis} cream{ellipsis}

&JUMP_TO_POSITION:Defense
&SET_POSE:Sweaty
&THINK:Arin
<color=lightblue>(Don't say that! You're gonna get yourself into more trouble!)

&JUMP_TO_POSITION:Prosecution
&SET_POSE:Confident
&SPEAK:Tutorial_Boy
And so you decided to sabotage the episode's production{ellipsis}
{ellipsis}so you wouldn't have to suffer through such a menial and humiliating task.
&SET_POSE:Angry
I see right through you, Jory Griffis!

&JUMP_TO_POSITION:Witness
&SET_POSE:Sweaty
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:stab
&SPEAK:Jory
N-No! I would never-

&HIDE_TEXTBOX
&JUMP_TO_POSITION:Prosecution
&PLAY_SFX:damage1
&SHAKE_SCREEN:0.25,0.2
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&SPEAK:Tutorial_Boy
Save your lies! This is a courtroom of truth!

&HIDE_TEXTBOX
&OBJECTION:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:Defense,{doublePanTime}
&SPEAK:Arin
There is no proof that Jory is lying!

&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SET_POSE:DeskSlam
You're simply pushing a false narrative to make my client look bad!

&HIDE_TEXTBOX
&SET_POSE:Confident,Tutorial_Boy
&PAN_TO_POSITION:Prosecution,{doublePanTime}
&SPEAK:Tutorial_Boy
Oh ho, I assure you, this is no bravado. We have true motivation.
We have evidence that connects Jory to the crime, and{ellipsis}

&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:smack
&SET_POSE:Angry
We have a witness to Jory's crime!

&HIDE_TEXTBOX
&SCENE:TMPH_WideShot
&WAIT:2
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&SCENE:TMPH_Judge
&SET_POSE:Angry
&SPEAK:JudgeBrent
That will be enough from the jury!
&SET_POSE:Surprised
You say you have a witness to the crime itself?

&SCENE:TMPH_Court
&SET_POSE:Normal
&SPEAK:Tutorial_Boy
That is correct, Your Honor! I have simply been building my case up to this point.
Members of the court, you see that Jory knows he is guilty, and his behavior is proof of that!
But now you will see that, with my next witness, there can be no doubt of Mr. Griffis' guilt!
I call to the stand a witness to the crime: Ross O'Donovan.

&ADD_RECORD:Ross

&HIDE_TEXTBOX
&FADE_OUT:2
&FADE_OUT_SONG:2
&WAIT:3

&LOAD_SCRIPT:Case1/1-5-RossWitnessTestimony

-> END