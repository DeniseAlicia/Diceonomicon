using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TabletManager : MonoBehaviour
{
    public static TabletManager Instance { get; private set; }

    public Entity owner;
    public bool enemy;

    private Vector3 goalPosition;

    public List<TabletData> tablets;

    private readonly float spacing = 0;

    void Awake()
    {
        Instance = this;

        Vector3 startPosition = new Vector3();

        if (enemy)
        {
            Opponent opponent = Object.FindFirstObjectByType<Opponent>();
            tablets = Encounters.SetEnemyRoster(1, "Green");
            owner = FindFirstObjectByType<Opponent>();
            opponent.SetEnemyRoster(tablets);
            goalPosition = new Vector3(4.9f, -2.5f, 0f);
            startPosition = new Vector3(goalPosition.x + 6f, goalPosition.y, goalPosition.z);
        }
        else
        {
            Player player = Object.FindFirstObjectByType<Player>();
            tablets = player.SetImplingRoster();
            owner = FindFirstObjectByType<Player>();
            goalPosition = new Vector3(-6.9f, -2.5f, 0f);
            startPosition = new Vector3(goalPosition.x - 6f, goalPosition.y, goalPosition.z); ;
        }

        SpawnTablets(startPosition);
    }

    public void SpawnTablets(Vector3 currentPosition)
    {
        float speed = 1f;

        foreach (TabletData tablet in tablets)
        {
            GameObject tabletInstance = Instantiate(tablet.tabletPrefab, currentPosition, Quaternion.identity);

            TabletController controller = tabletInstance.GetComponent<TabletController>();
            controller.SetData(tablet);
            controller.owner = owner;

            Renderer renderer = tabletInstance.GetComponentInChildren<Renderer>();
            float height = renderer.bounds.size.y;

            if (tablet.tabletPrefab.name.Contains("Small"))
            {
                height *= 0.33f;
            }
            else if (tablet.tabletPrefab.name.Contains("Medium"))
            {
                height *= 0.7f;
            }

            goalPosition.y = currentPosition.y;
            currentPosition.y -= height + spacing;

            if (controller.owner == enemy)
            {
                currentPosition.x += 3f;

            }
            else
            {
                currentPosition.x -= 3f;
            }

            controller.gameObject.transform.DOMove(goalPosition, speed).SetEase(Ease.OutQuad);

            speed += 0.1f;
        }
    }
}
