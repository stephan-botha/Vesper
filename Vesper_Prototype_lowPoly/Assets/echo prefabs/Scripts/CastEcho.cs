using UnityEngine;
using System.Collections;

public class CastEcho : MonoBehaviour
{
        
    public GameObject EchoWave;    
    [SerializeField] private AudioClip echoSound;
    AudioSource batAudio;
    GameObject camera;

    float timer;


    void Awake()
    {
        batAudio = GetComponent<AudioSource>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") )
        {
            //rotate echo object into camera direction
            Vector3 xAxis = new Vector3(1.0f, 0.0f, 0.0f);            
            Quaternion rotation = Quaternion.AngleAxis(90.0f, xAxis);
            Transform cameraTransform = camera.transform;
            Instantiate( EchoWave, transform.position, cameraTransform.rotation * rotation );
            batAudio.clip = echoSound;
            batAudio.Play();
            timer = 0f;
        }

    }
}

