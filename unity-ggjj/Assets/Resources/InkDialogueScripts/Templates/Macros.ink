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
        <color=orange><align=center>Witness' Account
        -> DONE
        

=== Present(presentedObject)
    &PRESENT_EVIDENCE
    + [Wrong]
        -> Present(presentedObject)
    + [{presentedObject}]
        &MODE:Dialogue
        ->->

=== function char(x)
    ~ return "<link=character>{x}</link>"

VAR period = "<link=character>.<link>"
VAR ellipsis = "<link=character>.<link><link=character>.<link>."