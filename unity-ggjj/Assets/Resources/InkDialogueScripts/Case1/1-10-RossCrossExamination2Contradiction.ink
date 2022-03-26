INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink

<- COURT_TMPH

&ADD_RECORD:Arin
&ADD_RECORD:Dan
&ADD_RECORD:Jory
&ADD_RECORD:JudgeBrent
&ADD_RECORD:TutorialBoy
&ADD_RECORD:Ross

&ADD_EVIDENCE:AttorneysBadge
&ADD_EVIDENCE:PlumberInvoice
&ADD_EVIDENCE:Switch
&ADD_EVIDENCE:Jory_Srs_Letter
&ADD_EVIDENCE:LivestreamRecording
&ADD_EVIDENCE:JorysBackpack
&ADD_EVIDENCE:StolenDinos
&ADD_EVIDENCE:BentCoins

&JUMP_TO_POSITION:2
&SPEAK:Ross
I was using my Switch! Duh! Like, how ELSE could I be making it?

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
That's the best question you can come up with? Laughable!

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&THINK:Arin
(Jeez, this guy is so annoying!)

&PLAY_SFX:realization
(...!)

&SET_POSE:Confident
&SPEAK:Arin
As a matter of fact, it IS the best question!

&PLAY_SONG:dragonObjection,{songFadeTime}

&SET_POSE:Point
And it's the only question I need to see your lies for what they are!

&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&JUMP_TO_POSITION:2
&SET_POSE:Sweaty
&SPEAK:Ross
W-W-What?

&JUMP_TO_POSITION:1
&SET_POSE:Point
&SPEAK:Arin
There's no possible way you could have been using your Switch when you said you were!

&HIDE_TEXTBOX
&SCENE:TMPHWideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit

&SCENE:TMPHJudge
&SET_POSE:Angry
&SPEAK:JudgeBrent
You say that he could not have been using his Switch as he claims?

&SCENE:TMPHCourt
&SET_POSE:DeskSlam
&SPEAK:Arin
That's correct, Your Honor!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:3
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Impossible! There's no way you can prove such a thing!

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
I CAN prove it... with THIS!

-> PresentEvidence

=== PresentEvidence ===
&PRESENT_EVIDENCE
+ [Wrong]
    -> PresentEvidence
+ [Switch]
    -> PresentSwitch
    
=== PresentSwitch ===
&TAKE_THAT:Arin
&SHOW_ITEM:Switch,Right

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
A... Nintendo Switch?

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
That's right. Remember how you told everyone that Jory went to get us a switch when ours broke?
Well it just so happens that I have this switch... RIGHT HERE!

&HIDE_TEXTBOX
&HIDE_ITEM
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:stab
&JUMP_TO_POSITION:2
&PLAY_EMOTION:Damage
&SPEAK:Ross
GAH!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
NO!
That can't be!
&HIDE_TEXTBOX
&WAIT:2
&SET_POSE:Normal
...
&STOP_SONG
Wait a second, how does that help you?

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Does the prosecution have an... objection?

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Oh right, my mistake.
&OBJECTION:TutorialBoy
&SET_POSE:Angry
This is a complete waste of time, Your Honor!
Not only is there no proof that Switch belongs to Ross...
...but there's no proof this is even the Switch Jory brought back!

&OBJECTION:Arin
&PAN_TO_POSITION:1,{doublePanTime}
&SPEAK:Arin
If you wouldn't interrupt, I'd show you all the proof you'll need!
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
Your Honor, if I may explain.

&SCENE:TMPHJudge
&SET_POSE:Normal
&SPEAK:JudgeBrent
Objection Overruled. The defense will proceed. I want to hear their explanation on this new piece of evidence.

&SCENE:TMPHCourt
&SET_POSE:Normal
&SPEAK:Arin
Thank you, Your Honor.
&PLAY_SONG:logicAndTrains,{songFadeTime}
Look closely on the back of this particular Switch.
&SHOW_ITEM:Switch,Right
You'll see that on the back there is clearly a Slimemantha<sup>(tm)</sup> sticker on it!
In case you don't know, Slimemantha<sup>(tm)</sup> is an original character made by Ross himself!
Not only that, but these stickers aren't yet available to the public...
&SET_POSE:Point
...meaning only Ross himself could have them!

&HIDE_ITEM
&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
Wow, dude. How did you even know about that?

&SCENE:TMPHCourt
&SPEAK:Arin
Easy. I've been helping produce merch for Ross. Isn't that right?

&JUMP_TO_POSITION:2
&SET_POSE:Sweaty
&SPEAK:Ross
Well... I...

&JUMP_TO_POSITION:1
&SET_POSE:Normal
&SPEAK:Arin
That's what I thought!
&PLAY_SONG:dragonObjection,{songFadeTime}
&SET_POSE:Point
Since this is clearly Ross' switch, his claim that he was making levels earlier today is a bunch of garbage!

&SCENE:TMPHAssistant
&SPEAK:Dan
Damn dude your deduction skills are incredible today!

