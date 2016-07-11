using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {

    Image energyIndicator;

    void OnEnable()
    {
        EventManager.OnHUDButton += DisplayEnergy;
        EventManager.OnFireButton += DisplayEnergy;
    }

    void OnDisable()
    {
        EventManager.OnHUDButton -= DisplayEnergy;
        EventManager.OnFireButton -= DisplayEnergy;
    }

    void Awake()
    {
        energyIndicator = GameObject.FindGameObjectWithTag("Energy").GetComponentInChildren<Image>();
    }
     
    void DisplayEnergy()
    {
        energyIndicator.enabled = true;
        StartCoroutine(FlashEnergy());
    }

    IEnumerator FlashEnergy()
    {
        yield return new WaitForSeconds(3.0f);
        energyIndicator.enabled = false;
    }
}
