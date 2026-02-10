using UnityEngine;

public class Sickness : MonoBehaviour
{
    public string sicknessName;
    public int maxHealth;
    public int currentHealth;
    public int damage;
    public int energyCost = 1;
    public int level;
    public bool isDead;

    void Start()
    {
        isDead = false;
        if (currentHealth == 0) currentHealth = maxHealth;
    }

    public virtual bool Attack()
    {
        Debug.Log($"{sicknessName} attacks with {damage} damage!");
        return true; 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " died!");
    }
}