&SCENE:TMPHCourt
&SET_POSE:Confident
&SPEAK:Arin
Don't you know, Dan? I'm the Video Game Boy...
&PLAY_SFX:objectionClean
&SET_POSE:Objection
I'm the one who's winning this case!

&OBJECTION:TutorialBoy
&JUMP_TO_POSITION:3
&SET_POSE:Angry
&SPEAK:TutorialBoy
Preposterous! It's too early to celebrate! That simply isn't enough proof on it's own.
Anyone could have put those stickers on their switch, not just Ross.
&SET_POSE:Normal
Your Honor, this is baseless conjecture.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Well I do happen to recognize that switch as Ross' being that I work with him every day.
And, being the manager, I know that these stickers aren't available to anyone but the Grumps.

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Urp...!

&STOP_SONG
&SCENE:TMPHJudge
&SPEAK:JudgeBrent
That being said, while I'm certain this switch does belong to Ross, I can't simply take the defense at their word.
I need further proof that this was indeed the switch Jory obtained for the livestream.
Mr. Hanson, Mr. Avidan, do you think you can provide that proof?

&SCENE:TMPHAssistant
&SET_POSE:Fist
&SPEAK:Dan
Damn, I have no idea dude. How can we prove that?

&SPEAK:Arin
Don't worry Dan, I've got this. And you know when I say I got it, I mean I GOT IT ;)

&SET_POSE:SideNormalTurned
&SPEAK:Dan
I dunno man, are you sure? Usually when you're this confident you tend to blow it immediately.

&SPEAK:Arin
Don't worry, Dan. Check this shit out.

-> PlayerChoice

=== PlayerChoice ===

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Normal
+ [Show them what games are on the Switch]
    -> Wrong1
+ [Show them the last game played on the Switch]
    -> Right
+ [SHOW EM UR TIIIIITS]
    -> Wrong2
    
=== Wrong1 ===

&TAKE_THAT:Arin
&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:PaperSlap
&SPEAK:Arin
This is every single game downloaded on the Switch!

&OBJECTION:TutorialBoy
&SET_POSE:Normal,TutorialBoy
&PAN_TO_POSITION:3,{doublePanTime}
&SPEAK:TutorialBoy
What exactly are you trying to prove here?
&SET_POSE:Confident
All we know from that is what games are on there! Nothing else!

&SCENE:TMPHAssistant
&SET_POSE:SideLean
&SPEAK:Dan
There are some interesting games on here though...
&SET_POSE:SideNormal
Er... Ross... what exactly is 'Mew Mew Cutie Girls Club'?

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SCREEN_SHAKE:0.25,0.25
&PLAY_SFX:stab
&SET_POSE:Sweaty
&SPEAK:Ross
Th-That's nothing! It's ironic, I-I swear!

&JUMP_TO_POSITION:1
&SET_POSE:Annoyed
&SPEAK:Arin
What HAVE you been playing?

&JUMP_TO_POSITION:2
&SET_POSE:Glaring
&SPEAK:Ross
Hey - don't act like you're better than me, with your Pantsu Hunter and your weird anime dating sim crap!

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&SPEAK:Arin
H-Hey! That's totally irrelevant! You can't judge me!

&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit
&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
Ahem... if we could get back to the case, you two?
&SET_POSE:Normal
The games on Ross' Switch does not have any bearing on this case.

&SCENE:TMPHCourt
&SET_POSE:Thinking
&THINK:Arin
(Damn... I thought I had the right answer there.)

-> PlayerChoice

=== Wrong2 ===

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
The answer will become clear, when those titties are near!
&PLAY_SFX:recordScratch
&STOP_SONG

&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
...You do realize this is a court of law, right?
&PLAY_SONG:logicAndTrains,{songFadeTime}
The titty showing will have to wait until AFTER the trial.

&SCENE:TMPHAssistant
&SET_POSE:SideLean
&SPEAK:Dan
Hey, now we've got something to look forward to when you win!

&SCENE:TMPHCourt
&SET_POSE:Thinking
&SPEAK:Arin
Wait... Will this change the age rating?

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
LET'S MOVE ON BEFORE WE FIND OUT!

-> PlayerChoice

=== Right ===

&TAKE_THAT:Arin
&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:PaperSlap
&SPEAK:Arin
If you've ever owned a Switch, you'd know that the last game played...
Always sits on the far left-hand side of the screen!
That way you have quick and easy access to the game you were most recently playing.

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
That's very true! I've noticed it on my own Switch.

&SCENE:TMPHCourt
&SET_POSE:Normal
&SPEAK:Arin
Exactly! And what do we see when we load Ross' Switch to the home screen?
Well, if the prosecution's claim that Ross was making a Mario Maker level with his Switch is true...
Then Mario Maker would be the game you see on the left hand side of the screen!

&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
But if you'd take a look at Ross' Switch, Your Honor...
&SET_POSE:Point
I'm willing to bet that is NOT the game you'll find!
Your Honor, would you please tell this court what game is actually there?

