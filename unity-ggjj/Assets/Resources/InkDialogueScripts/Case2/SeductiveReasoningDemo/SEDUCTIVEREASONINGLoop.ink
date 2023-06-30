&SPEAK:Dan
So what's this all about?

&SPEAK:Arin
&SELECT_PHRASE:I'm %1 to see you; you look %2, okay? Why %3?,
    happy[+3]/sad[-1]/annoyed[-2],
    great[+2]/bored to be here[-1]/terrible[-2],
    is that weird[+1]/not[0]/are you like this[-3],
    + 4 -> win,      # if we collected at least 4 points, we've succeeded
    + lost -> lost

=== lost ===
&SPEAK:Dan
Uhhh... what're you talking about Arin?
&ISSUE_PENALTY # this exits and calls the failure script after lives ran out or continues...
&SPEAK:Arin
Uhhhm... You see...
# ...and goes back to line 5, prompting the user to try again

=== win ===
&LOAD_SCRIPT:End
-> END
