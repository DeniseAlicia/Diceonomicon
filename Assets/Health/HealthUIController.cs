using UnityEngine;
using UnityEngine.UIElements;

public class HealthUIController : MonoBehaviour
{
    public UIDocument healthDisplay;
    public HealthData healthData;

    private IntegerField currentHealthField;
    private Button damageButton;

    private void OnEnable()
    {
        var root = healthDisplay.rootVisualElement;

        currentHealthField = root.Q<IntegerField>("CurrentHealth");
        damageButton = root.Q<Button>("Damage");

        UpdateUI();

        damageButton.clicked += OnDamageButtonClicked;
        healthData.OnValueChanged += UpdateUI;
    }

    private void OnDisable()
    {
        damageButton.clicked -= OnDamageButtonClicked;
        healthData.OnValueChanged -= UpdateUI;
    }

    private void OnDamageButtonClicked()
    {
        healthData.DamageTaken(1);
    }

    private void UpdateUI()
    {
        currentHealthField.value = healthData.currentHealth;
    }
}
