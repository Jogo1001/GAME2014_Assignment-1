using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;

    void Start()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + GameController.FinalScore;
        }
    }
}
