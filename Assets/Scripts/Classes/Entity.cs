
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public abstract class Entity : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public Material healthMaterial;
    public Material overlayMaterial;
    public TMP_Text healthText;

    public List<DiceData> diceDeck;
    public List<DiceData> drawnDice;
    public int drawSize; //how many dice can be drawn at round start
    public List<DiceData> discardPile;
    public int damage = 10;
    public int block = 0;
    public List<DiceSlotController> activeColumn;

    // Health UI Testing
    public Button healthUp;
    public Button healthDown;
    public float alpha;
    public GameObject diePrefab;

    public void Update()
    {
        // Add depleting health bar effect
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float yOffset = 0.5f - currentHealth * 0.2f / maxHealth;
        Vector4 healthOffset = healthMaterial.GetVector("_TopMap_ST");
        healthOffset.w = yOffset;
        healthMaterial.SetVector("_TopMap_ST", healthOffset);

        // Adjusting Alpha of Health UI elements
        alpha = Mathf.Clamp(alpha, 0f, 1f);

        Color healthColor = healthMaterial.GetColor("_BaseColor");
        healthColor.a = alpha / 4;
        healthMaterial.SetColor("_BaseColor", healthColor);

        Color overlayColor = overlayMaterial.color;
        overlayColor.a = alpha * 2;
        overlayMaterial.color = overlayColor;

        Color textColor = healthText.color;
        textColor.a = alpha;
        healthText.color = textColor;
    }

    public void DrawDice()
    {
        for (int i = 0; i < drawSize; i++)
        {
            if (diceDeck == null | diceDeck.Count == 0)
            {
                if (discardPile == null || discardPile.Count == 0)
                {
                    return;
                }
                diceDeck.AddRange(discardPile);
                discardPile.Clear();
            }
            int randomIndex = Random.Range(0, diceDeck.Count);
            DiceData drawnDie = diceDeck[randomIndex];
            diceDeck.RemoveAt(randomIndex);
            drawnDice.Add(drawnDie);
        }
    }
    
    public abstract void RollDice();
}