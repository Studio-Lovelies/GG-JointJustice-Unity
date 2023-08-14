INCLUDE ../Options.ink
INCLUDE ../Templates/Macros.ink
INCLUDE StartingEvidence.ink

<- Part1StartingEvidence

&PLAY_SONG:turnaboutGrumpsters,{songFadeTime}
&DIALOGUE_SPEED:0.05
&SPEAK:Arin
<color={blue}>(It started out just like every time after our livestream.)
<color={blue}>(We had just finished a session playing Penix Wright: Facial Attorney<sup>(tm)</sup>.)
<color={blue}>(Dan and I were discussing some very important matters{ellipsis})

&HIDE_TEXTBOX
&SCENE:TMPH_Lobby
&ACTOR:Dan
&FADE_IN:2
&WAIT:0.5

&DIALOGUE_SPEED:0.04
&SPEAK:Dan
So that's when I said, "that's not mayonnaise!"
And everyone immediately and violently threw up.
It was one hell of a graduation party, dude.

&SPEAK:Arin
What.<br>Are you talking about, Dan?

&SPEAK:Dan
Weren't you listening? I was telling you a very important story about how I graduated from Ninja Party School.

&SPEAK:Arin
Ninja Party School?<br>The infamous NPS?

&SET_POSE:Angry
&SPEAK:Dan
Dude!<br>You're the one who asked me about it!
&SET_POSE:Normal
You said it had something to do with the 10 Minute Power Hour we were doing today.

&PLAY_SFX:realization
&SPEAK:Arin
Oh yeah, that's right, I remember now!
Do you know what we're doing for the Power Hour today?

&SHAKE_SCREEN:0.25,0.25
&PLAY_SFX:stab
&SET_POSE:Angry
&DIALOGUE_SPEED:0.02
&SPEAK:Dan
NO, goddamnit, I'm asking you!
You asked me about my graduation party and said it had something to do with today's episode.
&SET_POSE:Lean
&DIALOGUE_SPEED:0.04
Now, what the heck is going on here? You okay, dude?<br>Your memory is worse than usual today.

&SPEAK:Arin
&DIALOGUE_SPEED:0.06
Ooooooooooooooooh m'bad?
&CONTINUE_DIALOGUE
&DIALOGUE_SPEED:0.08
Hah, I guess I got distracted.

&SET_POSE:Normal
&SPEAK:Dan
By what?

&SPEAK:Arin
Well,
&CONTINUE_DIALOGUE
&DIALOGUE_SPEED:0.08
I got the invoice for the bathrooms today.
The plumbers just finished fixing the toilets in the north end of the building, and that got me thinking{ellipsis}

&SET_POSE:Hair
&SPEAK:Dan
You mean you asked me to tell you all about one of the longest nights of my life, just to get distracted by that?

&SPEAK:Arin
&DIALOGUE_SPEED:0.06
Well, you know how much I like poopin!

&SET_POSE:Laughing
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
Yeah, yeah, you do like poopin'.

<- AddEvidence("PlumberInvoice")

&SET_POSE:Normal
&PLAY_SFX:realization
&SPEAK:Dan
So, what ARE we doing for the Power Hour?

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Well, remember the dunking pool the second night of your graduation party?

&SPEAK:Dan
You mean the whipped cream dunking machine I almost drowned in?

&PLAY_SFX:lightbulb
&SET_POSE:Surprised
&DIALOGUE_SPEED:0.08
You don't mean{ellipsis}?

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Yeah, dude! We're going to give people a glimpse into what it means to be a Ninja Sex Party Dude<sup>(tm)</sup>!

&DIALOGUE_SPEED:0.02
&SPEAK:Dan
You gotta be butt-fuckin' me dude, really?

&DIALOGUE_SPEED:0.06
&SPEAK:Arin
Well, if I gotta be doin' it, Dan{ellipsis}

