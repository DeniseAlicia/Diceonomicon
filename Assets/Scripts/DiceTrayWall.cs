using UnityEngine;

public class DiceTrayWall : MonoBehaviour
{
    BoxCollider _boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableCollision()
    {
        _boxCollider.enabled = false;
    }

    public void EnableCollision()
    {
        _boxCollider.enabled = true;
    }
}
