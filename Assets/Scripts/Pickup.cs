using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;

    bool wasCollected = false;  // be sure it will collect only one coin when it touches it.. otherwise, due the update rate, it can collects more coins in once

    // On trigger enter 2d
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("Pickup, this is: " + this.gameObject.name + ", and collide with: " + collider.gameObject.name);

        if (!wasCollected)
        {
            // play sound.. this play the sound from the position of the main camera
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            // add to coins
            FindFirstObjectByType<GameSession>().AddCoins(1);
            // destroy
            wasCollected = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
