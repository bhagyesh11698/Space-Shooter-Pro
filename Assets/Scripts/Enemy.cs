using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private GameObject _laserPrefab;

    private Player _player; // a global variable to reduce calls of getcomponent.
    //handle to animator component
    private Animator _anim;

    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        // This way, it will be called only one time and can be used where ever required
        
        //null check player
        if(_player ==null)
        {
            Debug.LogError("PLayer is null");
        }

        //assign component to anim
        _anim = GetComponent<Animator>(); // no need to find as it is in same gameobject
        if(_anim == null)
        {
            Debug.LogError("Animator is null");
        }
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
              
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            //Debug.Break(); // pause game in unity
        }
    }

    void CalculateMovement()
    {
        // move down by 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // If bottom of screen, respawn at top with a new random x positions
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // can change other to any new name
    {
            // Debug.Log("Hit: " + other.transform.name);
            // other gives object, transform gives root of the object and name - name of the object

            // If other is player - damage the player  - destroy us
        if (other.tag == "Player")
        {
            // Damage Player
            Player player = other.transform.GetComponent<Player>();
            
            if(player !=null)
            {
                player.Damage();
            }
            // trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0; // Freeze enemy once destroyed
            _audioSource.Play();

            //destroy object after 2.8 second so that animation plays
            
            //Destroy(this.gameObject,2.8f); 
        }
            // If other is laser - laser - destroy us
        if(other.tag=="Laser")
        {
            Destroy(other.gameObject);
            // Add 10 to the score
            if (_player !=null)
            {
                _player.AddScore(10);
            }
            // trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            
            Destroy(this.gameObject,2.8f);
            
        }
    }
}
