using UnityEngine;             
using System.Collections;      
using UnityEngine.UI;          

public enum BattleState { NONE,START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [Header("UI & Player References")]
    public GameObject battleUI;
    public PlayerMovement playerMovement;
    public Transform playerTransform;
    public Transform enemyTransform;

    [Header("Current Battle Info")]
    public Sickness Enemy;
    public Sickness Schizophrenia;
    public Sickness Insomnia;
    public Sickness Lactose; 

    [Header("Attack Prefabs")]
    public GameObject schizophreniaPrefab;
    public GameObject insomniaPrefab;
    public GameObject lactosePrefab;

    [Header("Visual Effects")]
    public GameObject floatingTextPrefab;

    [Header("Inventory Buttons")]
    public Button schizophreniaButton;
    public Button insomniaButton;
    public Button lactoseButton;

    // --- CACHED IMAGES ---
    private Image schizoBtnImage;
    private Image insomniaBtnImage;
    private Image lactoseBtnImage;

    [Header("Setup")]
    public Transform playerHand;
    public BattleState state;
    private int playerCurrentEnergy;
    private int playerMaxEnergy = 5;
    private int playerCurrentHealth;
    private int playerMaxHealth = 100;
    private bool playerIsAsleep = false;
    private bool enemyIsAsleep = false;

    void Start()
    {
        state = BattleState.NONE;
        playerCurrentHealth = playerMaxHealth;
        // Cache Images
        if (schizophreniaButton != null) schizoBtnImage = schizophreniaButton.GetComponent<Image>();
        if (insomniaButton != null) insomniaBtnImage = insomniaButton.GetComponent<Image>();
        if (lactoseButton != null) lactoseBtnImage = lactoseButton.GetComponent<Image>();

        if (HUDManager.Instance != null) HUDManager.Instance.ToggleBattleUI(false);
    }

    public void StartBattle(Sickness enemyFromWorld, Transform enemyTransFromWorld)
    {
        Enemy = enemyFromWorld;
        enemyTransform = enemyTransFromWorld;

        // Reset Sleep State
        playerIsAsleep = false;
        enemyIsAsleep = false;

        playerMovement.enabled = false;
        
        battleUI.SetActive(true); 
        HUDManager.Instance.ToggleBattleUI(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SwitchToBattle();
        }

        if (Enemy != null) HUDManager.Instance.SetEnemyName(Enemy.sicknessName);

        HUDManager.Instance.UpdatePlayerHealth(playerCurrentHealth, playerMaxHealth);
        
        playerCurrentEnergy = playerMaxEnergy;
        HUDManager.Instance.UpdateEnergy(playerCurrentEnergy, playerMaxEnergy);
        
        HUDManager.Instance.SetEnemyMaxHealth(Enemy.maxHealth);
        HUDManager.Instance.UpdateEnemyHealth(Enemy.currentHealth);

        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        // Update buttons initially
        UpdateButtonStates();
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // --- NEW: CHECKS BOTH INVENTORY AND ENERGY ---
    void UpdateButtonStates()
    {
        // 1. Schizophrenia
        bool hasSchizoItem = InventoryManager.Instance.HasItem("Schizophrenia");
        bool hasSchizoEnergy = (Schizophrenia != null && playerCurrentEnergy >= Schizophrenia.energyCost);
        
        if (schizophreniaButton != null)
        {
            // Must have Item AND Energy to click
            bool interactable = hasSchizoItem && hasSchizoEnergy;
            schizophreniaButton.interactable = interactable;
            SetButtonAlpha(schizoBtnImage, interactable);
        }

        // 2. Insomnia
        bool hasInsomniaItem = InventoryManager.Instance.HasItem("Insomnia");
        bool hasInsomniaEnergy = (Insomnia != null && playerCurrentEnergy >= Insomnia.energyCost);
        
        if (insomniaButton != null)
        {
            bool interactable = hasInsomniaItem && hasInsomniaEnergy;
            insomniaButton.interactable = interactable;
            SetButtonAlpha(insomniaBtnImage, interactable);
        }

        // 3. Lactose
        bool hasLactoseItem = InventoryManager.Instance.HasItem("Lactose");
        bool hasLactoseEnergy = (Lactose != null && playerCurrentEnergy >= Lactose.energyCost);

        if (lactoseButton != null)
        {
            bool interactable = hasLactoseItem && hasLactoseEnergy;
            lactoseButton.interactable = interactable;
            SetButtonAlpha(lactoseBtnImage, interactable);
        }
    }

    void SetButtonAlpha(Image targetImage, bool isActive)
    {
        if (targetImage == null) return;
        Color c = targetImage.color;
        c.a = isActive ? 1f : 0.5f;
        targetImage.color = c;
    }

    void BattleWon()
    {
        if (state == BattleState.WON)
        {
            if (Enemy != null)
            {
                InventoryManager.Instance.AddItem(Enemy.sicknessName, 1);
                Destroy(Enemy.gameObject);
                Enemy = null;
            }
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SwitchToNormal();
            }
            enemyTransform = null;
            battleUI.SetActive(false);
            HUDManager.Instance.ToggleBattleUI(false);
            playerMovement.enabled = true;
        }
    }

