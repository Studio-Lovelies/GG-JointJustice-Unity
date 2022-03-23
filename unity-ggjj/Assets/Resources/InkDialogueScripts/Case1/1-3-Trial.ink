INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink

<- COURT_TMPH

&FADE_OUT:0
&SET_ACTOR_POSITION:2,Jory

&DIALOGUE_SPEED:0.08
&NARRATE
<color=green><align=center>Some Undisclosed Date, 3:30PM<br>10 Minute Power Hour Courthouse

&HIDE_TEXTBOX
&PLAY_SFX:mutter
&SCENE:TMPHWideShot
&FADE_IN:0
&WAIT:2
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&PLAY_SONG:aBoyAndHisTrial

&SCENE:TMPHJudge
&DIALOGUE_SPEED:0.04
&SPEAK:JudgeBrent
Court is now in session for the trial of Jory Griffis.

&SCENE:TMPHCourt
&SPEAK:Arin
The defense is ready, Your H-

&PLAY_SFX:stab
&HIDE_TEXTBOX
&PLAY_EMOTION:ShockAnimation
&PLAY_SFX:realization
&SET_POSE:Sweaty
Wait, Brent? You're the judge here?

&PLAY_SFX:lightbulb
&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Yes, being the manager of Game Grumps is just my side hustle.
My main job is presiding as judge over the Attitude City Courthouse.
&SET_POSE:Surprised
After all, what is a judge if not a manager of the law?
&SET_POSE:Normal
So I will preside over this case for you all today.

&ADD_RECORD:JudgeBrent

&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
Well, I suppose that's fine. I mean, Brent is pretty fair in general.

&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:smack
&DIALOGUE_SPEED:0.1
&SPEAK_UNKNOWN:TutorialBoy
AHEM!
&CONTINUE_DIALOGUE
&DIALOGUE_SPEED:0.05
The prosecution is also ready, Your Honor.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Oh, right! I have a guest prosecutor to help this go over smoothly.
He just happened to be visiting this week.

&SPEAK_UNKNOWN:TutorialBoy
Mr. Hanson and Mr. Avidan... So we meet at last!

