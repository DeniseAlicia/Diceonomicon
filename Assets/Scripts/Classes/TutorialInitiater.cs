using System.Collections.Generic;
using UnityEngine;

public class TutorialInitiater : MonoBehaviour
{
    public string area;
    public int level;

    public List<TabletData> implings;
    public List<DiceData> diceDeck;

    public static TutorialInitiater Instance;

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
    }
    
    //         TabletData tutorialImpling = Resources.Load<TabletData>($"Implings/TutorialData");

    //         GameStateManager.Instance.CreateDiceDeck();
    //         GameStateManager.Instance.player.activeImplings.Add(tutorialImpling);
}
