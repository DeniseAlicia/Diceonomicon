using NUnit.Framework;
using UnityEngine;

public abstract class DiceData : ScriptableObject
{
    public GameObject prefab;

    public new string name;
    public string desc;
    public Texture texture;
    public Texture usedTexture;

    public int[] range; 
    public string[] tags;
    public int priority;


    public abstract void DoEffect(Die die);
}
