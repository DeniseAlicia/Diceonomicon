using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
//using UnityEngine.UIElements;

public class BattleSceneManager : MonoBehaviour
{
    public static BattleSceneManager Instance;

    public bool isTutorial;
    public Player player;
    public Opponent opponent;
    public GameObject[] playerColumnBanner;
    public GameObject[] opponentColumnBanner;
    public List<DiceSlotController> playerActiveColumn;
    public List<DiceSlotController> enemyActiveColumn;
    private static List<List<DiceSlotController>> playerSlots;
    private static List<List<DiceSlotController>> enemySlots;
    private static DiceSlotController[] allSlots;
    private GameStateManager gameState;
    public DiceData[] startingDiceDeck;

    // Combat Stats
    public int level;
    public int round;
    public float[] columnStartPositions = new float[] { -7.65f, -6.9f, -6.1f };
    public static int CurrentColumn { get; private set; }
    public bool intermission;
    public List<Die> unusedDice;

    // UI Elements & Buttons
    private InputAction tooltipAction;
    public EndBattle endScene;
    public Button placementButton;
    public Button intermissionButton;
    public Button rollDiceButton;
    public GameObject combatBolt;

    // Events
    public static UnityEvent OnSceneStart = new UnityEvent();
    public static UnityEvent OnRoundStart = new UnityEvent();
    public static UnityEvent OnPlacementDone = new UnityEvent();
    public static UnityEvent OnAcvitveCombatStart = new UnityEvent();
    public static UnityEvent OnAcvitveCombatEnd = new UnityEvent();
    public static UnityEvent OnLoss = new UnityEvent();

    // FMOD
    public FMODUnity.EventReference DiceRollEvent;
    public FMODUnity.EventReference DiceDrawEvent;
    public FMODUnity.EventReference RoundStartEvent;
    public FMODUnity.EventReference DamageEvent;

