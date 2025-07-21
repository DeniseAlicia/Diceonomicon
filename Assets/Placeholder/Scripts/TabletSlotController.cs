using UnityEngine;
using TMPro;

public class TabletSlotController : MonoBehaviour
{
    // public TMP_Text nameText;
    // public TMP_Text descText;
    public Renderer slotMaterial;
    public Renderer outlineMaterial;
    public Renderer symbolMaterial;

    public new string tag;

    public void SetData(TabletSlotData data)
    {
        // nameText.text = data.name;
        // descText.text = data.desc;
        slotMaterial.material = data.material;
        tag = data.tag;
        data.AssignColorMaterial(tag);
        outlineMaterial.material = data.outlineMaterial;
        symbolMaterial.material = data.symbolMaterial;
        
        
    }



}
