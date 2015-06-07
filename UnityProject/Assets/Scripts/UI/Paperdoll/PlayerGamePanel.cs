using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Contracts;
using UnityEngine.UI;
using System;
using Assets.Scripts.Logic;

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

    [SerializeField]
    private List<Text> gearEntryLabels;

    [SerializeField]
    private List<Text> gearValueLabels;

    private ICharacter character;
    private bool isInitialized;

    private int activeGearSlot = 0;

    private bool gearDataSetFirstTime;

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
                if(!gearDataSetFirstTime)
                {
                    SetActiveGearSlot(activeGearSlot, true);
                    gearDataSetFirstTime = true;
                }
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

            UpdateInput();

            rangedAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.RangedAccuracy) * 100) + "%";
            meleeAccuracy.text = (int)(character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.MeleeAccuracy) * 100) + "%";
            cooling.text = (int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.HeatCoolingRate) + "/s";
            armor.text = ((int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Armor)).ToString();
            shield.text = ((int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Shield)).ToString();
            hull.text = (int)character.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Health) + "/" + (int)character.GetMaxStat(Assets.Scripts.Logic.Enums.StatType.Health);
        }
    }

    private void UpdateInput()
    {
        if(character.InputDevice == null)
        {
            return;
        }

        if (character.InputDevice.RightBumper.WasPressed)
        {
            SetActiveGearSlot(FindNextGearSlot());
        }
        else if (character.InputDevice.LeftBumper.WasPressed)
        {
            SetActiveGearSlot(FindPreviousGearSlot());
        }
        else if (character.InputDevice.Action3.WasPressed)
        {
            if (gearSlots[activeGearSlot].Gear != null)
            {
                character.RemoveGear(gearSlots[activeGearSlot].Gear.Type);
                //TODO: Spawn a gear drop
            }
        }
    }

    private void SetActiveGearSlot(int newGearSlot, bool force = false)
    {
        if (force || newGearSlot != activeGearSlot)
        {
            gearSlots[activeGearSlot].SetSelected(false);
            gearSlots[newGearSlot].SetSelected(true);
            activeGearSlot = newGearSlot;

            for(int i=0; i<=2; ++i)
            {
                gearEntryLabels[i].text = "";
                gearValueLabels[i].text = "";
            }

            if (gearSlots[activeGearSlot].Gear.Type == Assets.Scripts.Logic.Enums.GearType.LeftWeapon ||
                    gearSlots[activeGearSlot].Gear.Type == Assets.Scripts.Logic.Enums.GearType.RightWeapon)
            {
                Eppy.Tuple<string, string, string> damage = Utility.Instance.GetCode(gearSlots[activeGearSlot].Gear as BaseWeapon);
                gearEntryLabels[0].text = damage.Item1;
                gearValueLabels[0].text = ((int)gearSlots[activeGearSlot].Gear.GetCurrentStat(Assets.Scripts.Logic.Enums.StatType.Damage)).ToString();

                gearEntryLabels[1].text = damage.Item2;
                gearEntryLabels[2].text = damage.Item3;
            }
            else
            {
                int entryIndex = 0;
                foreach (Assets.Scripts.Logic.Enums.StatType statType in Enum.GetValues(typeof(Assets.Scripts.Logic.Enums.StatType)))
                {
                    float statValue = gearSlots[activeGearSlot].Gear.GetInheritedStat(statType);
                    if (!Mathf.Approximately(statValue, 0))
                    {
                        gearEntryLabels[entryIndex].text = Utility.Instance.GetCode(statType);
                        gearValueLabels[entryIndex].text = Utility.Instance.ValueWithUnits(statType, statValue);

                        entryIndex++;

                        if (entryIndex > 2) //If you've found your third stat
                        {
                            break;
                        }
                    }
                }
            }

            
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