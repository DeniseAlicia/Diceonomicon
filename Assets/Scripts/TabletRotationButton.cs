namespace Diceonomicon
{
    using System.Collections;
    using UnityEngine;

    public class RotationButton : MonoBehaviour
    {
        public Transform targetObject;
        public float angle = 90f;
        public static int maxRotations = 2;

        private Color baseColor = new Color32(175, 175, 175, 255);
        private Color hoverColor = new Color32(255, 255, 255, 255);
        private Color clickColor = new Color32(100, 100, 100, 255);
        private float rotationDuration = 0.5f;

        private MeshRenderer meshRenderer;
        private Material materialInstance;


        private static bool isRotating = false;
        private static int currentRotations = 0;

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                materialInstance = meshRenderer.material;
                materialInstance.color = baseColor;
            }
        }

        private void OnMouseEnter()
        {
            materialInstance.color = hoverColor;
        }

        private void OnMouseDown()
        {
            if (!isRotating && targetObject != null && currentRotations < maxRotations)
            {
                materialInstance.color = clickColor;
                StartCoroutine(RotateParentSmooth_ChildrenInstant(angle));
            }
        }

        private void OnMouseUp()
        {
            materialInstance.color = hoverColor;
        }

        private void OnMouseExit()
        {
            materialInstance.color = baseColor;
        }

        private IEnumerator RotateParentSmooth_ChildrenInstant(float angle)
        {
            isRotating = true;
            currentRotations += 1;

            // Disable button visuals when player can't rotate anymore
            if (currentRotations >= maxRotations)
            {
                RotationButton[] allButtons = Object.FindObjectsByType<RotationButton>(FindObjectsSortMode.None);

                foreach (RotationButton button in allButtons)
                {
                    foreach (MeshRenderer mr in button.GetComponentsInChildren<MeshRenderer>())
                    {
                        mr.enabled = false;
                    }
                }
            }

            // Rotate tablet over time
            Quaternion startRot = targetObject.rotation;
            Quaternion endRot = startRot * Quaternion.Euler(0f, angle, 0f);

            float elapsed = 0f;
            while (elapsed < rotationDuration)
            {
                float t = elapsed / rotationDuration;
                targetObject.rotation = Quaternion.Slerp(startRot, endRot, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Rotate all tablet slots to starting rotation
            Transform[] children = new Transform[targetObject.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = targetObject.GetChild(i);
            }

            foreach (Transform child in children)
            {
                child.Rotate(0f, -angle, 0f);
            }

            targetObject.rotation = endRot;
            isRotating = false;
        }
    }
}
