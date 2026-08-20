using UnityEngine;
using System;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DiceSlotController : MonoBehaviour
{
    public Renderer slotMaterial;
    public Renderer outlineMaterial;
    public Renderer symbolMaterial;
    public ParticleSystem vfx;
    public HoverGlowController hoverTarget;
    public Tooltip tooltip;
    public TMP_Text slotName;
    public ComboDisplay comboDisplay;

    public Entity owner;
    public bool isFilled;
    public bool isHandled;
    public DiceSlotData slotData { get; private set; }
    public int priority;
    public int synergy;
    public int mult = 1;
    public List<DiceSlotController> comboSlots;
    public int tempMult = 0;
    public Die slottedDie;
    public string slotTag;
    //public Die die;

    public bool wasFrozen = false;

    public FMODUnity.EventReference DoEffectEvent;

    private void Start()
    {
        mult = 1;
        slotName.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (slottedDie != null && !slottedDie.used)
        {
            comboDisplay.UpdateCombo();
        }
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
        slotTag = slotData.tag;
        slotData.AssignColorMaterial(slotTag);
        outlineMaterial.material = slotData.outlineMaterial;
        symbolMaterial.material = slotData.symbolMaterial;
        priority = slotData.priority;
        synergy = slotData.synergy;
        slotName.text = slotData.name;
    }

    public void DoEffect()
    {
        int fmodParameter = 0;

        switch (slotTag)
        {
            case "Buff":
                fmodParameter = 3;
                break;
            case "Spell":
                fmodParameter = 2;
                break;
            case "Damage":
                fmodParameter = 0;
                break;
            case "Block":
                fmodParameter = 1;
                break;
            default:
                fmodParameter = 0;
                break;
        }

        FMOD.Studio.EventInstance effectAudio = FMODUnity.RuntimeManager.CreateInstance(DoEffectEvent);
        effectAudio.setParameterByName("SlotEventByTag", fmodParameter);
        effectAudio.start();

        if (vfx)
        {
            vfx.Play();
        }

        if (wasFrozen)
        {
            wasFrozen = false;
        }

        if (slottedDie.isFrozen)
        {
            wasFrozen = true;
        }

        mult = 1 + tempMult;
        if (!slottedDie.dieTags.Contains("Neutral"))
        {
            DetectLinksDown(this.transform.position);
            DetectLinksUp(this.transform.position);
        }
        slotData.Effect(slottedDie, mult, owner, this);
        tempMult = 0;
        return;
    }

    public bool ValidateCombo(Die die)
    {
        if (die != null && die.isPlaced)
        {
            Transform parent = die.transform.parent;
            DiceSlotController slot = parent.gameObject.GetComponent<DiceSlotController>();

            if (slotTag == slot.slotTag && die.dieTags.Contains(slotTag) && !slottedDie.dieTags.Contains("Neutral"))
            {
                return true;
            }
        }
        return false;
    }

    public void DetectLinksDown(Vector3 pos)
    {
        Vector3 rayPosition = new Vector3(pos.x, pos.y + 1, pos.z - 0.8f);
        Ray raydown = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(raydown, out RaycastHit hit, 666))
        {
            Die die = hit.collider.GetComponent<Die>();

            if (ValidateCombo(die))
            {
                mult += 1;
                DetectLinksDown(rayPosition);
            }
        }
    }

    public void DetectLinksUp(Vector3 pos)
    {
        Vector3 rayPosition = new Vector3(pos.x, pos.y + 1, pos.z + 0.8f);
        Ray raydown = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(raydown, out RaycastHit hit, 666))
        {
            Die die = hit.collider.GetComponent<Die>();

            if (ValidateCombo(die))
            {
                mult += 1;
                DetectLinksUp(rayPosition);
            }
        }
    }

    public void DetectComboDown(Vector3 pos)
    {
        Vector3 rayPosition = new Vector3(pos.x, 1.6f, pos.z - 0.8f);
        Ray raydown = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(raydown, out RaycastHit hit, 1))
        {
            Die die = hit.collider.GetComponent<Die>();

            if (ValidateCombo(die))
            {
                if (!comboSlots.Contains(die.parentSlot))
                {
                    comboSlots.Add(die.parentSlot);
                }
                DetectComboDown(rayPosition);
            }
        }
    }

    public void DetectComboUp(Vector3 pos)
    {
        Vector3 rayPosition = new Vector3(pos.x, 1.6f, pos.z + 0.8f);
        Ray raydown = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(raydown, out RaycastHit hit, 1))
        {
            Die die = hit.collider.GetComponent<Die>();

            if (ValidateCombo(die))
            {
                if (!comboSlots.Contains(die.parentSlot))
                {
                    comboSlots.Add(die.parentSlot);
                }
                DetectComboUp(rayPosition);
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

    private void OnMouseEnter()
    {
        if (hoverTarget)
        {
            hoverTarget.SetHover(true);
        }
    }

    private void OnMouseExit()
    {
        if (hoverTarget)
        {
            hoverTarget.SetHover(false);
        }
    }

    private void OnMouseDown()
    {
        Die die = null;

        if (!isFilled)
        {
            foreach (Die suitableDie in Player.Instance.dice.OrderBy(d => d.value))
            {
                if (suitableDie.dieTags.Contains(slotTag) && !suitableDie.isPlaced && owner is Player && suitableDie.isDraggable || suitableDie.dieTags.Contains("Neutral") && !suitableDie.isPlaced && owner is Player && suitableDie.isDraggable && slotTag != "Buff")
                {
                    die = suitableDie;

                }
            }

            if (!die)
            {
                foreach (Die suitableExtraDie in Player.Instance.tempDice.OrderBy(d => d.value))
                {
                    if (suitableExtraDie.dieTags.Contains(slotTag) && !suitableExtraDie.isPlaced && owner is Player && suitableExtraDie.isDraggable || suitableExtraDie.dieTags.Contains("Neutral") && !suitableExtraDie.isPlaced && owner is Player && suitableExtraDie.isDraggable && slotTag != "Buff")
                    {
                        die = suitableExtraDie;
                    }
                }
            }

            if (die)
            {
                // if (die.transform.parent != null)
                // {
                //     Transform parent = die.transform.parent;
                //     isFilled = false;
                //     slottedDie = null;
                //     die.parentSlot = null;
                //     die.transform.SetParent(null);
                // }
                die.lastPosition = die.transform.position;
                // //die.lastRotation = die.transform.eulerAngles;
                // die.transform.SetParent(this.transform);
                // die.transform.localPosition = new Vector3(0, 3, 0);
                // die.transform.localScale = die.scale;
                // die.parentSlot = this;
                // isFilled = true;
                // slottedDie = die;
                // die.isPlaced = true;

                die.PlaceDieInSlot(this);

                // comboDisplay.UpdateCombo();

                die.MoveToLayer("BattleTablets");
            }
        }
    }
}
