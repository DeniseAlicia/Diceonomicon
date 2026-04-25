using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class ImpSelectManager : MonoBehaviour
{
    public static ImpSelectManager Instance;


    [Header("Selection Settings")]
    public int maxSelections = 3;
    public List<TabletData> selectedImplings = new List<TabletData>();
    public bool newGame;

    [Header("Selection Preview")]
    public ImplingPreview[] preview;

    [Header("UI References")]
    [SerializeField] private GameObject warningPanel;
    public Button startButton;

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
            foreach (ImplingPreview imp in preview)
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
            startButton.interactable = false;
            return false;
        }

        if (selectedImplings.Count >= maxSelections)
        {
            return false;
        }

        selectedImplings.Add(data);
        if (selectedImplings.Count == maxSelections)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }

        foreach (ImplingPreview imp in preview)
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
