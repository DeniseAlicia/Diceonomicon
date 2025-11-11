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

    public void SetData(TabletData data)
    {

        currentData = data;

        Highlight(false);

        Texture2D tex = data.artwork as Texture2D;
        if (tex != null)
        {
            portraitImage.sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
        }
        else
        {
            Debug.LogWarning($"Artwork für {data.name} ist keine Texture.");
        }

        nameText.text = data.name;
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
        normalView.SetActive(false);
        hoverView.SetActive(true);
        Debug.Log("Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        normalView.SetActive(true);
        hoverView.SetActive(false);
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
