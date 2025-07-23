using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public LevelManager levelManager;
    public static MenuType menuToOpenOnLoad = MenuType.Main;
    public UISounds buttonsSoundManager;


    //Uxml Files (visivle in the Unity Inspector)
    public VisualTreeAsset mainMenu;
    public VisualTreeAsset settingsMenu;
    public VisualTreeAsset mapMenu;
    public VisualTreeAsset diceMenu;
    public VisualTreeAsset implingsMenu;


    // menu administration
    private Dictionary<MenuType, VisualTreeAsset> menuVisualAssetsByType;     //Dictionary to assign a MenuType to each VisualTreeAsset (maps a Type A to Type B)
    private VisualElement uiRoot;
    private VisualElement activeMenuInstance;


    public void Awake()

    {
        //Map each MenuType to its VisualTreeAsset
        menuVisualAssetsByType = new Dictionary<MenuType, VisualTreeAsset>
        {
            {MenuType.Main, mainMenu},
            {MenuType.Settings, settingsMenu},
            {MenuType.Map, mapMenu},
            {MenuType.Dice, diceMenu},
            {MenuType.Implings, implingsMenu}
        };

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {
            uiRoot = GetComponent<UIDocument>().rootVisualElement;
            ShowMenu(menuToOpenOnLoad);
        }
        else
        {
            activeMenuInstance.RemoveFromHierarchy();
            activeMenuInstance = null;

        }
    }

    public void OpenMainMenu()
    {
        ShowMenu(MenuType.Main);
    }


    public void ShowMenu(MenuType menuType)
    {
        if (uiRoot == null)
        {
            Debug.LogWarning("uiRoot is null");
            return;
        }

        //remove current menu if existing
        activeMenuInstance?.RemoveFromHierarchy();

        //check if the menu is an element of the dictionary
        if (!menuVisualAssetsByType.ContainsKey(menuType)) return;

        //initalize uxml file and add it to root
        activeMenuInstance = menuVisualAssetsByType[menuType].Instantiate();
        uiRoot.Add(activeMenuInstance);

        //connect buttons in the activeMenuInstance automaically
        SetupMenuButtons(activeMenuInstance);

        if (menuType == MenuType.Map && levelManager != null)
        {
            levelManager.SetupLevelButtons(activeMenuInstance);
        }

        buttonsSoundManager.HookAllButtons();
    }

    private void SetupMenuButtons(VisualElement menu)
    {
        //close menu button 
        var closeButton = menu.Q<Button>("CloseMenu");
        if (closeButton != null)
        {
            closeButton.clicked += () =>
             {
                 SceneManager.LoadScene("StartScreen");
             };
        }


        foreach (MenuType target in System.Enum.GetValues(typeof(MenuType)))
        {
            //converts button name
            string buttonName = $"button_to_{target}";
            var button = menu.Q<Button>(buttonName);

            if (button != null)
            {
                RegisterButtonCallback(button, target);
            }
        }
    }

    private void RegisterButtonCallback(Button button, MenuType targetMenu)
    {
        button.clicked += delegate
        {
            ShowMenu(targetMenu);
        };
    }
}
