namespace Diceonomicon
{
    using UnityEngine;

    public class DiceTrayWall : MonoBehaviour
    {
        BoxCollider boxCollider;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DisableCollision()
        {
            boxCollider.enabled = false;
        }

        public void EnableCollision()
        {
            boxCollider.enabled = true;
        }
    }
}
