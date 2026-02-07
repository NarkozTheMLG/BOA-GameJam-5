using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public static HUDManager Instance { get; private set; }

    [Header("Health")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text energyText;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider energyBar;



    private int currentHealth = 100;
    private int maxHealth = 100;
    private int currentEnergy = 5;
    private int maxEnergy = 5;


    void Awake()
    {
       

        Debug.Log("HUD Awake çalýþtý: " + gameObject.name);

        if (Instance == null)
        {
            Debug.Log("HUD Singleton atandý");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("HUD ZATEN VAR, bu siliniyor");
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (energyBar != null)
        {
            energyBar.maxValue = maxEnergy;
            energyBar.value = currentEnergy;
        }
        UpdateBars();


    }

    public void UpdateEnergy(int newEnergy)
    {
        currentEnergy = Mathf.Clamp(newEnergy, 0, maxEnergy);
        UpdateBars();

    }

    public void UpdateHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        UpdateBars();

    }

    public void getEnergy(int amount)
    {
        currentEnergy += amount;
        UpdateEnergy(currentEnergy);
    }
    public void lostEnergy(int amount)
    {
        currentEnergy -= amount;
        UpdateEnergy(currentEnergy);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealth(currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        UpdateHealth(currentHealth);
    }

 

 

    private void UpdateBars()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        if(energyText != null)
        {
            energyText.text = currentEnergy + " / " + maxEnergy;
        }
        if (energyBar != null)
        {
            energyBar.value = currentEnergy;
        }
    }

   

    // Test için (silebilirsiniz)
    void Update()
    {
        // K tuþu ile hasar test
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }

        // L tuþu ile can test
        if (Input.GetKeyDown(KeyCode.L))
        {
            Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            lostEnergy(1);
        }

        // L tuþu ile can test
        if (Input.GetKeyDown(KeyCode.O))
        {
            getEnergy(1);
        }


    }
}