using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int FinalScore;

    int score;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint; 
    [SerializeField] private AudioClip bossMusic;
    private bool bossSpawned = false;

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

        if (!bossSpawned && score >= 100)
        {
            bossSpawned = true;
            SpawnBoss();
       
        }
    }
    void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            BossBehaviour bossScript = boss.GetComponent<BossBehaviour>();
            if (bossScript != null)
            {
                bossScript.StartBossSequence();
            }

         
            if (bossMusic != null)
            {
                AudioManager.Instance.PlayMusic(bossMusic, 1f);
            }
        }
    }
}
