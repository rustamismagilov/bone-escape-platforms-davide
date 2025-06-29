using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] InputAction inputAction;
    [Header("Menu effect")]
    [SerializeField] float toggleEffectDuration = 0.5f;

    Animator animator;
    PlayerInput playerInput;

    bool isOpen = false;
    bool isOpening; // during the effect between open and close

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = FindFirstObjectByType<PlayerInput>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMenu();
    }

    void OnDestroy()
    {
        EnableWorld();
    }

    // OnEnable
    void OnEnable()
    {
        inputAction.Enable();
    }
    // OnDisable
    void OnDisable()
    {
        inputAction.Disable();
    }

    // UpdateMenu
    void UpdateMenu()
    {
        if (inputAction.WasPressedThisFrame())
        {
            if (isOpen) CloseMenu();
            else OpenMenu();
        }
    }

    // ResetMenu
    public void ResetMenu()
    {
        // animation
        animator.SetBool("isOpen", false);

        // enable everything
        EnableWorld();

        // set variables
        isOpen = false;
        isOpening = false;
    }

    // OpenMenu
    public void OpenMenu()
    {
        if (!isOpen && !isOpening && !checkOtherMenu())
            StartCoroutine(OpenMenuCoroutine());
    }
    // OpenMenuCoroutine
    IEnumerator OpenMenuCoroutine()
    {
        // animation
        isOpening = true;
        animator.SetBool("isOpen", true);

        // disable everything
        DisableWorld();

        // wait end animation
        yield return new WaitForSecondsRealtime(toggleEffectDuration);

        // set variables
        isOpen = true;
        isOpening = false;
    }
    // CloseMenu
    public void CloseMenu()
    {
        if (isOpen && !isOpening && !checkOtherMenu())
            StartCoroutine(CloseMenuCoroutine());
    }
    // CloseMenuCoroutine
    IEnumerator CloseMenuCoroutine()
    {
        // animation
        isOpening = true;
        animator.SetBool("isOpen", false);

        // wait end animation
        yield return new WaitForSecondsRealtime(toggleEffectDuration);

        // enable everything
        EnableWorld();

        // set variables
        isOpen = false;
        isOpening = false;
    }

    // check if a menu is already opened
    bool checkOtherMenu()
    {
        MenuHandler[] menuHandlers = FindObjectsByType<MenuHandler>(FindObjectsSortMode.None);
        foreach (MenuHandler mh in menuHandlers)
        {
            if (mh != this && mh.isOpen)
            {
                return true;
            }
        }
        return false;
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
