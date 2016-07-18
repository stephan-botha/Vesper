using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public int maxHealth = 100;
    public int maxEnergy = 100;
    public int echoCost = 10;
    public int collisionCost = 5;

    public int currentHealth;
    public int currentEnergy;
    
    public int startLevel = 0;        
    public int currentLevel = 0;

    public static GameManager instance = null;

    private LevelManager levelManager;

    public delegate void EnergyChange(float energyIndicator);
    public static event EnergyChange OnEnergyChange;

    public delegate void HealthChange(float healthIndicator);
    public static event HealthChange OnHealthChange;

    void OnEnable()
    {
        EventManager.OnFireButton += DecreaseEnergy;
        EventManager.OnTerrainObjectCollision += DecreaseHealth;
    }

    void OnDisable()
    {
        EventManager.OnFireButton -= DecreaseEnergy;
        EventManager.OnTerrainObjectCollision -= DecreaseHealth;
    }

    void Awake()
    {      
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        levelManager = GetComponent<LevelManager>();
        InitializeGame();

        Debug.Log("Game Initialized");
      
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        currentHealth = maxHealth;
    }

    void InitializeGame()
    {
        levelManager.InitializeLevel(currentLevel);
    }

    void DecreaseEnergy()
    {
        currentEnergy -= echoCost;
        if (OnEnergyChange != null)
        {
            OnEnergyChange( (float)currentEnergy / maxEnergy );
        }
      
    }

    void DecreaseHealth()
    {
        currentHealth -= collisionCost;
        if (OnHealthChange != null)
        {
            OnHealthChange((float)currentHealth / maxHealth);
        }
    }
}

