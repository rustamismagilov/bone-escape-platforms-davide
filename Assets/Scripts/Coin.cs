using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;

    bool wasCollected = false;  // be sure it will collect only one coin when it touches it.. otherwise, due the update rate, it can collects more coins in once

    // On trigger enter 2d
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !wasCollected)
        {
            // play sound.. this play the sound from the position of the main camera
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            // add to coins
            FindFirstObjectByType<GameSessionController>().AddCoins(1);
            // destroy
            wasCollected = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
