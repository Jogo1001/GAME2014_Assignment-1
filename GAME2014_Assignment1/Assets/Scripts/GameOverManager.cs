using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] private AudioClip Music;

    void Start()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + GameController.FinalScore;
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
        if (Music != null)
        {
            AudioManager.Instance.PlayMusic(Music, 0.6f);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        if (Music != null)
        {
            AudioManager.Instance.PlayMusic(Music, 0.6f);
        }
    }
}
