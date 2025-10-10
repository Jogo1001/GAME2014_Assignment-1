using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float invulnerableTime = 5f;
    [SerializeField] private float minX = -1.43f;
    [SerializeField] private float maxX = 1.43f;
    [SerializeField] private float targetY = 3.74f;

    [Header("Boss Stats")]
    [SerializeField] private int maxHealth = 500;
    private int currentHealth;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer bossSprite;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip Victory;

    private bool isInvulnerable = true;
    private Collider2D bossCollider;
    private Vector3 startPos;
    BulletManager bulletManager;
    private Coroutine flashRoutine;
 



    void Start()
    {
        startPos = transform.position;
        currentHealth = maxHealth;

        if (bossSprite == null)
            bossSprite = GetComponent<SpriteRenderer>();

        bossCollider = GetComponent<Collider2D>();
        bossCollider.enabled = false;

        bulletManager = FindObjectOfType<BulletManager>();
    }

    public void StartBossSequence()
    {
        StartCoroutine(BossSequence());
    }

    private IEnumerator BossSequence()
    {

       
        Vector3 targetPos = new Vector3(transform.position.x, targetY, transform.position.z);
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;
        bossCollider.enabled = true; 

     
        StartCoroutine(HorizontalMovement());
    }

    private IEnumerator HorizontalMovement()
    {
        Vector3 left = new Vector3(minX, transform.position.y, transform.position.z);
        Vector3 right = new Vector3(maxX, transform.position.y, transform.position.z);

        while (true)
        {
        
            while (Vector3.Distance(transform.position, left) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, left, moveSpeed * Time.deltaTime);
                yield return null;
            }

      
            while (Vector3.Distance(transform.position, right) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, right, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvulnerable && collision.CompareTag("Bullet"))
        {
            TakeDamage(3);

            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
                bossSprite.color = Color.white; 
            }

            flashRoutine = StartCoroutine(FlashSprite());
            bulletManager.ReturnBullet(collision.gameObject);



        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            bossCollider.enabled = false;
            StartCoroutine(Die());
        }
     
       
    }

    private IEnumerator Die()
    {

        if (explosionClip != null)
            AudioManager.Instance.PlayExplosion(explosionClip, 3f);
        

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
        }

      
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("WinScene");
        AudioManager.Instance.PlayMusic(Victory, 1f);
        AudioManager.Instance.sfxSource.Stop();
        Destroy(gameObject);
    }

    private IEnumerator FlashSprite()
    {
        Color originalColor = bossSprite.color;
        Color flashColor = hitColor;

        float elapsed = 0f;
        float totalFlashTime = 0.3f;

        while (elapsed < totalFlashTime)
        {
           
            float lerp = Mathf.PingPong(Time.time * 10f, 1f);
            bossSprite.color = Color.Lerp(originalColor, flashColor, lerp);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bossSprite.color = originalColor; 
        flashRoutine = null;
    }
    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}