&SET_POSE:Happy
&DIALOGUE_SPEED:0.02
This is no time for hilarious jokes, Arin, we have some tanks to set up!

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Hell yeah, dude. Let me just grab the <color=\#990a1d>backup Switch</color> we borrowed so we can put it back where Jory got it.

<- AddEvidence("Switch")

&SET_POSE:Normal
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
Good thinking. Let's get going then!

&STOP_SONG

&PLAY_SFX:doorOpens
&DIALOGUE_SPEED:0.06
&SPEAK_UNKNOWN:Jory
Actually, guys, we've got a problem{ellipsis}

&DIALOGUE_SPEED:0.08
&SPEAK:Arin
Huh?<br>Jory?

&ADD_RECORD:Jory

&PLAY_SONG:prelude6969,{songFadeTime}
&ACTOR:Jory
&SET_POSE:Sweaty
&WAIT:2
&SET_POSE:Nervous
&DIALOGUE_SPEED:0.04
&SPEAK:Jory
Apparently, the dinos were missing when we started to set up for the episode.
We've spent the last ten minutes looking for them.

&SPEAK:Arin
Well, that's not very long. They can't have gone far.

&SPEAK:Jory
That's the thing{ellipsis} we already found them.

&SET_POSE:Sweaty
&SPEAK:Arin
So then what's the problem?

&SET_POSE:Nervous
&SPEAK:Jory
The problem is{ellipsis}<br>apparently they were found in <color=\#990a1d>MY backpack</color>.

&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:damage1
&ACTOR:Dan
&SET_POSE:ShockAnimation
&DIALOGUE_SPEED:0.02
&SPEAK:Dan
What?

&ACTOR:Jory
&SET_POSE:Sweaty

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Did you take them?
    
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:damage1
&ACTOR:Dan
&SET_POSE:Angry
&DIALOGUE_SPEED:0.02
&SPEAK:Dan
ARIN!

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
What? It's a legitimate question!

&DIALOGUE_SPEED:0.02
&SPEAK:Dan
Obviously he didn't do it, look at his face! Is that the face of someone who is guilty?

&ACTOR:Jory
&SET_POSE:ThumbsUp

&DIALOGUE_SPEED:0.06
&SPEAK:Arin
{ellipsis}
Yes{ellipsis}?

&PLAY_SFX:stab
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
No! Why would he come to us then if he's guilty?<br>Tell us what happened, Jory.

&ACTOR:Jory
&SET_POSE:Nervous
&SPEAK:Jory
I wish I could, but everyone wants to hold a trial for this, and it's starting in just a few minutes!
Everyone is calling for my <color=\#990a1d>Good Boy Coins</color> to be revoked, even after all the work I put into getting them.
And I've got nobody in my corner!

&SET_POSE:Sweaty
&SPEAK:Arin
That sucks, dude.

&ACTOR:Dan
&SPEAK:Dan
Arin, don't you get it?

&SPEAK:Arin
Get what?

&SPEAK:Dan
He wants US to defend him in this trial!<br>Right, Jory?

&ACTOR:Jory
&SET_POSE:ThumbsUp
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:damage1
&DIALOGUE_SPEED:0.08
&SPEAK:Arin
Whaaaat?!

&WAIT:1
&SET_POSE:Normal
&DIALOGUE_SPEED:0.04
&SPEAK:Jory
Well, I guess so.
I didn't really know what to do, and you guys know I'd never do anything like that.

&DIALOGUE_SPEED:0.06
&SPEAK:Arin
I'm not really sure about all this{ellipsis}

&ACTOR:Dan
&SET_POSE:Angry
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
ARIN!

&SET_POSE:Normal
C'mon bro, be a bro and bro this one out for our bro{ellipsis} Brory.

&ACTOR:Jory
&SET_POSE:Nervous
&SPEAK:Arin
I'm sorry, Jory, but we don't know anything about criminal defense!
&DIALOGUE_SPEED:0.02
We're just idiots who play games while saying stupid things for money on the internet. How are we supposed to help?

