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

        string[] lines = { "Well, well, well, seems like you found yourself in a bit of a pickle.", 
        "But don't worry,I'll show you how to rumble and tumble with these goons." };
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

                string[] lines = { "That Bouncer is really mad. He is going to hit us if we don't shield us.", 
                    "The good news is any damage we take will be absorbed by the wax from our candle, but if we run out, we gotta start all over.", 
                    "Now back on topic. Place the dice in the Shield slots.", };
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
                string[] lines = { "Hmmm...You might have some talent for this afterall...",
                    "Always remember that attacking and blocking only count for the column their in, so you can not block damage the enemy deals in the first column by placing blocking dice in the second column."
                    };
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
                string[] lines = { "Now that you learned about blocking let's get on the offense.",
                    "You attack by placing red dice in your tablet's attack slots.", 
                    "The enemies will of course try to block your attacks in turn, so pay attention to the way they place their dice.",
                    "See how the enemie's blue dice touch each other vertically?", 
                    "This is called a combo and will make any dice with numbers on them more powerful.", 
                    "The values of all the dice in a combo are multiplied by however many dice there are in a combo.", 
                    "But be careful, a combo can only be made with same-colored dice and if they are directly touching each other vertically!", 
                    "Now try to build an attack combo in your second column."};
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
                string[] lines = { "Looks good! Try to build combos as often as you can.", 
                    "You need to master the art of combo building if you ever plan on getting out of here!",
                    "One last bit of advice on combos: You can build multiple different combos in a single column as long as the same-colored dice are touching."};
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
                string[] lines = { "I'm impressed! You are naturally gifted when it comes to dice combat." };
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
                string[] lines = { "But attacking and blocking are not the only techniques at your disposal when it comes to defeating other hell dwellers.", 
                    "For example, purple dice are for casting nasty spells on your opponents.",
                    "What kinda spell you cast depends on the spell slots you place the dice in.",
                    "These spell slots are poison spells, meaning that they will weaken the opponents dice in their respective column.",
                    "This is some powerful magic, so don't expect to be able to perfectly control which dice are affected when you cast it.",
                    "Now try poisoning your opponent with the poison spell!" };
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
                string[] lines = { "Great! Now you know how to cast spells! But remember that the spells you can cast depends on the available spell slots on your tablets.",
                    "If you are unsure which spell a spell slot unleashes, you can always hover above the slot to gain more information.",
                    "This works on all kinds of tablet slots, so hover your cursor over any slot to see its effects."};
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
                string[] lines = { "Very good, if you continue like this the bouncer will not hold out much longer!" };
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
                string[] lines = { "Now that you mastered the art of spellcasting let's improve our own dice.", 
                    "Green buff dice work a bit differently then your other dice.",
                    "You can place them in buff slots on your tablet to improve the other dice you placed around them.",
                    "The buff dice point to adjacent dice slots, making dice in those slots more powerful.",
                    "While holding a buff dice, press the right mouse button to rotate the buff dice and change the direction it points in.",
                    "These buff slots will add power to any affected die, indicated by the number in the buff slot.",
                    "But just like the spell slots, there are different buff slots with different effects, so make sure to check which buff is activated by hovering over the buff slot.",
                    "Now try buffing your dice before finalizing your die placement." };
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
                string[] lines = { "I must say, you are an excellent student!",
                    "Notice how the buff dice effects are not limited to their own column?",
                    "This means that you can place buff dice in a way that they affect multiple columns at once.",
                    "Remember this if you want to efficiently use your available buff slots!"};
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
                string[] lines = { "You have now mastered the basics of dice combat!",
                    "If you remember these techniques I taught you, most normal opponents will be no match for you.",
                    "It is now time for you to venture out into the world and test your skills against the many different infernal creatures that await you."};
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
