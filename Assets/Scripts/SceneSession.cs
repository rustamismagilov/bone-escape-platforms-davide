using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSession : MonoBehaviour
{
    int currentSceneIndex;

    // Awake is called once before the Start()
    void Awake()
    {
        // check if there is another instance
        int numSceneSession = FindObjectsByType<SceneSession>(FindObjectsSortMode.None).Length;
        if (numSceneSession > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // save current index
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        // first init
        InitSceneSession();
    }
    // OnDestroy is called once when the component is destroid
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called automatically when scene has finished loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitSceneSession();
    }

    // init scene
    public void InitSceneSession()
    {
        // get objects
        GameObject player = GameObject.FindWithTag("Player");
        CinemachineCamera cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        Start start = FindFirstObjectByType<Start>();

        if (player)
        {
            // set the camera
            if (cinemachineCamera) cinemachineCamera.Target.TrackingTarget = player.transform;
            // position to start
            if (start) player.transform.position = start.transform.position;
            // reset player
            player.GetComponent<PlayerController>().ResetPlayerUI();
        }

    }

    // reset scene persist
    public void ResetSceneSession()
    {
        Destroy(gameObject);
    }
}
