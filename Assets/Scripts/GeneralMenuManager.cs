using UnityEngine;

public class GeneralMenuManager : MonoBehaviour
{
    MenuHandler menuHandler;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        menuHandler = GetComponent<MenuHandler>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }

    // OnResumeButtonClick
    public void OnResumeButtonClick()
    {
        menuHandler.CloseMenu();
    }
    // OnRestartButtonClick
    public void OnRestartLevelButtonClick()
    {
        FindFirstObjectByType<GameSession>().ResetLevel();
    }
    // OnQuitButtonClick
    public void OnQuitButtonClick()
    {
        FindFirstObjectByType<GameSession>().QuitGame();
    }

}
