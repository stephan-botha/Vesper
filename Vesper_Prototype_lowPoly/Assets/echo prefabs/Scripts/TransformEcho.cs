using UnityEngine;
using System.Collections;

public class TransformEcho : MonoBehaviour {

    public float echoRange = 200f;
    public float echoSpeed = 10f;
    public float scaleFactor = 1.0f;
    
    CastEcho playerShooting;
    GameObject camera;

    float timer = 0.0f;
    Vector3 startEchoMarker;
    Vector3 targetEchoMarker;
    float echoTravelDistance;

//-------------------------------------------------------------------------------------------------------------

    void Awake()
    {
       playerShooting = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CastEcho>();
       camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

//-------------------------------------------------------------------------------------------------------------

    void Start()
    {        
       // this.transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);
        transform.position = playerShooting.transform.position;
        startEchoMarker = transform.position;
        targetEchoMarker = transform.position + camera.transform.forward * echoRange;
        echoTravelDistance = Vector3.Distance(startEchoMarker, targetEchoMarker);              
    }

//-------------------------------------------------------------------------------------------------------------
  
    void Update()
    {
                      
        timer += Time.deltaTime;

        float distanceMoved = timer * echoSpeed;
        float fractionMoved = distanceMoved / echoTravelDistance;

        this.transform.localScale += new Vector3( 
            fractionMoved * scaleFactor, 
            fractionMoved * scaleFactor / 5.0f, 
            fractionMoved * scaleFactor );

        transform.position = Vector3.Lerp(startEchoMarker, targetEchoMarker, fractionMoved);
        
        transform.Rotate(0f, -0.1f, 0f);

        if (timer > 4.0f || distanceMoved > echoTravelDistance)
        {
            DestroyEcho();
        }        
    }

//-------------------------------------------------------------------------------------------------------------

    public void DestroyEcho()
    {
        Destroy(gameObject, 0f);
    }

}
