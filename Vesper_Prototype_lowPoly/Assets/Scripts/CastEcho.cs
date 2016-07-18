using UnityEngine;
using System.Collections;

public class CastEcho : MonoBehaviour
{
        
    public GameObject EchoWave;
    PlayerAudioManager audioManager;
    GameObject camera;
    
    void OnEnable()
    {
        EventManager.OnFireButton += InstantiateEcho;
    }

    void OnDisable()
    {
        EventManager.OnFireButton -= InstantiateEcho;
    }

    void Awake()
    {
        
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        audioManager = GetComponent<PlayerAudioManager>();
    }


    void InstantiateEcho()
    {
        //rotate echo object into camera direction
        Vector3 xAxis = new Vector3(1.0f, 0.0f, 0.0f);
        Quaternion rotation = Quaternion.AngleAxis(90.0f, xAxis);
        Transform cameraTransform = camera.transform;
        Instantiate(EchoWave, transform.position, cameraTransform.rotation * rotation);
        audioManager.PlayAudioEcho();
    }
}

