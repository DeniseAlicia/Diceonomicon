using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DiceSlotData", menuName = "Data/DiceSlotData")]
public abstract class DiceSlotData : ScriptableObject
{
    // public new string name;
    // public string desc;

    public Material material;
    [HideInInspector] public Material outlineMaterial;
    [HideInInspector] public Material symbolMaterial;

    public string tag;
    public Entity owner;
    public Die slottedDie;
    public int mult;
    public int priority;
    
    

    public void AssignColorMaterial(string tag)
    {
        switch (tag.ToLower())
        {
            case "damage":
                outlineMaterial = Resources.Load<Material>("Slots/ColorOutlineRed_M");
                symbolMaterial = Resources.Load<Material>("Slots/ColorBGRed_M");
                return;
            case "block":
                outlineMaterial = Resources.Load<Material>("Slots/ColorOutlineBlue_M");
                symbolMaterial = Resources.Load<Material>("Slots/ColorBGBlue_M");
                return;
            case "buff":
                outlineMaterial = Resources.Load<Material>("Slots/ColorOutlineGreen_M");
                symbolMaterial = Resources.Load<Material>("Slots/ColorBGGreen_M");
                return;
            case "spell":
                outlineMaterial = Resources.Load<Material>("Slots/ColorOutlinePurple_M");
                symbolMaterial = Resources.Load<Material>("Slots/ColorBGPurple_M");
                return;
            default:
                outlineMaterial = Resources.Load<Material>("Slots/EmptyMaterial");
                symbolMaterial = Resources.Load<Material>("Slots/EmptyMaterial");
                return;
        }
    }

    public void attachDie(Die die)
    { }

    public void detachDie()
    { }

    public virtual void Effect()
    {
        Debug.Log("DoEffect not found (DiceSlot)");
    }

}
