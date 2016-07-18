using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    PlayerAudioManager audioManager;

    void Awake()
    {
        audioManager = GetComponent<PlayerAudioManager>();
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Insect"))
        {

            Destroy(other.gameObject, 0.0f);
            audioManager.PlayAudioMunch();

        }
       
    }

   
}