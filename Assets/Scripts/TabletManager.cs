using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabletManager : MonoBehaviour
{
    public static TabletManager Instance { get; private set; }

    public Entity owner;
    public bool enemy;

    private Vector3 startPosition;

    public GameObject tabletPrefab;
    public List<TabletData> tablets;

    private readonly float spacing = 0;

    void Awake()
    {
        Instance = this;

        if (enemy)
        {
            Opponent opponent = Object.FindFirstObjectByType<Opponent>();
            tablets = Encounters.SetEnemyRoster(1, "Green");
            owner = FindFirstObjectByType<Opponent>();
            opponent.SetEnemyRoster(tablets);
            startPosition = new Vector3(4.9f, -2.5f, 0f);
        }
        else
        {
            Player player = Object.FindFirstObjectByType<Player>();
            tablets = player.SetImplingRoster();
            owner = FindFirstObjectByType<Player>();
            startPosition = new Vector3(-6.9f, -2.5f, 0f);
        }
        Vector3 currentPosition = startPosition;
        SpawnTablets(currentPosition);
    }

    public void SpawnTablets(Vector3 currentPosition)
    {
        foreach (TabletData tablet in tablets)
        {
            GameObject tabletInstance = Instantiate(tabletPrefab, currentPosition, Quaternion.identity);

            TabletController controller = tabletInstance.GetComponent<TabletController>();
            controller.SetData(tablet);
            controller.owner = owner;

            Renderer renderer = tabletInstance.GetComponentInChildren<Renderer>();
            float height = renderer.bounds.size.y;

            currentPosition.y -= height + spacing;
        }
    }
}
