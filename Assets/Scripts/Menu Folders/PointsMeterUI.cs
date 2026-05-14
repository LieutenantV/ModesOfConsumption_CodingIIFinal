using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsMeterUI : MonoBehaviour
{
    [SerializeField] private Image meterFill; 
    [SerializeField] private TextMeshProUGUI pointsText; // to display the current and max points
    [SerializeField] private int maxPoints = 100; // Will be the same as the points required to win

    private void Update()
    {
        if (PointsManager.Instance != null)
        {
            // Update the meter fill amount with every item grabbed
            float fillAmount = (float)PointsManager.Instance.currentPoints / maxPoints;
            meterFill.fillAmount = Mathf.Clamp01(fillAmount);

            // Updating the text display
            pointsText.text = $"{PointsManager.Instance.currentPoints} / {maxPoints}";
        }
    }
}