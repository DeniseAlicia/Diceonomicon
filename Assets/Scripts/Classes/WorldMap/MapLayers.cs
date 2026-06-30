using System.Runtime.CompilerServices;
using UnityEngine;

public class MapLayers : MonoBehaviour
{
    public int layer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float baseScale = 2f;
        float adjustedScale = baseScale + 0.001f * layer;

        transform.localScale = new Vector3(adjustedScale, baseScale, adjustedScale);
    }
}
