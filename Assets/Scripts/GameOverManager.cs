using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    GameSession gameSession;
    GameObject player;

    [Header("Score containers")]
    [SerializeField] TextMeshProUGUI playerLivesTextbox;
    [SerializeField] TextMeshProUGUI playerCoinsTextbox;
    [SerializeField] TextMeshProUGUI levelReachedTextbox;

    // Awake is called once before the Start
    void Awake()
    {
        gameSession = FindFirstObjectByType<GameSession>();
        player = (GameObject.FindWithTag("Player"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ProcessGameOver();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // when home next button is clicked
    public void OnHomeClick()
    {
        SceneManager.LoadScene("Home");
    }
    // when play again button is clicked
    public void OnPlayAgainClick()
    {
        SceneManager.LoadScene("Level 1");
    }

    void ProcessGameOver()
    {
        if (gameSession)
        {
            // menage gamesession (move gamesession outside dontdestroyonload)
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.MoveGameObjectToScene(gameSession.gameObject, activeScene);
            Destroy(GameObject.FindWithTag("StatusBar").gameObject);
            // set score
            playerLivesTextbox.text = gameSession.playerLives.ToString();
            playerCoinsTextbox.text = gameSession.playerCoins.ToString();
            levelReachedTextbox.text = gameSession.ReachedLevel.ToString();
            // set player
            player.transform.position = new Vector2(0, 0);
        }
    }
}
