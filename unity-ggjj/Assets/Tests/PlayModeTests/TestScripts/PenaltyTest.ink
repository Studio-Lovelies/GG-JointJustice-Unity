// Script to test whether penatlies are enabled and disabled at the correct times
&ADD_FAILURE_SCRIPT:TMPH_FAIL_1
&ADD_EVIDENCE:AttorneysBadge
&SCENE:TMPHCourt
&SET_ACTOR_POSITION:Defense,Arin
&APPEAR_INSTANTLY
Penalties are disabled
&MODE:CrossExamination
&APPEAR_INSTANTLY
Penalties are enabled
&MODE:Dialogue
&APPEAR_INSTANTLY
Penalties are disabled
&MODE:CrossExamination
&APPEAR_INSTANTLY
Penalties are enabled
-> Choice

=== Choice
+ [Choice1]

&APPEAR_INSTANTLY
End of substory
&RESET_PENALTIES
&APPEAR_INSTANTLY
Penalties have been reset
-> END