using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI bossHealthText;

    [Header("Game References")]
    public PlayerController player;
    public BossController boss;

    private bool gameStarted = false;

    void Start()
    {
        // Show start panel, hide others
        if (startPanel != null) startPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        // Pause the game
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (gameStarted)
        {
            UpdateHealthUI();
            CheckGameState();
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        if (startPanel != null) startPanel.SetActive(false);
        Time.timeScale = 1f; // Unpause
    }

    void UpdateHealthUI()
    {
        if (player != null && playerHealthText != null)
        {
            playerHealthText.text = "Player HP: " + player.GetHealth();
        }

        if (boss != null && bossHealthText != null)
        {
            bossHealthText.text = "Boss HP: " + boss.GetHealth();
        }
    }

    void CheckGameState()
    {
        // Check if player died
        if (player != null && player.GetHealth() <= 0)
        {
            GameOver();
        }

        // Check if boss died
        if (boss != null && boss.GetHealth() <= 0)
        {
            Victory();
        }
    }

    void GameOver()
    {
        gameStarted = false;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause
    }

    void Victory()
    {
        gameStarted = false;
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f; // Pause
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}