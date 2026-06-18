using System;
using UnityEngine;

public class AnimationEventsAdapter : MonoBehaviour
{
    public event Action AnimationEnded;
    public event Action AnimationReachedPointOfInterest;

    public void CallToEndOfAnimation()
    {
        AnimationEnded?.Invoke();
    }

    public void CallToPointOfInterest ()
    {
        AnimationReachedPointOfInterest?.Invoke();
    }
}