    void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;

        if (playerIsAsleep)
        {
            Debug.Log("Zzz... You are asleep and skipped a turn!");
            battleUI.SetActive(false); 
            playerIsAsleep = false;
            StartCoroutine(EnemyTurn());
            return;
        }

        Debug.Log("Player's turn. Choose an action.");
        
        // Show UI and Refresh Buttons based on current Energy
        battleUI.SetActive(true); 
        UpdateButtonStates(); 
    }

    public void ExecuteMove(string moveName)
    {
        if (state != BattleState.PLAYERTURN) return;

        // Hide UI immediately so player can't click twice
        battleUI.SetActive(false);

        // --- SCHIZOPHRENIA CHECK ---
        if (moveName == "Schizophrenia")
        {
            // Extra safety check in case they somehow clicked it
            if (Schizophrenia != null && playerCurrentEnergy < Schizophrenia.energyCost) return;

            if (Random.value <= 0.20f)
            {
                Debug.Log("Schizo Attack Failed!");
                LoseEnergy(Schizophrenia.energyCost);
                StartCoroutine(ShowTextDelayed("What!?...", playerTransform, 0.5f));
                StartCoroutine(SkipTurnDelay(1f));
                return;
            }
        }
        // ---------------------------

        GameObject prefabToUse = null;
        int damageToDeal = 0;

        if (moveName == "Skip")
        {
            GainEnergy(3);
            StartCoroutine(EnemyTurn());
            return;
        }
        else if (moveName == "Defend")
        {
            GainEnergy(1);
            HealPlayer(5);
            StartCoroutine(EnemyTurn());
            return;
        }
        else if (moveName == "Schizophrenia")
        {
            if (Schizophrenia != null)
            {
                if (playerCurrentEnergy < Schizophrenia.energyCost) return;
                LoseEnergy(Schizophrenia.energyCost);
                damageToDeal = Schizophrenia.damage;
                prefabToUse = schizophreniaPrefab;
                state = BattleState.ENEMYTURN;
            }
        }
        else if (moveName == "Insomnia")
        {
            if (Insomnia != null)
            {
                if (playerCurrentEnergy < Insomnia.energyCost) return;
                LoseEnergy(Insomnia.energyCost);

                damageToDeal = Insomnia.damage;
                prefabToUse = insomniaPrefab;
                state = BattleState.ENEMYTURN;

                if (Random.value <= 0.25f)
                {
                    enemyIsAsleep = true;
                    StartCoroutine(ShowTextDelayed("Zzz...", Enemy.transform, 0.8f));
                }
            }
        }
        else if (moveName == "Lactose")
        {
            if (Lactose != null)
            {
                if (playerCurrentEnergy < Lactose.energyCost) return;
                LoseEnergy(Lactose.energyCost);

                damageToDeal = Lactose.damage;
                prefabToUse = lactosePrefab;
                state = BattleState.ENEMYTURN;
            }
        }

        if (prefabToUse != null)
        {
            GameObject projectile = Instantiate(prefabToUse, playerHand.position, Quaternion.identity);
            AttackAnimation anim = projectile.GetComponent<AttackAnimation>();
            if (anim != null) anim.Seek(Enemy.transform);
        }

        StartCoroutine(ProcessAttack(damageToDeal, 0.8f));
    }

    public void GainEnergy(int amount)
    {
        playerCurrentEnergy += amount;
        if (playerCurrentEnergy > playerMaxEnergy) playerCurrentEnergy = playerMaxEnergy;
        HUDManager.Instance.UpdateEnergy(playerCurrentEnergy, playerMaxEnergy);
        
        // Refresh buttons (in case we gained enough energy to use a move)
        UpdateButtonStates(); 
    }

    public void LoseEnergy(int amount)
    {
        playerCurrentEnergy -= amount;
        if (playerCurrentEnergy < 0) playerCurrentEnergy = 0;
        HUDManager.Instance.UpdateEnergy(playerCurrentEnergy, playerMaxEnergy);
        
        // Refresh buttons (buttons will dim if energy drops too low)
        UpdateButtonStates();
    }

    public void HealPlayer(int amount)
    {
        playerCurrentHealth += amount;
        if (playerCurrentHealth > playerMaxHealth) playerCurrentHealth = playerMaxHealth;
        HUDManager.Instance.UpdatePlayerHealth(playerCurrentHealth, playerMaxHealth);
    }

    IEnumerator ProcessAttack(int damage, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (Enemy != null)
        {
            Enemy.TakeDamage(damage);
            HUDManager.Instance.UpdateEnemyHealth(Enemy.currentHealth);

            if (Enemy.isDead)
            {
                state = BattleState.WON;
                BattleWon();
                yield break;
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        state = BattleState.ENEMYTURN;

        if (enemyIsAsleep)
        {
            Debug.Log("Enemy is sleeping...");
            yield return new WaitForSeconds(1f);
            enemyIsAsleep = false;
            PlayerTurn();
            yield break;
        }

        float waitTime = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(waitTime);

        // --- SCHIZO FAIL CHECK ---
        if (Enemy != null && Enemy.sicknessName == "Schizophrenia")
        {
            if (Random.value <= 0.20f)
            {
                Debug.Log("Enemy Schizo Missed!");
                StartCoroutine(ShowTextDelayed("What!?...", enemyTransform, 0.5f));
                yield return new WaitForSeconds(1f);
                PlayerTurn();
                yield break;
            }
        }

        Debug.Log("Enemy attacks!");
        GameObject prefabToUse = null;

        if (Enemy != null)
        {
            if (Enemy.sicknessName == "Schizophrenia")
            {
                prefabToUse = schizophreniaPrefab;
            }
            else if (Enemy.sicknessName == "Insomnia")
            {
                prefabToUse = insomniaPrefab;
                if (Random.value <= 0.25f)
                {
                    playerIsAsleep = true;
                    StartCoroutine(ShowTextDelayed("Zzz...", playerTransform, 0.5f));
                }
            }
            else if (Enemy.sicknessName == "Lactose")
            {
                prefabToUse = lactosePrefab;
            }

            if (prefabToUse != null)
            {
                GameObject projectile = Instantiate(prefabToUse, enemyTransform.position, Quaternion.identity);
                AttackAnimation anim = projectile.GetComponent<AttackAnimation>();
                if (anim != null) anim.Seek(playerTransform);
            }

            yield return new WaitForSeconds(0.5f);

            int incomingDamage = Enemy.damage;
            playerCurrentHealth -= incomingDamage;
            if (playerCurrentHealth < 0) playerCurrentHealth = 0;

            HUDManager.Instance.UpdatePlayerHealth(playerCurrentHealth, playerMaxHealth);

            if (playerCurrentHealth <= 0)
            {
                state = BattleState.LOST;
            }

            Enemy.Attack();
        }

        yield return new WaitForSeconds(0.5f);

        if (state != BattleState.LOST)
            PlayerTurn();
    }

    IEnumerator ShowTextDelayed(string message, Transform target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (floatingTextPrefab != null && target != null)
        {
            GameObject go = Instantiate(floatingTextPrefab, target.position, Quaternion.identity);
            FloatingText ft = go.GetComponent<FloatingText>();
            if (ft != null) ft.SetText(message);
        }
    }

    IEnumerator SkipTurnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(EnemyTurn());
    }
}