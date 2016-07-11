using UnityEngine;
using System.Collections;

public class PlayerAudioManager : MonoBehaviour {

    [SerializeField] private AudioClip echoSound;
    [SerializeField] private AudioClip insectMunch;

    AudioSource batAudio;

    void Awake()
    {
        batAudio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayAudioEcho()
    {
        batAudio.clip = echoSound;
        batAudio.Play();
    }

    public void PlayAudioMunch()
    {
        batAudio.clip = insectMunch;
        batAudio.Play();
    }
}

