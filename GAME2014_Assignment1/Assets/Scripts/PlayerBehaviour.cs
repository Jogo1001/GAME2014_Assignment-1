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

    bool isShooting = false;
    void Start()
    {
        MoveInput = _playerController.FindAction("Move");
        bulletManager = FindObjectOfType<BulletManager>();
        gamecontroller = FindObjectOfType<GameController>();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        camera = Camera.main;
  
    }
    private void Update()
    {

       
    
        TouchScreenMove();
        CheckBoundaries();
        HandleShootingState();
    
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
        }
        else
        {
            
            isShooting = false;
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

        // Convert to world position
        Vector3 targetPos = camera.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, Mathf.Abs(camera.transform.position.z)));

        // Keep current Z
        targetPos.z = transform.position.z;

        // Move towards target
        float step = speed * Time.deltaTime;
        Vector3 previousPos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        // Clamp to boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, HorizontalBoundary.min, HorizontalBoundary.max),
            Mathf.Clamp(transform.position.y, VerticalBoundary.min, VerticalBoundary.max),
            transform.position.z
        );

        // --- Rotate around Z-axis based on horizontal movement ---
        float deltaX = transform.position.x - previousPos.x; // horizontal movement
        float zRotation = Mathf.Clamp(-deltaX * 200f, -22f, 22f); // scale deltaX to reach ±22°

        // Smooth rotation (increase Lerp factor for faster response)
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
            // reduce health
            collision.GetComponent<Enemy>().DestroyingSequence();
        }

    }

}
