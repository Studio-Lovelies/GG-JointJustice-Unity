INCLUDE ../Colors.ink
INCLUDE ../Options.ink
INCLUDE ../Templates/SceneInitialization.ink
INCLUDE ../Templates/FailureStates.ink
INCLUDE ../Templates/Macros.ink

<- COURT_TMPH
<- Failures.TMPH

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
({ellipsis}!)

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
I CAN prove it{ellipsis} with THIS!

-> Present("Switch") ->
    
&TAKE_THAT:Arin
&SHOW_ITEM:Switch,Right

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
A{ellipsis} Nintendo Switch?

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
That's right. Remember how you told everyone that Jory went to get us a Switch when ours broke?
Well it just so happens that I have this switch{ellipsis} RIGHT HERE!

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
{ellipsis}
&STOP_SONG
Wait a second, how does that help you?

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Does the prosecution have an{ellipsis} objection?

&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Oh right, my mistake.
&OBJECTION:TutorialBoy
&SET_POSE:Angry
This is a complete waste of time, Your Honor!
Not only is there no proof that Switch belongs to Ross{ellipsis}
{ellipsis}but there's no proof this is even the Switch Jory brought back!

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
Not only that, but these stickers aren't yet available to the public{ellipsis}
&SET_POSE:Point
{ellipsis}meaning only Ross himself could have them!

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
Well{ellipsis} I{ellipsis}

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
Don't you know, Dan? I'm the Video Game Boy{ellipsis}
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
Urp{ellipsis}!

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
&SET_POSE:Normal#1 //#1 indicates which choice is correct so it can be picked last by tests
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
There are some interesting games on here though{ellipsis}
&SET_POSE:SideNormal
Er{ellipsis} Ross{ellipsis} what exactly is 'Mew Mew Cutie Girls Club'?

&SCENE:TMPHCourt
&JUMP_TO_POSITION:2
&SHAKE_SCREEN:0.25,0.25
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

&HIDE_TEXTBOX
&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit
&SCENE:TMPHJudge
&SET_POSE:Warning
&SPEAK:JudgeBrent
Ahem{ellipsis} if we could get back to the case, you two?
&SET_POSE:Normal
The games on Ross' Switch does not have any bearing on this case.

&SCENE:TMPHCourt
&SET_POSE:Thinking
&THINK:Arin
(Damn{ellipsis} I thought I had the right answer there.)

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
{ellipsis}You do realize this is a court of law, right?
&PLAY_SONG:logicAndTrains,{songFadeTime}
The titty showing will have to wait until AFTER the trial.

&SCENE:TMPHAssistant
&SET_POSE:SideLean
&SPEAK:Dan
Hey, now we've got something to look forward to when you win!

&SCENE:TMPHCourt
&SET_POSE:Thinking
&SPEAK:Arin
Wait{ellipsis} Will this change the age rating?

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
If you've ever owned a Switch, you'd know that the last game played{ellipsis}
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
Well, if the prosecution's claim that Ross was making a Mario Maker level with his Switch is true{ellipsis}
Then Mario Maker would be the game you see on the left hand side of the screen!

&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
But if you'd take a look at Ross' Switch, Your Honor{ellipsis}
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
That means the fact that Penix Wright is there{ellipsis}
&JUMP_TO_POSITION:2
&SET_POSE:Glaring
{ellipsis}was because it was the game we were playing on the livestream!

&HIDE_TEXTBOX
&STOP_SONG
&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&PLAY_EMOTION:Damage
&SET_POSE:Glaring
&SPEAK:Ross
AUGH{ellipsis}
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
You{ellipsis} guys{ellipsis}
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
Hmmm{ellipsis} the prosecution does bring up a point.
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
Be that as it may, unless the defense has evidence of someone else more likely to have committed the crime than Jory{ellipsis}
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
One was Jory, and the other{ellipsis}

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

-> Present("Ross") ->

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&SPEAK:Arin
Why, the only other person who wasn't at the livestream other than Jory!
&SET_POSE:Point
ROSS O'DIDITVAN!

&HIDE_TEXTBOX
&SCENE:TMPHWideShot
&PLAY_SFX:mutter
&WAIT:2

