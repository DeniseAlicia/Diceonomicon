using UnityEngine;

[CreateAssetMenu(fileName = "DieBonusRelicData", menuName = "Scriptable Objects/DieBonusRelicData")]
public class DieBonusRelicData : RelicData
{
    public int bonusAmount;
    public string bonusType;

    public override void DoEffect(){}
}
