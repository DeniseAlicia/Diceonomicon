using UnityEngine;

public abstract class Trait : MonoBehaviour
{
    public bool sceneStart;
    public bool roundStart;
    public bool placementDone;
    public bool acvitveCombatStart;
    public bool acvitveCombatEnd;
    public string description;

    public abstract void OnSceneStart();
    public abstract void OnRoundStart();
    public abstract void OnPlacementDone();
    public abstract void OnAcvitveCombatStart();
    public abstract void OnAcvitveCombatEnd();
}