&PLAY_SFX:gavel
&PLAY_ANIMATION:GavelHit
&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Order in this courtroom!
&SET_POSE:Thinking
Hmmm. That's a bold accusation you've raised, Arin. As well as a terrible pun on Ross' name.
&SET_POSE:Surprised
But how could this be true? Didn't Ross find them in the first place?
&SET_POSE:Normal
We saw him find them in Jory's backpack. Why would Ross find them himself if he was the one who stole them?

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SET_POSE:Confident
&SPEAK:TutorialBoy
The answer to that is simple, Your Honor! It's because he DIDN'T steal them!
The defense is simply grasping at straws!
They know Jory is guilty, so they're throwing out accusations to confuse the jury!
&SET_POSE:Angry
How dare you sully the name of JUSTICE by proposing this preposterous position!

&HIDE_TEXTBOX
&OBJECTION:Arin
&PAN_TO_POSITION:1,{doublePanTime}
&PLAY_EMOTION:ShakingHead
&SET_POSE:Normal
&SPEAK:Arin
We have no decisive evidence that it was either Jory or Ross, Your Honor!
All we have is eyewitness accounts from someone who himself could have been the culprit!

&SCENE:TMPHJudge
&SPEAK:JudgeBrent
Regardless of that fact, I need proof or evidence that suggests that Ross was the culprit.

&HIDE_TEXTBOX
&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
&SPEAK:TutorialBoy
HAH! Good luck proving that, "Video Game Boy". I'd like to see you try!

&STOP_SONG
&SET_POSE:Confident,Arin
&PAN_TO_POSITION:1,{doublePanTime}
&SPEAK:Arin
I'll do better than that...
&PLAY_SFX:ObjectionClean
&PLAY_SONG:dragonObjection
&SET_POSE:Objection
...because I'm the one who wins!

&JUMP_TO_POSITION:3
&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:stab
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Not that joke again...

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&SPEAK:Arin
Hell yea dude, check this shit out!

-> Present("BentCoins") ->

&TAKE_THAT:Arin
&SPEAK:Arin
These are the Goodboy Coins found inside the backpack!
Notice how they're bent and scuffed now? Don't you realize what that means?

&STOP_SONG
&PLAY_SFX:recordScratch
&SCENE:TMPHAssistant
&SET_POSE:SideNormal
&SPEAK:Dan
That we're cheap bastards making coins out of cardboard?

&SCENE:TMPHCourt
&PLAY_SFX:stab
&SHAKE_SCREEN:0.25,0.25
&SPEAK:Arin
Well--

&JUMP_TO_POSITION:3
&SET_POSE:Normal
&SPEAK:TutorialBoy
I second that notion.

&JUMP_TO_POSITION:1
&PLAY_SFX:stab
&SET_POSE:DeskSlam
&SPEAK:Arin
Now, that was uncalled f--

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
I mean, just use some quarters and paint them up. Clearly we don't care about being fancy.

&PLAY_SFX:damage2
&SCENE:TMPHCourt
&SET_POSE:Sweaty
&SPEAK:Arin
OK! I get it, I'll get REAL coins after this.
&SET_POSE:Normal
Regardless, Jory would never let his coins be ruined like this!
&PLAY_SONG:dragonObjection
&SET_POSE:Point
There is no way he would have stuffed the dinosaurs so carelessly with those coins there!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:2
&PLAY_SFX:stab
&PLAY_EMOTION:DamageNoHelmet
&SET_POSE:MadMilk
&SPEAK:Ross
GUH!

&HIDE_TEXTBOX
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
&SPEAK:TutorialBoy
RIDICULOUS! I don't believe a word of that! There's no way someone would be so careful around such pointless coins.

&HIDE_TEXTBOX
&JUMP_TO_POSITION:1
&PLAY_SFX:DeskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SPEAK:Arin
That might be true for most people, but not our Jory!
&SET_POSE:Point
We've already heard evidence that proves Jory cared about those coins, and I've got it right here!

-> Present("LivestreamRecording") ->

&TAKE_THAT:Arin
&SET_POSE:PaperSlap
&SPEAK:Arin
Recall Jory's testimony, how he was noted to be taking care of those coins carefully!
&SET_POSE:Thinking
Now I ask the court to consider this: Why would Jory take such care of his coins only to trash them in his backpack minutes later?
&HIDE_TEXTBOX
&PLAY_SFX:deskSlam
&PLAY_EMOTION:DeskSlamAnimation
&SCENE:Anime
&ACTOR:Arin
&PLAY_SFX:ObjectionClean
&SET_POSE:CloseUp
The answer is simple: HE DIDN'T PUT THE DINOS IN THERE AT ALL!

