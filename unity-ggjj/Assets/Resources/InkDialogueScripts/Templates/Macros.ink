INCLUDE ../Colors.ink

=== WitnessTestimony
    -> TestimonyAnimation("WitnessTestimony") ->
    &BEGIN_WITNESS_TESTIMONY
    <- Lines.Testimony
    -> DONE

=== CrossExamination
    -> TestimonyAnimation("CrossExamination") ->
    &MODE:CrossExamination
    <- Lines.Testimony
    -> DONE

=== TestimonyAnimation(animationName)
    &HIDE_TEXTBOX
    &PLAY_ANIMATION:{animationName}
    &NARRATE
    &APPEAR_INSTANTLY
    ->->
    
=== Lines
    = Testimony
        <color=orange><align=center>-- Witness' Account --
        -> DONE

=== Present(presentedObject)
    &HIDE_TEXTBOX
    &PRESENT_EVIDENCE
    + [Wrong]
        -> Present(presentedObject)
    + [{presentedObject}]
        &MODE:Dialogue
        ->->
        
=== AddEvidence(evidenceName)
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

=== function char(x)
    ~ return "<link=character>{x}</link>"

VAR period = "<link=character>.</link>"
VAR ellipsis = "<link=character>.</link><link=character>.</link><link=character>.</link>"