&SCENE:TMPHCourt
&THINK:Arin
<color=lightblue>(Who exactly is this? He seems familiar but I can't put my finger on how...)

&SCENE:TMPHAssistant
&SPEAK:Dan
Wait, doesn't he kind of look like...

&PLAY_SFX:DramaPound
&SCENE:TMPHCourt
&HIDE_TEXTBOX
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
T- Trivia Boy?!

&HIDE_TEXTBOX
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:damage1
&JUMP_TO_POSITION:3
&PLAY_EMOTION:HeadSlam
&PLAY_SFX:tutorialBoyWrong
&SET_POSE:Angry
&SPEAK_UNKNOWN:TutorialBoy
WRONG!
&SET_POSE:Normal
&SPEAK:TutorialBoy
I am Tutorial Boy! Trivia Boy is my brother...

&ADD_RECORD:TutorialBoy

And after what you did to him and his reputation for trivia...
&SET_POSE:Angry
I will BRING YOU ALL to JUSTICE, Grumps!
&SET_POSE:Normal
&PLAY_SONG:tutorialBoysTragicallyGenericReminiscence
&DIALOGUE_SPEED:0.06
But first, I will regale you a tale about my tragic past.<br>One gruesome night in 1984, my d--

&PLAY_SFX:recordScratch
&STOP_SONG
&JUMP_TO_POSITION:1
&SET_POSE:Annoyed
&DIALOGUE_SPEED:0.02
&SPEAK:Arin
Yeap, uh-huh, got it, tragic backstory tutorial blah blah blah. Can we get this show on the road?

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&DIALOGUE_SPEED:0.04
&SPEAK:TutorialBoy
Don't you dare skip me! I'm the-

&JUMP_TO_POSITION:1
&SPEAK:Arin
Man, if the artists drew me yawning, that's what you'd be seeing right now.
&SET_POSE:Normal
When the heck is this trial going to start?

&PLAY_SONG:aBoyAndHisTrial
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:damage2
&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
IT WILL BEGIN NOW!
&SET_POSE:Normal
The prosecution will now give their opening statement!

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SET_POSE:Normal
&FADE_OUT_SONG:2
&SPEAK:TutorialBoy
Of course, Your Honor.

&SCENE:TMPHAssistant
&SPEAK:Dan
This is easily the stupidest thing I've ever done.

&PLAY_SONG:logicAndTrains
&SCENE:TMPHCourt
&SPEAK:TutorialBoy
At about 12:00 PM, the Game Grumps and crew were partaking in a livestream.
A recording of the livestream COULD be added to the court record, if you riddle m-

&JUMP_TO_POSITION:1
&SET_POSE:Annoyed
&SPEAK:Arin
Yeah, yeah, I know. Press 'Z' to see the court record. Could you get to the point, please?

&PLAY_SFX:stab
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
W-Well, you don't have to be so rude about it! Fine, it's been added to the court record.

&PLAY_SFX:evidenceDing
&ADD_EVIDENCE:Livestream_Recording
&SHOW_ITEM:Livestream_Recording,Left
&DIALOGUE_SPEED:0.06
&NARRATE
<align=center><color={lightBlue}>The Livestream Recording has been added to the Court Record.
&PLAY_SFX:evidenceShoop
&HIDE_ITEM
&WAIT:0.1

&SET_POSE:Normal
&SPEAK:TutorialBoy
Now... where was I?
&DIALOGUE_SPEED:0.04
Oh yes. The livestream.
During the livestream, according to the transcript here, the Switch they were using suddenly failed.
It was at this point that the defendent, Jory Griffis, volunteered to go get a replacement one.
When the livestream was over, the crew went to prepare for a 10 Minute Power Hour episode.
However, when they arrived and started preparing, they noticed the Dinos were missing!
A quick search and a few minutes later, they were suspiciously found in THIS backpack!

&PLAY_SFX:evidenceDing
&ADD_EVIDENCE:Jorys_Backpack
&SHOW_ITEM:Jorys_Backpack,Left
&DIALOGUE_SPEED:0.06
&NARRATE
<align=center><color={lightBlue}>Jory's Backpack has been added to the Court Record.
&PLAY_SFX:evidenceShoop
&HIDE_ITEM
&WAIT:0.1

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&DIALOGUE_SPEED:0.04
&SPEAK:Arin
The heck is that <color={red}>white stain</color> there?

&SCENE:TMPHAssistant
&SET_POSE:SideLaughing
&SPEAK:Dan
Maybe Jory took the NSP song “Objects of Desire” as inspiration.<br>Know what I mean?

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SET_POSE:Angry
&SPEAK:TutorialBoy
Unfortunately, we ALL know what you mean, “Mr. Sexbang”...

&SCENE:TMPHAssistant
&SET_POSE:Normal
&SPEAK:Dan
That's “Mr. Business” to you, sir.

&SCENE:TMPHCourt
&DIALOGUE_SPEED:0.06
&SPEAK:TutorialBoy
Yes...<br>Quite...<br>Indeed.
As I was saying...
&SET_POSE:Normal
&DIALOGUE_SPEED:0.04
The missing dinos, while quickly found, put a big delay on setting up for the Power Hour.
I have been informed that recording cannot begin without them. As such they are critical pieces of evidence!

&PLAY_SFX:evidenceDing
&ADD_EVIDENCE:Stolen_Dinos
&SHOW_ITEM:Stolen_Dinos,Left
&NARRATE
&DIALOGUE_SPEED:0.06
<align=center><color={lightBlue}>The Stolen Dinos have been added to the Court Record
&PLAY_SFX:evidenceShoop
&HIDE_ITEM
&WAIT:0.1

&DIALOGUE_SPEED:0.04
&SPEAK:TutorialBoy
While Mr. Griffis' backpack is somewhat unique--
--the prosecution deemed it necessary to prove its owner's identity.
In the very same pocket in which the dinos were found, we discovered some Good Boy Coins!
These were the very same coins Jory was polishing during the livestream.
This was confirmed by other members of the Grump team present at the time the dinos were discovered.
This clearly suggests that the perpetrator is the defendant!
I am here to prove beyond any doubt that Jory Griffis stole the dinosaurs...
&SET_POSE:Angry
...in order to sabotage the Ten Minute Power Hour!

&FADE_OUT_SONG:2
&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
Hm... Yes, that seems to be a very solid opening statement.
&SET_POSE:Normal
You may call your first witness, Mr. Boy.
&PLAY_SONG:aBoyAndHisTrial

&SCENE:TMPHCourt
&SET_POSE:Normal
&SPEAK:TutorialBoy
Yes, Your Honor. I would now like to call said defendant, Jory Griffis, to the stand!

&OBJECTION:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{doublePanTime}
&SPEAK:Arin
Wait, you can't do that! It violates the Fifth Amendment!

&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
Yeah! Wait, is this even a real courthouse?

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Keep your pants on. I make up the rules in this courthouse. I will allow the testifying of the defendant.

&HIDE_TEXTBOX
&FADE_OUT_SONG:2
&FADE_OUT:2
&WAIT:3

&LOAD_SCRIPT:Case1/1-4-Trial