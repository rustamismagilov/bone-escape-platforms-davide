using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTools : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            FindFirstObjectByType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene(0);
            Destroy(FindFirstObjectByType<GameSessionController>());
        }
    }
}
