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
        string area = GameStateManager.Instance.player.area;
        int level = GameStateManager.Instance.player.level;

        if (enemy)
        {
            Opponent opponent = FindFirstObjectByType<Opponent>();
            Player player = FindFirstObjectByType<Player>();
            tablets = Encounters.SetEnemyRoster(level, area);
            owner = FindFirstObjectByType<Opponent>();
            opponent.SetEnemyRoster(tablets);
            goalPosition = new Vector3(4.9f, 1f, 3f);
            startPosition = new Vector3(goalPosition.x + 6f, goalPosition.y, goalPosition.z);
        }
        else
        {
            Player player = Object.FindFirstObjectByType<Player>();
            tablets = player.LoadPlayer();
            owner = FindFirstObjectByType<Player>();
            goalPosition = new Vector3(-6.9f, 1f, 3f);
            startPosition = new Vector3(goalPosition.x - 6f, goalPosition.y, goalPosition.z); ;
        }

        SpawnTablets(startPosition);
    }

    public void SpawnTablets(Vector3 currentPosition)
    {
        float speed = 1f;
        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

        foreach (TabletData tablet in tablets)
        {
            GameObject tabletInstance = Instantiate(tablet.tabletPrefab, currentPosition, rotation);

            TabletController controller = tabletInstance.GetComponent<TabletController>();
            controller.SetData(tablet);
            controller.owner = owner;

            Renderer renderer = tabletInstance.GetComponentInChildren<Renderer>();
            float height = renderer.bounds.size.z;

            if (tablet.tabletPrefab.name.Contains("Small"))
            {
                height *= 0.33f;
            }
            else if (tablet.tabletPrefab.name.Contains("Medium"))
            {
                height *= 1.5f;
            }

            goalPosition.z = currentPosition.z;
            currentPosition.z -= height + spacing;

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
