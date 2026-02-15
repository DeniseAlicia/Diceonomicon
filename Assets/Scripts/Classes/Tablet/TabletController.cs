using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class TabletController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descText;
    public Renderer artworkRenderer;
    public Entity owner;
    public Component script;
    public List<DiceSlotController> tabletSlots;

    public enum Emotions
    {
        Anger,
        Sadness,
        Envy,
        Fear,
        Indifference,
        Temptation
    }
    public int[] emotionValues = new int[System.Enum.GetValues(typeof(Emotions)).Length];

    public void SetData(TabletData data)
    {
        Transform tabletMainTransform = transform.Find("TabletMain");
        nameText.text = data.name;
        descText.text = data.desc;
        owner = data.owner;
        if (data.trait != null)
        {
            Type type = Type.GetType(data.trait);
            script = gameObject.AddComponent(type);
        }
        artworkRenderer.material.mainTexture = data.artwork;
        data.CreateSlots(tabletMainTransform, this);
    }
}
