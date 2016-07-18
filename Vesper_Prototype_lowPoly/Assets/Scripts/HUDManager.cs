using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {

    Image energyIndicator;
    Image healthIndicator;

    void OnEnable()
    {
        EventManager.OnHUDButton += DisplayEnergy;        

        EventManager.OnHUDButton += DisplayHealth;
        EventManager.OnTerrainObjectCollision += DisplayHealth;

        GameManager.OnEnergyChange += AdjustEnergyIndicator;
    }

    void OnDisable()
    {
        EventManager.OnHUDButton -= DisplayEnergy;
        EventManager.OnHUDButton -= DisplayHealth;
        
        EventManager.OnTerrainObjectCollision -= DisplayHealth;

        GameManager.OnEnergyChange -= AdjustEnergyIndicator;
    }

    void Awake()
    {
        energyIndicator = GameObject.FindGameObjectWithTag("Energy").GetComponentInChildren<Image>();
        healthIndicator = GameObject.FindGameObjectWithTag("Health").GetComponentInChildren<Image>();
    }
     
    void AdjustEnergyIndicator(float newValue)
    {
        energyIndicator.fillAmount = newValue;
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

    void DisplayHealth()
    {
        healthIndicator.enabled = true;
        StartCoroutine(FlashHealth());
    }

    IEnumerator FlashHealth()
    {
        yield return new WaitForSeconds(3.0f);
        healthIndicator.enabled = false;
    }
}
