using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Main Menu: select a game to play or exit the game
public class MainMenu : MonoBehaviour
{
    public string gameTitle = "THREE-IN-ONE GAME";
    public string authorName = "Wai Yan Mya Thaung";
    public Color backgroundColor = new Color(0.08f, 0.1f, 0.16f);

    void Start()
    {
        // Make sure the game is not still paused when coming back from a sub game
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        BuildMenu();
    }

    void BuildMenu()
    {
        RectTransform panel = MenuUI.CreateCanvas(transform, "Main Menu Canvas", backgroundColor, 0);

        // Title at the top
        MenuUI.CreateText(panel, gameTitle, 110, new Vector2(0.5f, 0.78f), TextAnchor.MiddleCenter, new Vector2(1800, 160));

        // Game list, left aligned, slightly right of center
        Button[] buttons =
        {
            MenuUI.CreateButton(panel, "Mad Driver", 72, new Vector2(0.32f, 0.53f), TextAnchor.MiddleLeft, () => LoadGame("Driving")),
            MenuUI.CreateButton(panel, "Fly Like a Bird", 72, new Vector2(0.32f, 0.44f), TextAnchor.MiddleLeft, () => LoadGame("Flying")),
            MenuUI.CreateButton(panel, "I'm a Sumo and a Ball", 72, new Vector2(0.32f, 0.35f), TextAnchor.MiddleLeft, () => LoadGame("Sumo")),
            MenuUI.CreateButton(panel, "Exit", 72, new Vector2(0.32f, 0.26f), TextAnchor.MiddleLeft, ExitGame),
        };
        MenuUI.LinkVertically(buttons);

        // Credit at the bottom right
        MenuUI.CreateText(panel, "By " + authorName, 44, new Vector2(0.96f, 0.07f), TextAnchor.MiddleRight, new Vector2(900, 70));

        EventSystem.current?.SetSelectedGameObject(buttons[0].gameObject);
    }

    void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
