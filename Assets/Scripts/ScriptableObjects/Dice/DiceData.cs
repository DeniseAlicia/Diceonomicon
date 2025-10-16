using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class DiceData : ScriptableObject
{
    public GameObject prefab;

    public new string name;
    public string desc;
    public Texture texture;
    public Texture usedTexture;

    public int[] range; 
    public string[] tags;

    public abstract void DoEffect();
}
