using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public GameObject rotatingObject;
    public float rotationX;
    public float rotationY;
    public float rotationZ;

    void Update()
    {
        rotatingObject.transform.Rotate(rotationX, rotationY, rotationZ);
    }
}
