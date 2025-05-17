using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 60f; // 1 minute
    private float timer;

    public Text timerText; // Assign via Inspector
    public Transform player;
    public Transform finishLine;

    private bool gameEnded = false;

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, timeLimit);

        UpdateTimerUI();

        float distanceToFinish = Vector3.Distance(player.position, finishLine.position);

        // Check if player reached the finish line
        if (distanceToFinish < 1.5f) // You can adjust the radius
        {
            Debug.Log("Player reached the finish line!");
            gameEnded = true;
            // You can show a win screen here instead of reloading
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Time is up and player didn’t reach the goal
        if (timer <= 0f)
        {
            Debug.Log("Game Over – Time’s Up!");
            gameEnded = true;
            // Replace with your Game Over logic
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
