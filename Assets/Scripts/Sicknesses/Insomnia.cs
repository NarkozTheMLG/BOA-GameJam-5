using UnityEngine;

public class Insomnia : Sickness
{
    public override bool Attack()
    {
        float chance = Random.value;

        if (chance <= 0.25f)
        {
            Debug.Log("Insomnia fell asleep! Turn skipped.");
            return false;
        }

        base.Attack();
        return true;
    }
}