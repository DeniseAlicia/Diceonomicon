using UnityEngine;
using TMPro;

public class DiceSlotController : MonoBehaviour
{
    public Renderer slotMaterial;
    public Renderer outlineMaterial;
    public Renderer symbolMaterial;


    public Entity owner;
    public bool filled;
    private DiceSlotData slotData;
    public int priority;
    public int mult;
    public Die slottedDie;
    private BattleSceneManager activeSceneManager;

    private void Start()
    {
        activeSceneManager = FindFirstObjectByType<BattleSceneManager>();
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
        owner = slotData.owner;
        slotData.AssignColorMaterial(tag);
        outlineMaterial.material = slotData.outlineMaterial;
        symbolMaterial.material = slotData.symbolMaterial;
        priority = slotData.priority;
    }


    public void DoEffect()
    {
        slotData.Effect(slottedDie.value, mult, activeSceneManager);
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


}
