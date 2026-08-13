using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
        MainMenuManager.Instance.diceViewObject.SetActive(true);
        MainMenuManager.Instance.healthObject.SetActive(true);

        for (int i = 0; i < MainMenuManager.Instance.diceSprites.Count(); i++)
        {
            MainMenuManager.Instance.diceSprites[i].sprite = currentData.startingDice[i].image;
            int emotionColor = Array.IndexOf(Emotions.types, currentData.startingDice[i].tags[0]);
            MainMenuManager.Instance.diceSprites[i].color = Emotions.colors[emotionColor];
        }

        MainMenuManager.Instance.healthObjectText.text = currentData.health.ToString();

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

        // trait.MoveObjects(trait.initialLocalPos, true);


        TabletController controller = currentTabletInstance.GetComponent<TabletController>();
        controller.SetData(currentData);

        GameObject traitBox = controller.descText.gameObject;
        traitBox.transform.position = new(traitBox.transform.position.x + 3.5f, traitBox.transform.position.y, traitBox.transform.position.z + 1.075f);

        GameObject nameBox = controller.nameText.gameObject;
        nameBox.transform.position = new(nameBox.transform.position.x, nameBox.transform.position.y, nameBox.transform.position.z - 1);
        //Debug.Log("Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //normalView.SetActive(true);
        hoverView.SetActive(false);
        Destroy(currentTabletInstance);
        MainMenuManager.Instance.diceViewObject.SetActive(false);
        MainMenuManager.Instance.healthObject.SetActive(false);
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
