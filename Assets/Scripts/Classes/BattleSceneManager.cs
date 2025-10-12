using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public GameObject[] playerColumnBanner;
    public GameObject[] opponentColumnBanner;
    public List<DiceSlotController> playerActiveColumn;
    public List<DiceSlotController> enemyActiveColumn;
    private static List<List<DiceSlotController>> playerSlots;
    private static List<List<DiceSlotController>> enemySlots;
    private static DiceSlotController[] allSlots;

    // Combat Stats
    public int level;
    public int round;
    public int Score { get; private set; }
    public List<int> ScoreList { get; private set; } = new List<int>();
    public float[] columnStartPositions = new float[] { -7.65f, -6.9f, -6.1f };
    public static int CurrentColumn { get; private set; }
    public bool intermission;

    // UI Elements & Buttons
    public EndBattle endScene;
    public Button placementButton;
    public Button intermissionButton;
    public Button rollDiceButton;
    public GameObject combatBolt;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;

    // FMOD
    public FMODUnity.EventReference DiceRollEvent;
    public FMODUnity.EventReference DiceDrawEvent;
    public FMODUnity.EventReference RoundStartEvent;
    public FMODUnity.EventReference DamageEvent;



    private void Awake()
    {
        Camera camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();
        camBattleTablets.gameObject.SetActive(false);
        camBattleTablets.gameObject.SetActive(true);


        player.CreateDiceDeck();
        //opponent.SetEnemyRoster();

        round = 0;
        Score = 0;
        player.level = 1;
        player.area = "Green";
        opponent.damage = 0;
        opponent.block = 0;
        player.damage = 0;
        player.block = 0;
        UpdateDamageBlockUI();
    }



    private void Start()
    {
        scoreText.text = Score.ToString();
        opponent.currentHealth = opponent.maxHealth;
        player.alpha = 0.9f;
        opponent.alpha = 0.9f;
        player.healthText.text = player.currentHealth.ToString();
        opponent.healthText.text = opponent.currentHealth.ToString();

        placementButton.onClick.AddListener(() => StartActiveCombat());
        placementButton.gameObject.SetActive(false);

        rollDiceButton.onClick.AddListener(() => StartPlacementPhase());
        rollDiceButton.gameObject.SetActive(false);

        intermissionButton.onClick.AddListener(() => ContinueColumnPhase());
        intermissionButton.gameObject.SetActive(false);

        StartNewRound();
    }



    private void StartNewRound()
    {
        allSlots = GameObject.FindObjectsByType<DiceSlotController>(FindObjectsSortMode.None);
        
        if (opponent.currentHealth == 0)
        {
            NewEncounter(player.level, player.area);
        }

        foreach (GameObject banner in playerColumnBanner)
        {
            Image sprite = banner.GetComponent<Image>();
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }

        foreach (GameObject opponentBanner in opponentColumnBanner)
        {
            Image sprite = opponentBanner.GetComponent<Image>();
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }

        foreach (DiceSlotController slot in allSlots)
        {
            if (slot.tag == "Buff")
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0f, 1f, 0.2f, 0.6f));
            }
            else if (slot.tag == "Spell")
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.6f, 0.2f, 1f, 0.6f));
            }
            else if (slot.tag == "Damage")
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(1f, 0f, 0.15f, 0.6f));
            }
            else
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.1f, 0.45f, 1f, 0.6f));
            }
        }

        FMOD.Studio.EventInstance roundStartAudio = FMODUnity.RuntimeManager.CreateInstance(RoundStartEvent);
        roundStartAudio.start();

        round += 1;
        opponent.DrawDice();
        StartDelay(1f, () => opponent.ai.RollDice());
        StartDelay(4f, () => StartPrePlacementPhase());
    }

    private void StartPrePlacementPhase()
    {
        rollDiceButton.gameObject.SetActive(true);
    }



    private void StartPlacementPhase()
    {
        rollDiceButton.gameObject.SetActive(false);

        FMOD.Studio.EventInstance drawDiceAudio = FMODUnity.RuntimeManager.CreateInstance(DiceDrawEvent);
        drawDiceAudio.start();
        player.DrawDice();

        FMOD.Studio.EventInstance rollDiceAudio = FMODUnity.RuntimeManager.CreateInstance(DiceRollEvent);
        rollDiceAudio.start();
        player.RollDice();

        player.drawSize = player.maxDrawSize;
        opponent.drawSize = opponent.maxDrawSize;

        StartDelay(3f, () => DiceManager.SortAllDice(player.dice, this));

        placementButton.gameObject.SetActive(true);
    }




    private void StartIntermissionPhase()
    {
        intermissionButton.gameObject.SetActive(true);

        if (player.extraDice.Count > 0)
        {
            DiceManager.CopyDice(player);
            player.extraDice.Clear();
        }
    }

    private void ContinueColumnPhase()
    {
        intermission = false;

        foreach (Die die in player.tempDice)
        {
            if (!die.isPlaced)
            {
                Destroy(die.gameObject);
            }
            else
            {
                die.isDraggable = false;
            }
        }

        StartCoroutine(HandleActiveColumnRoutine(CurrentColumn));
    }


    public void StartActiveCombat()
    {
        // Remove Confirm Button
        placementButton.gameObject.SetActive(false);

        // combatBolt.SetActive(true);
        // player.alpha = 0.9f;
        // opponent.alpha = 0.9f;

        // Remove Dice that haven't been placed
        Die[] unusedDice = GameObject.FindObjectsByType<Die>(FindObjectsSortMode.None);
        foreach (Die die in unusedDice)
        {
            if (!die.isPlaced)
            {
                die.transform.position = new Vector3(10, 0, 0);
            }
        }

        // Proceed to Next Phase
        StartCoroutine(StartActiveCombatRoutine());
    }



    private IEnumerator StartActiveCombatRoutine()
    {
        player.inColumnPhase = true;

        DiceSlotController[] slots = GameObject.FindObjectsByType<DiceSlotController>(FindObjectsSortMode.None);
        List<DiceSlotController> buffSlots = new List<DiceSlotController>();
        foreach (DiceSlotController slot in slots)
        {
            if (slot.slottedDie)
            {
                slot.slottedDie.isDraggable = false;
            }

            if (slot.priority == 1 && slot.slottedDie != null && slot.isFilled)
            {
                buffSlots.Add(slot);
            }
        }

        yield return DoSlotEffect(buffSlots, 1f, 1);

        StartCoroutine(HandleActiveColumnRoutine(0));
    }




    private IEnumerator HandleActiveColumnRoutine(int column)
    {
        intermissionButton.gameObject.SetActive(false);

        column++;

        if (opponent.currentHealth <= 0 || player.currentHealth <= 0)
            yield break;

        GetActiveColumn(column);

        foreach (GameObject banner in playerColumnBanner)
        {
            Image sprite = banner.GetComponent<Image>();
            sprite.color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }

        foreach (GameObject opponentBanner in opponentColumnBanner)
        {
            Image sprite = opponentBanner.GetComponent<Image>();
            sprite.color = new Color(0.25f, 0.25f, 0.25f, 1f);
        }

        Image playerBannerSprite = playerColumnBanner[column - 1].GetComponent<Image>();
        playerBannerSprite.color = new Color(1f, 1f, 1f, 1f);

        Image opponentBannerSprite = opponentColumnBanner[column - 1].GetComponent<Image>();
        opponentBannerSprite.color = new Color(1f, 1f, 1f, 1f);

        playerSlots = SortActiveSlots(playerActiveColumn);
        enemySlots = SortActiveSlots(enemyActiveColumn);

        float delay = Mathf.Max(playerSlots.Count, enemySlots.Count) + 0.5f;

        foreach (List<DiceSlotController> activeSlots in playerSlots)
        {
            yield return DoSlotEffect(activeSlots, 1f, 2);
        }

        foreach (List<DiceSlotController> activeSlots in enemySlots)
        {
            yield return DoSlotEffect(activeSlots, 1f, 2);
        }

        foreach (List<DiceSlotController> activeSlots in playerSlots)
        {
            yield return DoSlotEffect(activeSlots, 1f, 3);
        }

        foreach (List<DiceSlotController> activeSlots in enemySlots)
        {
            yield return DoSlotEffect(activeSlots, 1f, 3);
        }

        CalculateDamage();

        if (player.currentHealth <= 0 | opponent.currentHealth <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        ClearActiveColumn();

        CurrentColumn = column;

        playerSlots.Clear();
        enemySlots.Clear();

        if (column == 3 && opponent.currentHealth > 0)
        {
            EndOfRound();
            yield break;
        }
        else if (intermission == true)
        {
            StartIntermissionPhase();
            yield break;
        }
        else
        {
            StartCoroutine(HandleActiveColumnRoutine(column));
        }
    }



    public void DelaySlotEffect(List<DiceSlotController> slots, float delay, int priority)
    {
        StartCoroutine(DoSlotEffect(slots, delay, priority));
    }

    private IEnumerator DoSlotEffect(List<DiceSlotController> slots, float delay, int priority)
    {
        foreach (var slot in slots)
        {
            if (slot == null || slot.Equals(null))
                continue;

            if (slot.priority == priority)
            {
                if (slot != null && !slot.Equals(null))
                {
                    slot.DoEffect();
                    UpdateDamageBlockUI();
                }
                yield return new WaitForSeconds(delay);
            }
        }
    }



    private void EndOfRound()
    {

        combatBolt.SetActive(false);
        player.alpha = 0.9f;
        opponent.alpha = 0.9f;

        ResetEntity(player);
        ResetEntity(opponent);
        RotationButton.ResetRotationButton();

        Die[] dice = FindObjectsByType<Die>(FindObjectsSortMode.None);
        foreach (Die die in dice)
        {
            die.isFrozen = false;
        }

        StartDelay(1f, () => StartNewRound());
    }



    public void CalculateDamage()
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

        UpdateDamageBlockUI();
    }



    public void CheckLossState()
    {
        if (player.currentHealth <= 0)
        {
            endScene.Lose();
            ScoreList.Add(Score);
            UpdateScoreDisplay();
            StartIntermissionPhase();
        }
    }

    public void CheckWinState()
    {
        if (opponent.currentHealth <= 0 && player.currentHealth > 0)
        {
            // endScene.Win();
            // Time.timeScale = 0;
            // EndOfRound();

            opponent.ActiveImplings.Clear();
            opponent.drawnDice.Clear();
            opponent.discardPile.Clear();
            opponent.diceDeck.Clear();

            GameObject[] oldTablets = GameObject.FindGameObjectsWithTag("EnemyTablet");
            foreach (GameObject tablet in oldTablets)
            {
                Destroy(tablet);
            }
            EndOfRound();
        }
    }

    private IEnumerator AnimatePlayerHealthDecrease(int targetHealth, int damage)
    {
        RectTransform candle = player.candle.GetComponent<RectTransform>();

        float wait = 0.1f;
        float startY = 16.2f;   // full health
        float endY = -36.8f;  // zero health
        float rangeY = startY - endY;

        while (player.currentHealth > Mathf.Max(targetHealth, 0))
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

            float healthPercent = (float)player.currentHealth / player.maxHealth;
            float newY = Mathf.Lerp(endY, startY, healthPercent);

            Vector3 pos = candle.anchoredPosition;
            pos.y = newY;
            candle.anchoredPosition = pos;

            yield return new WaitForSeconds(wait);
            damageAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CheckLossState();
        }
    }



    private IEnumerator AnimateOpponentHealthDecrease(int targetHealth, int damage)
    {
        RectTransform candle = opponent.candle.GetComponent<RectTransform>();

        float wait = 0.1f;
        float startY = 16.2f;   // full health
        float endY = -36.8f;  // zero health
        float rangeY = startY - endY;

        while (opponent.currentHealth > Mathf.Max(targetHealth, 0))
        {
            if (damage < 11)
            {
                wait = 0.1f;
            }
            else
            {
                wait = 1.2f / damage;
            }

            FMOD.Studio.EventInstance damageAudio = FMODUnity.RuntimeManager.CreateInstance(DamageEvent);
            damageAudio.start();

            opponent.currentHealth -= 1;
            opponent.healthText.text = opponent.currentHealth.ToString();

            float healthPercent = (float)opponent.currentHealth / opponent.maxHealth;
            float newY = Mathf.Lerp(endY, startY, healthPercent);

            Vector3 pos = candle.anchoredPosition;
            pos.y = newY;
            candle.anchoredPosition = pos;

            if (opponent.currentHealth <= 0)
            {
                opponent.currentHealth = 0;
                opponent.healthText.text = opponent.currentHealth.ToString();
                damageAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                damageAudio.release();
                CheckWinState();
                yield break;
            }

            yield return new WaitForSeconds(wait);
            damageAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
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
        UpdateDamageBlockUI();
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
    }



    public void GetActiveColumn(int column)
    {
        float columnPosX = columnStartPositions[column - 1];
        Vector3 raycastVector = new Vector3(columnPosX, -1.8f, -0.1f);

        for (int i = 0; i < 9; i++)
        {
            float yJump = i * -0.8f;

            Vector3 rayPosition = new Vector3(raycastVector.x, raycastVector.y + yJump, raycastVector.z);
            Ray ray = new Ray(rayPosition, Vector3.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 666))
            {
                Vector3 ray2Position = new Vector3(raycastVector.x + 14.3f, raycastVector.y + yJump, raycastVector.z);
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
            }
        }
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
            slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.1f, 0.1f, 0.1f, 0.1f));
        }

        foreach (DiceSlotController slot in playerActiveColumn)
        {
            if (slot.wasFrozen == false)
            {
                slot.isFilled = false;
                slot.slottedDie = null;
            }
            slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.1f, 0.1f, 0.1f, 0.1f));
        }

        playerActiveColumn.Clear();
        enemyActiveColumn.Clear();
    }



    private static List<List<DiceSlotController>> SortActiveSlots(List<DiceSlotController> activeColumn)
    {
        List<DiceSlotController> priority1 = new List<DiceSlotController>();
        List<DiceSlotController> priority2 = new List<DiceSlotController>();
        List<DiceSlotController> priority3 = new List<DiceSlotController>();

        List<List<DiceSlotController>> sortedSlots = new List<List<DiceSlotController>>();

        foreach (DiceSlotController slot in activeColumn)
        {

            if (slot.isFilled && !slot.isHandled)
            {
                switch (slot.priority)
                {
                    case 1:
                        priority1.Add(slot);
                        break;
                    case 2:
                        priority2.Add(slot);
                        break;
                    case 3:
                        priority3.Add(slot);
                        break;
                }
            }
        }

        sortedSlots.Add(priority2);
        sortedSlots.Add(priority3);

        return sortedSlots;
    }



    public void StartDelay(float delay, Action method)
    {
        StartCoroutine(SortAfterDelay(delay, method));
    }

    private IEnumerator SortAfterDelay(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke();
    }



    public void UpdateDamageBlockUI()
    {
        if (player.block != 0)
        {
            player.blockText.text = player.block.ToString();
            player.blockText.gameObject.SetActive(true);
        }
        else
        {
            player.blockText.text = null;
            player.blockText.gameObject.SetActive(false);
        }

        if (opponent.block != 0)
        {
            opponent.blockText.text = opponent.block.ToString();
            opponent.blockText.gameObject.SetActive(true);
        }
        else
        {
            opponent.blockText.text = null;
            opponent.blockText.gameObject.SetActive(false);
        }

        if (player.damage != 0)
        {
            player.damageText.text = player.damage.ToString();
            player.damageText.gameObject.SetActive(true);
        }
        else
        {
            player.damageText.text = null;
            player.damageText.gameObject.SetActive(false);
        }

        if (opponent.damage != 0)
        {
            opponent.damageText.text = opponent.damage.ToString();
            opponent.damageText.gameObject.SetActive(true);
        }
        else
        {
            opponent.damageText.text = null;
            opponent.damageText.gameObject.SetActive(false);
        }
    }

    // TEMPORARY ENDLESS MODE
    private void NewEncounter(int level, string area)
    {
        Score += player.currentHealth + 20;
        scoreText.text = Score.ToString();
        opponent.currentHealth = opponent.maxHealth;
        opponent.healthText.text = opponent.currentHealth.ToString();
        RectTransform candle = opponent.candle.GetComponent<RectTransform>();
        Vector3 pos = candle.anchoredPosition;
        pos.y = 16.2f;
        candle.anchoredPosition = pos;

        List<TabletData> newTablets = Encounters.SetEnemyRoster(level, area);
        opponent.SetEnemyRoster(newTablets);

        TabletManager.Instance.tablets = newTablets;
        Vector3 startPosition = new Vector3(4.9f, -2.5f, 0f);
        TabletManager.Instance.SpawnTablets(startPosition);
    }

    public void UpdateScoreDisplay()
    {
        // Get top 5 scores (sorted descending)
        var topScores = ScoreList
            .OrderByDescending(s => s)
            .Take(5)
            .ToList();

        string scoreText = "High Scores:\n";
        for (int i = 0; i < topScores.Count; i++)
        {
            scoreText += $"{i + 1}. {topScores[i]}\n";
        }

        highscoreText.text = scoreText;
    }
}