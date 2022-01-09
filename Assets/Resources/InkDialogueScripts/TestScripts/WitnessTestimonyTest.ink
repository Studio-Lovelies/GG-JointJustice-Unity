INCLUDE ../Macros.ink

&SCENE:TMPH_Court
&SET_ACTOR_POSITION:2,Ross
&JUMP_TO_POSITION:2

<- WitnessTestimony
-> RossTestimony ->
&END_WITNESS_TESTIMONY

&NARRATE
The witness testimony sign should now be gone.

<- CrossExamination
-> RossTestimony ->

-> END

=== RossTestimony
&SPEAK:Ross
I was animating by myself over in my room at the office.
But then... I saw someone taking the dinos!!
It was Jory! He was on the 10 Minute Power Hour set taking the dinos!
Now that I know they were stolen, that means the culprit must be Jory!
->->