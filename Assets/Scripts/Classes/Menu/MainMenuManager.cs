using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    [SerializeField] public GameObject diceViewObject;
    [SerializeField] public GameObject healthObject;
    [SerializeField] public TMP_Text healthObjectText;

    [Header("Selection Preview")]
    public ImplingPreview[] preview;
    [SerializeField] public List<Image> diceSprites;


    [Header("UI References")]
    [SerializeField] public GameObject warningPanel;
    public Button startButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ImpSelectManager.Instance.ResetSelection();
    }

    public void ConfirmSelection()
    {
        if (ImpSelectManager.Instance.selectedImplings.Count < ImpSelectManager.Instance.maxSelections)
        {
            ImpSelectManager.Instance.ShowWarning();
            return;
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.player.activeImplings = ImpSelectManager.Instance.selectedImplings;
        }

        ImpSelectManager.Instance.newGame = true;

        foreach (TabletData imp in ImpSelectManager.Instance.selectedImplings)
        {
            ImpSelectManager.Instance.combinedHealth += imp.health;
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.player.maxHealth = ImpSelectManager.Instance.combinedHealth;
            GameStateManager.Instance.player.currentHealth = ImpSelectManager.Instance.combinedHealth;
        }

        SceneTransition.Load("Map");
    }
}
