using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Die : MonoBehaviour
{
    [SerializeField] Transform[] _diceSides;
    [SerializeField] DiceTrayWall[] _diceTrayWalls;
    [SerializeField] float _forceX = 0f;
    [SerializeField] float _forceY = 0f;
    [SerializeField] float _forceZ = 0f;
    [SerializeField] float _torque = 5f;
    [SerializeField] float _dieMass = 1f;
    [SerializeField] float _dieTempMass = 10f;
    [SerializeField] Vector3 _tempGravity = new Vector3(0, -100f, 0);
    [SerializeField] Vector3 _targetPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] float _movementSpeed = 1;
    //[SerializeField] float _pushOverForce = 0f;

    Rigidbody _rigidBody;
    BoxCollider _boxCollider;
    bool _isRolling = false;
    Vector3 DieStartPosition;
    Vector3 DefaultGravity = Physics.gravity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.useGravity = false;
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
        DieStartPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        if (_rigidBody.IsSleeping() & _isRolling)
        {
            GetSideFacingUp();
            //SetDiePosition(1);
            MoveDie();
        }
    }

    public void RollDice()
    {
        Vector3 force = new Vector3(_forceX, _forceY, _forceZ);
        Vector3 torque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * _torque);

        _rigidBody.useGravity = true;
        _boxCollider.enabled = true;
        _rigidBody.AddForce(force, ForceMode.Impulse); // add force and torque to roll the die
        _rigidBody.AddTorque(torque, ForceMode.Impulse);

        _isRolling = true;
    }

    //public void SetDiePosition(float _Xoffset)
    //{
    //    _rigidBody.useGravity = false;
    //    _boxCollider.enabled = false;
    //    transform.position = new Vector3(_Xoffset, 2f, -5f);
    //}

    public void ResetDiePosition()
    {
        _rigidBody.mass = _dieMass; // reset die mass
        Physics.gravity = DefaultGravity; // reset gravity

        // enable DiceTrayWall collision
        foreach (DiceTrayWall diceTrayWall in _diceTrayWalls)
        {
            diceTrayWall.EnableCollision();
        }

        // reset the die to its starting position
        gameObject.transform.position = DieStartPosition;
    }

    void GetSideFacingUp()
    {
        _rigidBody.mass = _dieTempMass; // in/decrease die mass
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

        //float checkDot = Vector3.Dot(upSide.up, Vector3.up);
        //Debug.Log(checkDot);


        //if (checkDot < 0.999)
        //{
        //    _rigidBody.AddForce(new Vector3(_pushOverForce, 0f, _pushOverForce), ForceMode.Impulse);

        //}

        if (upSide == null) return;
        Debug.Log(upSide.name);

        _isRolling = false;
    }

    void MoveDie()
    {

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _movementSpeed * Time.deltaTime);
    }

    
}
