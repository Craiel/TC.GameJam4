using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class DestructibleTile : MonoBehaviour 
{
    [SerializeField]
    private float hitPoints;

    [SerializeField]
    private Sprite destroyedSprite;

    private float currentHitPoints;

    private void Awake()
    {
        currentHitPoints = hitPoints;
    }

    public void TakeDamage(float damage)
    {
        currentHitPoints -= damage;
        if(currentHitPoints <= 0f)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        GetComponent<SpriteRenderer>().sprite = destroyedSprite;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
