=== WitnessTestimony
    -> TestimonyAnimation("WitnessTestimony") ->
    &BEGIN_WITNESS_TESTIMONY
    <- Lines.Testimony
    -> DONE

=== CrossExamination
    -> TestimonyAnimation("CrossExamination") ->
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
        <color=orange><align=center>Witness' Testimony
        -> DONE
        
=== function char(x)
    ~ return "<link=character>{x}</link>"