using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public int score = 0;
    public int highScore;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    public GameObject pause;
    private bool isPaused = false;

    public GameObject winPanel;

    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public TMP_Text timerText;


    public Image pauseButtonImage;
    public Sprite pauseSprite;
    public Sprite playSprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //for highest score
        if (highScoreText != null)
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "Highest Score:" + highScore;
        }

    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        //for highest score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (highScoreText != null)
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "Highest Score : " + highScore;
        }
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;

            if(pauseButtonImage != null &&  pauseSprite != null)
            {
                pauseButtonImage.sprite = pauseSprite;
            }
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
            if (pauseButtonImage != null && pauseSprite != null)
            {
                pauseButtonImage.sprite = playSprite;
            }
        }
    }

    public void ShowWin()
    {
        if (winPanel != null)
            winPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ShowGameOver()
    {
        if (BubbleSpawner.instance != null)

        {
            BubbleSpawner.instance.HideAllBubbles();
        }
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (gameOverText != null)
            gameOverText.text = "Game Over";

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
