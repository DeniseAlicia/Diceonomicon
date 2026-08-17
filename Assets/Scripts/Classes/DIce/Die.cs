using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

public class Die : MonoBehaviour
{
    [SerializeField] private Transform[] diceSides;
    public Transform[] GetDiceSides() { return diceSides; }

    public int[] range; //which values the die can have
    public int value; //which value the die rolled this round
    public int originalValue; //which value the die rolled this round (without buffs/debuffs)
    new public string tag;
    public string owner;
    public Vector3 lastPosition;
    public bool isDraggable;
    public bool isPlaced = false;
    public bool isResting = false;
    public bool isBeingDragged = false;

    public List<Status> statuses = new List<Status>();

    public bool isFrozen = false;
    public bool isCopy;
    public int didDamage;
    public int priority;

    [SerializeField] float forceX;
    [SerializeField] float forceY;
    [SerializeField] float forceZ;
    [SerializeField] float torque = 5f;
    [SerializeField] Vector3 tempGravity = new Vector3(0, -10f, 0);
    public Vector3 scale = new Vector3(4.75f, 4.75f, 4.75f);

    private Camera camGameplay;
    private Camera camBattleTablets;
    public Rigidbody rigidBody;
    private BoxCollider boxCollider;
    public bool isRolling = false;
    //public Vector3 lastRotation;
    public int dieRotation;
    Vector3 mouseOffset;
    private Vector3 defaultGravity = Physics.gravity;

    public string nameText;
    public string descText;
    public string[] dieTags;
    public Renderer textureRenderer;
    public DiceData data;
    public Texture usedTexture;
    public bool used;
    public DiceSlotController parentSlot;
    public Tooltip tooltip;

    public int currentValue;
    public Transform sideUp;
    [SerializeField] public ParticleSystem vfx;
    public FMODUnity.EventReference DiePlacementEvent;

    public static UnityEvent OnDieRemoved = new UnityEvent();

    public void SetData(DiceData dieData)
    {
        data = dieData;
        nameText = dieData.name;
        descText = dieData.desc;
        textureRenderer.material.SetTexture("_BaseMap", dieData.texture);
        range = dieData.range;
        dieTags = dieData.tags;
        usedTexture = dieData.usedTexture;
        priority = dieData.priority;
        DieAction.RangeToValue(this);
    }

    void Start()
    {
        camGameplay = GameObject.Find("Gameplay").GetComponent<Camera>();
        camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();
        dieRotation = 0;
        torque = 5f;
        Vector3 tempGravity = new Vector3(0, -10f, 0);
        rigidBody = this.gameObject.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        boxCollider = GetComponent<BoxCollider>();
        isRolling = false;
        isPlaced = false;

        if (!isCopy)
        {
            isDraggable = false;
            isResting = false;
        }
    }

    void FixedUpdate()
    {
        if (rigidBody.IsSleeping() && isRolling)
        {
            GetSideFacingUp();
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
            isResting = true;
        }
    }

    public void Roll(float x)
    {
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;

        forceX = UnityEngine.Random.Range(x, x / 2);
        forceY = UnityEngine.Random.Range(0.2f, 0.3f);
        forceZ = UnityEngine.Random.Range(0.25f, 0.3f);

        Vector3 force = new Vector3(forceX, forceY, forceZ);
        Vector3 torque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * this.torque);

        Physics.gravity = new Vector3(0, -10f, 0);

        rigidBody.AddForce(force, ForceMode.Impulse); // add force and torque to roll the die
        rigidBody.AddTorque(torque, ForceMode.Impulse);

