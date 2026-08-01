using System.Collections;
using UnityEngine;

public class RotationButton : MonoBehaviour
{
    public TabletController tablet;
    public float angle = 90f;

    private Color baseColor = new Color32(175, 175, 175, 255);
    private Color hoverColor = new Color32(255, 255, 255, 255);
    private Color clickColor = new Color32(100, 100, 100, 255);

    private MeshRenderer meshRenderer;
    private Material materialInstance;

    public static RotationButton[] allButtons;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            materialInstance = meshRenderer.material;
            materialInstance.color = baseColor;
        }

        allButtons = FindObjectsByType<RotationButton>(FindObjectsSortMode.None);

        BattleSceneManager.OnRoundStart.AddListener(ResetRotationButtons);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(DisableRotationButton);
    }

    public void ResetRotationButtons()
    {
        tablet.currentRotations = 0;
        foreach (RotationButton button in allButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void DisableRotationButton()
    {
        foreach (RotationButton button in allButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        materialInstance.color = hoverColor;
    }

    private void OnMouseDown()
    {
        if (!tablet.isRotating && tablet != null && tablet.currentRotations < tablet.maxRotations)
        {
            materialInstance.color = clickColor;
            tablet.Rotate(angle);
            if (tablet.currentRotations >= tablet.maxRotations)
            {
                foreach (RotationButton button in allButtons)
                {
                    button.DisableRotationButton();
                }
            }
        }
    }

    private void OnMouseUp()
    {
        materialInstance.color = hoverColor;
    }

    private void OnMouseExit()
    {
        materialInstance.color = baseColor;
    }

    public void OnDestroy()
    {
        BattleSceneManager.OnRoundStart.RemoveListener(ResetRotationButtons);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(DisableRotationButton);
    }
}
