using UnityEngine;             
using System.Collections;      
using UnityEngine.UI;          
public enum BattleState { NONE,START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
 [Header("UI & Player References")]
    public GameObject battleUI;       
    //public MonoBehaviour playerMovement;
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


    void Start()
    {
        state = BattleState.NONE;
    }
public void StartBattle(Sickness enemyFromWorld, Transform enemyTransFromWorld)
    {
        Enemy = enemyFromWorld;
        enemyTransform = enemyTransFromWorld;
     //   playerMovement.enabled = false;
        battleUI.SetActive(true);

        state = BattleState.START;
        SetupBattle();
    }
    void SetupBattle()
    {
        // klavye kontrollerini kapat 
        // UIları görünür yap
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
            StartCoroutine(EnemyTurn()); 
            return;
        }
        else if (moveName == "Defend")
        {
            Debug.Log("You Defended! +1 Energy");
            defenseBonus = 0.50f;
            StartCoroutine(EnemyTurn()); 
            return;
        }
        else if(moveName == "Schizophrenia")
        {
            if (Schizophrenia == null)
            {
                Debug.LogError("Schizophrenia move reference is missing or destroyed.");
                return;
            }
            damageToDeal = Schizophrenia.damage;
            prefabToUse = schizophreniaPrefab;
            state = BattleState.ENEMYTURN;
        }
        else if (moveName == "Insomnia")
        {
            if (Insomnia == null)
            {
                Debug.LogError("Insomnia move reference is missing or destroyed.");
                return;
            }
            damageToDeal = Insomnia.damage;
            prefabToUse = insomniaPrefab;
            state = BattleState.ENEMYTURN;
        }

        if (prefabToUse != null)
        {
            GameObject projectile = Instantiate(prefabToUse, playerHand.position, Quaternion.identity);
            AttackAnimation anim = projectile.GetComponent<AttackAnimation>();
            
            if (anim != null)
            {
                anim.Seek(Enemy.transform);
            }
        }

        StartCoroutine(ProcessAttack(damageToDeal, 0.5f)); 
    }

    IEnumerator ProcessAttack(int damage, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (Enemy != null)
        {
            Enemy.TakeDamage(damage);
            
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
            if(Enemy.sicknessName == "Schizophrenia")
                prefabToUse = schizophreniaPrefab;
            else if(Enemy.sicknessName == "Insomnia")
                prefabToUse = insomniaPrefab;
        if (prefabToUse != null)
        {
            GameObject projectile = Instantiate(prefabToUse, enemyTransform.position, Quaternion.identity);
            AttackAnimation anim = projectile.GetComponent<AttackAnimation>();
            
            if (anim != null)
            {
                anim.Seek(playerTransform);
            }

        }
                yield return new WaitForSeconds(0.1f);

             Enemy.Attack(); 
        }
                    yield return new WaitForSeconds(0.1f);

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
        //playerMovement.enabled = true;
    }
}
}