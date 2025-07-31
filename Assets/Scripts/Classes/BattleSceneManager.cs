using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class BattleSceneManager : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public List<DiceSlotController> playerActiveColumn;
    public List<DiceSlotController> enemyActiveColumn;
    public int level;
    public float[] columnStartPositions = new float[] { -7.65f, -6.9f, -6.1f }; // 1,2,3 = Player
    public GameObject[] playerColumnBanner;
    public GameObject[] opponentColumnBanner;

    public EndBattle endScene;
    public GameObject columnMaster;
    public Button confirmButton;
    public GameObject combatBolt;

    public FMODUnity.EventReference DiceRollEvent;
    public FMODUnity.EventReference DiceDrawEvent;
    public FMODUnity.EventReference RoundStartEvent;
    public FMODUnity.EventReference DamageEvent;

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

        Button confirm = confirmButton.GetComponent<Button>();
        confirm.onClick.AddListener(() => CombatManager.HandleActiveCombat(this));

        NewRound();
    }

    void Update()
    {
        if (player.inColumnPhase == true)
        {
            player.alpha = Mathf.MoveTowards(player.alpha, 0.9f, 0.21f * Time.deltaTime);
            opponent.alpha = Mathf.MoveTowards(player.alpha, 0.9f, 0.2f * Time.deltaTime);
        }

        if (player.block != 0)
        {
            player.blockText.text = player.block.ToString();
        }
        else
        {
            player.blockText.text = null;
        }

        if (opponent.block != 0)
        {
            opponent.blockText.text = opponent.block.ToString();
        }
        else
        {
            opponent.blockText.text = null;
        }

        if (player.damage != 0)
        {
            player.damageText.text = player.damage.ToString();
        }
        else
        {
            player.damageText.text = null;
        }

        if (opponent.damage != 0)
        {
            opponent.damageText.text = opponent.damage.ToString();
        }
        else
        {
            opponent.damageText.text = null;
        }
    }

    private void NewRound()
    {
        FMOD.Studio.EventInstance roundStartAudio = FMODUnity.RuntimeManager.CreateInstance(RoundStartEvent);
        roundStartAudio.start();

        opponent.DrawDice();
        opponent.ai.RollDice();
        PlacementPhase();
    }

    private void PlacementPhase()
    {
        FMOD.Studio.EventInstance drawDiceAudio = FMODUnity.RuntimeManager.CreateInstance(DiceDrawEvent);
        drawDiceAudio.start();
        player.DrawDice();

        FMOD.Studio.EventInstance rollDiceAudio = FMODUnity.RuntimeManager.CreateInstance(DiceRollEvent);
        rollDiceAudio.start();
        player.RollDice();

        player.drawSize = player.maxDrawSize;
        opponent.drawSize = opponent.maxDrawSize;

        StartCoroutine(SortAfterDelay());
    }

    private IEnumerator SortAfterDelay()
    {
        float delay = 3f;
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

    public async void CalculateDamage()
    {
        int damageTaken = opponent.damage - player.block;
        int targetHealth = Mathf.Max(player.currentHealth - damageTaken, 0);
        StartCoroutine(AnimatePlayerHealthDecrease(targetHealth, damageTaken));

        int opponentDamageTaken = player.damage - opponent.block;
        int targetOpponentHealth = Mathf.Max(opponent.currentHealth - opponentDamageTaken, 0);
        StartCoroutine(AnimateOpponentHealthDecrease(targetOpponentHealth, opponentDamageTaken));

        opponent.damage = 0;
        opponent.block = 0;
        player.damage = 0;
        player.block = 0;

        CheckWinLossState();

        if (CombatManager.currentColumn == 3)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            player.alpha = 0.1f; // 0.1f for rolling, 0.9f for post-placement
            opponent.alpha = 0.1f; // 0.1f for rolling, 0.9f for post-placement
            EndOfRound();
        }
    }

    public void CheckWinLossState()
    {
        if (player.currentHealth <= 0)
        {
            endScene.Lose();
            EndOfRound();
            Time.timeScale = 0;
        }

        if (opponent.currentHealth <= 0 && player.currentHealth > 0)
        {
            endScene.Win();
            EndOfRound();
            Time.timeScale = 0;
        }


    }

    private IEnumerator AnimatePlayerHealthDecrease(int targetHealth, int damage)
    {
        float wait = 0.1f;

        while (player.currentHealth > targetHealth)
        {
            if (damage < 11)
            {
                wait = 0.1f;
            }
            else
            {
                wait = 1.5f / damage;
            }

            FMOD.Studio.EventInstance damageAudio = FMODUnity.RuntimeManager.CreateInstance(DamageEvent);
            damageAudio.start();

            player.currentHealth -= 1;
            player.healthText.text = player.currentHealth.ToString();
            yield return new WaitForSeconds(wait);
            damageAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CheckWinLossState();
        }
    }

    private IEnumerator AnimateOpponentHealthDecrease(int targetHealth, int damage)
    {
        float wait = 0.1f;
        while (opponent.currentHealth > targetHealth)
        {
            if (damage < 11)
            {
                wait = 0.1f;
            }
            else
            {
                wait = 1.5f / damage;
            }

            FMOD.Studio.EventInstance damageAudio = FMODUnity.RuntimeManager.CreateInstance(DamageEvent);
            damageAudio.start();

            opponent.currentHealth -= 1;
            opponent.healthText.text = opponent.currentHealth.ToString();
            yield return new WaitForSeconds(wait);
            damageAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CheckWinLossState();
        }
    }

    private void EndOfRound()
    {
        ResetEntity(player);
        ResetEntity(opponent);
        RotationButton.ResetRotationButton();

        Die[] dice = FindObjectsByType<Die>(FindObjectsSortMode.None);
        foreach (Die die in dice)
        {
            die.isFrozen = false;
        }

        NewRound();
    }

    private void ResetEntity(Entity entity)
    {
        List<DiceData> dicardedDice = new List<DiceData>(entity.drawnDice);

        foreach (DiceData dieData in dicardedDice)
        {
            entity.discardPile.Add(dieData);
            entity.drawnDice.Remove(dieData);
        }

        Die[] dice = FindObjectsByType<Die>(FindObjectsSortMode.None);

        foreach (Die die in dice)
        {
            if (die.isFrozen == false)
            {
                GameObject dieObject = die.transform.gameObject;
                Destroy(dieObject);
            }
        }

        opponent.damage = 0;
        opponent.block = 0;
        player.damage = 0;
        player.block = 0;
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
                        return;
                    }

                }

                DiceSlotController slotController = hit.collider.GetComponent<DiceSlotController>();
                if (slotController != null)
                {
                    playerActiveColumn.Add(slotController);
                }
                else
                {
                    // Debug.Log("Hit object does not have a Slot component.");
                }
            }
        }
        Debug.Log("Slots: " + string.Join(", ", playerActiveColumn));
    }

    public void ClearActiveColumn()
    {
        foreach (DiceSlotController slot in enemyActiveColumn)
        {
            if (slot.wasFrozen == false)
            {
                slot.isFilled = false;
                slot.slottedDie = null;
            }
        }
        foreach (DiceSlotController slot in playerActiveColumn)
        {
            if (slot.wasFrozen == false)
            {
                slot.isFilled = false;
                slot.slottedDie = null;
            }
        }

        playerActiveColumn.Clear();
        enemyActiveColumn.Clear();
    }
}

