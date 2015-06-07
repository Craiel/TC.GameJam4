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

    public bool IsDestroyed { get; private set; }

    private void Awake()
    {
        currentHitPoints = hitPoints;
    }

    public bool TakeDamage(float damage)
    {
        if (this.IsDestroyed || this.currentHitPoints < 0)
        {
            return false;
        }

        currentHitPoints -= damage;
        if(currentHitPoints <= 0f)
        {
            Destroy();
        }

        return true;
    }

    private void Destroy()
    {
        GetComponent<SpriteRenderer>().sprite = destroyedSprite;
        GetComponent<BoxCollider2D>().enabled = false;
        IsDestroyed = true;
    }
}
