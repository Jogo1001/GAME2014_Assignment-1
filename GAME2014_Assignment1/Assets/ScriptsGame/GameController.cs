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
    private void FixedUpdate()
    {
        if (!bossSpawned && score >= 300)
        {
            bossSpawned = true;
            SpawnBoss();

        }
    }
    public void ChangeScore(int ScoreChangeAmount)
    {
        score += ScoreChangeAmount;
        FinalScore = score; 
        string scoreMessage = "Score: " + score;
        scoreText.text = scoreMessage;

  
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
