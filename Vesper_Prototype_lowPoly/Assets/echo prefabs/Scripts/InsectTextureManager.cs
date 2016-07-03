using UnityEngine;
using System.Collections;

public class InsectTextureManager : MonoBehaviour {

    public float visibleTime = 3.0f;
    float visibilityTimer;
    bool isVisible, isPlayerClose;
    float textureScaleY = 10f;

    MeshRenderer[] enemyMesh;
    MeshRenderer[] echoRend;
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyMesh = this.GetComponentsInChildren<MeshRenderer>();
        echoRend = this.GetComponentsInChildren<MeshRenderer>();
        isVisible = false;
        isPlayerClose = false;
    }

    // Use this for initialization
    void Start () {
        visibilityTimer = visibleTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 5.0f)
        {
            isPlayerClose = true;
        }
        else
        {
            isPlayerClose = false;
        }

        if (isVisible || isPlayerClose)
        {
            if (visibilityTimer < visibleTime || isPlayerClose)
            {
                visibilityTimer += Time.deltaTime;

                for (int i = 0; i < enemyMesh.Length; i++)
                {
                    enemyMesh[i].enabled = true;

                    //Color c = echoRend[i].material.GetColor("_MainTex");                    
                    //echoRend[i].material.SetColor("_MainTex", c);                    

                    if (i==0)
                        echoRend[i].material.mainTextureScale = new Vector2(1, textureScaleY + (visibilityTimer * 3));
                    else
                        echoRend[i].material.mainTextureScale = new Vector2(textureScaleY/2 + (visibilityTimer ), 1);

                }
            }
            else
            {
                for (int i = 0; i < enemyMesh.Length; i++)
                {
                    enemyMesh[i].enabled = false;                   

                }
            }

        } 
	}


    public void ActivateEchoTexture()
    {
        isVisible = true;
        visibilityTimer = 0.0f;
        for (int i = 0; i < enemyMesh.Length; i++)
        {
            enemyMesh[i].enabled = true;
            MeshRenderer echoRend = this.GetComponentInChildren<MeshRenderer>();
            echoRend.material.mainTextureScale = new Vector2(1, 20);

        }
    }
}
