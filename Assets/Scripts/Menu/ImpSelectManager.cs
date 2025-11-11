using System.Collections.Generic;
using UnityEngine;

public class ImpSelectManager : MonoBehaviour
{
    public static ImpSelectManager Instance;

    public int maxSelections = 5;

    public List<TabletData> selectedImplings = new List<TabletData>();

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool Selection(TabletData data)
    {
        if (selectedImplings.Contains(data))
        {
            selectedImplings.Remove(data);
            return false;
        }

        if (selectedImplings.Count >= maxSelections)
        {
            return false;
        }

        selectedImplings.Add(data);
        return true;
    }

}
