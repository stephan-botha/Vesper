using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

    public GameObject EchoWave;

    public OVRInput.Button fireButton = OVRInput.Button.One;
    public OVRInput.Button hUDButton = OVRInput.Button.Three;

    public delegate void HUDButton();
    public static event HUDButton OnHUDButton;
    public delegate void FireButton();
    public static event FireButton OnFireButton;

    PlayerAudioManager audioManager;
    GameObject camera;

    float timer;


    void Awake()
    {

        camera = GameObject.FindGameObjectWithTag("MainCamera");
        audioManager = GetComponent<PlayerAudioManager>();
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (OVRInput.GetDown (fireButton))        
        {
            //rotate echo object into camera direction
            Vector3 xAxis = new Vector3(1.0f, 0.0f, 0.0f);
            Quaternion rotation = Quaternion.AngleAxis(90.0f, xAxis);
            Transform cameraTransform = camera.transform;
            Instantiate(EchoWave, transform.position, cameraTransform.rotation * rotation);

            audioManager.PlayAudioEcho();
            timer = 0f;

            if (OnFireButton != null)
            {
                OnFireButton();
            }

        }
        else if (OVRInput.Get(hUDButton))
        {
            if (OnHUDButton != null)
            {
                OnHUDButton();
            }
        }

    }
}
