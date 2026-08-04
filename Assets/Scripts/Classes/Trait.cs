using UnityEngine;

public abstract class Trait : MonoBehaviour
{
    public TabletController tablet;
    public string description;

    public virtual void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public abstract void UnsubscribeFromEvents();
}