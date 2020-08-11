using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;

public class UIManager : MonoBehaviour
{
    // Handle to text
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _LivesImg;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;

    [SerializeField] private Sprite[] _liveSprites;
    private GameManager _gameManager;
    void Start()
    {
        // assign text component the handle
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
       
        /*if(_gameManager == null)
        {

        } */
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: "+ playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        // display img sprite
        // give it a new one based on the currentLives index

        _LivesImg.sprite = _liveSprites[currentLives];  

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = " ";
            yield return new WaitForSeconds(0.5f);

        }
    }
}
