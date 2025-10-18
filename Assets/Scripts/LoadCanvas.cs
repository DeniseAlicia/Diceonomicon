using UnityEngine;
using DG.Tweening;

public class LoadCanvas : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject[] banners;
    public GameObject score;
    public GameObject chest;
    public GameObject[] candles;
    public GameObject[] health;
    public GameObject buttons;

    [Header("Animation Settings")]
    [SerializeField] private Vector3 topEntryOffset = new Vector3(0f, 50f, 0f); 
    [SerializeField] private Vector3 bottomEntryOffset = new Vector3(0f, -50f, 0f);
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float staggerDelay = 0.2f;

    private void Start()
    {
        AnimateObjectsIn();
    }

    private void AnimateObjectsIn()
    {
        AnimateGroup(banners, staggerDelay, topEntryOffset);
        AnimateSingle(score, 0.3f, topEntryOffset);

        AnimateGroup(candles, staggerDelay * 0f, bottomEntryOffset * 2);
        AnimateGroup(health, staggerDelay * 0.3f, bottomEntryOffset);
        AnimateSingle(chest, 0.5f, bottomEntryOffset);
        AnimateSingle(buttons, 0.6f, bottomEntryOffset);
    }

    private void AnimateGroup(GameObject[] objects, float delayStep, Vector3 entryOffset)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];
            if (obj == null) continue;

            Transform t = obj.transform;
            Vector3 startPos = t.localPosition + entryOffset;
            Vector3 endPos = t.localPosition;

            t.localPosition = startPos;
            obj.SetActive(true);

            t.DOLocalMove(endPos, moveDuration)
             .SetEase(Ease.OutQuad)
             .SetDelay(i * delayStep);
        }
    }

    private void AnimateSingle(GameObject obj, float delay, Vector3 entryOffset)
    {
        if (obj == null) return;

        Transform t = obj.transform;
        Vector3 startPos = t.localPosition + entryOffset;
        Vector3 endPos = t.localPosition;

        t.localPosition = startPos;
        obj.SetActive(true);

        t.DOLocalMove(endPos, moveDuration)
         .SetEase(Ease.OutQuad)
         .SetDelay(delay);
    }
}

