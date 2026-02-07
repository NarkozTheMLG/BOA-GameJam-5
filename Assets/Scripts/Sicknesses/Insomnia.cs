using UnityEngine;

public class Insomnia : Sickness
{
       public override void Attack() 
        {
            base.Attack();
            Debug.Log("PIRT");
        }
}
