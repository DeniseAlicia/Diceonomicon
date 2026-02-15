using UnityEngine;

public class BannerSway : MonoBehaviour
{
    public float swayAmplitude = 5f; // degrees or pixels
    public float swaySpeed = 1f;     // how fast it sways
    public bool useRotation = true;  // rotate or move

    private RectTransform rectTransform;
    private float startRotation;
    private Vector2 startPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startRotation = rectTransform.localEulerAngles.z;
        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmplitude;

        if (useRotation)
            rectTransform.localRotation = Quaternion.Euler(0, 0, startRotation + sway);
        else
            rectTransform.anchoredPosition = startPosition + new Vector2(sway, 0);
    }
}