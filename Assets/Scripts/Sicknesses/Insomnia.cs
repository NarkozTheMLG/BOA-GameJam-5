using UnityEngine;

public class Insomnia : Sickness
{
    public override bool Attack()
    {
        // Generate a random number between 0.0 and 1.0
        float chance = Random.value;

        // If less than 0.25 (25%), we Sleep!
        if (chance <= 0.25f)
        {
            Debug.Log("Insomnia fell asleep! Turn skipped.");
            return false; // Return FALSE to tell BattleSystem to skip damage
        }

        // Otherwise, do normal attack
        base.Attack();
        return true;
    }
}