&SCENE:TMPHCourt
&SET_ACTOR_POSITION:1,Arin //This is to avoid a bug where SET_POSE will try to set the pose on Arin in the anime scene
&JUMP_TO_POSITION:2
&HIDE_TEXTBOX
&PLAY_SFX:stab2
&PLAY_EMOTION:DamageNoHelmet
&SPEAK:Ross
OOF!
&HIDE_TEXTBOX
&PLAY_SFX:stab2
&PLAY_EMOTION:DamageNoHelmet
ARGH!

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
Hmm... that makes a lot of sense. I, personally, have seen Jory care for those coins like they were his children.
&SET_POSE:Normal
I'm not sure how you polish cardboard... but regardless, Jory's care of those coins cannot be denied.
Does the prosecution have anything to say about this?

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SET_POSE:Sweaty
&SPEAK:TutorialBoy
Well... uh...

&JUMP_TO_POSITION:1
&SET_POSE:Confident
&THINK:Arin
(Yeah, let's see you sneak your way out of this one, dawg!)

&HIDE_TEXTBOX
&STOP_SONG
&JUMP_TO_POSITION:3
&PLAY_SFX:damage1
&PLAY_EMOTION:HeadSlam
&SET_POSE:Normal
&SPEAK:TutorialBoy
YES! There is an explanation!

&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&THINK:Arin
(Come ON dude!)

&PLAY_SONG:investigationUniCore
&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
Clearly, my witness made an error! The dinos were simply found in a different part of the backpack.
When he found them, Mr. O'Donovan simply was so shocked that Jory would do such a thing...
...that he mistook which pocket they were found in.

&JUMP_TO_POSITION:2
&PLAY_SFX:realization
&SPEAK:Ross
...!
&SET_POSE:SweatyNoHelmet
Oh yeah, I remember now! I looked in the front pocket first and saw the coins.
I realized that there was no room, so I checked the side pocket and that's where I found the dinos!

&HIDE_TEXTBOX
&OBJECTION:Arin
&SET_POSE:Point,Arin
&PAN_TO_POSITION:1,{panTime}
&SPEAK:Arin
That's fucking bullshit, Your Honor! That's not what he said in his testimony.

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
Hmm... That does seem like a reach.
&SET_POSE:Normal
There were a lot of people in the area at the time. Someone must have seen where the dinos were found in the backpack.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
Well, why don't we ask them?
Esteemed court members who were present when Ross found the dinos...
Did ANY of you see which part of the backpack they were found in?

&HIDE_TEXTBOX
&SCENE:TMPHWideShot
&PLAY_SFX:mutter
&WAIT:2
&PLAY_SFX:Triplegavel
&PLAY_ANIMATION:TripleGavelHit

&SCENE:TMPHJudge
&SET_POSE:Angry
&SPEAK:JudgeBrent
ORDER! ORDER I SAY!

&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&SET_POSE:Sweaty
&THINK:Arin
(I think Brent just likes hitting that gavel and yelling...)

&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
It is as I said, Your Honor. Ross simply mistook which pocket he found them in.
The coins were damaged at some point between Jory placing them inside the backpack and the dinos being found.
Most likely, Ross bent them while searching for the dinos Jory had put into his backpack!

&SCENE:TMPHJudge
&SET_POSE:Thinking
&SPEAK:JudgeBrent
Hmm... I could see that happening, certainly.
&SET_POSE:Normal
And since there is nobody here that can refute this point, I have to side with the prosecution.

&STOP_SONG
&HIDE_TEXTBOX
&SHAKE_SCREEN:0.25,0.25
&SCENE:TMPHCourt
&JUMP_TO_POSITION:1
&PLAY_SFX:stab2
&PLAY_EMOTION:ShockAnimation
&SET_POSE:Sweaty
&SPEAK:Arin
Are you freaking kidding me?!?

&SCENE:TMPHJudge
&SET_POSE:Angry
&SPEAK:JudgeBrent
HOWEVER.
I would like the witness to revise his testimony to reflect this change.
Let me be clear: This is the LAST time I will allow this.

&SCENE:TMPHCourt
&JUMP_TO_POSITION:3
&SPEAK:TutorialBoy
Such a wise judgement, Your Honor! Your years of judicial experience shine brightly today!

&JUMP_TO_POSITION:1
&SET_POSE:DeskSlam
&THINK:Arin
(This is it, then. I have to get him here. For Jory!)

&SCENE:TMPHAssistant
&SET_POSE:Fist
&SPEAK:Dan
You got this, dude. We can't give up now!

&HIDE_TEXTBOX
&FADE_OUT:3
&WAIT:1

&LOAD_SCRIPT:Case1/1-11-RossWitnessTestimony3

-> END