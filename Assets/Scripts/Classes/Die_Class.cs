namespace Diceonomicon
{
    using UnityEngine;
    using System.Collections.Generic;

    public class Die : MonoBehaviour
    {
        public int[] range; //which values the die can have
        public int value; //which value the die rolled this round
        public List<string> types;
        public string owner;
        public Vector3 lastPosition;
        public bool frozen;

        [SerializeField] Transform[] diceSides;
        [SerializeField] DiceTrayWall[] diceTrayWalls;
        [SerializeField] float forceX = 0f;
        [SerializeField] float forceY = 0f;
        [SerializeField] float forceZ = 0f;
        [SerializeField] float torque = 5f;
        [SerializeField] Vector3 tempGravity = new Vector3(0, -100f, 0);

        new private Rigidbody rigidbody;
        private BoxCollider boxCollider;
        private bool isRolling = false;
        private Vector3 defaultGravity = Physics.gravity;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }

        void FixedUpdate()
        {
            if (rigidbody.IsSleeping() & isRolling)
            {
                GetSideFacingUp();
            }
        }

        public void RollDice()
        {
            Vector3 force = new Vector3(forceX, forceY, forceZ);
            Vector3 torque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * this.torque);

            rigidbody.useGravity = true;

            foreach (DiceTrayWall diceTrayWall in diceTrayWalls) //enable collision for the walls of the dice tray
            {
                diceTrayWall.EnableCollision();
            }

            Physics.gravity = defaultGravity; // reset gravity

            rigidbody.AddForce(force, ForceMode.Impulse); // add force and torque to roll the die
            rigidbody.AddTorque(torque, ForceMode.Impulse);

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

        // public void ResetDiePosition()
        // {
        //     _rigidBody.mass = _dieMass; // reset die mass
        //     Physics.gravity = DefaultGravity; // reset gravity

        //     // enable DiceTrayWall collision
        //     foreach (DiceTrayWall diceTrayWall in _diceTrayWalls)
        //     {
        //         diceTrayWall.EnableCollision();
        //     }

        //     // reset the die to its starting position
        //     gameObject.transform.position = DieStartPosition;
        // }
    }
}