using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ImpSelectManager : MonoBehaviour
{
    public static ImpSelectManager Instance;

    [Header("Selection Settings")]
    public int maxSelections = 3;
    public List<TabletData> selectedImplings = new List<TabletData>();
    public bool newGame;

    private Coroutine hideCoroutine;
    public int combinedHealth;

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

        if (MainMenuManager.Instance.warningPanel != null)
            MainMenuManager.Instance.warningPanel.SetActive(false);
    }

    public bool Selection(TabletData data)
    {
        if (selectedImplings.Contains(data))
        {
            foreach (ImplingPreview imp in MainMenuManager.Instance.preview)
            {
                if (imp.impName.text == data.name)
                {
                    imp.impSprite.color = new Color(1, 1, 1, 0);
                    imp.impName.text = "";
                    imp.assigned = false;
                    float speed = 1f;
                    imp.gameObject.transform.DOMove(imp.startPosition, speed).SetEase(Ease.OutQuad);
                    speed += 0.1f;
                    break;
                }
            }

            selectedImplings.Remove(data);
            MainMenuManager.Instance.startButton.interactable = false;
            return false;
        }

        if (selectedImplings.Count >= maxSelections)
        {
            return false;
        }

        selectedImplings.Add(data);
        if (selectedImplings.Count == maxSelections)
        {
            MainMenuManager.Instance.startButton.interactable = true;
        }
        else
        {
            MainMenuManager.Instance.startButton.interactable = false;
        }

        foreach (ImplingPreview imp in MainMenuManager.Instance.preview)
        {
            if (!imp.assigned)
            {
                imp.impSprite.sprite = data.uiArtwork;
                imp.impSprite.color = new Color(1, 1, 1, 1);
                imp.impName.text = data.name;
                imp.assigned = true;
                break;
            }
        }

        return true;
    }

    public void ResetSelection()
    {
        selectedImplings = new List<TabletData>();
        combinedHealth = 0; 
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
        SceneTransition.Load("Tutorial");
    }

    public void ShowWarning()
    {
        if (MainMenuManager.Instance.warningPanel == null)
            return;

        MainMenuManager.Instance.warningPanel.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideWarningAfterDelay(3f));
    }

    public void HideWarning()
    {
        if (MainMenuManager.Instance.warningPanel == null)
            return;

        MainMenuManager.Instance.warningPanel.SetActive(false);
    }

    private IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideWarning();
    }


}
