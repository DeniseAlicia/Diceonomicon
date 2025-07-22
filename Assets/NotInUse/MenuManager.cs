using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private UIDocument _document;

    private Button _button;

    private List<Button> _menuButtons = new List<Button>();

    private AudioSource _audioSource;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _audioSource = GetComponent<AudioSource>();

        _button = _document.rootVisualElement.Q("Home") as Button;
        _button.RegisterCallback<ClickEvent>(OnHomeClick);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }

    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(OnHomeClick);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnHomeClick(ClickEvent evt)
    {
        Debug.Log("Home Button");

    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

}
