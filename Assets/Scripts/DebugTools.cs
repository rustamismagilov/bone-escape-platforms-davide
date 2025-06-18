using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTools : MonoBehaviour
{
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
