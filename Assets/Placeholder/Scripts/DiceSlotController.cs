using UnityEngine;
using TMPro;

public class DiceSlotController : MonoBehaviour
{
    // public TMP_Text nameText;
    // public TMP_Text descText;
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
        // nameText.text = data.name;
        // descText.text = data.desc;

        slotData = data;
        ReadData();


    }
    private void ReadData()
    {
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

}
