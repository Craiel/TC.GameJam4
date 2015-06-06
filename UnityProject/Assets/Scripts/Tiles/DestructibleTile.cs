using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class DestructibleTile : MonoBehaviour 
{
    [SerializeField]
    private float hitPoints;

    private float currentHitPoints;

    private void Awake()
    {
        currentHitPoints = hitPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: Implement Projectile collision detection
    }

    private void TakeDamage(float damage)
    {
        currentHitPoints -= damage;
        if(currentHitPoints <= 0f)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        //TODO: Implement destroyed vs. undestroyed 
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
