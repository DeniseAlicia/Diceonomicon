using UnityEngine;
using TMPro;

public class TabletController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descText;
    public Renderer artworkRenderer;
    public Entity owner;

    public void SetData(TabletData data)
    {
        Transform tabletMain = transform.Find("TabletMain");
        nameText.text = data.name;
        descText.text = data.desc;
        owner = data.owner;
        artworkRenderer.material.mainTexture = data.artwork;
        data.CreateSlots(tabletMain);
    }
}
