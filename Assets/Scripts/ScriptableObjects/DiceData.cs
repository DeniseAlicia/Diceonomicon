using UnityEngine;
using UnityEngine.UI;

public abstract class DiceData : ScriptableObject
{
    public GameObject prefab;

    public new string name;
    public string desc;
    public Texture texture;

    public int[] range; 
    public string tag;

    public abstract void DoEffect();
}
