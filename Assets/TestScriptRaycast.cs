using UnityEngine;
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

    public void Start()
    {
        int maxDistance = 4;

        for (int i = 0; i < arrows.Count; i++)
        {
            Arrows arrow = arrows[i];

            for (int j = 0; j < arrow.degrees.Length; j++)
            {
                float angle = arrow.degrees[j];
                Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

                Ray ray = new Ray(originTransform.position, originTransform.TransformDirection(direction));
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                {
                    Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");
                }
            }
        }
        return;
    }
}