    //////////////////////////////////////////////////////////////////////////
    // 
    // SCENE START
    // 
    //////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        tooltipAction = InputSystem.actions.FindAction("ShowInfo");
        Camera camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();
        camBattleTablets.gameObject.SetActive(false);
        camBattleTablets.gameObject.SetActive(true);
        gameState = FindFirstObjectByType<GameStateManager>();
        Instance = this;
    }

    private void Start()
    {
        round = 0;
        player.level = gameState.player.level;
        player.area = gameState.player.area;
        opponent.damage = 0;
        opponent.block = 0;
        player.damage = 0;
        player.block = 0;
        UpdateDamageBlockUI();

        startingDiceDeck = player.diceDeck.ToArray();
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

        OnSceneStart.Invoke();
        if (!isTutorial)
        {
            StartNewRound();
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // 
    // START OF ROUND
    // 
    //////////////////////////////////////////////////////////////////////////

    public void StartNewRound()
    {
        OnRoundStart.Invoke();
        // if (opponent.currentHealth == 0)
        // {
        //     NewEncounter(player.level, player.area);
        // }

        allSlots = FindObjectsByType<DiceSlotController>(FindObjectsSortMode.None);


        // Overload.DecayEmotions(allTablets);
        // Overload.UpdateEmotions(allTablets);

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
                slot.outlineMaterial.material.SetColor("_OutlineColor", new Color(0.2f, 1f, 0.4f, 1f));
            }
            else if (slot.tag == "Spell")
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.6f, 0.2f, 1f, 0.6f));
                slot.outlineMaterial.material.SetColor("_OutlineColor", new Color(0.7f, 0.3f, 1f, 1f));
            }
            else if (slot.tag == "Damage")
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(1f, 0f, 0.15f, 0.6f));
                slot.outlineMaterial.material.SetColor("_OutlineColor", new Color(1f, 0.2f, 0.4f, 1f));
            }
            else // "Block"
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.1f, 0.45f, 1f, 0.6f));
                slot.outlineMaterial.material.SetColor("_OutlineColor", new Color(0.3f, 0.6f, 1f, 1f));
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

        StartDelay(3f, () => DiceManager.SortAllDice(player.dice, this, placementButton));
    }

    //////////////////////////////////////////////////////////////////////////
    // 
    // START COMBAT
    // 
    //////////////////////////////////////////////////////////////////////////

    public void StartActiveCombat()
    {
        placementButton.gameObject.SetActive(false);

        // Remove Dice that haven't been placed
        Die[] dice = GameObject.FindObjectsByType<Die>(FindObjectsSortMode.None);
        foreach (Die die in dice)
        {
            if (!die.isPlaced)
            {
                die.isDraggable = false;
                die.transform.position = new Vector3(10, 0, 0);
                unusedDice.Add(die);
            }
        }

        //Overload.AddEmotion(allSlots);

        OnAcvitveCombatStart.Invoke();
        // Proceed to Next Phase
        StartCoroutine(StartActiveCombatRoutine());
    }

    private IEnumerator StartActiveCombatRoutine()
    {
        player.inColumnPhase = true;

        Die[] dice = FindObjectsByType<Die>(FindObjectsSortMode.None);
        foreach (Die die in dice)
        {
            if (die.priority == 1 && die.isPlaced)
            {
                die.data.DoEffect(die);
                if (die.didDamage > 0)
                {
                    if (die.parentSlot.owner.GetType() == typeof(Player))
                    {
                        int dmg = die.didDamage;
                        die.didDamage = 0;
                        int targetHealth = player.currentHealth - dmg;
                        player.currentHealth -= dmg;
                        player.healthText.text = player.currentHealth.ToString();
                        StartCoroutine(AnimatePlayerHealthDecrease(targetHealth, dmg));
                    }
                    if (die.parentSlot.owner.GetType() == typeof(Opponent))
                    {
                        int dmg = die.didDamage;
                        die.didDamage = 0;
                        int targetHealth = opponent.currentHealth - dmg;
                        opponent.currentHealth -= dmg;
                        opponent.healthText.text = opponent.currentHealth.ToString();
                        StartCoroutine(AnimateOpponentHealthDecrease(targetHealth, dmg));
                    }
                }
            }
        }

        foreach (Die die in dice)
        {
            if (die.priority == 2 && die.isPlaced)
            {
                die.data.DoEffect(die);
                if (die.didDamage > 0)
                {
                    if (die.parentSlot.owner.GetType() == typeof(Player))
                    {
                        int dmg = die.didDamage;
                        die.didDamage = 0;
                        int targetHealth = player.currentHealth - dmg;
                        player.currentHealth -= dmg;
                        player.healthText.text = player.currentHealth.ToString();
                        StartCoroutine(AnimatePlayerHealthDecrease(targetHealth, dmg));
                    }
                    if (die.parentSlot.owner.GetType() == typeof(Opponent))
                    {
                        int dmg = die.didDamage;
                        die.didDamage = 0;
                        int targetHealth = opponent.currentHealth - dmg;
                        opponent.currentHealth -= dmg;
                        opponent.healthText.text = opponent.currentHealth.ToString();
                        StartCoroutine(AnimateOpponentHealthDecrease(targetHealth, dmg));
                    }
                }
            }
        }

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
        UpdateBannersUI(column);

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

        yield return new WaitForSeconds(1f);
        ClearActiveColumn();

        CurrentColumn = column;
        playerSlots.Clear();
        enemySlots.Clear();

        if (player.currentHealth > 0 && opponent.currentHealth <= 0)
        {
            EndOfRound();
            yield break;
        }

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
                    if (slot.slottedDie.data != null && slot.slottedDie.priority == 3)
                    {
                        slot.slottedDie.data.DoEffect(slot.slottedDie);
                        if (slot.slottedDie.didDamage > 0)
                        {
                            if (slot.owner.GetType() == typeof(Player))
                            {
                                int dmg = slot.slottedDie.didDamage;
                                slot.slottedDie.didDamage = 0;
                                int targetHealth = player.currentHealth - dmg;
                                player.currentHealth -= dmg;
                                player.healthText.text = player.currentHealth.ToString();
                                StartCoroutine(AnimatePlayerHealthDecrease(targetHealth, dmg));
                            }
                            if (slot.owner.GetType() == typeof(Opponent))
                            {
                                int dmg = slot.slottedDie.didDamage;
                                slot.slottedDie.didDamage = 0;
                                int targetHealth = opponent.currentHealth - dmg;
                                opponent.currentHealth -= dmg;
                                opponent.healthText.text = opponent.currentHealth.ToString();
                                StartCoroutine(AnimateOpponentHealthDecrease(targetHealth, dmg));
                            }
                        }
                    }
                    slot.DoEffect();
                    UpdateDamageBlockUI();
                }
                yield return new WaitForSeconds(delay);
            }
        }
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

    public IEnumerator AnimatePlayerHealthDecrease(int targetHealth, int damage)
    {
        RectTransform candle = player.candle.GetComponent<RectTransform>();

        float wait;
        float startY = 16.2f;   // full health
        float endY = -36.8f;  // zero health

        while (player.currentHealth > Mathf.Max(targetHealth, 0))
        {
            if (damage < 11)
            {
                wait = 0.1f;
            }
            else
            {
                wait = 1f / damage;
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

    public IEnumerator AnimatePlayerHealthIncrease(int targetHealth, int damage)
    {
        RectTransform candle = player.candle.GetComponent<RectTransform>();

        float wait = 0.1f;
        float startY = 16.2f;   // full health
        float endY = -36.8f;  // zero health
        float rangeY = startY - endY;

        while (player.currentHealth < Mathf.Min(targetHealth, player.maxHealth))
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

            player.currentHealth += 1;
            player.healthText.text = player.currentHealth.ToString();

            float healthPercent = (float)player.currentHealth / player.maxHealth;
            float newY = Mathf.Lerp(endY, startY, healthPercent);

            Vector3 pos = candle.anchoredPosition;
            pos.y = newY;
            candle.anchoredPosition = pos;

            yield return new WaitForSeconds(wait);
            damageAudio.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public IEnumerator AnimateOpponentHealthDecrease(int targetHealth, int damage)
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

    public void CheckLossState()
    {
        if (player.currentHealth <= 0)
        {
            endScene.Lose();
            OnLoss.Invoke();
            StartIntermissionPhase();

        }
    }

    public void CheckWinState()
    {
        if (opponent.currentHealth <= 0 && player.currentHealth > 0)
        {
            opponent.ActiveImplings.Clear();
            opponent.drawnDice.Clear();
            opponent.discardPile.Clear();
            opponent.diceDeck.Clear();

            GameObject[] oldTablets = GameObject.FindGameObjectsWithTag("EnemyTablet");
            foreach (GameObject tablet in oldTablets)
            {
                Destroy(tablet);
            }

            gameState.battleWon = true;

            foreach (DiceData discardedDie in player.discardPile)
            {
                player.diceDeck.Add(discardedDie);
            }

            player.diceDeck = startingDiceDeck.ToList();
            GameStateManager.Instance.player.diceDeck = startingDiceDeck.ToList();

            gameState.OnBattleEnd();
            SceneManager.LoadScene("Map", LoadSceneMode.Single);
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // 
    // START INTERMISSION
    // 
    //////////////////////////////////////////////////////////////////////////

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
        OnPlacementDone.Invoke();
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

    //////////////////////////////////////////////////////////////////////////
    // 
    // END OF COMBAT // CLEAN UP
    // 
    //////////////////////////////////////////////////////////////////////////

    public void EndOfRound()
    {
        ResetEntity(player);
        ResetEntity(opponent);
        ResetDiceSlots();
        ResetDice();
        RotationButton.ResetRotationButton(RotationButton.allButtons);

        OnAcvitveCombatEnd.Invoke();
        StartDelay(1f, () => StartNewRound());
    }

    private void ResetEntity(Entity entity)
    {
        List<DiceData> dicardedDice = new List<DiceData>(entity.drawnDice);
        foreach (DiceData dieData in dicardedDice)
        {
            entity.discardPile.Add(dieData);
            entity.drawnDice.Remove(dieData);
        }

        entity.damage = 0;
        entity.block = 0;
        UpdateDamageBlockUI();
    }

    public void ResetDiceSlots()
    {
        foreach (DiceSlotController slot in allSlots)
        {
            if (slot.slottedDie != null)
            {
                if (!slot.slottedDie.isFrozen)
                {
                    slot.isFilled = false;
                }
            }
        }
    }

    public void ResetDice()
    {
        Die[] dice = FindObjectsByType<Die>(FindObjectsSortMode.None);
        foreach (Die die in dice)
        {
            if (!die.isFrozen)
            {
                GameObject dieObject = die.transform.gameObject;
                Destroy(dieObject);
            }
            die.isFrozen = false;
        }
        unusedDice.Clear();
    }

    //////////////////////////////////////////////////////////////////////////
    // 
    // HELPFUL METHODS
    // 
    //////////////////////////////////////////////////////////////////////////

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
        Vector3 raycastVector = new Vector3(columnPosX, 1.25f, 3.6f);

        for (int i = 0; i < 9; i++)
        {
            float zJump = i * -0.8f;

            Vector3 rayPosition = new Vector3(raycastVector.x, raycastVector.y, raycastVector.z + zJump);
            Ray ray = new Ray(rayPosition, Vector3.down);
            //Debug.DrawRay(rayPosition, Vector3.down * 1f, Color.red, 66);
            if (Physics.Raycast(ray, out RaycastHit hit, 66))
            {
                Vector3 ray2Position = new Vector3(raycastVector.x + 14.3f, raycastVector.y, raycastVector.z + zJump);
                Ray ray2 = new Ray(ray2Position, Vector3.down);
                //Debug.DrawRay(ray2Position, Vector3.down * 1f, Color.green, 66);
                if (Physics.Raycast(ray2, out RaycastHit hit2, 66))
                {
                    DiceSlotController slotController2 = hit2.collider.GetComponent<DiceSlotController>();
                    if (slotController2 != null)
                    {
                        enemyActiveColumn.Add(slotController2);
                    }
                    else
                    {
                        //continue;
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
                if (slot.slottedDie)
                {
                    slot.slottedDie.textureRenderer.material.SetTexture("_BaseMap", slot.slottedDie.usedTexture);
                }
            }

            if (slot)
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.1f, 0.1f, 0.1f, 0.1f));
                slot.outlineMaterial.material.SetColor("_OutlineColor", new Color(0.1f, 0.1f, 0.1f, 1f));
            }
        }

        foreach (DiceSlotController slot in playerActiveColumn)
        {
            if (slot.wasFrozen == false)
            {
                if (slot.slottedDie)
                {
                    slot.slottedDie.textureRenderer.material.SetTexture("_BaseMap", slot.slottedDie.usedTexture);
                }
            }

            if (slot)
            {
                slot.outlineMaterial.material.SetColor("_BaseColor", new Color(0.1f, 0.1f, 0.1f, 0.1f));
                slot.outlineMaterial.material.SetColor("_OutlineColor", new Color(0.1f, 0.1f, 0.1f, 1f));
            }
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

    private void OnEnable()
    {
        if (tooltipAction != null)
        {
            tooltipAction.performed += ctx => ShowInfo();
            tooltipAction.canceled += ctx => HideInfo();
            tooltipAction.Enable();
        }
    }

    private void OnDisable()
    {
        tooltipAction.Disable();
    }

    public void ShowInfo()
    {
        foreach (DiceSlotController slot in allSlots)
        {
            if (slot.isFilled)
            {
                slot.slotName.gameObject.SetActive(true);
            }
        }
    }

    public void HideInfo()
    {
        foreach (DiceSlotController slot in allSlots)
        {
            slot.slotName.gameObject.SetActive(false);
        }
    }

    public void UpdateBannersUI(int column)
    {
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
    }
}