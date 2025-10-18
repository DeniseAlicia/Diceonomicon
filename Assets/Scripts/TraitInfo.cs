using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class TraitInfo : MonoBehaviour
{
    public GameObject traitObject;
    public RectTransform nameObject;  // Works fine even though it's a RectTransform
    private InputAction tooltipAction;
    private Tween moveTween;

    // How far to move the object when hovered
    [SerializeField] private Vector3 hoverOffset = new Vector3(0f, 0f, -3f);
    [SerializeField] private float moveDuration = 0.3f;

    private Vector3 initialLocalPos;

    private void Awake()
    {
        traitObject.SetActive(false);
        tooltipAction = InputSystem.actions.FindAction("ShowInfo");
        initialLocalPos = nameObject.localPosition;

        moveDuration = 0.01f;
        MoveObjects(initialLocalPos - hoverOffset, false);
        moveDuration = 0.3f;
    }

    private void OnMouseEnter()
    {
        MoveObjects(initialLocalPos, true);
    }

    private void OnMouseExit()
    {
        traitObject.SetActive(false);
        MoveObjects(initialLocalPos - hoverOffset, false);
    }

    private void MoveObjects(Vector3 targetLocalPos, bool activateAfterMove)
    {
        moveTween?.Kill();

        var tween = nameObject.DOLocalMove(targetLocalPos, moveDuration)
                              .SetEase(Ease.OutQuad);

        if (activateAfterMove)
        {
            tween.OnComplete(() =>
            {
                traitObject.SetActive(true);
            });
        }

        moveTween = tween;
    }

    private void OnEnable()
    {
        if (tooltipAction == null) return;

        tooltipAction.performed += OnTooltipPerformed;
        tooltipAction.canceled += OnTooltipCanceled;
        tooltipAction.Enable();
    }

    private void OnDisable()
    {
        if (tooltipAction == null) return;

        tooltipAction.performed -= OnTooltipPerformed;
        tooltipAction.canceled -= OnTooltipCanceled;
        tooltipAction.Disable();
    }

    private void OnTooltipPerformed(InputAction.CallbackContext ctx) => OnMouseEnter();
    private void OnTooltipCanceled(InputAction.CallbackContext ctx) => OnMouseExit();
}
