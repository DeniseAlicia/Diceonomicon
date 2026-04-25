
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImplingPreview : MonoBehaviour
{
    public ImpSelectManager selectManager;
    public Image impSprite;
    public TMP_Text impName;
    public int order;
    public bool assigned;

    public Vector3 startPosition;
    public Vector3 goalPosition;

    public void Start()
    {
        startPosition = this.transform.position;
        goalPosition = new Vector3(startPosition.x + 135, startPosition.y, startPosition.z);
    }

    public void Update()
    {
        if (assigned && this.transform.position != goalPosition)
        {
            float speed = 1f;
            this.gameObject.transform.DOMove(goalPosition, speed).SetEase(Ease.OutQuad);
            speed += 0.1f;
        }
    }
}
