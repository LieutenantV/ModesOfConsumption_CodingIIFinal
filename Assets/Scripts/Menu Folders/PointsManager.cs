using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance { get; private set; }

    public int currentPoints = 0;
    public int pointsPerFood = 10; // Points gained per food pickup

    private void Awake()
    {
        // Makes sures theres only one point manager at a time
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persists between scene loads
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
        Debug.Log($"Points: {currentPoints}");
    }

    public void ResetPoints()
    {
        currentPoints = 0;
    }
}