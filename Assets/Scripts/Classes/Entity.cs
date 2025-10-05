
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public abstract class Entity : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public GameObject candle;
    public TMP_Text healthText;
    public TMP_Text damageText;
     public TMP_Text blockText;

    public List<DiceData> diceDeck;
    public List<DiceData> drawnDice;
    public int maxDrawSize;
    public int drawSize; //how many dice can be drawn at round start
    public List<DiceData> discardPile;
    public int damage = 10;
    public int block = 0;
    public List<DiceSlotController> activeColumn;

    // Health UI Testing
    public float alpha;
    public GameObject diePrefab;
    public bool inColumnPhase;

    public void Start()
    {
        drawSize = maxDrawSize;
    }

    public void Update()
    {
        
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