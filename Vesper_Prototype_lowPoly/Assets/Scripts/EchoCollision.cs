using UnityEngine;
using System.Collections;

public class EchoCollision : MonoBehaviour {

    // public Texture echoTexture;
    

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Insect"))
        {
            InsectTextureManager insectTextureManager = other.GetComponent<InsectTextureManager>();

            insectTextureManager.ActivateEchoTexture();            

        }
    }

}
