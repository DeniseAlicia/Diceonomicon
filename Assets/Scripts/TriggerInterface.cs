using UnityEngine;

public interface TriggerInterface : IEventSystemHandler
{
    void RoundStart();
    void RoundEnd();
    void Rotate();
    void ActiveCombat();
    void ChangeColumn();
}