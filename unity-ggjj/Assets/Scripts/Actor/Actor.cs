public class Actor : Animatable
{
    private ActorData _actorData;
    private IActorController _attachedController;

    public ActorData ActorData
    {
        get => _actorData;
        set
        {
            _actorData = value;
            Animator.runtimeAnimatorController = value.AnimatorController;
            Animator.Play("Normal");
        }
    }

    /// <summary>
    /// Call base awake method and also set animator to keep state on disable.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Animator.keepAnimatorControllerStateOnDisable = true;
    }

    /// <summary>
    /// This method is called by animations when they are completed and require the next line to be to be read.
    /// </summary>
    public override void OnAnimationComplete()
    {
        base.OnAnimationComplete();

        if (_attachedController != null)
        {
            _attachedController.OnAnimationDone();
        }
    }

    /// <summary>
    /// Used to attach an actor controller for callbacks
    /// </summary>
    /// <param name="controller">Actor controller to attach to this object</param>
    public void AttachController(IActorController controller)
    {
        _attachedController = controller;
    }

    /// <summary>
    /// Makes the actor talk while set to true
    /// </summary>
    /// <param name="isTalking">Move mouth or not</param>
    public void SetTalking(bool isTalking)
    {
        Animator.SetBool("Talking", isTalking);
    }

    /// <summary>
    /// Checks if this actor is the actor passed
    /// </summary>
    /// <param name="actor">Actor to compare to</param>
    /// <returns>If the actor is the actor given</returns>
    public bool MatchesActorData(ActorData actor)
    {
        return _actorData == actor;
    }
}