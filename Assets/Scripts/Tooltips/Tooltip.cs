using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;
    public float offset;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector2 position = Input.mousePosition;

        // Is the mouse on the left or right side of the screen?
        bool isLeftSide = position.x < Screen.width / 2f;

        // Responsive horizontal offset.
        float xOffset = Mathf.Clamp(Screen.width * 0.05f, 50f, 150f);

        // Put the object to the right of the mouse if on the left,
        // and to the left of the mouse if on the right.
        if (!isLeftSide)
            xOffset = -xOffset;

        float yOffset = 50f + offset;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = new Vector2(
            position.x + xOffset,
            position.y + yOffset
        );
    }
}
