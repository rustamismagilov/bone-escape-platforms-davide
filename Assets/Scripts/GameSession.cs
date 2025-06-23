using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerCoins = 0;
    [SerializeField] float levelLoadDelay = 1f;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isWinning = false;

    TextMeshProUGUI playerLivesTextbox;
    Slider playerLifeSlider;
    TextMeshProUGUI playerCoinsTextbox;

    // Awake is called once before the Start
    void Awake()
    {
        // we are going to put a GameSession in every scene.. i check if already exists from the scene before, if doesn t exist  i add the class to the DontDestroyOnLoad.. if exists i destroy the current class and keep the old one
        int numGameSession = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
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
        playerLivesTextbox = (GameObject.FindWithTag("LivesTextbox")).GetComponent<TextMeshProUGUI>();
        playerLifeSlider = (GameObject.FindWithTag("LifeSlider")).GetComponent<Slider>();
        playerCoinsTextbox = (GameObject.FindWithTag("CoinsTextbox")).GetComponent<TextMeshProUGUI>();

        playerLivesTextbox.text = playerLives.ToString();
        playerLifeSlider.value = FindFirstObjectByType<PlayerController>().GetHealthAmount();
        playerCoinsTextbox.text = playerCoins.ToString();
    }

    // check if the player is dead
    public void ProcessPlayerDeath()
    {
        if (playerLives > 0)
        {
            // set value
            playerLives--;
            Invoke(nameof(ResetLevel), levelLoadDelay);
        }
        else
        {
            isDead = true;
            Invoke(nameof(ResetGameSession), levelLoadDelay);
        }

        //Debug.Log("lives: " + playerLives);
    }
    // take one life and update view
    void ResetLevel()
    {
        // load scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    // reset game if you lose
    void ResetGameSession()
    {
        FindFirstObjectByType<SceneSession>().ResetSceneSession();
        SceneManager.LoadScene(0);

        Destroy(gameObject);
    }

    // check if the player is dead
    public void ProcessPlayerWin()
    {
        if (isWinning) return;

        isWinning = true;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Invoke(nameof(LoadNextLevel), levelLoadDelay);
        }

    }
    // load next level
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        FindFirstObjectByType<SceneSession>().ResetSceneSession();
        isWinning = false;
    }

    // add coin
    public void AddCoins(int amount)
    {
        // set value
        playerCoins += amount;
    }

}
