using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    // Awake is called once before the Start()
    void Awake()
    {
        // we are going to put a ScenePersist in every scene.. i check if already exists from the scene before, if doesn t exist  i add the class to the DontDestroyOnLoad.. if exists i destroy the current class and keep the old one
        int numScenePersist = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (numScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // reset scene persist
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
