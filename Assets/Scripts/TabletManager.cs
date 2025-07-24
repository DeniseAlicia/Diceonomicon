using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabletManager : MonoBehaviour
{
    public static TabletManager Instance { get; private set; }

    public bool enemy;

    private Vector3 startPosition;

    public GameObject tabletPrefab;
    public List<TabletData> tablets;

    private readonly float spacing = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (enemy == true)
        {
            startPosition = new Vector3(4.9f, -2.5f, 0f);
        }
        else
        {
            startPosition = new Vector3(-6.9f, -2.5f, 0f);
        }


        // tablets = new List<TabletData> { impling1, impling2, impling3 };
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
