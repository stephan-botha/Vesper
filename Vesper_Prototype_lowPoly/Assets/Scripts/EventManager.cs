using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
       
    public OVRInput.Button fireButton = OVRInput.Button.One;
    public OVRInput.Button hUDButton = OVRInput.Button.Three;

    public delegate void HUDButton();
    public static event HUDButton OnHUDButton;
    public delegate void FireButton();
    public static event FireButton OnFireButton;
    public delegate void TerrainObjectCollision();
    public static event TerrainObjectCollision OnTerrainObjectCollision;
        
    void Start()
    {
        
    }
        
    void Update()
    {   
        if (OVRInput.GetDown (fireButton))        
        {
            if (EnoughEnergy())
            {
                if (OnFireButton != null)
                {
                    OnFireButton();
                }
            }

            if (OnHUDButton != null)
            {
                OnHUDButton();
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

    void OnTriggerEnter(Collider other)
    {

        if (CheckForTerrainObjectCollision(other.gameObject) != null)
        {
            //Debug.Log("collision!!!");
            if (OnTerrainObjectCollision != null)
            {
                OnTerrainObjectCollision();
            }

        }
    }

    GameObject CheckForTerrainObjectCollision(GameObject other)
    {
        Transform t = other.transform;
        while (t.parent != null)
        {
            if (t.parent.CompareTag("TerrainObject"))
            {
                return t.parent.gameObject;
            }
            t = t.parent;
        }

        return null;
    }

    bool EnoughEnergy()
    {
        return GameManager.instance.currentEnergy >= GameManager.instance.echoCost;
    }

}
