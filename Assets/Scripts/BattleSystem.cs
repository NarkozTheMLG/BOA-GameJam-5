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

    [Header("Attack Prefabs")]
    public GameObject schizophreniaPrefab; 
    public GameObject insomniaPrefab;      

    [Header("Setup")]
    public Transform playerHand;

    public BattleState state;
    private float defenseBonus;

    private int playerMaxHealth = 100;
    private int playerCurrentHealth;

    void Start()
    {
        state = BattleState.NONE;
    }

    public void StartBattle(Sickness enemyFromWorld, Transform enemyTransFromWorld)
    {
        Enemy = enemyFromWorld;
        enemyTransform = enemyTransFromWorld;
        
        // 1. Setup UI
        playerMovement.enabled = false;
        battleUI.SetActive(true);

        // 2. Initialize Player Health
        playerCurrentHealth = playerMaxHealth;
        HUDManager.Instance.UpdatePlayerHealth(playerCurrentHealth, playerMaxHealth);

        // 3. Initialize Enemy Health in HUD
        // We use the Enemy stats directly
        HUDManager.Instance.SetEnemyMaxHealth(Enemy.maxHealth);
        HUDManager.Instance.UpdateEnemyHealth(Enemy.currentHealth);

        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        Debug.Log("Player's turn. Choose an action.");
    }

    public void ExecuteMove(string moveName) 
    {
        defenseBonus = 0f; 
        if (state != BattleState.PLAYERTURN) return;

        GameObject prefabToUse = null;
        int damageToDeal = 0;

        if (moveName == "Skip")
        {
            Debug.Log("You Skipped! +3 Energy");
            // Optional: HUDManager.Instance.getEnergy(3);
            StartCoroutine(EnemyTurn()); 
            return;
        }
        else if (moveName == "Defend")
        {
            Debug.Log("You Defended! +1 Energy");
            defenseBonus = 0.50f;
            // Optional: HUDManager.Instance.getEnergy(1);
            StartCoroutine(EnemyTurn()); 
            return;
        }
        else if(moveName == "Schizophrenia")
        {
            if (Schizophrenia != null)
            {
                damageToDeal = Schizophrenia.damage;
                prefabToUse = schizophreniaPrefab;
                state = BattleState.ENEMYTURN; // Lock input
            }
        }
        else if (moveName == "Insomnia")
        {
            if (Insomnia != null)
            {
                damageToDeal = Insomnia.damage;
                prefabToUse = insomniaPrefab;
                state = BattleState.ENEMYTURN; // Lock input
            }
        }

        // Spawn Projectile
        if (prefabToUse != null)
        {
            GameObject projectile = Instantiate(prefabToUse, playerHand.position, Quaternion.identity);
            AttackAnimation anim = projectile.GetComponent<AttackAnimation>();
            if (anim != null) anim.Seek(Enemy.transform);
        }

        // Process Damage
        StartCoroutine(ProcessAttack(damageToDeal, 0.8f)); 
    }

    IEnumerator ProcessAttack(int damage, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (Enemy != null)
        {
            // A. Apply Damage
            Enemy.TakeDamage(damage);

            // B. UPDATE HUD (Enemy Health)
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
        GameObject prefabToUse = null;
        
        float waitTime = Random.Range(0.5f, 1.5f); 
        yield return new WaitForSeconds(waitTime);

        Debug.Log("Enemy attacks!");
        
        if (Enemy != null)
        {
            // Visuals
            if(Enemy.sicknessName == "Schizophrenia") prefabToUse = schizophreniaPrefab;
            else if(Enemy.sicknessName == "Insomnia") prefabToUse = insomniaPrefab;
            
            if (prefabToUse != null)
            {
                GameObject projectile = Instantiate(prefabToUse, enemyTransform.position, Quaternion.identity);
                AttackAnimation anim = projectile.GetComponent<AttackAnimation>();
                if (anim != null) anim.Seek(playerTransform);
            }

            yield return new WaitForSeconds(0.5f); // Wait for hit

            // --- LOGIC: DEAL DAMAGE TO PLAYER ---
            
            // 1. Calculate Damage (Enemy damage minus defense)
            int incomingDamage = Enemy.damage;
            if (defenseBonus > 0) incomingDamage = Mathf.RoundToInt(incomingDamage * (1 - defenseBonus));

            // 2. Apply to Player
            playerCurrentHealth -= incomingDamage;
            if (playerCurrentHealth < 0) playerCurrentHealth = 0;

            // 3. UPDATE HUD (Player Health)
            HUDManager.Instance.UpdatePlayerHealth(playerCurrentHealth, playerMaxHealth);

            // 4. Check Player Death
            if (playerCurrentHealth <= 0)
            {
                state = BattleState.LOST;
                Debug.Log("Player Died!");
                // Handle Game Over here
            }

            Enemy.Attack(); // Just logs the message
        }

        yield return new WaitForSeconds(0.5f);
        
        if (state != BattleState.LOST)
            PlayerTurn();
    }

    void BattleWon()
    {
        if (state == BattleState.WON)
        {
            if (Enemy != null)
            {
                Destroy(Enemy.gameObject);
                Enemy = null;
            }
            enemyTransform = null;
            battleUI.SetActive(false);
            playerMovement.enabled = true;
        }
    }
}