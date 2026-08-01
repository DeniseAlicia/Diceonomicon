using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class Player : Entity
{
    public List<string> ImplingRoster;
    public List<TabletData> ActiveImplings;
    public int MaxImplings;
    public List<Die> dice;
    public List<RelicData> relics;
    public Owner owner = Owner.Player;

    public int level;
    public string area;
    public static Player Instance;

    public void Awake()
    {
        Instance = this;
    }

    public List<TabletData> LoadPlayer()
    {
        ActiveImplings = GameStateManager.Instance.player.activeImplings;
        diceDeck = GameStateManager.Instance.player.diceDeck;
        return ActiveImplings;
    }

    public override void SetHealth()
    {
        foreach (TabletData imp in ActiveImplings)
        {
            maxHealth += imp.health;
        }
        currentHealth = maxHealth;
    }

    public override void RollDice()
    {
        dice = new List<Die>();
        Vector3 startPosition = new Vector3(-1f, 5f, -5f);
        float distance = 0.5f;

        for (int i = 0; i < drawnDice.Count; i++)
        {
            DiceData dieData = drawnDice[i];

            float overflow = Mathf.Floor(i / 3f);
            float spacing = (i - overflow * 3) * distance;

            Vector3 diePos = startPosition;
            diePos.x += spacing;
            diePos.z += overflow * distance;

            // Instantiate prefab at the calculated position
            GameObject dieObject = Instantiate(diePrefab, diePos, Quaternion.identity);

            // Set data on the die script
            Die die = dieObject.GetComponent<Die>();
            die.SetData(dieData);



            // Add dice to die Class list
            dice.Add(die);

        }

        // Debug.Log("Slots: " + string.Join(", ", dice));

        foreach (Die dieInstance in dice)
        {
            dieInstance.Roll(0.05f);
        }
    }
}
