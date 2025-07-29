using UnityEngine;
using UnityEngine.InputSystem;

public class Die : MonoBehaviour
{
    [SerializeField] Transform[] diceSides;
    [SerializeField] DiceTrayWall[] diceTrayWalls;

    public int[] range; //which values the die can have
    public int value; //which value the die rolled this round
    new public string tag;
    public string owner;
    public Vector3 lastPosition;
    public bool isDraggable = false;
    public bool isPlaced = false;
    public bool isResting = false;
    public bool isFrozen = false;

    [SerializeField] float forceX;
    [SerializeField] float forceY;
    [SerializeField] float forceZ;
    [SerializeField] float torque = 5f;
    [SerializeField] Vector3 tempGravity = new Vector3(0, -10f, 0);

    private Camera camGameplay;
    private Camera camBattleTablets;
    public Rigidbody rigidBody;
    private BoxCollider boxCollider;
    private bool isRolling = false;
    Vector3 lastRotation;
    public int dieRotation;
    Vector3 mouseOffset;
    private Vector3 defaultGravity = Physics.gravity;
    private Vector3 DieStartPos; // Test; remove later

    public string nameText;
    public string descText;
    public string dieTag;
    public Renderer textureRenderer;
    public DiceData dieData;

    public void SetData(DiceData dieData)
    {
        nameText = dieData.name;
        descText = dieData.desc;
        textureRenderer.material.SetTexture("_BaseMap", dieData.texture);
        range = dieData.range;
        dieTag = dieData.tag;
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
        isDraggable = false;
        isPlaced = false;
        isResting = false;
        DieStartPos = transform.position; // Test; remove later
    }

    void FixedUpdate()
    {
        if (rigidBody.IsSleeping() & isRolling)
        {
            GetSideFacingUp();
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
            isResting = true;
        }
    }

    public void Roll()
    {
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;

        forceX = UnityEngine.Random.Range(-0.02f, 0.02f);
        forceY = UnityEngine.Random.Range(0.2f, 0.3f);
        forceZ = UnityEngine.Random.Range(0.25f, 0.3f);

        Vector3 force = new Vector3(forceX, forceY, forceZ);
        Vector3 torque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * this.torque);

        foreach (DiceTrayWall diceTrayWall in diceTrayWalls) //enable collision for the walls of the dice tray
        {
            diceTrayWall.EnableCollision();
        }

        Physics.gravity = new Vector3(0, -10f, 0);

        rigidBody.AddForce(force, ForceMode.Impulse); // add force and torque to roll the die
        rigidBody.AddTorque(torque, ForceMode.Impulse);

        isRolling = true;
    }

    public void GetSideFacingUp()
    {
        Physics.gravity = tempGravity; // increase gravity to help the die "fall" into place

        // disable DiceTrayWall collision to prevent crooked dice
        foreach (DiceTrayWall diceTrayWall in diceTrayWalls)
        {
            diceTrayWall.DisableCollision();
        }

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
        // Debug.Log(upSide.name); // log the die value

        boxCollider.enabled = false;
        value = int.Parse(upSide.name);
        //Debug.Log("Value: " + value);

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

        boxCollider.enabled = true;
        isRolling = false;
    }

    public void ResetDiePosition() // Test; remove later
    {
        Physics.gravity = defaultGravity; // reset gravity
        rigidBody.isKinematic = false;
        boxCollider.enabled = true;

        // enable DiceTrayWall collision
        foreach (DiceTrayWall diceTrayWall in diceTrayWalls)
        {
            diceTrayWall.EnableCollision();
        }

        // reset the die to its starting position
        transform.position = DieStartPos;
    }

    private Vector3 GetDiePosition() // convert the die position to screen coordinates
    {
        Vector3 diePos = camGameplay.WorldToScreenPoint(transform.position);
        return diePos;
    }

    private void OnMouseDown()
    {
        if (isDraggable)
        {
            lastPosition = transform.position;
            mouseOffset = Input.mousePosition - GetDiePosition();

            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            switch (value)
            {
                case 1:
                    lastRotation = transform.eulerAngles;
                    transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                    break;
                case 2:
                    lastRotation = transform.eulerAngles;
                    transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                    break;
                case 3:
                    lastRotation = transform.eulerAngles;
                    transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                    break;
                case 4:
                    lastRotation = transform.eulerAngles;
                    transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                    break;
                case 5:
                    lastRotation = transform.eulerAngles;
                    transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                    break;
                case 6:
                    lastRotation = transform.eulerAngles;
                    transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                    break;
            }

            MoveToLayer("BattleTablets");
        }
    }

    private void OnMouseDrag()
    {
        if (isDraggable)
        {
            transform.position = camBattleTablets.ScreenToWorldPoint(Input.mousePosition - mouseOffset);

            if (Mouse.current.rightButton.wasPressedThisFrame && this.dieTag == "Buff")
            {
                transform.Rotate(new Vector3(0, 0, 90), Space.World);
                dieRotation += 90;
            }
        }
    }

    private void OnMouseUp()
    {
        if (isDraggable)
        {
            switch (value)
            {
                case 1:
                    transform.eulerAngles = lastRotation;
                    break;
                case 2:
                    transform.eulerAngles = lastRotation;
                    break;
                case 3:
                    transform.eulerAngles = lastRotation;
                    break;
                case 4:
                    transform.eulerAngles = lastRotation;
                    break;
                case 5:
                    transform.eulerAngles = lastRotation;
                    break;
                case 6:
                    transform.eulerAngles = lastRotation;
                    break;
            }


            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 100))
            {
                GameObject hitSlot = hit.transform.gameObject;
                // Debug.Log(hitSlot);

                DiceSlotController slotController = hitSlot.GetComponent<DiceSlotController>();
                if (slotController != null)
                {
                    DiceSlotData slotData = slotController.slotData;
                    // Debug.Log(slotData);

                    if (slotData.tag == this.dieTag)
                    {
                        //Debug.Log("Slotted!");

                        transform.SetParent(hitSlot.transform);
                        transform.localPosition = new Vector3(0, 3, 0);
                        transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                        transform.Rotate(new Vector3(0, 0, dieRotation), Space.World);
                        transform.localScale = new Vector3(6f, 6f, 6f);

                        slotController.isFilled = true;
                        slotController.slottedDie = this;
                        isPlaced = true;
                    }
                    else
                    {
                        //Debug.Log(slotData.slottedDie);
                        MoveToLayer("Gameplay");
                        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        transform.position = lastPosition;
                    }
                    // Debug.Log(hit.collider.transform.gameObject.name);
                    // Debug.Log(hitSlot);
                }
                else
                {
                    MoveToLayer("Gameplay");
                    transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    transform.position = lastPosition;
                }

            }
            else
            {
                MoveToLayer("Gameplay");
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                transform.position = lastPosition;
            }
        }
    }

    public void MoveToLayer(string _layerName)
    {
        int layer = LayerMask.NameToLayer(_layerName);
        gameObject.layer = layer;
    }
}