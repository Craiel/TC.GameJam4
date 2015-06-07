using UnityEngine;
using System.Collections;
using Assets.Scripts.Arena;
using Assets.Scripts.Contracts;
using Assets.Scripts.Logic;
using System.Collections.Generic;

public class GearView : MonoBehaviour 
{
    [SerializeField]
    private List<Sprite> sprites;
    
    public IGear Gear { get; private set; }

    public void Init(IGear gear)
    {
        Gear = gear;
        GetComponent<SpriteRenderer>().sprite = sprites[(int)gear.Type];
    }
}