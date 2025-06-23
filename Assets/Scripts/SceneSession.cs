using UnityEngine;

public class SceneSession : MonoBehaviour
{
    // Awake is called once before the Start()
    void Awake()
    {
        // we are going to put a SceneSession in every scene.. i check if already exists from the scene before, if doesn t exist  i add the class to the DontDestroyOnLoad.. if exists i destroy the current class and keep the old one
        int numSceneSession = FindObjectsByType<SceneSession>(FindObjectsSortMode.None).Length;
        if (numSceneSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // reset scene persist
    public void ResetSceneSession()
    {
        Destroy(gameObject);
    }
}
