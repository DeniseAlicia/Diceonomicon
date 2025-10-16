using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.EventSystems;

public class DiceSlotController : MonoBehaviour
{
    public Renderer slotMaterial;
    public Renderer outlineMaterial;
    public Renderer symbolMaterial;
    public ParticleSystem vfx;
    public HoverGlowController hoverTarget;
    public Tooltip tooltip;
    public TMP_Text slotName;

    public Entity owner;
    public Player player;
    public bool isFilled;
    public bool isHandled;
    public DiceSlotData slotData { get; private set; }
    public int priority;
    public int synergy;
    public int mult = 1;
    public Die slottedDie;
    public new string tag;
    public Die die;

    private BattleSceneManager activeSceneManager;
    public bool wasFrozen = false;

    public FMODUnity.EventReference DoEffectEvent;

    private void Start()
    {
        activeSceneManager = FindFirstObjectByType<BattleSceneManager>();
        player = FindFirstObjectByType<Player>();
        mult = 1;
        slotName.gameObject.SetActive(false);
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
        synergy = slotData.synergy;
        slotName.text = slotData.name;
    }

    public void DoEffect()
    {
        int fmodParameter = 0;

        switch (tag)
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

        mult = 1;
        DetectLinksDown(this.transform.position);
        DetectLinksUp(this.transform.position);
        slotData.Effect(slottedDie, mult, activeSceneManager, owner, this);
        return;
    }

    public void DetectLinksDown(Vector3 pos)
    {
        Vector3 rayPosition = new Vector3(pos.x, pos.y - 0.8f, pos.z - 10);
        Ray raydown = new Ray(rayPosition, Vector3.forward);

        if (Physics.Raycast(raydown, out RaycastHit hit, 666))
        {
            // Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");

            Die die = hit.collider.GetComponent<Die>();
            if (die != null && die.dieTags.Intersect(slottedDie.dieTags).Any())
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
            // Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");

            Die die = hit.collider.GetComponent<Die>();
            if (die != null && die.dieTags.Intersect(slottedDie.dieTags).Any())
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
        die = null;

        if (!isFilled)
        {
            foreach (Die suitableDie in player.dice.OrderBy(d => d.value))
            {
                if (suitableDie.dieTags.Contains(tag) && !suitableDie.isPlaced && owner is Player && suitableDie.isDraggable)
                {
                    die = suitableDie;
                }
            }

            if (!die)
            {
                foreach (Die suitableExtraDie in player.tempDice.OrderBy(d => d.value))
                {
                    if (suitableExtraDie.dieTags.Contains(tag) && !suitableExtraDie.isPlaced && owner is Player && suitableExtraDie.isDraggable)
                    {
                        die = suitableExtraDie;
                    }
                }
            }

            if (die)
            {
                if (die.transform.parent != null)
                {
                    Transform parent = die.transform.parent;
                    isFilled = false;
                    slottedDie = null;
                    die.transform.SetParent(null);
                }
                die.lastPosition = die.transform.position;
                die.lastRotation = die.transform.eulerAngles;
                die.transform.SetParent(this.transform);
                die.transform.localPosition = new Vector3(0, 3, 0);
                die.transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                die.transform.Rotate(new Vector3(0, 0, die.dieRotation), Space.World);
                die.transform.localScale = new Vector3(6f, 6f, 6f);

                isFilled = true;
                slottedDie = die;
                die.isPlaced = true;
                die.MoveToLayer("BattleTablets");
            }
        }
    }

    private void OnPointerMove()
    {
        tooltip.UpdatePosition();
    }
}
