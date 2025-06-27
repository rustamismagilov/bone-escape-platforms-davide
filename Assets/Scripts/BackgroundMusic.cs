using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioClip backgroundClip;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        int numBackgroundMusic = FindObjectsByType<BackgroundMusic>(FindObjectsSortMode.None).Length;
        if (numBackgroundMusic > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = backgroundClip;
        audio.loop = true;
        audio.playOnAwake = true;
        audio.spatialBlend = 0; // 2D sound
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
