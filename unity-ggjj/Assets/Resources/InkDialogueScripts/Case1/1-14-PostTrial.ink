INCLUDE ../Templates/Macros.ink
INCLUDE StartingEvidence.ink

<- Part5StartingEvidence

&HIDE_TEXTBOX
&FADE_OUT:0

&PLAY_SONG:objectsOfVictory
&SCENE:TMPH_Lobby
&ACTOR:Dan
&FADE_IN:2

&SPEAK:Dan
KSST.
&PLAY_SFX:chug
\*glug{ellipsis} glug{ellipsis}*
Ah{ellipsis} this can of La Croix is as refreshing as that victory{ellipsis}!
&SET_POSE:Happy
You did it Arin! I can't believe we won that!

&SPEAK:Arin
Well, technically WE won the case together. Teamwork bro!

&SPEAK:Dan
Hell yea dude. I'm still in shock though, that was intense as hell!

&SPEAK:Jory
Well, so am I. Shocked with joy, that is!

&SPEAK:Dan
Oh hey Jory!

&ACTOR:Jory
&SPEAK:Jory
You guys really saved my butt out there! I can't thank you enough!

&SPEAK:Arin
Anytime, man. We did what any good friend would have done.

&SPEAK:Dan
You mean pose as lawyers illegally and thus tainting the sanctity of the law and all it stands for?

&SPEAK:Arin
EXACTLY!

&SPEAK:Dan
hahahahaha!

&HIDE_TEXTBOX
&WAIT:1

&PLAY_SFX:doorOpens
&SPEAK:Arin
Ross!

&ACTOR:Ross
&SET_POSE:SadNoHelmet
&SPEAK:Ross
Hey guys{ellipsis}

&SPEAK:Arin
Come by to sulk, Ross?

&SPEAK:Ross
Nah, I just wanted to say sorry to everyone.

&SPEAK:Ross
I mean, I knew all this was just for the 10 Minute Power Hour, but even still--"

&SPEAK:Arin
Wait, 10 minute power hour? What the hell do you mean?

&SPEAK:Ross
Yeah, of course! Why else would we all go along with that ridiculous crap?
&SET_POSE:SweatyNoHelmet
You mean you guys didn't know? I thought you were in on it.

&ACTOR:Dan
&SET_POSE:Angry
Are you kidding? We thought that was for real!

&SPEAK:Arin
Jory, did you know about this?

&ACTOR:Jory
No way man! Nobody ever tells me anything.

&ACTOR:Ross
&SET_POSE:SweatyNoHelmet
Huh{ellipsis} so whose idea was all this? Brent?

&SPEAK:Arin
No way, he's way too responsible. Maybe it was that tutorial guy. He's the only variable here.

&ACTOR:Dan
&SET_POSE:Normal
&SPEAK:Dan
Huh. Yeah, that's right. He's still here right?

&SPEAK:Arin
I think so. We should go find out what all this was about.

&ACTOR:Ross
&SPEAK:Ross
&SET_POSE:NormalNoHelmet
Well looks like there's no hard feelings! I'm going to go work on those levels now{ellipsis}

&HIDE_TEXTBOX
&HIDE_ACTOR
&PLAY_SFX:doorOpens
&WAIT:2

&SHOW_ACTOR
&ACTOR:Dan
&SPEAK:Dan
I hope to God those levels are gonna actually be fun to play.

&SPEAK:Arin
Dude it's Ross. I don't think that's gonna happen.

&SPEAK:Dan
Yeah you're probably right{ellipsis} but I'm gonna hold out hope for once.
&PLAY_SFX:pageTurn
&PLAY_SFX:realization
&SET_POSE:Surprised
Wha{ellipsis}? Another letter?

&SPEAK:Arin
Oh god what now?

&PLAY_SFX:pageTurn

&SPEAK:Arin
<color=green>"Hello again Grumps, I've already heard tale of your valiant defense of my boy Jory."
<color=green>"As a matter of fact, I think you two are naturals at this lawyer business."
<color=green>"I was a hot shot lawyer in Attitude City before I changed to a career with the Merchant Marines."
<color=green>"After defending that albino whale from that crazy peg-legged stalker, I guess I felt the call of the ocean."
<color=green>"But I still have contacts at the Attitude City courthouse if you ever decide to have a career change of your own!"
<color=green>"Thank you again for saving my beloved Jory Jr. Sincerely, Jory Sr."

&SET_POSE:Normal
&SPEAK:Dan
Wow, Jory's dad has done a lot of things.

&SPEAK:Arin
He also says a lot of words.
Hey Dan, by the way{ellipsis}

&SPEAK:Dan
Hm? What's up?

&SPEAK:Arin
Thanks for having my back out there, dude.
The end of that trial was tough. I really thought that we actually lost for a second there.
Thanks to you, we managed to save Jory from a life behind bars!
{ellipsis}or losing fake coins, I guess.

&SPEAK:Dan
Like I said - teamwork, man. Guess you could say we served{ellipsis}
&SET_POSE:SideLean
{ellipsis}Some sweet JOINT JUSTICE!

&SPEAK:Arin
{ellipsis}

&SPEAK:Dan
{ellipsis}

&SPEAK:Arin
{ellipsis}did you really just-?

&SET_POSE:Happy
&SPEAK:Dan
Yeah, I went there. Couldn't help myself. Anyways{ellipsis}
&SET_POSE:Normal
Let's go look for that Tutorial Boy and find out what's going on here.

&SPEAK:Arin
Right. I don't know about you, but there might be more to this than just being 'a bit'.

&SPEAK:Dan
&AUTO_SKIP:true
Guess we'll have to find out --
&AUTO_SKIP:false
&SPEAK:Arin
With some sweet Joint Justice?"

&SET_POSE:Angry
&SPEAK:Dan
{ellipsis}
&SET_POSE:Normal
Arin, you're a man after my own heart.

&HIDE_TEXTBOX
&SET_POSE:Laughing

&FADE_OUT:3
&WAIT:2

&LOAD_SCENE:Credits
-> END