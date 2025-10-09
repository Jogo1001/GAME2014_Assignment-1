using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBehaviour : MonoBehaviour
{
    InputAction MoveInput;
    public Vector2 Direction;
    public Vector2 Destination;
    Camera camera;



    [SerializeField]
    InputActionAsset _playerController;

    [SerializeField]
    Boundary VerticalBoundary;

    [SerializeField]
    Boundary HorizontalBoundary;

    [SerializeField]
    public float speed;
   
    void Start()
    {
        MoveInput = _playerController.FindAction("Move");
        camera = Camera.main;
    }
    private void Update()
    {

        TouchScreenMove();
       
        CheckBoundaries();
    }

   
    void TraditionalMove() 
    {
        Direction = MoveInput.ReadValue<Vector2>();
        Debug.Log(Direction);
        transform.position = new Vector3(transform.position.x + Direction.x * speed * Time.deltaTime
                                         , transform.position.y + Direction.y * speed * Time.deltaTime
                                         , transform.position.z);



    }

    void TouchScreenMove() 
    {
  
        Destination = camera.ScreenToWorldPoint(MoveInput.ReadValue<Vector2>());
        transform.position = Vector3.Lerp(transform.position, Destination, speed * Time.deltaTime);
       

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
        }

    }

}
