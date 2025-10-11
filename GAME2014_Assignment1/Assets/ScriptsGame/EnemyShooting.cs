using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootInterval = 5f;
    [SerializeField] private float projectileSpeed = 5f;

    [Header("Spawn Offset")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, -0.5f, 0); 

    private bool canShoot = true;

    void Start()
    {
        if (projectilePrefab == null)
        {
            enabled = false;
            return;
        }

        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (canShoot)
        {
            yield return new WaitForSeconds(shootInterval);
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position + spawnOffset, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = Vector2.down * projectileSpeed;
        }
        else
        {
          
            projectile.transform.Translate(Vector3.down * projectileSpeed * Time.deltaTime);
        }

        Destroy(projectile, 6f);
    }

    private void OnDisable()
    {
        canShoot = false;
        StopAllCoroutines();
    }
}
