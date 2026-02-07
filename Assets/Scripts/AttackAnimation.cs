using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    public float speed = 10f; 
    private Transform target = null;

    public void Seek(Transform _target)
    {
        Debug.Log("Seeking target: " + _target.name);
        target = _target;
    }

    void Update()
    {
     if (target == null)
            return;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Debug.Log("Hit somethins");
            Destroy(gameObject);
        }
    }
}