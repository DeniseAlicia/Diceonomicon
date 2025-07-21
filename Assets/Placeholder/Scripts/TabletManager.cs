using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabletManager : MonoBehaviour
{
    public static TabletManager Instance { get; private set; }

    public GameObject tabletPrefab;
    public List<TabletData> tablets;

    // Assign Implings for testing:
    public TabletData impling1;
    public TabletData impling2;
    public TabletData impling3;

    private Vector3 startPosition = new Vector3(-6.9f, -2.5f, 0f);
    private readonly float spacing = 2.5f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        tablets = new List<TabletData> { impling1, impling2, impling3 };
        Vector3 currentPosition = startPosition;

        foreach (TabletData tablet in tablets)
        {
            GameObject tabletInstance = Instantiate(tabletPrefab, currentPosition, Quaternion.identity);

            TabletController controller = tabletInstance.GetComponent<TabletController>();
            controller.SetData(tablet);

            Renderer renderer = tabletInstance.GetComponentInChildren<Renderer>();
            float height = renderer.bounds.size.y;

            currentPosition.y -= height + spacing;

            Debug.Log("Test");
        }
    }
}
