using UnityEngine;

public enum Emotion
{
    Amnger,
    Sadness,
    Fear,
    Envy,
    Indifference,
    Tempation
}

public class Emotions
{

    public static readonly Color[] colors = new Color[]
        {
        Color.red,
        new Color(a: 1.0f, r: 0.0f, g: 0.5f, b: 1.0f),
        Color.green,
        Color.purple,
        Color.white,
        Color.yellow
        };

    public static readonly string[] types = new string[]
    {
        "Damage", "Block", "Buff", "Spell", "Neutral", "Debuff"
    };

    public static readonly string[] areas = { "Red", "Blue", "Green", "Purple", "White", "Yellow" };

}
