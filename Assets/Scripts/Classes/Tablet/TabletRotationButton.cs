using System.Collections;
using UnityEngine;

public class RotationButton : MonoBehaviour
{
    public Transform targetObject;
    [SerializeField] private Transform tabletShadow;
    [SerializeField] private Transform tabletLight;
    private Quaternion shadowRotation;
    private Quaternion lightRotation;
    public float angle = 90f;
    public static int maxRotations = 1;

    private Color baseColor = new Color32(175, 175, 175, 255);
    private Color hoverColor = new Color32(255, 255, 255, 255);
    private Color clickColor = new Color32(100, 100, 100, 255);
    private float rotationDuration = 0.5f;

    private MeshRenderer meshRenderer;
    private Material materialInstance;
    public static RotationButton[] allButtons;

    private static bool isRotating = false;
    private static int currentRotations = 0;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            materialInstance = meshRenderer.material;
            materialInstance.color = baseColor;
        }

        allButtons = Object.FindObjectsByType<RotationButton>(FindObjectsSortMode.None);

        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);

        shadowRotation = tabletShadow.rotation;
        lightRotation = tabletLight.rotation;
    }

    private void OnRoundStart()
    {
        currentRotations = 0;
        foreach (RotationButton button in allButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    private void OnAcvitveCombatStart()
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
        if (!isRotating && targetObject != null && currentRotations < maxRotations)
        {
            materialInstance.color = clickColor;
            StartCoroutine(RotateParentSmooth_ChildrenInstant(angle));
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

    private IEnumerator RotateParentSmooth_ChildrenInstant(float angle)
    {
        isRotating = true;
        currentRotations += 1;

        // Rotate tablet over time
        Quaternion startRot = targetObject.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, angle, 0f);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            targetObject.rotation = Quaternion.Slerp(startRot, endRot, t);
            tabletLight.rotation = Quaternion.Slerp(startRot, endRot, t);
            tabletShadow.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Rotate all tablet slots to starting rotation
        Transform[] children = new Transform[targetObject.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = targetObject.GetChild(i);
        }

        foreach (Transform child in children)
        {
            child.Rotate(0f, -angle, 0f);
        }

        tabletLight.rotation = lightRotation;
        tabletShadow.rotation = shadowRotation;

        targetObject.rotation = endRot;
        isRotating = false;

        // Disable button visuals when player can't rotate anymore
        if (currentRotations >= maxRotations)
        {
            foreach (RotationButton button in allButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public void OnDestroy()
    {
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}
