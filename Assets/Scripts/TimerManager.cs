using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float timer = 60f;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver || Time.timeScale == 0f) return;

        timer -= Time.deltaTime;
        if (UIManager.instance != null && UIManager.instance.timerText != null)
        {
            int secondsLeft = Mathf.CeilToInt(timer);
            UIManager.instance.timerText.text = "Time: " + secondsLeft;
        }

        if (timer <= 0f && !isGameOver)
        {
            isGameOver = true;
            Bubble[] bubbles = FindObjectsOfType<Bubble>();
            foreach (Bubble bubble in bubbles)
            {
                Destroy(bubble.gameObject);
            }
            if (UIManager.instance != null)
                UIManager.instance.ShowGameOver();
        }
    }
}
