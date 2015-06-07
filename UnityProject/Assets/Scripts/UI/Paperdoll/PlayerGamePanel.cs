using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Contracts;
using UnityEngine.UI;

public class PlayerGamePanel : MonoBehaviour
{
    [SerializeField]
    private List<GearSlot> gearSlots;

    [SerializeField]
    private Text rangedAccuracy;

    [SerializeField]
    private Text meleeAccuracy;

    private ICharacter character;

    private bool isInitialized;

    public void Init(ICharacter character)
    {
        this.character = character;
        foreach (GearSlot gearSlot in gearSlots)
        {
            gearSlot.Init(character);
        }
        isInitialized = true;
    }

    private void Update()
    {
        if(isInitialized)
        {
            foreach(GearSlot gearSlot in gearSlots)
            {
                gearSlot.UpdateUI();
            }
        }

        rangedAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.RangedAccuracy)*100) + "%";
        meleeAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.MeleeAccuracy) * 100) + "%";
    }
}