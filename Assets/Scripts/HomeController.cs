using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    // Awake is called once before the Start
    void Awake()
    {
        ResetSessions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // when play next button is clicked
    public void OnPlayNextClick()
    {
        SceneManager.LoadScene("Level 1");
    }
    // when list of levels button is clicked
    public void OnLevelListClick()
    {
        //Debug.Log("LevelListClick");
    }
    // when credits button is clicked
    public void OnCreditsClick()
    {
        //Debug.Log("OnCreditsClick");
    }

    // reset active sessions to start from zero
    void ResetSessions()
    {
        GameSession[] gameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None);
        SceneSession[] sceneSessions = FindObjectsByType<SceneSession>(FindObjectsSortMode.None);

        foreach (GameSession gameSession in gameSessions)
        {
            Destroy(gameSession.gameObject);
        }
        foreach (SceneSession sceneSession in sceneSessions)
        {
            Destroy(sceneSession.gameObject);
        }
    }
}
