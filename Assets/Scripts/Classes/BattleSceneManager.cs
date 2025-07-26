
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public List<DiceSlotController> playerActiveColumn;
    public List<DiceSlotController> enemyActiveColumn;
    public int level;
    public float[] columnStartPositions = new float[] { -7.65f, -6.9f, -6.1f }; // 1,2,3 = Player

    public EndBattle endScene;
    public GameObject columnMaster;
    public Button confirmButton;

    private void Awake()
    {
        Camera camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();
        camBattleTablets.gameObject.SetActive(false);
        camBattleTablets.gameObject.SetActive(true);
        player.SetImplingRoster();
        opponent.SetEnemyRoster();
    }

    private void Start()
    {
        // Debug.Log("BattleSceneManager.BuildScene");
        opponent.currentHealth = opponent.maxHealth;
        // player.currentHealth = player.maxHealth;

        player.alpha = 0.1f; // 0.1f for rolling, 0.9f for post-placement
        opponent.alpha = 0.1f; // 0.1f for rolling, 0.9f for post-placement
        player.healthText.text = player.currentHealth.ToString();
        opponent.healthText.text = opponent.currentHealth.ToString();

        // Add Test Buttons
        Button upButton = player.healthUp.GetComponent<Button>();
        upButton.onClick.AddListener(GainHealth);

        Button downButton = player.healthDown.GetComponent<Button>();
        downButton.onClick.AddListener(LoseHealth);

        Button confirm = confirmButton.GetComponent<Button>();
        confirm.onClick.AddListener(() => CombatManager.HandleActiveCombat(this));

        NewRound();
    }

    private void NewRound()
    {
            opponent.DrawDice();
            opponent.RollDice();
            //opponent.ai.PlaceDice(opponent.drawnDice);
            PlacementPhase();
    }

    private void PlacementPhase()
    {
        player.DrawDice();
        player.RollDice();

        StartCoroutine(SortAfterDelay());
    }

    private IEnumerator SortAfterDelay()
    {
        float delay = 4f;
        yield return new WaitForSeconds(delay);
        foreach (Die die in player.dice)
        {
            die.GetSideFacingUp();
            die.isResting = true;
            die.isDraggable = true;
            die.rigidBody.isKinematic = true;
            die.rigidBody.useGravity = false;

        }
        DiceManager.SortAllDice(player.dice);
    }

    public void CalculateDamage()
    {
        player.currentHealth -= Math.Max(opponent.damage - player.block, 0);
        player.healthText.text = player.currentHealth.ToString();

        opponent.currentHealth -= Math.Max(player.damage - opponent.block, 0);
        opponent.healthText.text = opponent.currentHealth.ToString();

        opponent.damage = 0;
        opponent.block = 0;
        player.damage = 0;
        player.block = 0;

        if (player.currentHealth <= 0)
        {
            endScene.Lose();
        }

        if (opponent.currentHealth <= 0 && player.currentHealth > 0)
        {
            endScene.Win();
        }

        if (CombatManager.currentColumn == 3)
        {
            EndOfRound();
        }
    }
    private void EndOfRound()
    {
        ResetEntity(player);
        ResetEntity(opponent);
        NewRound();
    }
    private void ResetEntity(Entity entity)
    {
        List<DiceData> dicardedDice = new List<DiceData>(entity.drawnDice);

        foreach (DiceData die in dicardedDice)
        {
            entity.discardPile.Add(die);
            entity.drawnDice.Remove(die);
        }

        Die[] dice = FindObjectsByType<Die>(FindObjectsSortMode.None);

        foreach (Die dieInstance in dice)
        {
            GameObject dieObject = dieInstance.transform.gameObject;
            Destroy(dieObject);
        }

        entity.damage = 0;
        entity.block = 0;
    }

    public void GainHealth()
    {
        player.currentHealth += 1;
        player.healthText.text = player.currentHealth.ToString();

    }

    public void LoseHealth()
    {
        player.currentHealth -= 1;
        player.healthText.text = player.currentHealth.ToString();

        if (player.currentHealth <= 0)
        {
            endScene.Lose();
        }

        if (opponent.currentHealth <= 0 && player.currentHealth > 0)
        {
            endScene.Win();
        }
    }

    public void GetActiveColumn(int column)
    {
        playerActiveColumn.Clear();
        enemyActiveColumn.Clear();
        float columnPosX = columnStartPositions[column - 1];

        for (int i = 0; i < 9; i++)
        {
            float yJump = i * -0.8f;

            Vector3 rayPosition = new Vector3(columnPosX, columnMaster.transform.position.y + yJump, columnMaster.transform.position.z);
            Ray ray = new Ray(rayPosition, Vector3.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 666))
            {
                Vector3 ray2Position = new Vector3(columnPosX + 14.3f, columnMaster.transform.position.y + yJump, columnMaster.transform.position.z);
                Ray ray2 = new Ray(ray2Position, Vector3.forward);
                Debug.DrawRay(ray2Position, Vector3.forward * 10, Color.green, 666);
                if (Physics.Raycast(ray2, out RaycastHit hit2, 666))
                {
                    DiceSlotController slotController2 = hit2.collider.GetComponent<DiceSlotController>();
                    if (slotController2 != null)
                    {
                        enemyActiveColumn.Add(slotController2);
                    }
                    else
                    {
                        Debug.Log("Hit object does not have a Slot component.");
                    }

                }

                DiceSlotController slotController = hit.collider.GetComponent<DiceSlotController>();
                if (slotController != null)
                {
                    playerActiveColumn.Add(slotController);
                }
                else
                {
                    Debug.Log("Hit object does not have a Slot component.");
                }
            }
        }
        Debug.Log("Slots: " + string.Join(", ", playerActiveColumn));
        Debug.Log("Slots: " + string.Join(", ", enemyActiveColumn));
    }

}
