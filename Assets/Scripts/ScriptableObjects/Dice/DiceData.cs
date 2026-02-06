using UnityEngine;
using UnityEngine.UI;

public abstract class DiceData : CollectableData
{
    public GameObject prefab;

    public string title;
    public string desc;
    public string flavorText;
    public Texture texture;
    public Texture usedTexture;
    public Sprite image;

    public int[] range; 
    public string[] tags;
    public int priority;


    public abstract void DoEffect(Die die);
}
