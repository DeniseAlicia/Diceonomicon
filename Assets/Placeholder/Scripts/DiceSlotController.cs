using UnityEngine;
using TMPro;

public class DiceSlotController : MonoBehaviour
{
    // public TMP_Text nameText;
    // public TMP_Text descText;
    public Renderer slotMaterial;
    public Renderer outlineMaterial;
    public Renderer symbolMaterial;

    public new string tag;
    public Entity owner;
    public bool filled;

    public void SetData(DiceSlotData data)
    {
        // nameText.text = data.name;
        // descText.text = data.desc;
        slotMaterial.material = data.material;
        tag = data.tag;
        owner = data.owner;
        data.AssignColorMaterial(tag);
        outlineMaterial.material = data.outlineMaterial;
        symbolMaterial.material = data.symbolMaterial;


    }



}
