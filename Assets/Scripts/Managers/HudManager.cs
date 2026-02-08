using UnityEngine;
using UnityEngine.UI;  
using TMPro;           

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Containers")]
    [SerializeField] private GameObject energyContainer;
    [SerializeField] private GameObject enemyHealthContainer;
    [SerializeField] private GameObject enemyNameContainer;

    [Header("Player UI")]
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Text energyText;
    [SerializeField] private Slider energyBar;

    [Header("Enemy UI")]
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private Text enemyHealthText;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    void Awake()
    {
        // --- SINGLETON LOGIC START ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the HUD (and this script) alive
        }
        else
        {
            Destroy(gameObject); // Destroys duplicate HUDs if you reload the scene
        }
        // --- SINGLETON LOGIC END ---
    }

    public void ToggleBattleUI(bool isActive)
    {
        if (energyContainer != null) energyContainer.SetActive(isActive);
        if (enemyHealthContainer != null) enemyHealthContainer.SetActive(isActive);
        if (enemyNameContainer != null) enemyNameContainer.SetActive(isActive);
    }

    public void SetEnemyName(string name)
    {
        if (enemyNameText != null) enemyNameText.text = name;
    }

    public void UpdatePlayerHealth(int current, int max)
    {
        if (playerHealthBar != null) { playerHealthBar.maxValue = max; playerHealthBar.value = current; }
        if (playerHealthText != null) { playerHealthText.text = current + " / " + max; }
    }

    public void UpdateEnergy(int current, int max)
    {
        if (energyBar != null) { energyBar.maxValue = max; energyBar.value = current; }
        if (energyText != null) { energyText.text = current + " / " + max; }
    }

    public void SetEnemyMaxHealth(int max)
    {
        if (enemyHealthBar != null) { enemyHealthBar.maxValue = max; enemyHealthBar.value = max; }
        UpdateEnemyUI(max, max);
    }

    public void UpdateEnemyHealth(int current)
    {
        float max = (enemyHealthBar != null) ? enemyHealthBar.maxValue : 100;
        UpdateEnemyUI(current, (int)max);
    }

    private void UpdateEnemyUI(int current, int max)
    {
        if (enemyHealthBar != null) enemyHealthBar.value = current;
        if (enemyHealthText != null) enemyHealthText.text = current + " / " + max;
    }
}