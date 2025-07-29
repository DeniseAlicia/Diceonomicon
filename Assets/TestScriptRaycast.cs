using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TestScriptRaycast : MonoBehaviour
{
    public Transform originTransform;

    public List<int> arrows;

    public Button raycastButton;

    public Die testDie;

    public void Start()
    {
        Button raycast = raycastButton.GetComponent<Button>();
        raycast.onClick.AddListener(RaycastOnClick);
        arrows = new List<int> { 0, 90 };
    }

    public void RaycastOnClick()
    {

        int maxDistance = 1;

      foreach (int angle in arrows)
        {
                Vector3 direction = Quaternion.Euler(20, testDie.dieRotation + angle, -20) * Vector3.back;

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
    }

