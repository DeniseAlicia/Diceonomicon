using Mono.Cecil.Cil;
using UnityEngine;

public class ImpSelectList : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentParent; //grid-object
    [SerializeField] private GameObject impSelectDisplayPrefab; //DispalyPrefab

    private void Start()
    {
        PopulateSelectList();
    }

    private void PopulateSelectList()
    {
        //delete old displays
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        //load impling data from Resources/Implings
        TabletData[] allImplings = Resources.LoadAll<TabletData>("Implings");

        if (allImplings.Length == 0)
        {
            Debug.LogWarning("Keine Implings gefunden.");
            return;
        }

        // create a display for every tablet
        foreach (var data in allImplings)
        {
            if (data.unlocked == true)
            {
                GameObject obj = Instantiate(impSelectDisplayPrefab, contentParent);
                obj.GetComponent<ImpSelectDisplay>().SetData(data);
                obj.transform.localScale = Vector3.one;
            }
        }
    }
}
