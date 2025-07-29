using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;

public class DiceSlotController : MonoBehaviour
{
    public Renderer slotMaterial;
    public Renderer outlineMaterial;
    public Renderer symbolMaterial;
    public ParticleSystem vfx;
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
    public bool wasFrozen = false;

    private void Start()
    {
        activeSceneManager = FindFirstObjectByType<BattleSceneManager>();
        GameObject columnMaster = activeSceneManager.columnMaster;
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

        vfx.Play();
        
        if (slottedDie.isFrozen)
        {
            wasFrozen = true;
        }

        mult = 1;
        DetectLinksDown(this.transform.position);
        DetectLinksUp(this.transform.position);
        if (slottedDie.value > 0 && slottedDie.value < 7)
        {
            slottedDie.value = slottedDie.range[slottedDie.value - 1];
        }
        slotData.Effect(slottedDie, mult, activeSceneManager, owner, this);
        return;
    }

    public void DetectLinksDown(Vector3 pos)
    {
        Vector3 rayPosition = new Vector3(pos.x, pos.y - 0.8f, pos.z - 10);
        Ray raydown = new Ray(rayPosition, Vector3.forward);

        if (Physics.Raycast(raydown, out RaycastHit hit, 666))
        {
            Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");

            Die die = hit.collider.GetComponent<Die>();
            if (die != null && die.dieTag == slottedDie.dieTag)
            {
                mult += 1;
                DetectLinksDown(rayPosition);
            }
        }
    }

    public void DetectLinksUp(Vector3 pos)
    {
        Vector3 rayPosition = new Vector3(pos.x, pos.y + 0.8f, pos.z - 10);
        Ray raydown = new Ray(rayPosition, Vector3.forward);

        if (Physics.Raycast(raydown, out RaycastHit hit, 666))
        {
            Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");

            Die die = hit.collider.GetComponent<Die>();
            if (die != null && die.dieTag == slottedDie.dieTag)
            {
                mult += 1;
                DetectLinksUp(rayPosition);
            }
        }
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
