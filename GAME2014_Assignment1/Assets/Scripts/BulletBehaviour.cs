using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    Boundary verticalboundry;

    Vector3 direction;

    BulletManager manager;

    void Start()
    {
        direction = transform.up;
        manager = FindObjectOfType<BulletManager>();
    }


    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        if (transform.position.y > verticalboundry.max)
        {
            manager.ReturnBullet(gameObject);
        }
    }
}
