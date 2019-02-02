using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public delegate void RefreshUIEvent();
    public static RefreshUIEvent Refresh;

    public static MenuManager ActiveManager;

    // Enum that controls the menu
    public enum MenuState
    {
        Off,
        StartMenu = 1,
        MainMenu = 2,
        MissionSelectMenu = 3,
        MissionEndMenu = 4,
        OptionsMenu = 5,
        PauseMenu = 6,
        HeadsUpDisplay = 7,
        MissionBrief = 8
    }

    // Unity UI Event System
    public GameObject EventSystem;

    // List of menu/ui subobjects
    public GameObject Background;
    public GameObject MainMenu;
    public GameObject MissionSelectMenu;
    public GameObject OptionsMenu;
    public GameObject LoadingScreen;
    public GameObject PauseMenu;
    public GameObject HeadsUpDisplay;
    public GameObject MissionEndMenu;
    public GameObject MissionBriefMenu;

    // Allows tracking of loading state
    public bool Loading { get; private set; }

    // Tracks the current state
    private MenuState state;

    private string currentLevel;


    // Hides all menus and UIs and goes to start menu
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(EventSystem);

        ActiveManager = this;
        Background.SetActive(false);
        MainMenu.SetActive(false);
        MissionSelectMenu.SetActive(false);
        LoadingScreen.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        HeadsUpDisplay.SetActive(false);
        MissionEndMenu.SetActive(false);
        MissionBriefMenu.SetActive(false);
        LoadMenu(MenuState.StartMenu);
    }

    // Handles any menu logic that needs to be checked frame by frame
    private void Update()
    {
        if (state == MenuState.StartMenu && (Input.anyKeyDown || Input.GetMouseButton(0)))
        {
            LoadMenu(MenuState.MainMenu);
        }
    }

    // Quits the application
    public void Exit()
    {
        Application.Quit();
    }

    // Loads a menu and unloads the current menu
    public void LoadMenu(int state)
    {
        LoadMenu((MenuState)state);
    }

    // Performs logic to exit one menu state and enter another
    public void LoadMenu(MenuState newState)
    {
        // Unload old menus
        switch (state)
        {
            case MenuState.MainMenu:
                MainMenu.SetActive(false);
                break;
            case MenuState.MissionSelectMenu:
                MissionSelectMenu.SetActive(false);
                break;
            case MenuState.MissionEndMenu:
                MissionEndMenu.SetActive(false);
                break;
            case MenuState.OptionsMenu:
                OptionsMenu.SetActive(false);
                break;
            case MenuState.PauseMenu:
                PauseMenu.SetActive(false);
                break;
            case MenuState.HeadsUpDisplay:
                Cursor.lockState = CursorLockMode.None;
                HeadsUpDisplay.SetActive(false);
                break;
            case MenuState.StartMenu:
                // This is the only state that uses Update logic
                enabled = false;
                break;
            case MenuState.Off:
                break;
            case MenuState.MissionBrief:
                MissionBriefMenu.SetActive(false);
                break;
            default:
                Debug.Log("Unregistered State: " + state);
                break;
        }

        // Load new menus
        switch (newState)
        {
            case MenuState.MainMenu:
                MainMenu.SetActive(true);
                break;
            case MenuState.MissionSelectMenu:
                MissionSelectMenu.SetActive(true);
                break;
            case MenuState.MissionEndMenu:
                MissionEndMenu.SetActive(true);
                break;
            case MenuState.OptionsMenu:
                OptionsMenu.SetActive(true);
                break;
            case MenuState.PauseMenu:
                PauseMenu.SetActive(true);
                break;
            case MenuState.HeadsUpDisplay:
                Cursor.lockState = CursorLockMode.Locked;
                HeadsUpDisplay.SetActive(true);
                break;
            case MenuState.StartMenu:
                Background.SetActive(true);
                // This is the only state that uses Update logic
                enabled = true;
                break;
            case MenuState.Off:
                break;
            case MenuState.MissionBrief:
                MissionBriefMenu.SetActive(true);
                break;
            default:
                Debug.Log("Unregistered State: " + state);
                break;
        }
        state = newState;
    }

    // Loads a stage async
    public void LoadStage(string stageName)
    {
        // Loads scene async with a loading screen
        StartCoroutine(LoadStageHelper(stageName));
    }

    public void UnloadStage()
    {
        SceneManager.LoadSceneAsync("EMPTYSCENE");
        Background.SetActive(true);
    }

    public void ReloadStage()
    {
        // Loads scene async with a loading screen
        StartCoroutine(LoadStageHelper(currentLevel));
    }

    private IEnumerator LoadStageHelper(string stageName)
    {
        // Set up loading screen
        Loading = true;
        LoadingScreen.SetActive(true);

        //// Unload last level
        //Scene old = SceneManager.GetSceneByName(currentLevel);
        //if (old.isLoaded)
        //{
        //    yield return SceneManager.UnloadSceneAsync(old);
        //}

        // Load new level
        currentLevel = stageName;
        yield return SceneManager.LoadSceneAsync(stageName);
        LoadingScreen.SetActive(false);

        // Deactivate loading screen
        Loading = false;
        LoadMenu(MenuState.HeadsUpDisplay);

        Background.SetActive(false);

        // Send a refresh signal
        if (Refresh != null)
        {
            Refresh();
        }
        if (UIDeathTracker.ActiveInScene)
        {
            UIDeathTracker.ActiveInScene.Hide();
        }
        if (UICameraFlash.ActiveInScene)
        {
            UICameraFlash.ActiveInScene.Hide();
        }
    }
}
