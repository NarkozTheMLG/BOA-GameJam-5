using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Player UI")]
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Text energyText;
    [SerializeField] private Slider energyBar;

    [Header("Enemy UI")]
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private Text enemyHealthText;

    private int playerCurrentHealth = 100;
    private int playerMaxHealth = 100;
    private int currentEnergy = 5;
    private int maxEnergy = 5;

    private int enemyCurrentHealth;
    private int enemyMaxHealth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void UpdatePlayerHealth(int current, int max)
    {
        playerCurrentHealth = current;
        playerMaxHealth = max;

        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = playerMaxHealth;
            playerHealthBar.value = playerCurrentHealth;
        }
        if (playerHealthText != null)
        {
            playerHealthText.text = playerCurrentHealth + " / " + playerMaxHealth;
        }
    }

    public void UpdateEnergy(int current, int max)
    {
        currentEnergy = current;
        maxEnergy = max;

        if (energyBar != null)
        {
            energyBar.maxValue = maxEnergy;
            energyBar.value = currentEnergy;
        }
        if (energyText != null)
        {
            energyText.text = currentEnergy + " / " + maxEnergy;
        }
    }


    public void SetEnemyMaxHealth(int max)
    {
        enemyMaxHealth = max;
        enemyCurrentHealth = max; 

        if (enemyHealthBar != null)
        {
            enemyHealthBar.maxValue = enemyMaxHealth;
            enemyHealthBar.value = enemyCurrentHealth;
        }
        UpdateEnemyUI();
    }

    public void UpdateEnemyHealth(int current)
    {
        enemyCurrentHealth = current;
        UpdateEnemyUI();
    }

    private void UpdateEnemyUI()
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = enemyCurrentHealth;
        }
        if (enemyHealthText != null)
        {
            enemyHealthText.text = enemyCurrentHealth + " / " + enemyMaxHealth;
        }
    }
}