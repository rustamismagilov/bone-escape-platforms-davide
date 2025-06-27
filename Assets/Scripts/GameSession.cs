using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] public int playerLives = 3;
    [SerializeField] public int playerCoins = 0;
    [SerializeField] float levelLoadDelay = 1f;

    TextMeshProUGUI playerLivesTextbox;
    Slider playerLifeSlider;
    TextMeshProUGUI playerCoinsTextbox;

    bool isDead = false;
    bool isWinning = false;

    // Awake is called once before the Start
    void Awake()
    {
        // we are going to put a GameSession in every scene.. i check if already exists from the scene before, if doesn t exist  i add the class to the DontDestroyOnLoad.. if exists i destroy the current class and keep the old one
        int numGameSession = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numGameSession > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        // player status
        playerLivesTextbox = (GameObject.FindWithTag("LivesTextbox")).GetComponent<TextMeshProUGUI>();
        playerLifeSlider = (GameObject.FindWithTag("LifeSlider")).GetComponent<Slider>();
        playerCoinsTextbox = (GameObject.FindWithTag("CoinsTextbox")).GetComponent<TextMeshProUGUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    // update UI
    void UpdateUI()
    {
        playerLivesTextbox.text = playerLives.ToString();
        playerLifeSlider.maxValue = FindFirstObjectByType<PlayerController>().GetTotalHealthAmount();
        playerLifeSlider.value = FindFirstObjectByType<PlayerController>().GetHealthAmount();
        playerCoinsTextbox.text = playerCoins.ToString();
    }

    // check if the player is dead
    public void ProcessPlayerDeath()
    {
        //Debug.Log("lives: " + playerLives);
        if (playerLives > 0) 
            Invoke(nameof(RepeatLevel), levelLoadDelay);
        else 
            Invoke(nameof(GameOver), levelLoadDelay);
    }
    // check if the player is dead
    public void ProcessPlayerWin()
    {
        if (isWinning) return;

        isWinning = true;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    // reset from 0 the level
    void RepeatLevel()
    {
        playerLives--;
        FindFirstObjectByType<PlayerController>().ResetLive();
        //FindFirstObjectByType<SceneSession>().InitSceneSession();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    // when the game finish
    void GameOver()
    {
        isDead = true;
        //FindFirstObjectByType<SceneSession>().ResetSceneSession();
        SceneManager.LoadScene("Game Over");
    }

    // load next level
    void LoadNextLevel()
    {
        //FindFirstObjectByType<SceneSession>().ResetSceneSession();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        isWinning = false;
    }


    // reset level with same condition
    public void ResetLevel()
    {
        //FindFirstObjectByType<SceneSession>().InitSceneSession();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    // reset game if you lose
    public void ResetGameSession()
    {
        SceneSession sceneSession = FindFirstObjectByType<SceneSession>();
        if (sceneSession) Destroy(sceneSession);
        Destroy(gameObject);
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
