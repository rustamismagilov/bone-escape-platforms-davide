using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("Initial state")]
    [SerializeField] public int playerLives = 3;
    [SerializeField] public int playerCoins = 0;

    [Header("State containers")]
    [SerializeField] TextMeshProUGUI playerLivesTextbox;
    [SerializeField] Slider playerLifeSlider;
    [SerializeField] TextMeshProUGUI playerCoinsTextbox;

    [Header("Level delay")]
    [SerializeField] float levelLoadDelay = 1f;

    bool isWinning = false;
    int reachedLevel;
    public int ReachedLevel { get { return reachedLevel; } }

    // Awake is called once before the Start
    void Awake()
    {
        // we are going to put a GameSession in every scene.. i check if already exists from the scene before, if doesn t exist  i add the class to the DontDestroyOnLoad.. if exists i destroy the current class and keep the old one
        int numGameSession = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numGameSession > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        UpdateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    // OnDestroy is called once when the component is destroyed
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called automatically when scene has finished loading next scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateLevel();
    }

    // update UI
    void UpdateUI()
    {
        if (playerLivesTextbox) playerLivesTextbox.text = playerLives.ToString();
        if (playerLifeSlider)
        {
            playerLifeSlider.maxValue = FindFirstObjectByType<PlayerController>().GetTotalHealthAmount();
            playerLifeSlider.value = FindFirstObjectByType<PlayerController>().GetHealthAmount();
        }
        if (playerCoinsTextbox) playerCoinsTextbox.text = playerCoins.ToString();
    }

    // update level
    void UpdateLevel()
    {
        // set current level
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level"))
        {
            int newLevel = int.Parse(sceneName.Replace("Level", "").Trim());
            reachedLevel = newLevel;
        }
    }

    // check if the player is dead
    public void ProcessPlayerDeath()
    {
        //Debug.Log("lives: " + playerLives);
        if (playerLives > 0)
        {
            playerLives--;
            Invoke(nameof(RepeatLevel), levelLoadDelay);
        }
        else
        {
            //isDead = true;
            Invoke(nameof(GameOver), levelLoadDelay);
        }
    }
    // check if the player is dead
    public void ProcessPlayerWin()
    {
        if (isWinning) return;

        isWinning = true;
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    // reset from 0 the level
    void RepeatLevel()
    {
        FindFirstObjectByType<PlayerController>().ResetLive();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    // load next level
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        isWinning = false;
    }
    // reset level with same condition
    public void ResetLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // restart game from 1 level
    public void RestartGame()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Level 1");
    }
    // quit game to home
    public void QuitGame()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Home");
    }

    // when the game finish
    void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    // add coin
    public void AddCoins(int amount)
    {
        playerCoins += amount;
    }

    // add lives
    public void AddLives(int amount)
    {
        playerLives += amount;
    }

}
