using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Contracts;

public class PlayerGamePanel : MonoBehaviour 
{
    [SerializeField]
    private List<GearSlot> gearSlots;

    private ICharacter character;
    
    public void Init(ICharacter character)
    {   
        this.character = character;
        foreach (GearSlot gearSlot in gearSlots)
        {
            gearSlot.Init(character);
        }
    }
}