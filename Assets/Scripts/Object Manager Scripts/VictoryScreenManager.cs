using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // If using TextMeshPro, otherwise use Text

public class VictoryScreenManager : MonoBehaviour
{
    public static VictoryScreenManager Instance { get; private set; }

    [SerializeField] private int pointsRequiredToWin = 100;
    [SerializeField] private GameObject victoryPanel; // UI Panel to show on victory
    [SerializeField] private TextMeshProUGUI pointsDisplay; // Display final points
    [SerializeField] private TextMeshProUGUI nextLevelButtonText;

    private bool hasWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    private void Update()
    {
        // Check if player has won
        if (!hasWon && PointsManager.Instance.currentPoints >= pointsRequiredToWin)
        {
            ShowVictoryScreen();
        }
    }

    private void ShowVictoryScreen()
    {
        hasWon = true;
        
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (pointsDisplay != null)
            pointsDisplay.text = $"Points: {PointsManager.Instance.currentPoints}";

        Time.timeScale = 0f; // Pause the game
        Debug.Log("Victory! You've reached the point goal!");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f; // Resume the game
        
        // Load next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            PointsManager.Instance.ResetPoints();
        }
        else
        {
            Debug.Log("No more levels!");
            // You could load a "Game Complete" screen here
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PointsManager.Instance.ResetPoints();
    }
}