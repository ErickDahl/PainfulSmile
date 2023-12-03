using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/String Event Channel")]
public class StringEventChannel : ScriptableObject
{
    public event Action<string> OnEventRaised;

    public void RaiseEvent(string value)
    {
        OnEventRaised?.Invoke(value);
    }
}
