using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerResourceManager : MonoBehaviour
{

    public int maxHealth = 100;
    public int maxEnergy = 100;
    public int echoCost = 10;

    int currentHealth;
    int currentEnergy;

    Image energyIndicator;

    void OnEnable()
    {

        EventManager.OnFireButton += DecreaseEnergy;
    }

    void OnDisable()
    {

        EventManager.OnFireButton -= DecreaseEnergy;
    }

    void Awake()
    {
        energyIndicator = GameObject.FindGameObjectWithTag("Energy").GetComponentInChildren<Image>();
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        currentHealth = maxHealth;
    }

    void DecreaseEnergy()
    {
        currentEnergy -= echoCost;
        energyIndicator.fillAmount = (float)currentEnergy / maxEnergy;
    }
}

