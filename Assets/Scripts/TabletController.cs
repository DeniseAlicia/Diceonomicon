using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class TabletController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descText;
    public Renderer artworkRenderer;
    public Entity owner;
    public Component trait;
    public List<DiceSlotController> tabletSlots;

    public void SetData(TabletData data)
    {
        Transform tabletMainTransform = transform.Find("TabletMain");
        nameText.text = data.name;
        descText.text = data.desc;
        owner = data.owner;
        if (data.trait != null) {
            Type type = Type.GetType(data.trait);
            trait = gameObject.AddComponent(type);
        }
        artworkRenderer.material.mainTexture = data.artwork;
        data.CreateSlots(tabletMainTransform, this);
    }
}
