using UnityEngine;

public class ImpMenuList : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentParent; //grid-object
    [SerializeField] private GameObject implingDisplayPrefab; //DispalyPrefab

    private void Start()
    {
        PopulateList();
    }

    private void PopulateList()
    {
        //delete old displays
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        //load impling data from Resources/Implings
        TabletData[] allImplings = Resources.LoadAll<TabletData>("Implings");

        if (allImplings.Length == 0)
        {
            Debug.LogWarning("Keine Implings gefunden! Sie müssen unter 'Assets/Resources/Implings/' liegen.");
            return;
        }

        // create a display for every tablet
        foreach (var data in allImplings)
        {
            GameObject instance = Instantiate(implingDisplayPrefab, contentParent);
            instance.GetComponent<ImplingDisplay>().SetData(data);
            instance.transform.localScale = Vector3.one;
        }
    }
}
