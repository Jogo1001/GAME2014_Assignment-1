using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int FinalScore;

    int score;
    [SerializeField] TextMeshProUGUI scoreText;

    public void ChangeScene(int sceneIndex)
    {
 
        FinalScore = score;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeScore(int ScoreChangeAmount)
    {
        score += ScoreChangeAmount;
        FinalScore = score; 
        string scoreMessage = "Score: " + score;
        scoreText.text = scoreMessage;
    }
}
