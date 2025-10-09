using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    Boundary speedRange;
    float speed;

    [SerializeField]
    Boundary screenHorizontalBoundary;

    [SerializeField]
    Boundary screenVerticalBoundary;

    bool IsDying = false;

    BulletManager bulletManager;
    GameController gameController;

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
        gameController = FindObjectOfType<GameController>();
        speed = Random.Range(speedRange.min, speedRange.max);
        Reset();

    }


    void Update()
    {
        DetectPlayer();
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        float xPos = Mathf.PingPong(Time.time * speed, screenHorizontalBoundary.max - screenHorizontalBoundary.min) + screenHorizontalBoundary.min;
        transform.position = new Vector3(xPos, transform.position.y - speed * Time.deltaTime);

        if (screenVerticalBoundary.min > transform.position.y)
        {

            Reset();

        }
     

    }
    private void FixedUpdate()
    {
        if (IsDying)
        {
            transform.Rotate(0, 0, 5);
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x - 0.05f, 0, 1),
                                                Mathf.Clamp(transform.localScale.y - 0.05f, 0, 1), 1);
        }
    }
    public void DestroyingSequence()
    {



        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        IsDying = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            DestroyingSequence();
            bulletManager.ReturnBullet(collision.gameObject);
            gameController.ChangeScore(5);
        }
    }
    private void Reset()
    {
        transform.position = new Vector3(Random.Range(screenHorizontalBoundary.min, screenHorizontalBoundary.max),
                                                        screenVerticalBoundary.max, transform.position.z);
        speed = Random.Range(speedRange.min, speedRange.max);

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        IsDying = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}
