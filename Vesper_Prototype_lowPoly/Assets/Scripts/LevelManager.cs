using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class LevelManager : MonoBehaviour {

	
    public void InitializeLevel(int level)
    {               
        SceneManager.LoadScene(level);
        
    }

}
