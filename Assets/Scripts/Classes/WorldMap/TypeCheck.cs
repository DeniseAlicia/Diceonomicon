using TMPro;
using UnityEngine;

public class TypeCheck : MonoBehaviour
{
    public TMP_Text typeText;
    public GameStateManager gameState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = FindFirstObjectByType<GameStateManager>();
        string type = gameState.player.type;
        typeText.text = type;
    }
}
