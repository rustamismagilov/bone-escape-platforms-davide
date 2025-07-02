using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(EnemyController))]
public class MinotaurController : MonoBehaviour
{
    [Header("Voice")]
    [SerializeField] AudioClip voiceSound;
    [SerializeField] float voiceFrequency = 3f;

    [Header("Move")]
    [SerializeField] bool autoMove = true;
    [SerializeField] float moveFrequency = 2f;
    [SerializeField] float speed = 10f;
    [SerializeField] AudioClip moveSound;
    [SerializeField] bool followPlayer;

    [Header("Attack")]
    [SerializeField] bool autoAttack = true;
    [SerializeField] float attackFrequency = 2f;
    [SerializeField] AudioClip attackSound;

    [Header("Die")]
    [SerializeField] AudioClip dieSound;
    [SerializeField] float afterDeadBlock = 1f;

    EnemyController enemyController;
    AudioSource audioSource;
    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;

    bool hasHorizontalSpeed = false;
    bool isBlocked = false;
    bool isDeadProcessed = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        audioSource = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartAI();
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyController.isAlive)
        {
            CheckMove();
            CheckFlipSprite();
        }
        else
        {
            ProcessDead();
        }
    }

    // start auto move, jump...
    void StartAI()
    {
        InvokeRepeating(nameof(OnVoiceSound), voiceFrequency, voiceFrequency);
        InvokeRepeating(nameof(OnMove), moveFrequency, moveFrequency);
        InvokeRepeating(nameof(OnAttack), attackFrequency, attackFrequency);
    }

    // on move
    void OnMove()
    {
        if (!enemyController.isAlive) return;

        if (!autoMove)
        {
            moveInput = new Vector2(0, rb2d.linearVelocity.y);
        }
        else
        {
            List<int> possibleValues = new List<int> { -1, 0, 1 };

            // add possibility to follow the player
            if (followPlayer)
            {
                GameObject player = GameObject.FindWithTag("Player");
                float delta = transform.position.x - player.transform.position.x;
                if (delta > 1f) possibleValues.Add(-1);
                else if (delta < -1f) possibleValues.Add(1);
            }

            // set move input
            int randomIndex = Random.Range(0, possibleValues.Count);
            int randomX = possibleValues[randomIndex];
            moveInput = new Vector2(randomX, rb2d.linearVelocity.y);
        }
    }

    // on attack
    void OnAttack()
    {
        if (!enemyController.isAlive) return;

        if (!autoAttack)
        {
            animator.ResetTrigger("attack1");
            animator.ResetTrigger("attack2");
        }
        else
        {
            bool randomValue = Random.Range(0, 2) == 1 ? true : false;
            string randomAttack = Random.Range(0, 2) == 1 ? "attack1" : "attack2";
            if (randomValue)
            {
                animator.SetTrigger(randomAttack);
                if (attackSound != null)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(attackSound);
                }
            }
        }
    }

    // on voice sound
    void OnVoiceSound()
    {
        bool randomValue = Random.Range(0, 2) == 1 ? true : false;
        if (randomValue)
        {
            if (voiceSound != null && !audioSource.isPlaying)
                audioSource.PlayOneShot(voiceSound);
        }
    }

    // check move
    void CheckMove()
    {
        Vector2 velocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = velocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isMoving", hasHorizontalSpeed);
        if (hasHorizontalSpeed && moveSound != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(moveSound);
    }

    // flip the player when it changes direction
    void CheckFlipSprite()
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    // block the object smoothly after dead
    void CheckBlockAfterDead()
    {
        if (!isBlocked)
        {
            rb2d.linearVelocity = Vector2.zero;
            rb2d.angularVelocity = 0;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            isBlocked = true;
        }
    }

    // process dead only once
    void ProcessDead()
    {
        if (!enemyController.isAlive && !isDeadProcessed)
        {
            animator.SetTrigger("die");
            if (dieSound != null)
                audioSource.PlayOneShot(dieSound);
            rb2d.linearVelocity = new Vector2(0, 0);
            Invoke(nameof(CheckBlockAfterDead), afterDeadBlock);
            isDeadProcessed = true;
            //Destroy(this.gameObject, dieDelay);
        }
    }
}
