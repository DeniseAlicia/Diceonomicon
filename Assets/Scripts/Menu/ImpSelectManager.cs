using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ImpSelectManager : MonoBehaviour
{
    public static ImpSelectManager Instance;


    [Header("Selection Settings")]
    public int maxSelections = 3;
    public List<TabletData> selectedImplings = new List<TabletData>();
    public bool newGame;

    [Header("UI References")]
    [SerializeField] private GameObject warningPanel;

    private Coroutine hideCoroutine;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (warningPanel != null)
            warningPanel.SetActive(false);
    }

    public bool Selection(TabletData data)
    {
        if (selectedImplings.Contains(data))
        {
            selectedImplings.Remove(data);
            return false;
        }

        if (selectedImplings.Count >= maxSelections)
        {
            return false;
        }

        selectedImplings.Add(data);
        return true;
    }

    public void ConfirmSelection()
    {
        if (selectedImplings.Count < maxSelections)
        {
            ShowWarning();
            return;
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.player.activeImplings = selectedImplings;
        }

        newGame = true;
        SceneManager.LoadScene("Map");
    }

        public void ConfirmSelectionTutorial()
    {
        if (selectedImplings.Count < maxSelections)
        {
            ShowWarning();
            return;
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.player.activeImplings = selectedImplings;
        }

        newGame = true;
        SceneManager.LoadScene("Tutorial");
    }

    private void ShowWarning()
    {
        if (warningPanel == null)
            return;

        warningPanel.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideWarningAfterDelay(3f));
    }

    private void HideWarning()
    {
        if (warningPanel == null)
            return;

        warningPanel.SetActive(false);
    }

    private IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideWarning();
    }

}
