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
            if (pickupSound)
            {
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            }

            // check what is pickuping
            switch (gameObject.tag)
            {
                case "CoinPickup":
                    FindFirstObjectByType<GameSession>().AddCoins(1);
                    break;
                case "LifePickup":
                    FindFirstObjectByType<PlayerController>().AddHelth(40);
                    break;
                case "LivePickup":
                    FindFirstObjectByType<GameSession>().AddLives(1);
                    break;
                default:
                    break;
            }

            // destroy
            wasCollected = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
