using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    int score;
    [SerializeField]
    TextMeshProUGUI scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Debug.Log("test");
    }

    public void ChangeScore(int ScoreChangeAmount)
    {
        score += ScoreChangeAmount;
        string scoreMessage = "Score: " + score;
        scoreText.text = scoreMessage;
    }
}