&DIALOGUE_SPEED:0.04
&SPEAK:Jory
Yeah{ellipsis} I don't know, I didn't really have a plan or anything.
The whole thing has just thrown me for a loop and I'm kind of grasping here{ellipsis}

&ACTOR:Dan
&SET_POSE:Sad
&SPEAK:Dan
Sorry dude, but Arin's right.<br>We'd probably just screw it up.
We're not lawyers.

&SET_POSE:Normal
The closest thing we've been to being lawyers is playing that Penix Wright<sup>(tm)</sup> game.
&DIALOGUE_SPEED:0.02
And that's just a stupid game that totally exists and isn't changed for copyright purposes!

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Sorry Jory, but if we were real lawyers, we would help out.

&ACTOR:Jory
&SET_POSE:Nervous
&SPEAK:Jory
Yeah, I understand guys.

&PLAY_SFX:pageTurn
&WAIT:1

&PLAY_SFX:realization
&SET_POSE:Normal
Hey{ellipsis} someone just slipped a letter under the door.

&SPEAK:Arin
A letter? Who's it from?

&SET_POSE:thinking
&DIALOGUE_SPEED:0.06
&SPEAK:Jory
It's from{ellipsis}<br>My dad?

&PLAY_SFX:realization
&ACTOR:Dan
&SET_POSE:Surprised
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
From <color=\#990a1d>Jory Sr</color>? Why doesn't he just text{ellipsis}

&SPEAK:Arin
Open it up, what does it say?

&PLAY_SFX:pageTurn
&ACTOR:Jory
&SET_POSE:Thinking
&SPEAK:Jory
{ellipsis}<br>It's from my dad alright, but it's addressed to you two!

&PLAY_SFX:realization
&SPEAK:Arin
Us? Like{ellipsis} me and Dan?

&ACTOR:Dan
&SET_POSE:Hair
&SPEAK:Dan
I thought we made that bit up for our episodes.

&ACTOR:Jory
&SET_POSE:Thinking
&SPEAK:Jory
Well, judging from the handwriting and the little hearts dotting the I's{ellipsis}
It's definitely from my dad.

&SPEAK:Arin
&DIALOGUE_SPEED:0.02
Let me see that!

&SET_POSE:Normal
&PLAY_SFX:pageTurn
&DIALOGUE_SPEED:0.04
{ellipsis}<br>Wow, he's right! We should hold on to this, I think it might be important later{ellipsis}

<- AddEvidence("JorySrsLetter")

&ACTOR:Dan
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
So? What does it say?

&SPEAK:Arin
<sup>\*</sup>ahem<sup>\*</sup>
&DIALOGUE_SPEED:0.06
<color=green>"Hello Grumps, first of all I want to thank you for all the hard work you've done taking care of Jory Jr."</color>

&SET_POSE:Surprised
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
You've gotta be kidding me.

&DIALOGUE_SPEED:0.06
&SPEAK:Arin
<color=green>"I've heard what happened to my son with the dinosaurs."</color>

&DIALOGUE_SPEED:0.04
&SPEAK:Dan
Wha- How? Didn't this happen like, 10 minutes ago?

&SPEAK:Arin
Don't think about it too hard, Dan. Anyways{ellipsis}

&SET_POSE:Normal
&DIALOGUE_SPEED:0.06
<color=green>"While I know it looks bad, I know my son is a good boy who only does good things."</color>
<color=green>"I know you know that too.<br>So please, defend him in court."</color>
<color=green>"Show the world he is innocent of such a heinous and despicable crime and find out who the real culprit is."</color>
<color=green>"P.S. Remember, the second most important thing to winning this case is love and trust!"</color>

&DIALOGUE_SPEED:0.04
&SPEAK:Dan
Wow, that was beautiful.

&DIALOGUE_SPEED:0.06
&SPEAK:Arin
<color=green>"And the most important thing is payment!"</color>
<color=green>"I have something for you if you win!"</color>

