namespace Diceonomicon
{
    using UnityEngine;

    public class Die : MonoBehaviour
    {
        public int[] Range; //which values the die can have
        public int Value; //which value the die rolled this round
        public string Tag;
        public Vector3 LastPosition;

        [SerializeField] Transform[] _diceSides;
        [SerializeField] DiceTrayWall[] _diceTrayWalls;
        [SerializeField] float _forceX = 0f;
        [SerializeField] float _forceY = 0f;
        [SerializeField] float _forceZ = 0f;
        [SerializeField] float _torque = 5f;
        [SerializeField] Vector3 _tempGravity = new Vector3(0, -100f, 0);

        private Rigidbody _rigidBody;
        private BoxCollider _boxCollider;
        private bool _isRolling = false;
        private Vector3 DefaultGravity = Physics.gravity;

        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _rigidBody.useGravity = false;
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.enabled = false;
        }

        void FixedUpdate()
        {
            if (_rigidBody.IsSleeping() & _isRolling)
            {
                GetSideFacingUp();
            }
        }

        public void RollDice()
        {
            Vector3 force = new Vector3(_forceX, _forceY, _forceZ);
            Vector3 torque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * _torque);

            _rigidBody.useGravity = true;

            foreach (DiceTrayWall diceTrayWall in _diceTrayWalls) //enable collision for the walls of the dice tray
            {
                diceTrayWall.EnableCollision();
            }

            Physics.gravity = DefaultGravity; // reset gravity

            _rigidBody.AddForce(force, ForceMode.Impulse); // add force and torque to roll the die
            _rigidBody.AddTorque(torque, ForceMode.Impulse);

            _isRolling = true;
        }

        void GetSideFacingUp()
        {
            Physics.gravity = _tempGravity; // increase gravity to help the die "fall" into place

            // disable DiceTrayWall collision to prevent crooked dice
            foreach (DiceTrayWall diceTrayWall in _diceTrayWalls)
            {
                diceTrayWall.DisableCollision();
            }

            // find out die value by calculating the most upward facing face with the dot product
            Transform upSide = null;
            float maxDot = -1;

            foreach (Transform side in _diceSides)
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

            _isRolling = false;
        }
    }
}