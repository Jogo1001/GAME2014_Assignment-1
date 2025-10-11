using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField]
    Boundary verticalBoundary;
    [SerializeField]
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);

        if (transform.position.y < verticalBoundary.min)
        {
            transform.position = new Vector3(transform.position.x, verticalBoundary.max, transform.position.z);
        }
    }
}
