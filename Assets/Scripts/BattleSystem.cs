using UnityEngine;             
using System.Collections;      
using UnityEngine.UI;          
public enum BattleState { NONE,START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
 [Header("UI & Player References")]
    public GameObject battleUI;       
    //public MonoBehaviour playerMovement;

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
        StartBattle(Enemy);
    }
public void StartBattle(Sickness enemyFromWorld)
    {
        Enemy = enemyFromWorld;

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
            damageToDeal = Schizophrenia.damage;
            prefabToUse = schizophreniaPrefab;
            state = BattleState.ENEMYTURN;
        }
        else if (moveName == "Insomnia")
        {
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

        StartCoroutine(EnemyTurn());
    }

  IEnumerator EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        
        float waitTime = Random.Range(1f, 3f); 
        yield return new WaitForSeconds(waitTime);

        Debug.Log("Enemy attacks!");
        
        if (Enemy != null)
        {
             Enemy.Attack(); 
        }

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    void BattleWon()
{
    if (state == BattleState.WON)
    {
        Destroy(Enemy.gameObject); 
        battleUI.SetActive(false);
        //playerMovement.enabled = true;
    }
}
}