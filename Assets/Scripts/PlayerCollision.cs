using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public BattleSystem battleSystem;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Sickness enemyScript = collision.gameObject.GetComponent<Sickness>();
            if (enemyScript != null)
            {
                battleSystem.StartBattle(enemyScript);
            }
        }
    }
}