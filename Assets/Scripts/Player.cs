using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _lasterPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    private float _canFire = -1;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;

    private bool _isSpeedBoostActive = false;

    private bool _isShielsdBoostActive = false;
    private bool _isShieldsActive = false;

    // variable reference to shield visualiser
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngine, _rightEngine;

    [SerializeField] private int _score;
    // variable to store the audio clip
    [SerializeField] private AudioClip _laserSoundClip;

    // Method 1 - Serialise and drag and drop audio source in unity 
    private AudioSource _audioSource; 

    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();// Find the game component then get component


        if(_spawnManager==null)
        {
            Debug.LogError("Spawn manager is null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // check if ui manager is called
        if (_uiManager == null)
        {
            Debug.LogError("UI Manger is null");
        }

        //Method 2 - Audio Source
        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on the player is Null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }


    }

    void Update()
    {
        CalculateMovement();

#if UNITY_ANDROID
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire")) && Time.time > _canFire)
        {
            FireLaser();
        }
#else
        if ((Input.GetKeyDown(KeyCode.Space) /*|| Input.GetMouseButton(0)*/) && Time.time > _canFire)
        {
            FireLaser();
        }
#endif
    }

    void CalculateMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            transform.Translate(direction * _speed * Time.deltaTime);

        // Mathf.Clamp will limit movement of player between given min and max values
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));


        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
            // Cooldown system - fire for 1.5 seconds and then cooldown 

        _canFire = Time.time + _fireRate;

        // If space key pressed,
        //if tripleshotActive is true
        //fire 3 lasers (triple shot prefab)
        //else
        // instantiate 3 lasers (triple shot prefab)

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        // Quaternion.identity - Default Location
        // New vector to fire laser from 0.8 units on Y axis. 
        {
            Instantiate(_lasterPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        // plan the laser audio clip
        _audioSource.Play();
    }

    public void Damage()
    {
        // if shields is active
        // do nothing ..
        // deactivate shields
        //return;

        if(_isShieldsActive == true)
        {
            _isShieldsActive = false;
            // Disable shield visualiser
            _shieldVisualizer.SetActive(false);
            return;
        }
        else
        {

        }

        _lives--;                   // Method 1
         // _lives = _lives - 1;    // Method 2
         // _lives -= 1;            // Method 3 to decrese lives

        if(_lives==2)
        {
            _leftEngine.SetActive(true);
        }
        else if(_lives ==1)
        {
            _rightEngine.SetActive(true);
        }


        _uiManager.UpdateLives(_lives);

        // Check if Dead
        // Destroy us
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        // tripleShotActive becomes true
        // start the power down coroutine for triple shot

        _isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //IEnumerator TripleShotPowerDownRoutine
    // wait for 5 seconds
    //set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {
       yield return new WaitForSeconds(5.0f);
       _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        // enable shield Visualiser
        _shieldVisualizer.SetActive(true);
    }

    //method to add 10 to the score
    // Comminicate with the UI to update the score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    } // points to collect points according to enemy killed

}
