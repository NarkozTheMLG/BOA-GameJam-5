using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public BattleSystem battleSystem;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered Trigger: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Sickness enemyScript = other.gameObject.GetComponent<Sickness>();
            Transform enemyTransform = other.gameObject.GetComponent<Transform>();
            if (enemyScript != null)
            {
                battleSystem.StartBattle(enemyScript,enemyTransform);
            }
        }
    }
}