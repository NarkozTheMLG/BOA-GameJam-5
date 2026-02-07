using UnityEngine;

public abstract class Sickness : MonoBehaviour
{
   public string sicknessName;
   public int maxHealth;
    public int currentHealth;
   public int damage;
   public int level;
   public bool isDead;

   public virtual void Attack(){
        Debug.Log($"{sicknessName} attacks with a {damage} damage!");
   }

    void Start()
    {
        isDead = false;
        currentHealth = maxHealth + level;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " died!");
    }
}
