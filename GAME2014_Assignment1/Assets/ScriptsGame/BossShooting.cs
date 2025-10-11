using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootInterval = 5f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] Transform bulletspawn;

   

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
        GameObject projectile = Instantiate(projectilePrefab,bulletspawn.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * projectileSpeed;
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
