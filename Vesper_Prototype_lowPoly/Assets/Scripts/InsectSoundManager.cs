using UnityEngine;
using System.Collections;

public class InsectSoundManager : MonoBehaviour {

    public float soundIntervals;

    AudioSource insectAudio;
    public bool isPlayerInRange;
    bool isInsectBuzzing = false;
    float soundTimer = 0.0f;
    GameObject[] enemies;

    void Awake()
    {
        insectAudio = GetComponentInParent<AudioSource>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

    }

	// Use this for initialization
	void Start () {
        isPlayerInRange = false;
	}
	
	// Update is called once per frame
	void Update () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (isInsectBuzzing)
        {
            isInsectBuzzing = false;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].GetComponentInParent<AudioSource>().isPlaying == true)
                {
                    isInsectBuzzing = true;
                    break;
                }
            }
        }

        if (isPlayerInRange && !isInsectBuzzing)
        {
            soundTimer += Time.deltaTime;
          //  if (soundTimer> soundIntervals)
            {
                if (insectAudio.isPlaying == false)
                    insectAudio.Play();                
                soundTimer = 0.0f;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                //if (enemies[i].GetComponentInParent<AudioSource>().isPlaying == true)
                //{
                //    isInsectBuzzing = true;
                //    break;
                //}
            }

            
                isPlayerInRange = true;
                soundTimer = 0.0f;
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsectBuzzing = false;
            isPlayerInRange = false;
            insectAudio.Stop();
        }
    }

}
