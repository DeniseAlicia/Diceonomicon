namespace Diceonomicon
{
    using UnityEngine;

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
        public bool stoop = false;

        [SerializeField] float forceX = 0f;
        [SerializeField] float forceY = 0f;
        [SerializeField] float forceZ = 0f;
        [SerializeField] float torque = 5f;
        [SerializeField] float liftOffValue = 3; // how much the die is lift off the ground when dragging
        [SerializeField] Vector3 tempGravity = new Vector3(0, -100f, 0);

        private Camera camera;
        private Rigidbody rigidBody;
        private BoxCollider boxCollider;
        private bool isRolling = false;
        Vector3 mouseOffset;
        private Vector3 defaultGravity = Physics.gravity;
        private Vector3 DieStartPos; // Test; remove later


        void Start()
        {
            camera = GameObject.Find("Gameplay").GetComponent<Camera>();
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
            boxCollider = GetComponent<BoxCollider>();

            DieStartPos = transform.position; // Test; remove later
        }

        void FixedUpdate()
        {
            if (rigidBody.IsSleeping() & isRolling)
            {
                GetSideFacingUp();
                rigidBody.isKinematic = true;
                rigidBody.useGravity = false;
                stoop = true;
            }
        }

        public void RollDice()
        {
            Vector3 force = new Vector3(forceX, forceY, forceZ);
            Vector3 torque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * this.torque);

            foreach (DiceTrayWall diceTrayWall in diceTrayWalls) //enable collision for the walls of the dice tray
            {
                diceTrayWall.EnableCollision();
            }

            rigidBody.useGravity = true;
            Physics.gravity = defaultGravity; // reset gravity

            rigidBody.AddForce(force, ForceMode.Impulse); // add force and torque to roll the die
            rigidBody.AddTorque(torque, ForceMode.Impulse);

            isRolling = true;
        }

        private void GetSideFacingUp()
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
            float rotationX = transform.eulerAngles.x;
            float rotationZ = transform.eulerAngles.z;
            Debug.Log("Value: " + value);
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
            // rotate die to a "straight" position
            // float rotationX = transform.eulerAngles.x;
            // float rotationZ = transform.eulerAngles.z;
            // transform.eulerAngles = new Vector3(rotationX, 0f, rotationZ);

            isRolling = false;
        }

        // public void FixPosition()
        // {
            
        // }

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
            Vector3 diePos = camera.WorldToScreenPoint(transform.position);
            return diePos;
        }

        private void OnMouseDown()
        {
            if (isDraggable)
            {
                lastPosition = transform.position;
                // rigidBody.isKinematic = true; // Test, remove here and uncomment in FixedUpdate
                mouseOffset = Input.mousePosition - GetDiePosition();
            }
        }

        private void OnMouseDrag()
        {
            if (isDraggable)
            {
                transform.position = camera.ScreenToWorldPoint(Input.mousePosition - mouseOffset - new Vector3(0f, 0f, liftOffValue));
            }
        }

        private void OnMouseUp()
        {
            if (isDraggable)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit, 100))
                {
                    // if (hitSlot.transform.gameObject is a Slot)
                    // {
                    //     transform.position = hitSlot.transform.position;
                    //     also compare color of slot and die
                    //     isPlaced = true; 
                    // }
                }
                else
                {
                    transform.position = lastPosition;
                }

                transform.position = lastPosition;
                // rigidBody.isKinematic = false; // it wont need physics now or will it?
            }
        }

        private void MoveToLayer(string _layerName)
        {
            int layer = LayerMask.NameToLayer(_layerName);
            gameObject.layer = layer;
        }
    }
}