&SET_POSE:Surprised
&SPEAK:Dan
Wow{ellipsis} that was{ellipsis} beautiful?

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Wait, there's one more thing.
&DIALOGUE_SPEED:0.06
<color=green>"P.P.S. I hope you two have been continuing to provide my boy with wholesome food as well."</color>
<color=green>"And I hope you've been avoiding giving him </color><color=\#990a1d>milk</color><color=green> as he's </color><color=\#990a1d>deathly allergic</color><color=green> to it."</color>

&DIALOGUE_SPEED:0.04
Well that was oddly specific.
&DIALOGUE_SPEED:0.06
<color=green>"P.P.P.S. don't tell Jory this, but I hope he can be on your show again soon!"</color>

&SPEAK:Dan
Huh{ellipsis}

&SET_POSE:Normal
&DIALOGUE_SPEED:0.04
That's a lot of P.

&HIDE_TEXTBOX
&ACTOR:Jory
&SET_POSE:Sweaty
&WAIT:2

&SPEAK:Arin
{ellipsis}

&ACTOR:Dan
&SPEAK:Dan
{ellipsis}<br>So{ellipsis}?
    
&ACTOR:Jory
&SET_POSE:Normal

&SPEAK:Arin
Jory, don't worry. We're on it buddy, because we trust you!
We'll be in your corner. We got you no matter what!

&DIALOGUE_SPEED:0.06
&SPEAK:Jory
Oh! Uh, ok!

&ACTOR:Dan
&SET_POSE:Happy
&SPEAK:Dan
Really, Arin?

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Yep! I have no doubt we'll clear your good name, Jory!

&ACTOR:Jory
&SPEAK:Jory
Wow, thanks guys!<br>I guess I'll head to the, uh, <color=\#990a1d>"Courtroom"</color> then.
&SET_POSE:Thinking
They turned the Power Hour room into a makeshift courtroom just for this.
&SET_POSE:Normal
&DIALOGUE_SPEED:0.06
So uh, see you there.

&HIDE_TEXTBOX
&HIDE_ACTOR
&PLAY_SFX:doorOpens
&WAIT:2

&ACTOR:Dan
&SHOW_ACTOR
&SET_POSE:Surprised
&DIALOGUE_SPEED:0.04
&SPEAK:Dan
Wow, I didn't expect you to change your mind so quickly like that.

&SET_POSE:Hair
I guess what Jory Sr. said about trust and love made a difference, huh?

&DIALOGUE_SPEED:0.02
&SPEAK:Arin
Yeah, trust and love and whatever.<br>What do you think Jory Sr. is going to give us when we win?

&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:stab
&SET_POSE:Angry
&SPEAK:Dan
Arin!

&SPEAK:Arin
WHAT? We're helping him, aren't we?
&CONTINUE_DIALOGUE
That's the whole point, right?!

&DIALOGUE_SPEED:0.04
&SPEAK:Dan
You are just{ellipsis} UN{ellipsis}

&DIALOGUE_SPEED:0.010
&SPEAK:Arin
{ellipsis}<br>{ellipsis}<br>{ellipsis}
&DIALOGUE_SPEED:0.06
&AUTO_SKIP:True
I'm wha---
&AUTO_SKIP:False

&SPEAK:Dan
&SHAKE_SCREEN:0.25,0.2
&PLAY_SFX:smack
&SET_POSE:Angry
&DIALOGUE_SPEED:0.02
BELIEVABLE!

&DIALOGUE_SPEED:0.04
&SPEAK:Arin
Ok, ok, no need to yell{ellipsis} Let's just get ready.<br>I kind of want to look sharp for this.
And I just got a new suit that I think will be perfect.
&CONTINUE_DIALOGUE
You're gonna love it.

&HIDE_TEXTBOX
&FADE_OUT:2
&FADE_OUT_SONG:2
&WAIT:2

&LOAD_SCRIPT:Case1/1-3-TrialStart