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

    [SerializeField]
    private Text cooling;

    [SerializeField]
    private Text armor;

    [SerializeField]
    private Text shield;

    [SerializeField]
    private Text hull;

    private ICharacter character;
    private bool isInitialized;

    private int activeGearSlot = 0;

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

            if(gearSlots[activeGearSlot].Gear != null)
            {
                gearSlots[activeGearSlot].SetSelected(true);
            }
            else
            {
                int newGearSlot = FindNextGearSlot();
                if (newGearSlot != activeGearSlot)
                {
                    gearSlots[activeGearSlot].SetSelected(false);
                    gearSlots[newGearSlot].SetSelected(true);
                    activeGearSlot = newGearSlot;
                }
            }

            if(character.InputDevice.RightBumper.WasPressed)
            {
                int newGearSlot = FindNextGearSlot();
                if(newGearSlot != activeGearSlot)
                {
                    gearSlots[activeGearSlot].SetSelected(false);
                    gearSlots[newGearSlot].SetSelected(true);
                    activeGearSlot = newGearSlot;
                }
            }
            else if (character.InputDevice.LeftBumper.WasPressed)
            {
                int newGearSlot = FindPreviousGearSlot();
                if (newGearSlot != activeGearSlot)
                {
                    gearSlots[activeGearSlot].SetSelected(false);
                    gearSlots[newGearSlot].SetSelected(true);
                    activeGearSlot = newGearSlot;
                }
            }

            rangedAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.RangedAccuracy) * 100) + "%";
            meleeAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.MeleeAccuracy) * 100) + "%";
            cooling.text = (int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.HeatCoolingRate) + "/s";
            armor.text = ((int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Armor)).ToString();
            shield.text = ((int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Shield)).ToString();
            hull.text = (int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Health) + "/" + (int)character.GetMaxStat(Assets.Scripts.Logic.Enums.StatType.Health);
        }
    }

    private int FindNextGearSlot()
    {
        int gearSlot = activeGearSlot;
        while(true)
        {
            gearSlot++;
            if(gearSlot >= gearSlots.Count)
            {
                gearSlot = 0;
            }

            if(gearSlot == activeGearSlot || gearSlots[gearSlot].Gear != null)
            {
                return gearSlot;
            }
        }
    }

    private int FindPreviousGearSlot()
    {
        int gearSlot = activeGearSlot;
        while (true)
        {
            gearSlot--;
            if (gearSlot < 0)
            {
                gearSlot = gearSlots.Count-1;
            }

            if (gearSlot == activeGearSlot || gearSlots[gearSlot].Gear != null)
            {
                return gearSlot;
            }
        }
    }

}