using UnityEngine;

public class Schizophrenia : Sickness
{
    public override bool Attack() 
        {
            base.Attack();
            Debug.Log("ZORT");
        return true;
        }
}
