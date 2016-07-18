using UnityEngine;
using System.Collections;

public class testcollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("OnTriggerEnter: Enemy");
        }
        else
        {
            Debug.Log("OnTriggerEnter: No enemy");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("OnCollisionEnter: Enemy");
        }
        else
        {
            Debug.Log("OnCollisionEnter: No enemy");
        }
        Debug.Log("OnCollisionEnter: xxx");
    }
}
