using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImpSelectDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Normal View")]
    [SerializeField] private GameObject normalView;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text nameText;

    [Header("Hover View")]
    [SerializeField] private GameObject hoverView;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text traitText;

    [SerializeField] private Image selectionColor;

    private TabletData currentData;
    private GameObject currentTabletInstance;

    public GameObject cameras;

    public void SetData(TabletData data)
    {

        currentData = data;

        Highlight(false);

        Texture2D texture = data.artwork as Texture2D;
        if (texture != null)
        {
            portraitImage.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }
        else
        {
            Debug.LogWarning("No Texture.");
        }

        //nameText.text = data.name;
        descriptionText.text = data.desc;
        traitText.text = data.trait;

        normalView.SetActive(true);
        hoverView.SetActive(false);
    }

    public TabletData GetCurrentData()
    {
        return currentData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //normalView.SetActive(false);
        hoverView.SetActive(true);
        
        currentTabletInstance = Instantiate(currentData.tabletPrefab);
        currentTabletInstance.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

        cameras = FindFirstObjectByType<ImpSelectList>().cameras;

        if (currentTabletInstance.name.Contains("Small"))
        {
            cameras.transform.position = new(cameras.transform.position.x, cameras.transform.position.y, 4f);
        }
        else if (currentTabletInstance.name.Contains("Medium"))
        {
            cameras.transform.position = new(cameras.transform.position.x, cameras.transform.position.y, 3f);
        }
        else
        {
            cameras.transform.position = new(cameras.transform.position.x, cameras.transform.position.y, 2f);
        }

        TraitInfo trait = currentTabletInstance.GetComponentInChildren<TraitInfo>();

        trait.MoveObjects(trait.initialLocalPos, true);

        TabletController controller = currentTabletInstance.GetComponent<TabletController>();
        controller.SetData(currentData);

       //Debug.Log("Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //normalView.SetActive(true);
        hoverView.SetActive(false);

        Destroy(currentTabletInstance);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool added = ImpSelectManager.Instance.Selection(currentData);

        Highlight(added);
    }

    private void Highlight(bool selected)
    {
        if (selectionColor != null)
            selectionColor.enabled = selected;
    }
}
