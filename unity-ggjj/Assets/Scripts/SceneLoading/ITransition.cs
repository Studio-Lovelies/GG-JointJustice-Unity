using System;

public interface ITransition
{
    public void Transition(Action callback);
}