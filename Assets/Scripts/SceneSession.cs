using UnityEngine;

public class SceneSession : MonoBehaviour
{
    // Awake is called once before the Start()
    void Awake()
    {
        int numSceneSession = FindObjectsByType<SceneSession>(FindObjectsSortMode.None).Length;
        if (numSceneSession > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    // reset scene persist
    public void ResetSceneSession()
    {
        Destroy(gameObject);
    }
}
