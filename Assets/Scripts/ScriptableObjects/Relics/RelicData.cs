using UnityEngine;

[CreateAssetMenu(fileName = "RelicData", menuName = "Scriptable Objects/RelicData")]
public abstract class RelicData : ScriptableObject
{
    public string title;
    public string desc;
    public string flavorText;
    public Sprite image;

    public abstract void DoEffect();
}
