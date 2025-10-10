using System.Collections;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float invulnerableTime = 5f;
    [SerializeField] private float minX = -1.43f;
    [SerializeField] private float maxX = 1.43f;
    [SerializeField] private float targetY = 3.74f;

    private bool isInvulnerable = true;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public void StartBossSequence()
    {
        StartCoroutine(BossSequence());
    }

    private IEnumerator BossSequence()
    {
       
        Vector3 targetPos = new Vector3(transform.position.x, targetY, transform.position.z);
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

    
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;

        
        StartCoroutine(HorizontalMovement());
    }

    private IEnumerator HorizontalMovement()
    {
        Vector3 left = new Vector3(minX, transform.position.y, transform.position.z);
        Vector3 right = new Vector3(maxX, transform.position.y, transform.position.z);

        while (true)
        {
        
            while (Vector3.Distance(transform.position, left) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, left, moveSpeed * Time.deltaTime);
                yield return null;
            }

      
            while (Vector3.Distance(transform.position, right) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, right, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}
