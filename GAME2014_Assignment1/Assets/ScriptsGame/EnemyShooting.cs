using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;

    [Header("Spawn Offset")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, -0.5f, 0);

    [Header("Shoot Activation Y Position")]
    [SerializeField] private float shootActivationY = 4.28f; 

    private bool hasShot = false;
    private Enemy enemy;

    void Start()
    {
        if (projectilePrefab == null)
        {
         
            enabled = false;
            return;
        }

        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
           
            enabled = false;
        }
    }

    void Update()
    {
        
        if (!hasShot && transform.position.y <= shootActivationY && enemy != null && !enemy.GetIsDying())
        {
            FireProjectile();
            hasShot = true;
        }
    }
    public void ResetShot()
    {
        hasShot = false;
    }
    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + spawnOffset, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * projectileSpeed;
        }

        Destroy(projectile, 6f);
    }

}
