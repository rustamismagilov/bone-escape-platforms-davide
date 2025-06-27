using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] InputAction openInputAction;
    [Header("Menu effect")]
    [SerializeField] float toggleEffectDuration = 0.5f;

    Animator myAnimator;
    PlayerInput playerInput;

    bool isOpen = false;
    bool isOpening; // during the effect between open and close

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerInput = FindFirstObjectByType<PlayerInput>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitMenu();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMenu();
    }

    // OnResumeButtonClick
    public void OnResumeButtonClick()
    {
        StartCoroutine(CloseMenu());
    }
    // OnRestartButtonClick
    public void OnRestartLevelButtonClick()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        EnableWorld();
    }
    // OnQuitButtonClick
    public void OnQuitButtonClick()
    {
        SceneManager.LoadScene(0);
        EnableWorld();
    }
    // OnEnable
    void OnEnable()
    {
        openInputAction.Enable();
    }
    // OnDisable
    void OnDisable()
    {
        openInputAction.Disable();
    }

    // UpdateMenu
    void UpdateMenu()
    {
        if (openInputAction.WasPressedThisFrame())
        {
            if (isOpen) StartCoroutine(CloseMenu());
            else StartCoroutine(OpenMenu());
        }
    }

    // InitMenu
    void InitMenu()
    {
        if (isOpen)
        {
            myAnimator.SetBool("isOpen", true);
            DisableWorld();
        }
        else
        {
            myAnimator.SetBool("isOpen", false);
            EnableWorld();
        }
    }
    // OpenMenu
    public IEnumerator OpenMenu()
    {
        if (!isOpen && !isOpening)
        {
            // animation
            isOpening = true;
            myAnimator.SetBool("isOpen", true);

            // disable everything
            DisableWorld();

            // wait end animation
            yield return new WaitForSecondsRealtime(toggleEffectDuration);

            // set variables
            isOpen = true;
            isOpening = false;
        }
    }
    // CloseMenu
    public IEnumerator CloseMenu()
    {
        if (isOpen && !isOpening)
        {
            // animation
            isOpening = true;
            myAnimator.SetBool("isOpen", false);

            // wait end animation
            yield return new WaitForSecondsRealtime(toggleEffectDuration);

            // enable everything
            EnableWorld();

            // set variables
            isOpen = false;
            isOpening = false;
        }
    }

    // disable / freeze the world when the menu is open
    void DisableWorld()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        if (playerInput) playerInput.enabled = false;
    }

    // enable / unfreeze the world when the menu is close
    void EnableWorld()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (playerInput) playerInput.enabled = true;
    }
}
