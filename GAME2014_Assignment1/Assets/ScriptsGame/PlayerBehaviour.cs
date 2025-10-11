using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBehaviour : MonoBehaviour
{
    InputAction MoveInput;
    public Vector2 Direction;
    public Vector2 Destination;
    Camera camera;
    BulletManager bulletManager;
    GameController gamecontroller;
    GameObject bulletPrefab;


    [SerializeField]
    InputActionAsset _playerController;

    [SerializeField]
    Boundary VerticalBoundary;

    [SerializeField]
    Boundary HorizontalBoundary;

    [SerializeField]
    public float speed;

    [SerializeField]
    float shootingSpeed;

    [Header("Respawn Settings")]
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] SpriteRenderer playerSprite;

    [Header("Audio")]
    [SerializeField] AudioClip helicopterClip;
    [SerializeField] AudioClip explosionClip;
    private bool isHeliPlaying = false;

    bool isShooting = false;
    bool isRespawning = false;
    private Collider2D playerCollider;
    void Start()
    {

        MoveInput = _playerController.FindAction("Move");
        bulletManager = FindObjectOfType<BulletManager>();
        gamecontroller = FindObjectOfType<GameController>();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        camera = Camera.main;

        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();

        playerCollider = GetComponent<Collider2D>();

    }
    private void Update()
    {
        if (!isRespawning)
        {
            TouchScreenMove();
            HandleShootingState();
        }

        CheckBoundaries();

    }
    void HandleShootingState()
    {

        Vector2 moveValue = MoveInput.ReadValue<Vector2>();

        if (moveValue.magnitude > 0.05f)
        {

            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootingRoutine());
            }
            if (!isHeliPlaying && helicopterClip != null)
            {
                AudioManager.Instance.PlayHelicopter(helicopterClip, 0.3f);
                isHeliPlaying = true;
            }
        }
        else
        {

            isShooting = false;
            if (isHeliPlaying)
            {
                AudioManager.Instance.StopSFX();
                isHeliPlaying = false;
            }
        }

    }
    IEnumerator ShootingRoutine()
    {
        while (isShooting)
        {
            bulletManager.GetBullet().transform.position = transform.position;
            yield return new WaitForSeconds(shootingSpeed);
        }
    }

    void TouchScreenMove()
    {
        Vector2 pointerPos = MoveInput.ReadValue<Vector2>();


        Vector3 targetPos = camera.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, Mathf.Abs(camera.transform.position.z)));


        targetPos.z = transform.position.z;


        float step = speed * Time.deltaTime;
        Vector3 previousPos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);


        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, HorizontalBoundary.min, HorizontalBoundary.max),
            Mathf.Clamp(transform.position.y, VerticalBoundary.min, VerticalBoundary.max),
            transform.position.z
        );


        float deltaX = transform.position.x - previousPos.x;
        float zRotation = Mathf.Clamp(-deltaX * 200f, -22f, 22f);


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, zRotation), 0.5f);


    }
    public void CheckBoundaries()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, HorizontalBoundary.min, HorizontalBoundary.max),
                                          Mathf.Clamp(transform.position.y, VerticalBoundary.min, VerticalBoundary.max),
                                          transform.position.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("You Got Hit!");
            FindObjectOfType<PlayerHealthUI>().TakeDamage(1);

            if (explosionClip != null)
                AudioManager.Instance.PlayExplosion(explosionClip, 1f);

            playerSprite.enabled = false;
            playerCollider.enabled = false;
             isShooting = false;
            isRespawning = true; 

         
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }

            collision.GetComponent<Enemy>().DestroyingSequence();

            StartCoroutine(PlayerRespawn());
        }
        else if(collision.CompareTag("BossBullet") || collision.CompareTag("EnemyBullet"))
        {
            FindObjectOfType<PlayerHealthUI>().TakeDamage(1);

            if (explosionClip != null)
                AudioManager.Instance.PlayExplosion(explosionClip, 1f);
            playerSprite.enabled = false;
            playerCollider.enabled = false;
            isShooting = false;
            isRespawning = true;
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);

            }
            StartCoroutine(PlayerRespawn());
        }

    }
    IEnumerator PlayerRespawn()
    {
       
        yield return new WaitForSeconds(1f);

   
        transform.position = spawnPosition.position;

    
        playerSprite.enabled = true;
    

        yield return StartCoroutine(FlashPlayerSprite());
     

        playerCollider.enabled = true;
        isShooting = false;
        isRespawning = false;

    }

    IEnumerator FlashPlayerSprite()
    {
        Color originalColor = playerSprite.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        float flashDuration = 2.0f;
        float flashSpeed = 0.1f;
        float elapsed = 0f;

       
        while (elapsed < flashDuration)
        {
            playerSprite.color = (playerSprite.color.a > 0.5f) ? transparentColor : originalColor;
            elapsed += flashSpeed;
            yield return new WaitForSeconds(flashSpeed);
           
        }

       
        playerSprite.color = originalColor;
     
    }
}
