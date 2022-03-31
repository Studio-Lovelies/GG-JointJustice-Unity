INCLUDE ../../../Resources/InkDialogueScripts/Colors.ink
INCLUDE ../../../Resources/InkDialogueScripts/Templates/FailureStates.ink

&ADD_FAILURE_SCRIPT:TMPH_FAIL_1

&ADD_EVIDENCE:Bent_Coins
&ADD_EVIDENCE:Jorys_Backpack
&ADD_EVIDENCE:Jory_Srs_Letter
&ADD_EVIDENCE:Livestream_Recording
&ADD_EVIDENCE:Plumber_Invoice
&ADD_EVIDENCE:Stolen_Dinos
&REMOVE_EVIDENCE:Bent_Coins
&REMOVE_EVIDENCE:Jorys_Backpack
&REMOVE_EVIDENCE:Jory_Srs_Letter
&REMOVE_EVIDENCE:Livestream_Recording
&REMOVE_EVIDENCE:Plumber_Invoice
&REMOVE_EVIDENCE:Stolen_Dinos
&APPEAR_INSTANTLY
This script tests the &ADD_RECORD action which adds an actor to the court
&ADD_RECORD:Ross
Ross has been added to the court record


&PLAY_SFX:evidenceDing
&ADD_EVIDENCE:Plumber_Invoice
&SHOW_ITEM:Plumber_Invoice,Left
&DIALOGUE_SPEED:0.06
&NARRATE
<align="center"><color={lightBlue}>Plumber Invoice has been added to the Court Record.
&PLAY_SFX:evidenceShoop
&HIDE_ITEM
&WAIT:0.1
-> DONE
Correct

-> END