using UnityEngine;

public class Sickness : MonoBehaviour
{
   public string sicknessName;
   public int maxHealth;
   public int currentHealth; // BattleSystem reads this for HUD
   public int damage;
   public int level;
   public bool isDead;

   public virtual void Attack(){
        Debug.Log($"{sicknessName} attacks with {damage} damage!");
   }

    void Start()
    {
        isDead = false;
        // Ensure health is set on spawn
        if(currentHealth == 0) currentHealth = maxHealth; 
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
    }
}