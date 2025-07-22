namespace Diceonomicon
{
    using UnityEngine;

    public class Die : MonoBehaviour
    {
        public int[] range; //which values the die can have
        public int value; //which value the die rolled this round
        new public string tag;
        public string owner;
        public Vector3 lastPosition;
        public bool isDraggable = false;
        public bool isPlaced = false;

        [SerializeField] Transform[] diceSides;
        [SerializeField] DiceTrayWall[] diceTrayWalls;
        [SerializeField] float forceX = 0f;
        [SerializeField] float forceY = 0f;
        [SerializeField] float forceZ = 0f;
        [SerializeField] float torque = 5f;
        [SerializeField] int liftOffValue = 3; // how much the die is lift off the ground when dragging
        [SerializeField] Vector3 tempGravity = new Vector3(0, -100f, 0);

        private Rigidbody rigidBody;
        private bool isRolling = false;
        Vector3 mouseOffset;
        private Vector3 defaultGravity = Physics.gravity;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
        }

        void FixedUpdate()
        {
            if (rigidBody.IsSleeping() & isRolling)
            {
                GetSideFacingUp();
            }
        }

        public void RollDice()
        {
            Vector3 force = new Vector3(forceX, forceY, forceZ);
            Vector3 torque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * this.torque);

            rigidBody.useGravity = true;

            foreach (DiceTrayWall diceTrayWall in diceTrayWalls) //enable collision for the walls of the dice tray
            {
                diceTrayWall.EnableCollision();
            }

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

            // rotate die to a "straight" position
            float rotationX = transform.eulerAngles.x;
            float rotationZ = transform.eulerAngles.z;
            transform.eulerAngles = new Vector3(rotationX, 0f, rotationZ);

            if (upSide == null) return;
            Debug.Log(upSide.name); // log the die value

            isRolling = false;
        }

        private Vector3 GetDiePosition() // convert the die position to screen coordinates
        {
            Vector3 diePos = Camera.main.WorldToScreenPoint(transform.position);
            return diePos;
        }

        private void OnMouseDown()
        {
            if (isDraggable)
            {
                lastPosition = transform.position;
                rigidBody.isKinematic = true;
                mouseOffset = Input.mousePosition - GetDiePosition();
            }
        }

        private void OnMouseDrag()
        {
            if (isDraggable)
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mouseOffset - new Vector3(0f, 0f, liftOffValue));
            }
        }

        private void OnMouseUp()
        {
            if (isDraggable)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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