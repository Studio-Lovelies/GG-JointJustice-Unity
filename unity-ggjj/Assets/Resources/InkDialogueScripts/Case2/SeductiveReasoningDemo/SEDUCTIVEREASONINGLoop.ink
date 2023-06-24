&SPEAK:Dan
So what's this all about?

&SPEAK:Arin
&SELECT_PHRASE:I'm %1 to see you; you look %2, okay? Why %3?
    happy[+3]/sad[-1]/annoyed[-2],
    great[+2]/bored to be here[-1]/terrible[-2],
    is that weird[+1]/not[0]/are you like this[-3]
+ 4 -> win
+ lost -> lost

=== lost ===
&SPEAK:Dan
Uhhh... what're you talking about Arin?
&ISSUE_PENALTY # this calls the failure script after lives ran out
&SPEAK:Arin
Uhhhm... You see...

=== win ===
&LOAD_SCRIPT:End
-> END