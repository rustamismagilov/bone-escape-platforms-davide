using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSession : MonoBehaviour
{
    int sceneKey;

    // Awake is called once before the Start()
    void Awake()
    {
        // check if there is another instance
        sceneKey = SceneManager.GetActiveScene().buildIndex;
        SceneSession[] activeSceneSessions = FindObjectsByType<SceneSession>(FindObjectsSortMode.None);
        foreach (var session in activeSceneSessions)
        {
            if (session == this) continue;

            if (session.sceneKey == sceneKey)
            {
                Destroy(gameObject);
                return;
            }
            else Destroy(session.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        // first init
        InitSceneSession();
    }
    // OnDestroy is called once when the component is destroyed
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called automatically when scene has finished loading next scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneKey == scene.buildIndex) InitSceneSession();
        else Destroy(gameObject);
    }

    // init scene
    public void InitSceneSession()
    {
        // get objects
        GameObject player = GameObject.FindWithTag("Player");
        CinemachineCamera cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        StartHandler startHandler = FindFirstObjectByType<StartHandler>();

        if (player)
        {
            // set the camera
            if (cinemachineCamera) cinemachineCamera.Target.TrackingTarget = player.transform;
            // position to startHandler
            if (startHandler) player.transform.position = startHandler.transform.position;
            // reset player
            player.GetComponent<PlayerController>().ResetPlayerUI();
        }

    }
}
