using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// In-Game Menu: press Escape in any sub game to pause, then Resume, Restart or go Back to Main Menu
public class InGameMenu : MonoBehaviour
{
    const string MainMenuScene = "MainMenu";

    GameObject menuCanvas;
    Button firstButton;
    bool isPaused;

    // Add the In-Game Menu to every sub game scene automatically
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RegisterSceneHook()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == MainMenuScene) return;
        if (FindAnyObjectByType<InGameMenu>() != null) return;
        new GameObject("In-Game Menu").AddComponent<InGameMenu>();
    }

    void Start()
    {
        RectTransform panel = MenuUI.CreateCanvas(transform, "In-Game Menu Canvas", new Color(0f, 0f, 0f, 0.75f), 100);
        menuCanvas = panel.parent.gameObject;

        MenuUI.CreateText(panel, "PAUSED", 120, new Vector2(0.5f, 0.76f), TextAnchor.MiddleCenter, new Vector2(1200, 170));

        Button[] buttons =
        {
            MenuUI.CreateButton(panel, "Resume", 72, new Vector2(0.5f, 0.53f), TextAnchor.MiddleCenter, Resume),
            MenuUI.CreateButton(panel, "Restart", 72, new Vector2(0.5f, 0.44f), TextAnchor.MiddleCenter, Restart),
            MenuUI.CreateButton(panel, "Back to Main Menu", 72, new Vector2(0.5f, 0.35f), TextAnchor.MiddleCenter, BackToMainMenu),
        };
        MenuUI.LinkVertically(buttons);
        firstButton = buttons[0];

        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuCanvas.SetActive(true);
        EventSystem.current?.SetSelectedGameObject(firstButton.gameObject);
    }

    void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        EventSystem.current?.SetSelectedGameObject(null);
        menuCanvas.SetActive(false);
    }

    void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void BackToMainMenu()
    {
        Resume();
        SceneManager.LoadScene(MainMenuScene);
    }
}
