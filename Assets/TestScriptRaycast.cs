using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TestScriptRaycast : MonoBehaviour
{
    public Transform originTransform;

    [System.Serializable]
    public struct Arrows
    {
        public int value;
        public int[] degrees;
    }

    public List<Arrows> arrows;

    public Button raycastButton;

    public void Start()
    {
        Button raycast = raycastButton.GetComponent<Button>();
        raycast.onClick.AddListener(RaycastOnClick);
    }

    public void RaycastOnClick()
    {

        int maxDistance = 4;

        for (int i = 0; i < arrows.Count; i++)
        {
            Arrows arrow = arrows[i];

            for (int j = 0; j < arrow.degrees.Length; j++)
            {
                float angle = arrow.degrees[j];
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.left;

                Ray ray = new Ray(originTransform.position, originTransform.TransformDirection(direction));
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                {
                    Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");

                    Die dieComponent = hit.collider.GetComponent<Die>();
                    if (dieComponent != null)
                    {
                        int dieValue = dieComponent.value;
                        Debug.Log($"Hit a Die with value: {dieValue}");
                    }
                    else
                    {
                        Debug.Log("Hit object does not have a Die component.");
                    }
                }
            }
        }
        return;
    }
}
