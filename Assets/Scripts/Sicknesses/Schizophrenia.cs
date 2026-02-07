using UnityEngine;

public class Schizophrenia : Sickness
{
    public override void Attack() 
        {
            base.Attack();
            Debug.Log("ZORT");
        }
}
