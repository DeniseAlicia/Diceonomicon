using UnityEngine;

[CreateAssetMenu(fileName = "CandleData", menuName = "Scriptable Objects/CandleData")]
public abstract class CandleData : ScriptableObject
{
    public string title;
    public string desc;
    public string flavorText;
    public Sprite image;

    public abstract void DoEffect();
}
