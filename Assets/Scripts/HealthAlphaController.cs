using UnityEngine;
using TMPro;

public class HealthAlphaController : MonoBehaviour
{
    public Material playerHealth;
    public Material enemyHealth;
    public Material healthOverlay;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;

    public float alpha;

    public void Update()
    {
        alpha = Mathf.Clamp(alpha, 0f, 0.5f);

        Color playerColor = playerHealth.GetColor("_BaseColor");
        playerColor.a = alpha;
        playerHealth.SetColor("_BaseColor", playerColor);

        Color enemyColor = enemyHealth.GetColor("_BaseColor");
        enemyColor.a = alpha;
        enemyHealth.SetColor("_BaseColor", enemyColor);

        Color overlayColor = healthOverlay.color;
        overlayColor.a = alpha + 0.5f;
        healthOverlay.color = overlayColor;

        Color playerTextColor = playerHealthText.color;
        playerTextColor.a = alpha + 0.5f;
        playerHealthText.color = playerTextColor;

        Color enemyTextColor = enemyHealthText.color;
        enemyTextColor.a = alpha + 0.5f;
        enemyHealthText.color = enemyTextColor;
    }
}
