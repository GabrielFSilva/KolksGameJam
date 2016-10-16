using UnityEngine;
using System;
using System.Collections;

public class CustomAnimEvent : MonoBehaviour
{
    public event Action OnAnimationCalled;

    public void CallAnimation()
    {
        if (OnAnimationCalled != null)
            OnAnimationCalled();
    }
}
