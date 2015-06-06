using UnityEngine;
using System.Collections;
using Assets.Scripts.Arena;
using Assets.Scripts.Contracts;

public class GearView : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    public IGear Gear { get; private set; }

    public void Init(IGear gear)
    {
        Gear = gear;
        //TODO: Assign sprite based on gear type
    }
}