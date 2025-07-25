
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
    public List<Die> dice;
    public int drawSize; //how many dice can be drawn at round start
    public List<DiceData> discardPile;
    public int damage = 0;
    public int block = 0;
    public List<DiceSlotController> activeColumn;

    // Health UI Testing
    public Button healthUp;
    public Button healthDown;
    public float alpha;

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
            if (diceDeck == null)
            {
                if (discardPile == null)
                {
                    return;
                }
                foreach (DiceData die in discardPile)
                {
                    discardPile.Remove(die);
                    diceDeck.Add(die);
                }
            }
            int randomIndex = Random.Range(0, diceDeck.Count);
            DiceData drawnDie = diceDeck[randomIndex];
            diceDeck.Remove(drawnDie);
            drawnDice.Add(drawnDie);
        }
    }

    public void RollDice()
    {
        // foreach (DiceData die in drawnDice)
        // {
        //     die.Roll();
        // }
    }

    public void CalculateColumns()
    {
        Debug.Log("Entity.CalculateColumns");
    }
}