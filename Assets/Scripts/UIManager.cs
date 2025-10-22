using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; 

    public int score = 0;
    public TMP_Text scoreText;

    public GameObject pause;
    private bool isPaused = false;

    public GameObject winPanel;

    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public TMP_Text timerText;

    
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
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
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
}