        isRolling = true;
    }

    public void GetSideFacingUp()
    {
        Physics.gravity = tempGravity; // increase gravity to help the die "fall" into place

        // find out die value by calculating the most upward facing face with the dot product
        Transform upSide = null;
        float maxDot = -1;

        foreach (Transform side in diceSides)
        {
            float dot = Vector3.Dot(side.up, Vector3.up);

            if (dot > maxDot)
            {
                maxDot = dot;
                upSide = side;
            }
        }

        if (upSide == null) return;

        sideUp = upSide;
        value = int.Parse(upSide.name);

        switch (value)
        {
            case 1:
                transform.eulerAngles = new Vector3(-90f, 0f, 90f);
                break;
            case 2:
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;
            case 3:
                transform.eulerAngles = new Vector3(0f, 180f, -90f);
                break;
            case 4:
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                break;
            case 5:
                transform.eulerAngles = new Vector3(180f, 90f, 0f);
                break;
            case 6:
                transform.eulerAngles = new Vector3(90f, 0f, 90f);
                break;
        }

        isRolling = false;

        value = range[value - 1];
        originalValue = value;
    }

    private Vector3 GetDiePosition(Camera _camera) // convert the die position to screen coordinates
    {
        Vector3 diePos = _camera.WorldToScreenPoint(transform.position);
        return diePos;
    }

    public void PlaceDieInSlot(DiceSlotController slotController)
    {
        DiceSlotData slotData = slotController.slotData;

        if (this.dieTags.Contains(slotData.tag) && slotController.isFilled == false && slotController.owner.GetType() == typeof(Player) || dieTags.Contains("Debuff") && slotController.owner.GetType() == typeof(Opponent) && slotController.slotTag != "Buff" && slotController.slotTag != "Spell" || dieTags.Contains("Neutral") && slotController.isFilled == false && slotController.owner.GetType() == typeof(Player) && slotController.slotTag != "Buff")
        {
            if (transform.parent != null)
            {
                Transform parent = transform.parent;
                parentSlot = parent.gameObject.GetComponent<DiceSlotController>();
                parentSlot.isFilled = false;
                parentSlot.slottedDie = null;
                transform.SetParent(null);
            }
            transform.SetParent(slotController.gameObject.transform);
            transform.localPosition = new Vector3(0, 3, 0);
            transform.localScale = scale;

            slotController.isFilled = true;
            slotController.slottedDie = this;
            parentSlot = slotController;
            isPlaced = true;

            //parentSlot.comboDisplay.UpdateCombo();

            FMOD.Studio.EventInstance placeDieAudio = FMODUnity.RuntimeManager.CreateInstance(DiePlacementEvent);
            placeDieAudio.start();
        }
    }

    private void RemoveDieFromSlot()
    {
        if (parentSlot == null)
            return;

        DiceSlotController oldSlot = parentSlot;

        oldSlot.comboSlots.Clear();
        oldSlot.comboDisplay.UpdateComboText();
        oldSlot.isFilled = false;
        oldSlot.slottedDie = null;
        isPlaced = false;
        parentSlot = null;
    }

    private void OnMouseDown()
    {
        isBeingDragged = false;
        if (isDraggable)
        {
            RemoveDieFromSlot();
            //OnDieRemoved.Invoke();

            if (gameObject.layer == LayerMask.NameToLayer("Gameplay"))
            {
                lastPosition = transform.position;
                MoveToLayer("BattleTablets");

                mouseOffset = Input.mousePosition - GetDiePosition(camGameplay);
                transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            }
            else
            {
                mouseOffset = Input.mousePosition - GetDiePosition(camBattleTablets);
                transform.localScale = scale * 1.2f;
            }
        }
    }

    private void OnMouseDrag()
    {
        if (isDraggable)
        {
            isBeingDragged = true;
            transform.position = camBattleTablets.ScreenToWorldPoint(Input.mousePosition - mouseOffset);
            transform.position = new Vector3(transform.position.x, 2f, transform.position.z);

            if (Mouse.current.rightButton.wasPressedThisFrame && this.dieTags.Contains("Buff"))
            {
                transform.Rotate(new Vector3(0, 90, 0), Space.World);
                dieRotation += 90;
            }
        }
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        if (isDraggable)
        {

            //transform.eulerAngles = lastRotation;
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
            {
                GameObject hitSlot = hit.transform.gameObject;
                DiceSlotController slotController = hitSlot.GetComponent<DiceSlotController>();
                if (slotController != null)
                {
                    DiceSlotData slotData = slotController.slotData;

                    if (this.dieTags.Contains(slotData.tag) && slotController.isFilled == false && slotController.owner.GetType() == typeof(Player) || dieTags.Contains("Debuff") && slotController.owner.GetType() == typeof(Opponent) && slotController.slotTag != "Buff" && slotController.slotTag != "Spell" || dieTags.Contains("Neutral") && slotController.isFilled == false && slotController.owner.GetType() == typeof(Player) && slotController.slotTag != "Buff")
                    {
                        PlaceDieInSlot(slotController);
                    }
                    else
                    {
                        if (transform.parent == null)
                        {
                            MoveToLayer("Gameplay");
                            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            transform.position = lastPosition;
                            isPlaced = false;
                        }
                        else
                        {
                            transform.localPosition = new Vector3(0, 3, 0);
                            transform.localScale = scale;

                        }
                    }
                }
                else
                {
                    if (transform.parent != null)
                    {
                        Transform parent = transform.parent;
                        parentSlot = parent.gameObject.GetComponent<DiceSlotController>();
                        parentSlot.isFilled = false;
                        parentSlot.slottedDie = null;
                        transform.SetParent(null);
                    }
                    MoveToLayer("Gameplay");
                    transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    transform.position = lastPosition;
                    isPlaced = false;
                }

            }
            else
            {
                if (transform.parent != null)
                {
                    Transform parent = transform.parent;
                    parentSlot = parent.gameObject.GetComponent<DiceSlotController>();
                    parentSlot.isFilled = false;
                    parentSlot.slottedDie = null;
                    transform.SetParent(null);
                }
                MoveToLayer("Gameplay");
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                transform.position = lastPosition;
                isPlaced = false;
            }
        }
    }

    public void MoveToLayer(string _layerName)
    {
        int layer = LayerMask.NameToLayer(_layerName);
        gameObject.layer = layer;

        Transform[] children = gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    public void InitializeAsCopy()
    {
        MoveToLayer("Gameplay");
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        if (!dieTags.Contains("Buff"))
        {
            foreach (TMP_Text t in GetComponentsInChildren<TMP_Text>(true))
            {
                t.text = value.ToString();
                t.ForceMeshUpdate();
            }
        }

        if (dieTags.Contains("Buff"))
        {
            DieAction.ShowBuffArrows(this);
        }
        isCopy = true;
        isPlaced = false;
        isResting = true;
        isDraggable = true;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        parentSlot = null;
    }

    public bool HasDieData()
    {
        return data != null;
    }

    public string GetTooltipHeader()
    {
        return data != null ? data.title : "???";
    }

    public string GetTooltipDescription()
    {
        return data != null ? data.desc : "";
    }

    private void OnPointerMove()
    {
        tooltip.UpdatePosition();
    }
}