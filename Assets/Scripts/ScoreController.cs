using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score = 0;
    private int buildIndex;
    [SerializeField] private TextMeshProUGUI powerUpText;

    void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score: " + score;
        SetHighScore();
        buildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        
        powerUpText.text = "";
        
    }
    void Update()
    {
        if (buildIndex != 1)
        {
            SetHighScore();
        }
    }
    
    public void ChangeScore(int increment)
    {
        if(score < 1 && increment < 0) { return;  }
        score += increment;
        scoreText.text = "Score: " + score;
    }

    public void SetHighScore()
    {
        if (score > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", score);
        }
    }

    public void PowerUpText(Collectibles powerUp)
    {
        switch (powerUp)
        {
            case Collectibles.shield:
                powerUpText.text = "Shield Active";
                break;
            case Collectibles.doubleSpeed:
                powerUpText.text = "Double Speed Active";
                break;
            case Collectibles.doublePoint:
                powerUpText.text = "Double Score Active";
                break;
            case Collectibles.powerUp:
                powerUpText.text = "";
                break;
        }
    }

}
