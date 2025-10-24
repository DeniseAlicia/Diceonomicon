using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImplingDisplay : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text nameText;

    public void SetData(TabletData data)
    {
        // Texture in Sprite umwandeln
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
            Debug.LogWarning($"Artwork für {data.name} ist keine Texture2D!");
        }

        nameText.text = data.name;
    }
}