&SCENE:TMPHJudge
&SET_POSE:Normal
&SPEAK:JudgeBrent
Yes, let me see.
&HIDE_TEXTBOX
&SET_POSE:Thinking
&SHOW_ITEM:Switch,Right
&WAIT:1
&PLAY_SONG:ninjaSexPursuit,{songFadeTime}
&PLAY_SFX:realization
&SET_POSE:Surprised
Penix Wright: Facial Attorney<sup>(tm)</sup>?!
&HIDE_ITEM

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&PLAY_EMOTION:Damage

&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Absurd!

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
That's right! If Ross was truly making a Mario Maker level, it would be Mario Maker there!
That means the fact that Penix Wright is there...
&JUMP_TO_POSITION:2
&SET_POSE:Glaring
...was because it was the game we were playing on the livestream!

&HIDE_TEXTBOX
&STOP_SONG
&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&PLAY_EMOTION:Damage
&SET_POSE:Glaring
&SPEAK:Ross
AUGH...
&HIDE_TEXTBOX
&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
UURGH!
&HIDE_TEXTBOX
&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
A--A-A-AAAHAAAAAA!!!
&HIDE_TEXTBOX
&PLAY_EMOTION:HelmetThrow

&JUMP_TO_POSITION:1
&PLAY_SFX:smack
&PLAY_EMOTION:HelmetHit
&SET_POSE:Sweaty
&SPEAK:Arin
OWWW-WUH!

&JUMP_TO_POSITION:2
&SET_POSE:MadMilk
&SPEAK:Ross
You... guys...
&PLAY_SFX:stab
&PLAY_SONG:ninjaSexPursuit,{songFadeTime}
Grrr!!! You're all lying!
You just want to protect Jory!
He's the culprit, I tell you!
I saw him! He's the only one who could have taken it!

&HIDE_TEXTBOX
&PAN_TO_POSITION:1,{panTime}
&PLAY_EMOTION:DeskSlamAnimation
&PLAY_SFX:deskSlam
&SPEAK:Arin
The only one lying here is you, Ross!
&PLAY_SFX:objectionClean
&SET_POSE:Objection
So tell us the truth! What really happened?!

&OBJECTION:TutorialBoy
&PLAY_SONG:confessionPatrol,{songFadeTime}
&PAN_TO_POSITION:3,{doublePanTime}
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Angry
&SPEAK:TutorialBoy
It's just as the witness said! There's no proof that any of these claims are true!
There's no conclusive evidence that the Switch they used was actually Ross'.
I request that last bit of evidence be stricken from the record!
&HIDE_TEXTBOX
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:damage2
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
The only hard evidence we have shows the dinosaurs in Jory's backpack! Who else but him would have done it?!

&JUMP_TO_POSITION:2
&SPEAK:Ross
YEAH, MAN! I'm the one looking out for you guys here, I've done nothing wrong!
I just was mistaken about a few things! That's no way to treat one of your oldest friends!

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
Hmmm... the prosecution does bring up a point.
&STOP_SONG
&HIDE_TEXTBOX
&WAIT:2
&SET_POSE:Normal
Under these circumstances I cannot refute what the prosecution says. Objection sustained.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Point
&SPEAK:Arin
Your Honor, we've just put the witness' credibility in the furnace!
We can't rely on what he said when many things were just now proven to have glaring contradictions.

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Be that as it may, unless the defense has evidence of someone else more likely to have committed the crime than Jory...
I have no choice but to overrule your objection.

&SCENE:TMPHCourt
&SET_POSE:DeskSlam
&THINK:Arin
(Damn, how could I let that slip through my fingers?)
(Sorry, Jory, I did my best…)
&HIDE_TEXTBOX
&WAIT:1

&PLAY_SFX:lightbulb
&SPEAK:Dan
Wait, Arin! It's not over yet!

&SCENE:TMPHAssistant
&SET_POSE:SideNormal
All we have to do is show that someone else could have done it, right?
Well, there were only two people who weren't with us during the livestream today.
One was Jory, and the other...

&HIDE_TEXTBOX
&PLAY_SFX:realization
&SCENE:TMPHCourt
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
You're right, Dan!
&PLAY_SFX:objectionClean
&SET_POSE:Objection
Your Honor! There is one other person who could have done the deed!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
!!!

&JUMP_TO_POSITION:3
&PLAY_SFX:stab
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
What?!

&SCENE:TMPHJudge
&SET_POSE:Surprised
&SPEAK:JudgeBrent
Another person? And who could that be?
Please point out exactly who you are accusing.

-> Accuse

=== Accuse ===

&PRESENT_EVIDENCE
+ [Wrong]
    -> Accuse
+ [Ross]
    -> AccuseRoss

=== AccuseRoss ===

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
Why, the only other person who wasn't at the livestream other than Jory!
&SET_POSE:Point
ROSS O'DIDITVAN!

&SCENE:TMPHWideShot
&PLAY_SFX:mutter
&WAIT:2

-> END