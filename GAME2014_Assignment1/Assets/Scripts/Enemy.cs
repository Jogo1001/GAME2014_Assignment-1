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



    void Start()
    {
        speed = Random.Range(speedRange.min, speedRange.max);
        Reset();

    }


    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        float xPos = Mathf.PingPong(Time.time * speed, screenHorizontalBoundary.max - screenHorizontalBoundary.min) + screenHorizontalBoundary.min;
        transform.position = new Vector3(xPos, transform.position.y - speed * Time.deltaTime);

        if (screenVerticalBoundary.min > transform.position.y)
        {

            Reset();

        }


    }
    private void Reset()
    {
        transform.position = new Vector3(Random.Range(screenHorizontalBoundary.min, screenHorizontalBoundary.max),
                                                        screenVerticalBoundary.max, transform.position.z);
        speed = Random.Range(speedRange.min, speedRange.max);
    }

}
