using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

public class TabletController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descText;
    public Renderer artworkRenderer;
    public Entity owner;
    public Component script;
    public List<DiceSlotController> tabletSlots;
    public Animator animator;

    [Header("Tablet References")]
    [SerializeField] GameObject mainTablet;
    [SerializeField] GameObject tabletShadow;
    [SerializeField] GameObject tabletLight;
    private Quaternion shadowRotation;
    private Quaternion lightRotation;

    // Private References
    public bool isRotating = false;
    public int currentRotations = 0;
    public int maxRotations = 1;
    private float rotationDuration = 0.5f;

    public int[] emotionValues = new int[Enum.GetValues(typeof(Emotion)).Length];

    public void Start()
    {
        shadowRotation = tabletShadow.transform.rotation;
        lightRotation = tabletLight.transform.rotation;
    }

    public void SetData(TabletData data)
    {
        Transform tabletMainTransform = transform.Find("TabletMain");
        nameText.text = data.name;
        descText.text = data.desc;
        owner = data.owner;
        if (data.trait != null)
        {
            Type type = Type.GetType(data.trait);
            script = gameObject.AddComponent(type);
        }
        artworkRenderer.material.mainTexture = data.artwork;
        data.CreateSlots(tabletMainTransform, this);
    }

    public void Rotate(float angle)
    {
        StartCoroutine(RotateSmooth(angle));
    }

    private IEnumerator RotateSmooth(float angle)
    {
        isRotating = true;
        currentRotations += 1;

        // Rotate tablet over time
        Quaternion startRot = mainTablet.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, angle, 0f);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            mainTablet.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            tabletLight.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            tabletShadow.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Rotate all tablet slots to starting rotation
        Transform[] children = new Transform[mainTablet.transform.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = mainTablet.transform.GetChild(i);
        }

        foreach (Transform child in children)
        {
            child.Rotate(0f, -angle, 0f);
        }

        tabletLight.transform.rotation = lightRotation;
        tabletShadow.transform.rotation = shadowRotation;

        mainTablet.transform.rotation = endRot;
        isRotating = false;
    }
}
