using UnityEngine;
using TMPro;

public class DiceSlotController : MonoBehaviour
{
    public Renderer slotMaterial;
    public Renderer outlineMaterial;
    public Renderer symbolMaterial;
    public HoverGlowController hoverTarget;

    public Entity owner;
    public bool isFilled;
    public bool isHandled;
    public DiceSlotData slotData { get; private set; }
    public int priority;
    public int mult = 1;
    public Die slottedDie;
    public new string tag;

    private BattleSceneManager activeSceneManager;


    private void Start()
    {
        activeSceneManager = FindFirstObjectByType<BattleSceneManager>();
        mult = 1;

    }

    public void SetData(DiceSlotData data)
    {
        slotData = data;
        ReadData();
    }

    private void ReadData()
    {
        if (slotData == null) return;

        slotMaterial.material = slotData.material;
        tag = slotData.tag;
        slotData.AssignColorMaterial(tag);
        outlineMaterial.material = slotData.outlineMaterial;
        symbolMaterial.material = slotData.symbolMaterial;
        priority = slotData.priority;
    }


    public void DoEffect()
    {
        slotData.Effect(slottedDie.value, mult, activeSceneManager, owner);
    }

    public void DetectLinks()
    {

    }


    public bool HasSlotData()
    {
        return slotData != null;
    }

    public string GetTooltipHeader()
    {
        return slotData != null ? slotData.name : "???";
    }

    public string GetTooltipDescription()
    {
        return slotData != null ? slotData.desc : "";
    }

    void OnMouseEnter()
    {
        if (hoverTarget != null)
            hoverTarget.SetHover(true);

        if (slotData != null)
        {
            // TooltipSystem.ShowTooltip(slotData.desc, slotData.name);
        }
    }

    void OnMouseExit()
    {
        if (hoverTarget != null)
            hoverTarget.SetHover(false);

        TooltipSystem.HideTooltip();
    }

}
