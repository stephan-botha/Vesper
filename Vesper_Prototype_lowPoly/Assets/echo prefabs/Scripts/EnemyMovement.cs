using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public Transform startPosition;
    public Transform endPosition;
    public float speed = 10.0f;
    AudioSource insectAudio;
    

    private Rigidbody rb;
    Vector3 direction;
    Transform destination;

    //bool isMunching = false;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        insectAudio = GetComponent<AudioSource>();
    }

    void Start()
    {        
        SetDestination(startPosition);

    }

    void FixedUpdate()
    {
        //if (isMunching)
        //{
            
        //    rb.constraints = RigidbodyConstraints.FreezeAll;
        //}
        //else
        {

            rb.MovePosition(this.transform.position + direction * speed * Time.fixedDeltaTime);

            if (Vector3.Distance(destination.position, this.transform.position) < 1.0f)
            {
                SetDestination(destination == startPosition ? endPosition : startPosition);
            }
        }
        
    }

    void SetDestination(Transform dest)
    {
        destination = dest;
        direction = (destination.position - this.transform.position).normalized;
        this.transform.right = -direction;
    }
    
    
}
