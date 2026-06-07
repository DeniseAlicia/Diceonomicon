using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private BattleSceneManager battleSceneManager;
    private Player player;
    private Opponent opponent;
    [SerializeField] private EnemyAI ai;

    private TabletController playerTablet;
    private TabletController enemyTablet;
    [SerializeField] private DiceSlotData emptyData;
    [SerializeField] private DiceSlotData attackData;
    [SerializeField] private DiceSlotData blockData;
    [SerializeField] private DiceSlotData buffData;
    [SerializeField] private DiceSlotData spellData;
    [SerializeField] private DiceData attackDie;
    [SerializeField] private DiceData blockDie;
    [SerializeField] private DiceData buffDie;
    [SerializeField] private DiceData spellDie;
    [SerializeField] private Animation dialogueAnim;
    private int roundCount;

    // Dialogue
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject textbox;
    private string[] currentLines;
    [SerializeField] private float textSpeed;
    private int lineIndex;
    private int dialogueIndex;
    private bool canClick = true;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    // UI
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private GameObject playerCandle;
    [SerializeField] private GameObject enemyCandle;

    // Slot States
    private bool emptyState;
    private bool blockState;
    private bool attackState;
    private bool spellState;
    private bool buffState;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        player = battleSceneManager.player;
        opponent = battleSceneManager.opponent;



        // Disable UI
        RotationButton[] allButtons = Object.FindObjectsByType<RotationButton>(FindObjectsSortMode.None);
        foreach (RotationButton button in allButtons)
        {
            button.gameObject.SetActive(false);
        }

        textbox.SetActive(false);
        GameObject tabletObjEnemy = GameObject.FindGameObjectWithTag("EnemyTablet");
        enemyTablet = tabletObjEnemy.GetComponent<TabletController>();

        GameObject tabletObjPlayer = GameObject.FindGameObjectWithTag("PlayerTablet");
        playerTablet = tabletObjPlayer.GetComponent<TabletController>();

        text.text = string.Empty;

        string[] lines = { "These guys don't play fair, but don't fret you got me now.", "I'll show you how to rumble and tumble with these goons." };
        currentLines = lines;

        dialogueAnim.Play();
        StartDialogue();

        // BattleSceneManager.OnSceneStart.AddListener(OnSceneStart);
        // BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        // BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        // BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
        BattleSceneManager.OnAcvitveCombatEnd.AddListener(OnAcvitveCombatEnd);

        roundCount = 0;

        Debug.Log("Tutorial setting up...");
    }


    void Update()
    {
        if (textbox.activeInHierarchy && canClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    if (typingCoroutine != null)
                        StopCoroutine(typingCoroutine);

                    text.text = currentLines[lineIndex];
                    isTyping = false;
                }
                else
                {
                    NextLine();
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////
        // 
        // BLOCK ROUND (START)
        // 
        //////////////////////////////////////////////////////////////////////////

        if (roundCount == 0)
        {
            if (dialogueIndex <= 0)
            {
                return;
            }
            else if (dialogueIndex == 1)
            {
                dialogueIndex++;
                blockState = true;
            }
            else if (dialogueIndex == 3)
            {

                for (int i = 0; i < 2; i++)
                {
                    player.diceDeck.Add(blockDie);
                    opponent.diceDeck.Add(attackDie);
                }
                battleSceneManager.StartNewRound();
                buttonContainer.SetActive(true);
                dialogueIndex++;
            }
            else if (dialogueIndex == 4)
            {
                string[] lines = { "Press the button to Roll your dice!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 6)
            {
                if (player.dice.Count > 0)
                {
                    if (player.dice[0].isDraggable)
                    {
                        dialogueIndex++;
                    }
                }
            }
            else if (dialogueIndex == 7)
            {
                if (player.dice.Count > 0)
                {
                    foreach (Die die in player.dice)
                    {
                        die.isDraggable = false;
                    }
                }

                string[] lines = { "That Bouncer is really mad. He is going to hit us if we don't shield us.", "The good news is any damage we take will be absorbed by the wax from our candle, but if we run out, we gotta start all over.", "Now back on topic. Place the dice in the Shield slots.", };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                playerCandle.SetActive(true);
                enemyCandle.SetActive(true);
                dialogueIndex++;
            }
            else if (dialogueIndex == 9)
            {
                DiceSlotController slot1 = playerTablet.tabletSlots[0];
                DiceSlotController slot7 = playerTablet.tabletSlots[6];

                if (player.dice.Count > 0)
                {
                    foreach (Die die in player.dice)
                    {
                        die.isDraggable = true;
                    }
                }

                if (slot1.isFilled && slot7.isFilled)
                {
                    dialogueIndex++;
                }
            }
            else if (dialogueIndex == 10)
            {
                int blockValue = 0;
                int damageValue = 0;
                string line;

                DiceSlotController slot1 = enemyTablet.tabletSlots[0];
                DiceSlotController slot7 = enemyTablet.tabletSlots[6];

                if (player.dice.Count > 0)
                {
                    int value = 0;
                    foreach (Die die in player.dice)
                    {
                        die.isDraggable = false;
                        value += die.value;
                    }
                    blockValue = value;
                }

                if (slot1 != null && slot7 != null)
                {
                    int value = 0;
                    value += slot1.slottedDie.value;
                    value += slot7.slottedDie.value;
                    damageValue = value;
                }

                if (damageValue < blockValue)
                {
                    line = "Great job! We might get out of here harmfree.";
                }
                else
                {
                    line = "Watch out! It looks like we will catch some heat.";
                }

                string[] lines = { line, "The values of each dice in a column will be added up. once you confirm your placement!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                player.drawnDice.Clear();
                opponent.drawnDice.Clear();
                dialogueIndex = -1;
            }
        }

        //////////////////////////////////////////////////////////////////////////
        // 
        // ATTACK CHAIN ROUND
        // 
        //////////////////////////////////////////////////////////////////////////

        if (roundCount == 1)
        {
            //Debug.Log(dialogueIndex.ToString());
            if (dialogueIndex <= 0)
            {
                return;
            }
            else if (dialogueIndex == 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    player.diceDeck.Add(blockDie);
                    opponent.diceDeck.Add(attackDie);
                    opponent.diceDeck.Add(blockDie);
                }

                for (int i = 0; i < 3; i++)
                {
                    player.diceDeck.Add(attackDie);
                }

                text.text = string.Empty;
                string[] lines = { "Test" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 3)
            {
                attackState = true;
                dialogueIndex++;
            }
            else if (dialogueIndex == 4)
            {
                text.text = string.Empty;
                string[] lines = { "Now that you mastered the art of blocking let's get on the offense.", "Insert Combo Text", "Let me give you some more help." };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 6)
            {
                ChangeSlotData(5, playerTablet, attackData);
                dialogueIndex++;
            }
            else if (dialogueIndex == 7)
            {
                text.text = string.Empty;
                string[] lines = { "Place your dice!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 9)
            {
                DiceSlotController slot1 = playerTablet.tabletSlots[0];
                DiceSlotController slot2 = playerTablet.tabletSlots[1];
                DiceSlotController slot5 = playerTablet.tabletSlots[4];
                DiceSlotController slot7 = playerTablet.tabletSlots[6];
                DiceSlotController slot8 = playerTablet.tabletSlots[7];

                if (slot1.isFilled && slot2.isFilled && slot5.isFilled && slot7.isFilled && slot8.isFilled)
                {
                    dialogueIndex++;
                }
            }
            else if (dialogueIndex == 10)
            {
                text.text = string.Empty;
                string[] lines = { "This is another Test!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                player.drawnDice.Clear();
                opponent.drawnDice.Clear();
                dialogueIndex = -1;
            }
        }

        //////////////////////////////////////////////////////////////////////////
        // 
        // SPELL ROUND
        // 
        //////////////////////////////////////////////////////////////////////////

        if (roundCount == 2)
        {
            //Debug.Log(dialogueIndex.ToString());
            if (dialogueIndex <= 0)
            {
                return;
            }
            else if (dialogueIndex == 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    player.diceDeck.Add(blockDie);
                    player.diceDeck.Add(spellDie);
                    opponent.diceDeck.Add(attackDie);
                    opponent.diceDeck.Add(blockDie);
                    opponent.diceDeck.Add(attackDie);
                }

                for (int i = 0; i < 3; i++)
                {
                    player.diceDeck.Add(attackDie);
                }

                text.text = string.Empty;
                string[] lines = { "Spell Round" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 3)
            {
                spellState = true;
                dialogueIndex++;
            }
            else if (dialogueIndex == 4)
            {
                text.text = string.Empty;
                string[] lines = { "Now that you mastered the art of creating chains let's mess with the opponents dice.", "Insert Spell Text" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 6)
            {
                dialogueIndex++;
            }
            else if (dialogueIndex == 7)
            {
                text.text = string.Empty;
                string[] lines = { "Roll and place your dice!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 9)
            {
                DiceSlotController slot1 = playerTablet.tabletSlots[0];
                DiceSlotController slot2 = playerTablet.tabletSlots[1];
                DiceSlotController slot3 = playerTablet.tabletSlots[2];
                DiceSlotController slot5 = playerTablet.tabletSlots[4];
                DiceSlotController slot6 = playerTablet.tabletSlots[5];
                DiceSlotController slot7 = playerTablet.tabletSlots[6];
                DiceSlotController slot8 = playerTablet.tabletSlots[7];

                if (slot1.isFilled && slot2.isFilled && slot5.isFilled && slot7.isFilled && slot8.isFilled && slot6.isFilled && slot3.isFilled)
                {
                    dialogueIndex++;
                }
            }
            else if (dialogueIndex == 10)
            {
                text.text = string.Empty;
                string[] lines = { "This is another Test! Let's go!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                player.drawnDice.Clear();
                opponent.drawnDice.Clear();
                dialogueIndex = -1;
            }
        }

        //////////////////////////////////////////////////////////////////////////
        // 
        // BUFF ROUND
        // 
        //////////////////////////////////////////////////////////////////////////

        if (roundCount == 3)
        {
            //Debug.Log(dialogueIndex.ToString());
            if (dialogueIndex <= 0)
            {
                return;
            }
            else if (dialogueIndex == 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    player.diceDeck.Add(blockDie);
                    player.diceDeck.Add(spellDie);
                    opponent.diceDeck.Add(attackDie);
                    opponent.diceDeck.Add(blockDie);
                    opponent.diceDeck.Add(attackDie);
                    player.diceDeck.Add(buffDie);
                    opponent.diceDeck.Add(buffDie);
                }

                for (int i = 0; i < 3; i++)
                {
                    player.diceDeck.Add(attackDie);
                }

                text.text = string.Empty;
                string[] lines = { "Buff Round" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 3)
            {
                buffState = true;
                dialogueIndex++;
            }
            else if (dialogueIndex == 4)
            {
                text.text = string.Empty;
                string[] lines = { "Now that you mastered the art of spellslinging let's improve our own dice.", "Insert Buff Text" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 6)
            {
                dialogueIndex++;
            }
            else if (dialogueIndex == 7)
            {
                text.text = string.Empty;
                string[] lines = { "Roll and place your dice!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
            }
            else if (dialogueIndex == 9)
            {
                DiceSlotController slot1 = playerTablet.tabletSlots[0];
                DiceSlotController slot2 = playerTablet.tabletSlots[1];
                DiceSlotController slot3 = playerTablet.tabletSlots[2];
                DiceSlotController slot4 = playerTablet.tabletSlots[3];
                DiceSlotController slot5 = playerTablet.tabletSlots[4];
                DiceSlotController slot6 = playerTablet.tabletSlots[5];
                DiceSlotController slot7 = playerTablet.tabletSlots[6];
                DiceSlotController slot8 = playerTablet.tabletSlots[7];
                DiceSlotController slot9 = playerTablet.tabletSlots[8];

                if (slot1.isFilled && slot2.isFilled && slot5.isFilled && slot7.isFilled && slot8.isFilled && slot6.isFilled && slot3.isFilled && slot4.isFilled && slot9.isFilled)
                {
                    dialogueIndex++;
                }
            }
            else if (dialogueIndex == 10)
            {
                text.text = string.Empty;
                string[] lines = { "This is another Test! Let's go!" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex = -1;
            }
        }

        if (roundCount == 4)
        {
            //Debug.Log(dialogueIndex.ToString());
            if (dialogueIndex <= 0)
            {
                return;
            }
            else if (dialogueIndex == 1)
            {
                text.text = string.Empty;
                string[] lines = { "End of Tutorial?" };
                currentLines = lines;
                text.text = string.Empty;
                StartDialogue();
                dialogueIndex++;
                
                GameStateManager gameManagerObject = FindFirstObjectByType<GameStateManager>();

                GameObject.Destroy(gameManagerObject);

                SceneManager.LoadScene("MainMenu");
            }

        }

        //////////////////////////////////////////////////////////////////////////
        // 
        // Dice Slot States
        // 
        //////////////////////////////////////////////////////////////////////////

        if (emptyState)
        {
            for (int i = 0; i < 9; i++)
            {
                ChangeSlotData(i, playerTablet, emptyData);
                ChangeSlotData(i, enemyTablet, emptyData);
            }
            emptyState = false;
        }
        else if (blockState)
        {
            ChangeSlotData(1, playerTablet, blockData);
            ChangeSlotData(7, playerTablet, blockData);
            ChangeSlotData(1, enemyTablet, attackData);
            ChangeSlotData(7, enemyTablet, attackData);

            foreach (GameObject banner in battleSceneManager.playerColumnBanner)
            {
                banner.SetActive(true);
            }

            foreach (GameObject banner in battleSceneManager.opponentColumnBanner)
            {
                banner.SetActive(true);
            }

            string[] lines = { "First we will need some dice!" };
            currentLines = lines;
            text.text = string.Empty;
            StartDialogue();

            blockState = false;
        }
        else if (attackState)
        {
            ChangeSlotData(2, playerTablet, attackData);
            ChangeSlotData(8, playerTablet, attackData);
            ChangeSlotData(2, enemyTablet, blockData);
            ChangeSlotData(5, enemyTablet, blockData);
            attackState = false;
        }
        else if (spellState)
        {
            ChangeSlotData(3, playerTablet, spellData);
            ChangeSlotData(6, playerTablet, spellData);
            ChangeSlotData(3, enemyTablet, attackData);
            ChangeSlotData(9, enemyTablet, attackData);
            spellState = false;
        }
        else if (buffState)
        {
            ChangeSlotData(4, playerTablet, buffData);
            ChangeSlotData(9, playerTablet, buffData);
            ChangeSlotData(4, enemyTablet, buffData);
            ChangeSlotData(6, enemyTablet, buffData);
            ChangeSlotData(8, enemyTablet, spellData);
            buffState = false;
        }
    }

    public void OnSceneStart()
    {
        Debug.Log("Scene starting... (Tutorial)");
    }

    public void OnRoundStart()
    {
        Debug.Log("Triggered on RoundStart");
    }

    public void OnPlacementDone()
    {
        Debug.Log("Triggered on PlacementDone");
    }

    public void OnAcvitveCombatStart()
    {
        Debug.Log("Triggered on AcvitveCombatStart");
    }

    public void OnAcvitveCombatEnd()
    {
        roundCount++;
        dialogueIndex++;
    }

    private void ChangeSlotData(int slotNumber, TabletController tablet, DiceSlotData newSlot)
    {
        int index = slotNumber - 1;
        DiceSlotController slotController = tablet.tabletSlots[index];
        slotController.SetData(newSlot);
    }

    private void StartDialogue()
    {
        textbox.SetActive(true);
        lineIndex = 0;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        text.text = "";

        foreach (char c in currentLines[lineIndex])
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void NextLine()
    {
        if (lineIndex < currentLines.Length - 1)
        {
            lineIndex++;
            text.text = string.Empty;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            textbox.SetActive(false);
            dialogueIndex++;
        }
    }

    private IEnumerator HandleClick()
    {
        canClick = false;

        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            text.text = currentLines[lineIndex];
            isTyping = false;
        }
        else
        {
            NextLine();
        }

        yield return new WaitForSeconds(0.35f);
        canClick = true;
    }
}
