using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;

    // 0 - Triple shot, 1 - Speed up, 2 - Shield
    [SerializeField] private int powerupID;

    [SerializeField] private AudioClip _clip;

    void Update()
    {
        // Move down at a speed of 3 (Adjust in the inspector)
        // When we leave the screen, destroy the object
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    // OnTrigger Collisoin
    // Only be collectable by the player (Hint: Use Tags)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //communicate with player script
            //handle to the component i want
            //assign the handle to the component

            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;

                    case 1:
                        player.SpeedBoostActive();
                        break;

                    case 2:
                        player.ShieldsActive();
                        break;

                    default:
                        break;
                }
            }


            Destroy(this.gameObject);
        }
    }
}
