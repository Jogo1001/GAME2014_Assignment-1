using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] Boundary verticalBoundary;
    float speed;
    bool isCollected = false;

    [SerializeField] GameObject pickupEffectPrefab; // The other GameObject with animation

    public void Initialize(Boundary boundary, float moveSpeed)
    {
        verticalBoundary = boundary;
        speed = moveSpeed;
    }

    void Update()
    {
        if (isCollected)
            return;

 
        transform.Translate(Vector3.down * speed * Time.deltaTime);

      
        if (transform.position.y < verticalBoundary.min)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            FindObjectOfType<GameController>().ChangeScore(10);

          
            if (pickupEffectPrefab != null)
            {
                GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                Animator effectAnim = effect.GetComponent<Animator>();

            
                if (effectAnim != null && effectAnim.runtimeAnimatorController != null)
                {
                    float animLength = GetAnimationClipLength(effectAnim, "CollectEffect");
                    Destroy(effect, animLength);
                }
                else
                {
                    Destroy(effect, 1f);
                }
            }

          
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 0.2f);
        }
    }

    float GetAnimationClipLength(Animator animator, string clipName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 1f; 
    }
}
