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

    // Unsubscribe to EventListeners to prevent Memory Leak 
    protected virtual void OnDestroy()
    {
        Debug.Log("Base OnDestroy called");
        UnsubscribeFromEvents();
    }

    protected void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnSceneStart.RemoveListener(OnSceneStart);
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
        BattleSceneManager.OnPlacementDone.RemoveListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
        BattleSceneManager.OnAcvitveCombatEnd.RemoveListener(OnAcvitveCombatEnd);
    }
}
